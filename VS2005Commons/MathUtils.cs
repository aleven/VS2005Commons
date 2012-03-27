using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    public class MathUtils
    {
        public static int InteroPiuPiccolo(double valore)
        {
            return Convert.ToInt32(Math.Floor(valore));
        }

        public static int InteroPiuGrande(double valore)
        {
            return Convert.ToInt32(Math.Ceiling(valore));
        }

        public static int Maggiore(int numero1, int numero2)
        {
            return (numero1 >
                                                       numero2)
                                                          ? numero1
                                                          : numero2; ;
        }

        public static int Minore(int numero1, int numero2)
        {
            return (numero1 <
                                                       numero2)
                                                          ? numero1
                                                          : numero2; ;
        }

        public static int DifferenzaAssoluta(int numero1, int numero2)
        {
            return Math.Abs(numero1 - numero2);
        }
    }
}
