using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace QuestryGameGeneral
{
    public static class ReflectionUtilities
    {

        /// <summary>
        /// Returns the most base type in the inheritance tree that this Type inherits from.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Type GetMostPrimitive(Type t)
        {
            Type returned = t;
            if (returned == null)
                return returned;

            while (true)
            {
                Type temp = t.BaseType;
                if (temp == null)
                    break;
                returned = temp;
            }
            return returned;
        }

    }
}
