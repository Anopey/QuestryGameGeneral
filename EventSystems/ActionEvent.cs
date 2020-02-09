using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.EventSystems
{

    /// <summary>
    /// A class that contains an event that accepts one attribute
    /// </summary>
    public class ActionEvent<T>
    {
        protected Action<T> calledEvent;

        public void Dispatch(T arg)
        {
            if (calledEvent != null)
            {
                calledEvent(arg);
            }
        }

        public void Clear()
        {
            calledEvent = null;
        }

        public static ActionEvent<T> operator +(ActionEvent<T> called, Action<T> subscriber)
        {
            called.calledEvent += subscriber;
            return called;
        }

        public static ActionEvent<T> operator -(ActionEvent<T> called, Action<T> subscriber)
        {
            called.calledEvent -= subscriber;
            return called;
        }

    }
}
