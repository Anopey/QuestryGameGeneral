using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace QuestryGameGeneral
{
    public static class ReflectionUtilities
    {


        public static Type GetMostPrimitive(Type t)
        {
            Type returned = t;
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
