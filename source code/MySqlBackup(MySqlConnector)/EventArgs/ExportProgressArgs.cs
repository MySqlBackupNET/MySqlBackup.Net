using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlConnector
{
    public class ExportProgressArgs : EventArgs
    {
        string _currentTableName = "";
        long _totalRowsInCurrentTable = 0;
        long _totalRowsInAllTables = 0;
        long _currentRowIndexInCurrentTable = 0;
        long _currentRowIndexInAllTables = 0;
        int _totalTables = 0;
        int _currentTableIndex = 0;

        public string CurrentTableName { get { return _currentTableName; } }
        public long TotalRowsInCurrentTable { get { return _totalRowsInCurrentTable; } }
        public long TotalRowsInAllTables { get { return _totalRowsInAllTables; } }
        public long CurrentRowIndexInCurrentTable { get { return _currentRowIndexInCurrentTable; } }
        public long CurrentRowIndexInAllTables { get { return _currentRowIndexInAllTables; } }
        public int TotalTables { get { return _totalTables; } }
        public int CurrentTableIndex { get { return _currentTableIndex; } }

        public ExportProgressArgs(string currentTableName,
            long totalRowsInCurrentTable,
            long totalRowsInAllTables,
            long currentRowIndexInCurrentTable,
            long currentRowIndexInAllTable,
            int totalTables,
            int currentTableIndex)
        {
            _currentTableName = currentTableName;
            _totalRowsInCurrentTable = totalRowsInCurrentTable;
            _totalRowsInAllTables = totalRowsInAllTables;
            _currentRowIndexInCurrentTable = currentRowIndexInCurrentTable;
            _currentRowIndexInAllTables = currentRowIndexInAllTable;
            _totalTables = totalTables;
            _currentTableIndex = currentTableIndex;
        }
    }
}
