using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    public class DateUtils
    {
        public static String FORMAT_IT = "dd/MM/yyyy";
        public static String FORMAT_IT_MINUS = "dd-MM-yyyy";
        public static String FORMAT_ISO_DATE = "yyyy-MM-dd";
        public static String FORMAT_ISO = "s";
        public static String FORMAT_ISO_DATE_COMPACT = "yyyyMMdd";
        // public static String FORMAT_ISO_DATE_AND_HOURS = "yyyy-MM-dd HH:mm:ss";

        public static DateTime getYearStart(int year)
        {
            return new DateTime(year, 1, 1);
        }

        public static DateTime getYearEnd(int year)
        {
            return new DateTime(year, 12, 31);
        }

    }
}
