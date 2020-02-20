using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored uint class that can be used such that whenever the uint value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredUint
    {
        private uint value;

        /// <summary>
        /// Creates a monitored uint.
        /// </summary>
        /// <param name="val">the initial value of the uint.</param>
        public MonitoredUint(uint val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredUint()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredUint> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored uint, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new uint value. </param>
        public void SetValue(uint val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored uint.
        /// </summary>
        /// <returns>the value of the monitored uint.</returns>
        public uint GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the uint such that the function will be called whenever the uint is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this uint as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredUint> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredUint> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the uint.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs uint.Equals(object obj) on the base uint value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns uint.GetHashCode(), where the uint is the base uint value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static uint operator +(MonitoredUint f1)
        {
            return f1.value;
        }

        public static long operator -(MonitoredUint f1)
        {
            return -f1.value;
        }

        public static MonitoredUint operator ++(MonitoredUint f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredUint operator --(MonitoredUint f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static uint operator +(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value + f2.value;
        }

        public static uint operator -(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value - f2.value;
        }

        public static uint operator *(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value * f2.value;
        }

        public static uint operator /(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value / f2.value;
        }

        public static uint operator %(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static uint operator &(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value & f2.value;
        }

        public static uint operator |(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value | f2.value;
        }

        public static uint operator ^(MonitoredUint f1, MonitoredUint f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }

}
