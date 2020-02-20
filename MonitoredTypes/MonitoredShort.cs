using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored short class that can be used such that whenever the short value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredShort
    {
        private short value;

        /// <summary>
        /// Creates a monitored short.
        /// </summary>
        /// <param name="val">the initial value of the short.</param>
        public MonitoredShort(short val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredShort()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredShort> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored short, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new short value. </param>
        public void SetValue(short val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored short.
        /// </summary>
        /// <returns>the value of the monitored short.</returns>
        public short GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the short such that the function will be called whenever the short is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this short as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredShort> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredShort> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the short.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs short.Equals(object obj) on the base short value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns short.GetHashCode(), where the short is the base short value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static short operator +(MonitoredShort f1)
        {
            return f1.value;
        }

        public static int operator -(MonitoredShort f1)
        {
            return -f1.value;
        }

        public static MonitoredShort operator ++(MonitoredShort f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredShort operator --(MonitoredShort f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static int operator +(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value + f2.value;
        }

        public static int operator -(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value - f2.value;
        }

        public static int operator *(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value * f2.value;
        }

        public static int operator /(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value / f2.value;
        }

        public static int operator %(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static int operator &(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value & f2.value;
        }

        public static int operator |(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value | f2.value;
        }

        public static int operator ^(MonitoredShort f1, MonitoredShort f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }

}
