using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class GetTotalRowsArgs : EventArgs
    {
        int _totalTables = 0;
        int _curTable = 0;

        public GetTotalRowsArgs(int totalTables, int curTable)
        {
            _totalTables = totalTables;
            _curTable = curTable;
        }
    }
}
