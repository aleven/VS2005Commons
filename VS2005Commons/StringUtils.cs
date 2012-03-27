using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    public class StringUtils
    {
        public const Char SPLITTER = '|';

        /// <summary>
        /// Check if a string is null or empty
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static bool isNotEmpty(String aString)
        {
            bool res = false;

            if (aString != null && aString.Length > 0)
            {
                res = true;
            }

            return res;
        }

        /// <summary>
        /// Check if a trimmed string is null or empty 
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static bool isNullOrEmpty(String aString)
        {
            bool res = false;

            if (aString == null || aString == String.Empty || aString.Trim() == string.Empty)
            {
                res = true;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<int> ToListOfInt(String source)
        {
            List<int> res = new List<int>();

            if (source != null && source.Trim() != string.Empty)
            {
                String[] lista = source.Split(StringUtils.SPLITTER);
                if (lista.Length > 0)
                {
                    foreach (String valore in lista)
                    {
                        res.Add(Convert.ToInt32(valore));
                    }
                }
            }

            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<double> ToListOfDouble(String source)
        {
            List<double> res = new List<double>();

            if (source != null && source.Trim() != string.Empty)
            {
                String[] lista = source.Split(StringUtils.SPLITTER);
                if (lista.Length > 0)
                {
                    foreach (String valore in lista)
                    {
                        res.Add(Convert.ToDouble(valore));
                    }
                }
            }

            return res;
        }

        public static List<String> ToListOfString(String source)
        {
            List<String> res = new List<String>();

            if (source != null && source.Trim() != string.Empty)
            {
                String[] lista = source.Split(StringUtils.SPLITTER);
                if (lista.Length > 0)
                {
                    res = new List<string>(lista);
                    //foreach (String valore in lista)
                    //{
                    //    res.Add(valore);
                    //}
                }
            }

            return res;
        }

        public static String Concatena<T>(List<T> unaLista)
        {
            StringBuilder res = new StringBuilder();

            if (unaLista != null && unaLista.Count >= 0)
            {
                foreach (T obj in unaLista)
                {
                    if (res.ToString() != String.Empty)
                    {
                        // res += StringUtils.SPLITTER;
                        res.Append(StringUtils.SPLITTER);
                    }
                    //res += obj.ToString();
                    res.Append(obj.ToString());
                }
            }

            return res.ToString();
        }

        public static String TrimDoppiApici(String aString)
        {
            String res = aString;
            if (aString.StartsWith("\""))
            {
                res = res.Remove(0, 1);
            }

            if (aString.EndsWith("\""))
            {
                res = res.Remove(res.Length - 1, 1);
            }
            return res;
        }

        public static string FirstChar(string aString)
        {
            String res = null;

            if (StringUtils.isNotEmpty(aString))
            {
                res = aString.Substring(0, 1);
            }

            return res;
        }
    }
}
