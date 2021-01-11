using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral
{
    public static class QMath
    {

        public static List<List<T>> GetAllPermutations<T>(List<T> list) //TODO: Can do a much more performant implementation. 
        {
            List<List<T>> returned = new List<List<T>>();
            if (list.Count < 2)
            {
                returned.Add(list);
                return returned;
            }

            for (int i = 0; i < list.Count; i++)
            {
                List<T> elem = new List<T>(list);
                T removed = elem[i];
                elem[i] = elem[0];
                elem.RemoveAt(0);
                var perm = GetAllPermutations<T>(elem);
                foreach(var permElem in perm)
                {
                    permElem.Insert(0, removed);
                    returned.Add(permElem);
                }
            }

            return returned;
        }

    }
}
