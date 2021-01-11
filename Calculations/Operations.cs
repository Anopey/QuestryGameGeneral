using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.Calculations
{
    public static class Operations
    {
        /// <summary>
        /// returns a^b for two integers by using Exponentiation by Squaring
        /// </summary>
        /// <param name="a"> the integer base </param>
        /// <param name="b"> the integer exponent </param>
        /// <returns> a^b. 0 if too big to be stored in an integer. </returns>
        public static int IntPower(int a, int b)
        {
            try
            {
                int result = 1;
                int factor = b;

                while (b > 0)
                {
                    if ((b & 1) == 1) result *= factor;
                    factor *= b;
                    b = b >> 1;
                }
                return result;
            }
            catch
            {
                return 0;
            }
        }

        //TODO Switch to latest versions of .NET and implement a BigInt Version.

        /// <summary>
        /// Rounds the given value to the specified number of significant figures/digits.
        /// </summary>
        /// <param name="val"> The value to be rounded </param>
        /// <param name="digits"> the number of significant figures </param>
        /// <returns> the rounded number </returns>
        public static double RoundToSignificantDigits(double val, int digits)
        {
            if (val == 0)
                return 0;

            double scale = QMath.Pow(10, QMath.Floor(QMath.Log10(QMath.Abs(val))) + 1);
            return scale * QMath.Round(val / scale, digits);
        }


    }
}
