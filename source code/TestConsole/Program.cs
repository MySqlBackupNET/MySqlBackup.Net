using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows.Forms;
using System.IO;

namespace System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string constr = "server=localhost;user=root;pwd=1234;convertzerodatetime=true;treattinyasboolean=true;";
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;

            bool runapp = true;

            while (runapp)
            {
                Console.Clear();

                Console.WriteLine("MySqlBackup.NET Test Suite");
                Console.WriteLine("==========================\n");

                // Run Test 1: Basic functionality test
                Console.WriteLine("Beginning Test1Basic...");

                Test1Basic test1 = new Test1Basic(constr, baseFolder);
                string result = test1.RunTest();

                Console.WriteLine();
                Console.WriteLine(result);
                Console.WriteLine();

                Console.WriteLine("\nCompleted Test1Basic");

                // Future: Add Test2 for benchmark/large data test
                // Console.WriteLine("\nBeginning Test2Benchmark...");
                // Test2Benchmark test2 = new Test2Benchmark();
                // test2.RunTest();
                // Console.WriteLine("Completed Test2Benchmark");

                Console.WriteLine("\nPress 'C' to continue, or any key to exit...");
                var k = Console.ReadKey();

                if (k.Key != ConsoleKey.C)
                {
                    runapp = false;
                }
            }
        }
    }
}
