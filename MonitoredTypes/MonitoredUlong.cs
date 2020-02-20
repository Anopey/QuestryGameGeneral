using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored ulong class that can be used such that whenever the ulong value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredUlong
    {
        private ulong value;

        /// <summary>
        /// Creates a monitored ulong.
        /// </summary>
        /// <param name="val">the initial value of the ulong.</param>
        public MonitoredUlong(ulong val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredUlong()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredUlong> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored ulong, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new ulong value. </param>
        public void SetValue(ulong val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored ulong.
        /// </summary>
        /// <returns>the value of the monitored ulong.</returns>
        public ulong GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the ulong such that the function will be called whenever the ulong is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this ulong as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredUlong> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredUlong> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the ulong.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs ulong.Equals(object obj) on the base ulong value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns ulong.GetHashCode(), where the ulong is the base ulong value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static ulong operator +(MonitoredUlong f1)
        {
            return f1.value;
        }

        public static MonitoredUlong operator ++(MonitoredUlong f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredUlong operator --(MonitoredUlong f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static ulong operator +(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value + f2.value;
        }

        public static ulong operator -(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value - f2.value;
        }

        public static ulong operator *(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value * f2.value;
        }

        public static ulong operator /(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value / f2.value;
        }

        public static ulong operator %(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static ulong operator &(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value & f2.value;
        }

        public static ulong operator |(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value | f2.value;
        }

        public static ulong operator ^(MonitoredUlong f1, MonitoredUlong f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }

}
