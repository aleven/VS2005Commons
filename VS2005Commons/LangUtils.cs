using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    public class LangUtils
    {
        /// <summary>
        /// Verifica se l'oggetto e' null o DBNull o stringa vuota
        /// </summary>
        public static bool isEmpty(Object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == string.Empty;
        }

        public static bool isNotEmpty(Object obj)
        {
            return !isEmpty(obj);
        }

        public static bool isDbNull(Object obj)
        {
            return obj != null && obj == DBNull.Value;
        }
    }
}
