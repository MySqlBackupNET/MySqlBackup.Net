using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class HeadersAndFootersTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btRun_Click(object sender, EventArgs e)
        {
            try
            {
                string result = "";

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    DocumentHeadersFootersTest t = new DocumentHeadersFootersTest(cmd);
                    result = t.RunValidationTest();
                }

                ltResult.Text = result;
            }
            catch (Exception ex)
            {
                ltResult.Text = $"Error: {ex.Message}";
                ((masterPage1)this.Master).ShowMessage("Error", ex.Message, false);
            }
        }
    }

    /// <summary>
    /// Test class to validate that document headers and footers properly preserve session state
    /// </summary>
    public class DocumentHeadersFootersTest
    {
        private MySqlCommand _command;
        private ExportInformations _exportInfo;
        private Stopwatch _stopwatch;

        public DocumentHeadersFootersTest(MySqlCommand command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _exportInfo = new ExportInformations();
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Runs the complete validation test and returns a detailed report
        /// </summary>
        public string RunValidationTest()
        {
            _stopwatch.Start();
            var report = new StringBuilder();
            report.AppendLine("=".PadRight(80, '='));
            report.AppendLine("MySqlBackup.NET - Document Headers/Footers Validation Test");
            report.AppendLine("=".PadRight(80, '='));
            report.AppendLine($"Test Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine($"MySQL Server: {GetServerVersion()}");
            report.AppendLine();

            try
            {
                // Step 1: Capture original session state
                var stepTimer = Stopwatch.StartNew();
                report.AppendLine("STEP 1: Capturing Original Session State");
                report.AppendLine("-".PadRight(50, '-'));
                var originalState = CaptureSessionState();
                AppendSessionStateToReport(report, originalState, "ORIGINAL");
                stepTimer.Stop();
                report.AppendLine($"⏱️ Step 1 Duration: {stepTimer.ElapsedMilliseconds}ms");
                report.AppendLine();

                // Step 2: Execute headers (first time)
                stepTimer.Restart();
                report.AppendLine("STEP 2: Executing Document Headers (Setup)");
                report.AppendLine("-".PadRight(50, '-'));
                var headers = _exportInfo.GetDocumentHeaders(_command);
                var headerExecutionTimer = Stopwatch.StartNew();
                ExecuteStatements(headers, "Header");
                headerExecutionTimer.Stop();
                var afterHeadersState = CaptureSessionState();
                AppendSessionStateToReport(report, afterHeadersState, "AFTER HEADERS");
                stepTimer.Stop();
                report.AppendLine($"⏱️ Header Execution: {headerExecutionTimer.ElapsedMilliseconds}ms");
                report.AppendLine($"⏱️ Step 2 Total Duration: {stepTimer.ElapsedMilliseconds}ms");
                report.AppendLine();

                // Step 3: Check @OLD_* variables were created
                stepTimer.Restart();
                report.AppendLine("STEP 3: Checking @OLD_* Variables Were Created");
                report.AppendLine("-".PadRight(50, '-'));
                var oldVariables = CaptureOldVariables();
                AppendOldVariablesToReport(report, oldVariables);
                stepTimer.Stop();
                report.AppendLine($"⏱️ Step 3 Duration: {stepTimer.ElapsedMilliseconds}ms");
                report.AppendLine();

                // Step 4: Execute footers (validation)
                stepTimer.Restart();
                report.AppendLine("STEP 4: Executing Document Footers (Restore Test)");
                report.AppendLine("-".PadRight(50, '-'));
                var footers = _exportInfo.GetDocumentFooters();
                var footerExecutionTimer = Stopwatch.StartNew();
                ExecuteStatements(footers, "Footer");
                footerExecutionTimer.Stop();
                var afterFootersState = CaptureSessionState();
                AppendSessionStateToReport(report, afterFootersState, "AFTER FOOTERS");
                stepTimer.Stop();
                report.AppendLine($"⏱️ Footer Execution: {footerExecutionTimer.ElapsedMilliseconds}ms");
                report.AppendLine($"⏱️ Step 4 Total Duration: {stepTimer.ElapsedMilliseconds}ms");
                report.AppendLine();

                // Step 5: Execute headers again (for real export)
                stepTimer.Restart();
                report.AppendLine("STEP 5: Executing Document Headers Again (Real Export Setup)");
                report.AppendLine("-".PadRight(50, '-'));
                var secondHeaderExecutionTimer = Stopwatch.StartNew();
                ExecuteStatements(headers, "Header (2nd time)");
                secondHeaderExecutionTimer.Stop();
                var afterSecondHeadersState = CaptureSessionState();
                AppendSessionStateToReport(report, afterSecondHeadersState, "AFTER 2ND HEADERS");
                stepTimer.Stop();
                report.AppendLine($"⏱️ 2nd Header Execution: {secondHeaderExecutionTimer.ElapsedMilliseconds}ms");
                report.AppendLine($"⏱️ Step 5 Total Duration: {stepTimer.ElapsedMilliseconds}ms");
                report.AppendLine();

                // Step 6: Final footer execution
                stepTimer.Restart();
                report.AppendLine("STEP 6: Final Footer Execution (Cleanup)");
                report.AppendLine("-".PadRight(50, '-'));
                var finalFooterExecutionTimer = Stopwatch.StartNew();
                ExecuteStatements(footers, "Footer (final)");
                finalFooterExecutionTimer.Stop();
                var finalState = CaptureSessionState();
                AppendSessionStateToReport(report, finalState, "FINAL STATE");
                stepTimer.Stop();
                report.AppendLine($"⏱️ Final Footer Execution: {finalFooterExecutionTimer.ElapsedMilliseconds}ms");
                report.AppendLine($"⏱️ Step 6 Total Duration: {stepTimer.ElapsedMilliseconds}ms");
                report.AppendLine();

                // Step 7: Comparison and validation
                stepTimer.Restart();
                report.AppendLine("STEP 7: Validation Results");
                report.AppendLine("-".PadRight(50, '-'));
                ValidateStates(report, originalState, finalState);
                stepTimer.Stop();
                report.AppendLine($"⏱️ Step 7 Duration: {stepTimer.ElapsedMilliseconds}ms");
                report.AppendLine();

                _stopwatch.Stop();

                // Performance Summary
                report.AppendLine("PERFORMANCE SUMMARY");
                report.AppendLine("-".PadRight(50, '-'));
                report.AppendLine($"⏱️ Total Headers Execution Time: {headerExecutionTimer.ElapsedMilliseconds + secondHeaderExecutionTimer.ElapsedMilliseconds}ms");
                report.AppendLine($"⏱️ Total Footers Execution Time: {footerExecutionTimer.ElapsedMilliseconds + finalFooterExecutionTimer.ElapsedMilliseconds}ms");
                report.AppendLine($"⏱️ Total SQL Execution Time: {headerExecutionTimer.ElapsedMilliseconds + footerExecutionTimer.ElapsedMilliseconds + secondHeaderExecutionTimer.ElapsedMilliseconds + finalFooterExecutionTimer.ElapsedMilliseconds}ms");
                report.AppendLine($"⏱️ OVERALL TEST DURATION: {_stopwatch.ElapsedMilliseconds}ms");

                report.AppendLine();
                report.AppendLine("=".PadRight(80, '='));
                report.AppendLine("TEST COMPLETED SUCCESSFULLY");
                report.AppendLine("=".PadRight(80, '='));

                report.AppendLine();
                report.AppendLine();
                report.AppendLine("** Final Run **");

                _stopwatch.Restart();

                using (var conn = config.GetNewConnection())
                using (var Command = conn.CreateCommand())
                {
                    conn.Open();
                    var exportInfo = new ExportInformations();

                    List<string> lstHeadersFooters = new List<string>();

                    // round 1: save the initial values
                    lstHeadersFooters.AddRange(exportInfo.GetDocumentHeaders(Command));

                    report.AppendLine();
                    foreach (var h in exportInfo.GetDocumentHeaders(Command))
                        report.AppendLine(h);

                    // round 2: attempt to restore values
                    lstHeadersFooters.AddRange(exportInfo.GetDocumentFooters());

                    report.AppendLine();
                    foreach (var h in exportInfo.GetDocumentFooters())
                        report.AppendLine(h);

                    // round 3: real execution
                    lstHeadersFooters.AddRange(exportInfo.GetDocumentHeaders(Command));

                    report.AppendLine();
                    foreach (var h in exportInfo.GetDocumentHeaders(Command))
                        report.AppendLine(h);

                    report.AppendLine();

                    for (int i = 0; i < lstHeadersFooters.Count; i++)
                    {
                        string header = lstHeadersFooters[i];

                        if (!header.StartsWith("--") && !string.IsNullOrWhiteSpace(header))
                        {
                            try
                            {
                                Command.CommandText = header;
                                Command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException(
                                    $"Error message: {ex.Message}. Document headers/footers validation failed. Failed statement: [{i}] '{header}'. " +
                                    "If you have customized headers/footers, verify all SQL statements are valid.",
                                    ex);
                            }
                        }
                    }
                }

                _stopwatch.Stop();

                report.AppendLine("** Completed Final Run **");
                report.AppendLine($"Time elapsed for final run: {_stopwatch.Elapsed.TotalMilliseconds}ms");
            }
            catch (Exception ex)
            {
                _stopwatch.Stop();
                report.AppendLine();
                report.AppendLine("❌ TEST FAILED WITH EXCEPTION:");
                report.AppendLine($"Error: {ex.Message}");
                report.AppendLine($"SQL: {_command.CommandText}");
                report.AppendLine($"⏱️ Test Duration Before Failure: {_stopwatch.ElapsedMilliseconds}ms");
                report.AppendLine($"Stack Trace: {ex.StackTrace}");
            }

            return report.ToString();
        }

        private Dictionary<string, object> CaptureSessionState()
        {
            var state = new Dictionary<string, object>();

            var sessionVariables = new[]
            {
                "@@session.character_set_client",
                "@@session.character_set_results",
                "@@session.collation_connection",
                "@@session.time_zone",
                "@@session.sql_mode",
                "@@session.unique_checks",
                "@@session.foreign_key_checks",
                "@@session.sql_notes"
            };

            foreach (var variable in sessionVariables)
            {
                try
                {
                    _command.CommandText = $"SELECT {variable}";
                    var value = _command.ExecuteScalar();
                    state[variable] = value ?? "NULL";
                }
                catch (Exception ex)
                {
                    state[variable] = $"ERROR: {ex.Message}";
                }
            }

            return state;
        }

        private Dictionary<string, object> CaptureOldVariables()
        {
            var variables = new Dictionary<string, object>();

            var oldVariables = new[]
            {
                "@OLD_CHARACTER_SET_CLIENT",
                "@OLD_CHARACTER_SET_RESULTS",
                "@OLD_COLLATION_CONNECTION",
                "@OLD_TIME_ZONE",
                "@OLD_SQL_MODE",
                "@OLD_UNIQUE_CHECKS",
                "@OLD_FOREIGN_KEY_CHECKS",
                "@OLD_SQL_NOTES"
            };

            foreach (var variable in oldVariables)
            {
                try
                {
                    _command.CommandText = $"SELECT {variable}";
                    var value = _command.ExecuteScalar();
                    variables[variable] = value ?? "NULL";
                }
                catch (Exception ex)
                {
                    variables[variable] = $"ERROR: {ex.Message}";
                }
            }

            return variables;
        }

        private void ExecuteStatements(List<string> statements, string description)
        {
            foreach (var statement in statements)
            {
                if (!statement.StartsWith("--") && !string.IsNullOrWhiteSpace(statement))
                {
                    try
                    {
                        _command.CommandText = statement;
                        _command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"Failed to execute {description} statement: '{statement}'", ex);
                    }
                }
            }
        }

        private void AppendSessionStateToReport(StringBuilder report, Dictionary<string, object> state, string title)
        {
            report.AppendLine($"{title}:");
            foreach (var kvp in state)
            {
                report.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }
            report.AppendLine();
        }

        private void AppendOldVariablesToReport(StringBuilder report, Dictionary<string, object> variables)
        {
            report.AppendLine("@OLD_* Variables:");
            foreach (var kvp in variables)
            {
                var status = kvp.Value.ToString() == "NULL" ? "❌ NOT SET" : "✅ SET";
                report.AppendLine($"  {kvp.Key}: {kvp.Value} {status}");
            }
            report.AppendLine();
        }

        private void ValidateStates(StringBuilder report, Dictionary<string, object> originalState, Dictionary<string, object> finalState)
        {
            bool allMatch = true;

            report.AppendLine("Comparing Original vs Final State:");

            foreach (var kvp in originalState)
            {
                var originalValue = kvp.Value?.ToString() ?? "NULL";
                var finalValue = finalState.ContainsKey(kvp.Key) ?
                    finalState[kvp.Key]?.ToString() ?? "NULL" : "MISSING";

                bool matches = originalValue == finalValue;
                string status = matches ? "✅ MATCH" : "❌ MISMATCH";

                report.AppendLine($"  {kvp.Key}:");
                report.AppendLine($"    Original: {originalValue}");
                report.AppendLine($"    Final:    {finalValue}");
                report.AppendLine($"    Status:   {status}");

                if (!matches) allMatch = false;
            }

            report.AppendLine();
            report.AppendLine($"OVERALL RESULT: {(allMatch ? "✅ ALL SESSION VARIABLES RESTORED CORRECTLY" : "❌ SOME SESSION VARIABLES NOT RESTORED")}");
        }

        private string GetServerVersion()
        {
            try
            {
                _command.CommandText = "SELECT VERSION()";
                return _command.ExecuteScalar()?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unable to retrieve";
            }
        }

        /// <summary>
        /// Simple test method that just validates the cycle works without exceptions and returns timing
        /// </summary>
        public (bool Success, long ElapsedMs) TestHeadersFootersCycle()
        {
            var timer = Stopwatch.StartNew();
            try
            {
                var headers = _exportInfo.GetDocumentHeaders(_command);
                var footers = _exportInfo.GetDocumentFooters();

                // Execute the full cycle
                ExecuteStatements(headers, "Header");
                ExecuteStatements(footers, "Footer");
                ExecuteStatements(headers, "Header (2nd)");
                ExecuteStatements(footers, "Footer (final)");

                timer.Stop();
                return (true, timer.ElapsedMilliseconds);
            }
            catch
            {
                timer.Stop();
                return (false, timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Benchmarks just the validation cycle multiple times to get average timing
        /// </summary>
        public string BenchmarkValidationCycle(int iterations = 10)
        {
            var report = new StringBuilder();
            var times = new List<long>();

            report.AppendLine($"VALIDATION CYCLE BENCHMARK ({iterations} iterations)");
            report.AppendLine("=".PadRight(60, '='));

            for (int i = 1; i <= iterations; i++)
            {
                var result = TestHeadersFootersCycle();
                times.Add(result.ElapsedMs);
                report.AppendLine($"Iteration {i,2}: {result.ElapsedMs,3}ms - {(result.Success ? "✅" : "❌")}");
            }

            if (times.Count > 0)
            {
                report.AppendLine();
                report.AppendLine("BENCHMARK RESULTS:");
                report.AppendLine($"  Average: {times.Average():F1}ms");
                report.AppendLine($"  Minimum: {times.Min()}ms");
                report.AppendLine($"  Maximum: {times.Max()}ms");
                report.AppendLine($"  Total:   {times.Sum()}ms");
            }

            return report.ToString();
        }
    }
}