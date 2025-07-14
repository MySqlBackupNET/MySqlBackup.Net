using System;
using System.Collections.Generic;
using System.Text;

namespace Devart.Data.MySql
{
    public enum GetTotalRowsMethod
    {
        Auto = 0,
        Skip = 1,
        InformationSchema = 2,
        SelectCount = 3
    }
}
