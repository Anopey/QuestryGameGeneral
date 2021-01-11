using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.Calculations
{
    public static class VariableCalculations
    {

        #region Bits

        /// <summary>
        /// Gets the number of minimum bits required to represent this integer. Of course, all integers take the same space but a lower integer can be represented by
        /// less bits.
        /// </summary>
        /// <param name="number"> the integer </param>
        /// <returns> the bits required to represent said integer. if negative, will return 32 as negative values require the flag. input the absolute value
        /// if you wish the minimum bits if the number was not negative to be returned. </returns>
        public static int GetIntMinimumBits(int number)
        {
            if (number < 0) return 32;
            for (int i = 31; i >= 0; i--)
            {
                if (number % QMath.Pow(2, i) != number) return i + 1; //TODO Left here.
            }
            return 1;
        }

        #endregion



    }
}
