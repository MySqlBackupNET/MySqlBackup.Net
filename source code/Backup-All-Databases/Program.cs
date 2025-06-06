using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal; // For NTFS permissions
using System.Security.AccessControl; // For NTFS permissions
using System.Drawing.Text;
//using MySqlConnector;
//using System.Data;
//using System.Diagnostics;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
//using Backup_All_Databases.Properties;
//using System.Drawing;

namespace Backup_All_Databases
{
    internal static class Program
    {
        internal static PrivateFontCollection myFontCollection = new PrivateFontCollection();

        internal static string ConfigFilePath => Path.Combine(Application.StartupPath, "config.dat");
        internal static string KeyFilePath => Path.Combine(Application.StartupPath, "kdac.bin");
        internal static string saltFilePath => Path.Combine(Application.StartupPath, "salt.bin");

        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg.ToUpper() == "/SILENT")
                    {
                        try
                        {
                            var config = ReadConfigFile(false);
                            if (config != null)
                            {
                                Backup backup = new Backup();
                                backup.Run(config);
                                config = null;
                            }
                            GC.Collect();
                        }
                        catch { }
                        return;
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            myFontCollection.AddFontFile(Path.Combine(Application.StartupPath, "CascadiaCode.ttf"));

            Application.Run(new Form1());
        }

        /// <summary>
        /// Prompts the user for a password via a dialog.
        /// </summary>
        private static string GetUserInputPwd()
        {
            using (FormPwd f1 = new FormPwd())
            {
                if (f1.ShowDialog() != DialogResult.OK)
                    return null;
                string pwd = f1.pwd?.Trim();
                if (string.IsNullOrEmpty(pwd))
                {
                    MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                return pwd;
            }
        }

        /// <summary>
        /// Reads and decrypts the configuration file.
        /// </summary>
        public static Config ReadConfigFile(bool p)
        {
            if (!File.Exists(ConfigFilePath))
                return null;

            byte[] key;
            if (File.Exists(KeyFilePath) && !p)
            {
                byte[] protectedKey = File.ReadAllBytes(KeyFilePath);
                try
                {
                    key = EncryptionHelper.UnprotectKey(protectedKey);
                }
                catch (CryptographicException)
                {
                    throw new Exception("Unable to access stored key.");
                }
            }
            else
            {
                string password = GetUserInputPwd();
                if (string.IsNullOrEmpty(password))
                    return null;
                key = EncryptionHelper.GenerateKey(password);
            }

            try
            {
                byte[] fileData = File.ReadAllBytes(ConfigFilePath);
                byte[] decryptedData = EncryptionHelper.Decrypt(fileData, key);
                string configString = Encoding.UTF8.GetString(decryptedData);
                string[] parts = configString.Split(new[] { "|||" }, StringSplitOptions.None);

                if (parts.Length != 8)
                {
                    throw new Exception("Invalid, outdated, incompatible configuration file");
                }

                int.TryParse(parts[2], out int maxBackupCopies);

                Config config = new Config();
                config.ConnectionString = parts[0];
                config.BackupPath = parts[1];
                config.MaxBackupCopies = maxBackupCopies;
                config.BackupAllDatabases = bool.Parse(parts[3]);
                config.IncludeList = parts[4].Split(',').ToList();
                config.ExcludeList = parts[5].Split(',').ToList();
                config.StartHour = Convert.ToInt32(parts[6]);
                config.StartMinute = Convert.ToInt32(parts[7]);
                config.Key = key;

                Array.Clear(decryptedData, 0, decryptedData.Length);
                return config;
            }
            catch (CryptographicException ex)
            {
                throw new Exception("Decryption Error: Incorrect password or corrupted file.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read config: {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the configuration to an encrypted file.
        /// </summary>
        public static void WriteConfigFile(Config newConfig)
        {
            if (newConfig == null)
            {
                throw new Exception("Configuration cannot be null.");
            }

            byte[] key = null;
            string password = null;
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    Config existingConfig = ReadConfigFile(true);
                    if (existingConfig == null)
                    {
                        throw new Exception("Cannot write config: unable to read existing config.");
                    }
                    key = existingConfig.Key;
                }
                else
                {
                    password = GetUserInputPwd();
                    if (string.IsNullOrEmpty(password))
                        return;

                    try
                    {
                        key = EncryptionHelper.GenerateKey(password);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to generate encryption key: {ex.Message}");
                    }

                    try
                    {
                        byte[] protectedKey = EncryptionHelper.ProtectKey(key);
                        File.WriteAllBytes(KeyFilePath, protectedKey);
                        SetFilePermissions(KeyFilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to store protected key: {ex.Message}");
                    }
                }

                try
                {
                    string configString = string.Join("|||",
                        newConfig.ConnectionString ?? "",
                        newConfig.BackupPath ?? "",
                        newConfig.MaxBackupCopies.ToString(),
                        newConfig.BackupAllDatabases.ToString(),
                        string.Join(",", newConfig.IncludeList ?? new List<string>()),
                        string.Join(",", newConfig.ExcludeList ?? new List<string>()),
                        newConfig.StartHour.ToString(),
                        newConfig.StartMinute.ToString());
                    byte[] data = Encoding.UTF8.GetBytes(configString);
                    byte[] encryptedData = EncryptionHelper.Encrypt(data, key);
                    File.WriteAllBytes(ConfigFilePath, encryptedData);
                    SetFilePermissions(ConfigFilePath); // Set NTFS permissions for config file

                    Array.Clear(data, 0, data.Length);
                    Array.Clear(encryptedData, 0, encryptedData.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to write config file: {ex.Message}");
                }
            }
            finally
            {
                if (key != null)
                    Array.Clear(key, 0, key.Length);
                if (password != null)
                {
                    Array.Clear(Encoding.UTF8.GetBytes(password), 0, password.Length);
                    password = null;
                }
            }
        }

        /// <summary>
        /// Sets NTFS permissions on a file to allow access only to SYSTEM and the current user.
        /// </summary>
        public static void SetFilePermissions(string filePath)
        {
            try
            {
                FileSecurity fileSecurity = File.GetAccessControl(filePath);
                fileSecurity.SetAccessRuleProtection(true, false); // Disable inheritance

                // Remove all existing access rules
                foreach (FileSystemAccessRule rule in fileSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    fileSecurity.RemoveAccessRule(rule);
                }

                // Add access for SYSTEM
                fileSecurity.AddAccessRule(new FileSystemAccessRule(
                    new NTAccount("NT AUTHORITY\\SYSTEM"),
                    FileSystemRights.FullControl,
                    AccessControlType.Allow));

                // Add access for the current user
                string currentUser = WindowsIdentity.GetCurrent().Name;
                fileSecurity.AddAccessRule(new FileSystemAccessRule(
                    new NTAccount(currentUser),
                    FileSystemRights.FullControl,
                    AccessControlType.Allow));

                File.SetAccessControl(filePath, fileSecurity);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set permissions on {filePath}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}