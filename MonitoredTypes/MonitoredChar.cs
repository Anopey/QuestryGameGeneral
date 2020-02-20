using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored char class that can be used such that whenever the char value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredChar
    {
        private char value;

        /// <summary>
        /// Creates a monitored char.
        /// </summary>
        /// <param name="val">the initial value of the char.</param>
        public MonitoredChar(char val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredChar()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredChar> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored char, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new char value. </param>
        public void SetValue(char val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored char.
        /// </summary>
        /// <returns>the value of the monitored char.</returns>
        public char GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the char such that the function will be called whenever the char is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this char as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredChar> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredChar> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the char.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs char.Equals(object obj) on the base char value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns char.GetHashCode(), where the char is the base char value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static char operator +(MonitoredChar f1)
        {
            return f1.value;
        }


        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredChar f1, MonitoredChar f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredChar f1, MonitoredChar f2)
        {
            return f1.value != f2.value;
        }

        #endregion

        #region Bitwise Operators

        public static int operator &(MonitoredChar f1, MonitoredChar f2)
        {
            return f1.value & f2.value;
        }

        public static int operator |(MonitoredChar f1, MonitoredChar f2)
        {
            return f1.value | f2.value;
        }

        public static int operator ^(MonitoredChar f1, MonitoredChar f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }
}
