using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    /// <summary>
    /// Un set di funzioni di conversione da interfaccia grafica a oggetti tipizzati
    /// </summary>
    public class UIUtils
    {
        //public static double ToDouble(String input)
        //{
        //    double res = 0d;
        //    try
        //    {
        //        res = Convert.ToDouble(input);
        //    }
        //    catch
        //    {
        //    }
        //    return res;
        //}

        public static double ToDouble(Object input)
        {
            double res = 0d;
            try
            {
                res = Convert.ToDouble(input.ToString().Replace(".",","));
            }
            catch
            {
            }
            return res;
        }

        public static decimal ToDecimal(Object input)
        {
            decimal res = 0;
            try
            {
                res = Convert.ToDecimal(input.ToString().Replace(".",","));
            }
            catch
            {
            }
            return res;            
        }

        //public static int ToInt(String input)
        //{
        //    int res = 0;
        //    try
        //    {
        //        Convert.ToInt32(input);
        //    }
        //    catch
        //    {
        //    }
        //    return res;
        //}

        public static int ToInt(Object input)
        {
            int res = 0;
            try
            {
                res = Convert.ToInt32(input);
            }
            catch
            {
            }
            return res;
        }
    }
}
