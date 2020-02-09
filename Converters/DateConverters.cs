using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.Converters
{
    public static class DateConverters
    {
        /// <summary>
        /// Converts the american date system to the one understood by regular human beings.
        /// </summary>
        /// <param name="s"> a date of the format "month/day/..." in numerical form</param>
        /// <returns>the same date of the format "day/month/..."</returns>
        public static string ConvertAmericanToSensible(string s)
        {
            string month = "", day = "";
            int i = 0;
            int rest_index = 0;
            for (i = 0; i < s.Length; i++)
            {
                if (s[i] != '/')
                {
                    month += s[i];
                    continue;
                }
                i++;
                break;
            }
            for (i = i; i < s.Length; i++)
            {
                if (s[i] != '/')
                {
                    day += s[i];
                    continue;
                }
                rest_index = i;
                break;
            }
            return day + '/' + month + s.Substring(rest_index);
        }
    }
}
