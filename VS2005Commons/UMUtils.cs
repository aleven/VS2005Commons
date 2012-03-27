using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    public class UMUtils
    {

        public static String convertMicronToMm(int micron)
        {
            // Decimal result = micron / 1000;
            return Math.Round(micron / 1000d, 3).ToString(); // .ToString("N3");
        }

        public static int convertMmToMicron(decimal mm)
        {
            int result = (int)(mm * 1000);
            return result; //  Math.Round(mm, 0);
        }
    }
}
