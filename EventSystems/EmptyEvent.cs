using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.EventSystems
{

    public delegate void EmptyEventType();

    /// <summary>
    /// This is a class that contains an event that neither returns nor accepts any values.
    /// </summary>
    public class EmptyEvent
    {

        protected EmptyEventType calledEvent;

        public void Dispatch()
        {
            if (calledEvent != null)
            {
                calledEvent();
            }
        }

        public void Clear()
        {
            calledEvent = null;
        }

        public static EmptyEvent operator +(EmptyEvent called, EmptyEventType subscriber)
        {
            called.calledEvent += subscriber;
            return called;
        }

        public static EmptyEvent operator -(EmptyEvent called, EmptyEventType subscriber)
        {
            called.calledEvent -= subscriber;
            return called;
        }


    }
}
