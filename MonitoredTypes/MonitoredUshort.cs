using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{

    /// <summary>
    /// A monitored ushort class that can be used such that whenever the ushort value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredUshort
    {
        private ushort value;

        /// <summary>
        /// Creates a monitored ushort.
        /// </summary>
        /// <param name="val">the initial value of the ushort.</param>
        public MonitoredUshort(ushort val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredUshort()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredUshort> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored ushort, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new ushort value. </param>
        public void SetValue(ushort val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored ushort.
        /// </summary>
        /// <returns>the value of the monitored ushort.</returns>
        public ushort GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the ushort such that the function will be called whenever the ushort is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this ushort as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredUshort> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredUshort> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the ushort.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs ushort.Equals(object obj) on the base ushort value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns ushort.GetHashCode(), where the ushort is the base ushort value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static ushort operator +(MonitoredUshort f1)
        {
            return f1.value;
        }

        public static int operator -(MonitoredUshort f1)
        {
            return -f1.value;
        }

        public static MonitoredUshort operator ++(MonitoredUshort f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredUshort operator --(MonitoredUshort f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static int operator +(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value + f2.value;
        }

        public static int operator -(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value - f2.value;
        }

        public static int operator *(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value * f2.value;
        }

        public static int operator /(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value / f2.value;
        }

        public static int operator %(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static int operator &(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value & f2.value;
        }

        public static int operator |(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value | f2.value;
        }

        public static int operator ^(MonitoredUshort f1, MonitoredUshort f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }
}
