using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.Converters
{
    public static class ToBinary
    {


        /// <summary>
        /// Converts an integer into its binary form.
        /// </summary>
        /// <param name="number"> the number to be converted. </param>
        /// <returns> The LSB is represented by index 0. True is for 1 and false is for 0. </returns>
        public static bool[] IntToBinary(int number)
        {
            List<bool> returned = new List<bool>();
            if (number < 0)
            {
                returned[31] = true;
                number = -number;
            }

            int i = 0;
            while (number > 0)
            {
                if ((number & 1) == 1)
                    returned[i] = true;
                else
                    returned[i] = false;
                number = number >> 1;
                i++;
            }

            return returned.ToArray();
        }

    }
}
