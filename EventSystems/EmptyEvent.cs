using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.EventSystems
{

    /// <summary>
    /// A delegate that represents an event that takes no parameters and returns void.
    /// </summary>
    public delegate void EmptyEventType();

    /// <summary>
    /// This is a class that contains an event that neither returns nor accepts any values.
    /// </summary>
    public class EmptyEvent
    {

        /// <summary>
        /// the event that is called.
        /// </summary>
        protected EmptyEventType calledEvent;

        /// <summary>
        /// Executes all the functions subscribed to the event.
        /// </summary>
        public void Dispatch()
        {
            calledEvent?.Invoke();
        }

        /// <summary>
        /// Clears all the subscribed functions.
        /// </summary>
        public void Clear()
        {
            calledEvent = null;
        }

        /// <summary>
        /// subscribes subscriber to the event called.
        /// </summary>
        /// <param name="called"></param>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public static EmptyEvent operator +(EmptyEvent called, EmptyEventType subscriber)
        {
            called.calledEvent += subscriber;
            return called;
        }

        /// <summary>
        /// unsubscribes subscriber from the event called.
        /// </summary>
        /// <param name="called"></param>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public static EmptyEvent operator -(EmptyEvent called, EmptyEventType subscriber)
        {
            called.calledEvent -= subscriber;
            return called;
        }


    }
}
