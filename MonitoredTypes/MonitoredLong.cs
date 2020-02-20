using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored long class that can be used such that whenever the long value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredLong
    {
        private long value;

        /// <summary>
        /// Creates a monitored long.
        /// </summary>
        /// <param name="val">the initial value of the long.</param>
        public MonitoredLong(long val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredLong()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredLong> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored long, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new long value. </param>
        public void SetValue(long val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored long.
        /// </summary>
        /// <returns>the value of the monitored long.</returns>
        public long GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the long such that the function will be called whenever the long is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this long as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredLong> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredLong> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the long.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs long.Equals(object obj) on the base long value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns long.GetHashCode(), where the long is the base long value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static long operator +(MonitoredLong f1)
        {
            return f1.value;
        }

        public static long operator -(MonitoredLong f1)
        {
            return -f1.value;
        }

        public static MonitoredLong operator ++(MonitoredLong f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredLong operator --(MonitoredLong f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static long operator +(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value + f2.value;
        }

        public static long operator -(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value - f2.value;
        }

        public static long operator *(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value * f2.value;
        }

        public static long operator /(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value / f2.value;
        }

        public static long operator %(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static long operator &(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value & f2.value;
        }

        public static long operator |(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value | f2.value;
        }

        public static long operator ^(MonitoredLong f1, MonitoredLong f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }

}
