﻿using System;

namespace MySql.Data.MySqlClient
{
    public class GetTotalRowsArgs : EventArgs
    {
        public GetTotalRowsArgs(int totalTables, int curTable)
        {
            TotalTables = totalTables;
            CurrTable = curTable;
        }

        public int TotalTables { get; private set; }
        public int CurrTable { get; private set; }
    }
}
