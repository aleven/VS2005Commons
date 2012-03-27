using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace VS2005Commons
{
    public class ListUtils
    {
        /// <summary>
        /// Verifica se l'oggetto e' null o DBNull o stringa vuota
        /// </summary>
        public static bool isEmpty(IList aList)
        {
            return aList == null || aList.Count == 0;
        }

        public static bool isNotEmpty(IList aList)
        {
            return !isEmpty(aList);
        }

    }
}
