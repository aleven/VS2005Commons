using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VS2005Commons
{
    public class ADONetUtils
    {
        public static bool hasRows(DataTable dt)
        {
            bool res = false;

            if (dt != null && dt.Rows.Count > 0)
            {
                res = true;
            }

            return res;
        }

        public static int drToInt(IDataReader dr, String campo)
        {
            return (dr[campo] != Convert.DBNull) ? (int)dr[campo] : 0;
        }

        public static long drToLong(IDataReader dr, String campo)
        {
            return (dr[campo] != Convert.DBNull) ? (long)dr[campo] : 0l;
        }
    }
}
