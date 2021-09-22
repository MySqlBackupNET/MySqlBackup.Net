using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlConnector
{
    public enum GetTotalRowsMethod
    {
        Skip = 1,
        InformationSchema = 2,
        SelectCount = 3
    }
}
