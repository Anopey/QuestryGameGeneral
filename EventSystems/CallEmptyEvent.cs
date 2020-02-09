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
    class CallEmptyEvent
    {

        protected EmptyEventType calledEvent;

        public void Dispatch()
        {
            if (calledEvent != null)
            {
                calledEvent();
            }
        }

        public static CallEmptyEvent operator +(CallEmptyEvent called, EmptyEventType subscriber)
        {
            called.calledEvent += subscriber;
            return called;
        }

        public static CallEmptyEvent operator -(CallEmptyEvent called, EmptyEventType subscriber)
        {
            called.calledEvent -= subscriber;
            return called;
        }

    }
}
