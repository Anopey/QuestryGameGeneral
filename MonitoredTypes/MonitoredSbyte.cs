using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored sbyte class that can be used such that whenever the sbyte value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredSbyte
    {
        private sbyte value;

        /// <summary>
        /// Creates a monitored sbyte.
        /// </summary>
        /// <param name="val">the initial value of the sbyte.</param>
        public MonitoredSbyte(sbyte val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredSbyte()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredSbyte> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored sbyte, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new sbyte value. </param>
        public void SetValue(sbyte val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored sbyte.
        /// </summary>
        /// <returns>the value of the monitored sbyte.</returns>
        public sbyte GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the sbyte such that the function will be called whenever the sbyte is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this sbyte as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredSbyte> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredSbyte> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the sbyte.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs sbyte.Equals(object obj) on the base sbyte value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns sbyte.GetHashCode(), where the sbyte is the base sbyte value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static sbyte operator +(MonitoredSbyte f1)
        {
            return f1.value;
        }

        public static int operator -(MonitoredSbyte f1)
        {
            return -f1.value;
        }

        public static MonitoredSbyte operator ++(MonitoredSbyte f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredSbyte operator --(MonitoredSbyte f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static int operator +(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value + f2.value;
        }

        public static int operator -(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value - f2.value;
        }

        public static int operator *(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value * f2.value;
        }

        public static int operator /(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value / f2.value;
        }

        public static int operator %(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static int operator &(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value & f2.value;
        }

        public static int operator |(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value | f2.value;
        }

        public static int operator ^(MonitoredSbyte f1, MonitoredSbyte f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }
}
