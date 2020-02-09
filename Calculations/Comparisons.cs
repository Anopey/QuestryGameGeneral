using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.Calculations
{
    public static class Comparisons
    {
        /// <summary>
        /// Returns true if the input floats a or b are within the margin of one another.
        /// </summary>
        /// <param name="a"> first value</param>
        /// <param name="b"> second value</param>
        /// <param name="margin"> the margin by which the two values will be compared.</param>
        /// <returns></returns>
        public static bool WithinMargin(float a, float b, float margin)
        {
            if (a > b)
                return b + margin >= a;
            return a + margin >= b;
        }

    }
}
