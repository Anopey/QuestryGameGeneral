using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored byte class that can be used such that whenever the byte value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredByte
    {
        private byte value;

        /// <summary>
        /// Creates a monitored byte.
        /// </summary>
        /// <param name="val">the initial value of the byte.</param>
        public MonitoredByte(byte val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredByte()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredByte> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored byte, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new byte value. </param>
        public void SetValue(byte val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored byte.
        /// </summary>
        /// <returns>the value of the monitored byte.</returns>
        public byte GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the byte such that the function will be called whenever the byte is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this byte as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredByte> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredByte> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the byte.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs byte.Equals(object obj) on the base byte value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns byte.GetHashCode(), where the byte is the base byte value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static byte operator +(MonitoredByte f1)
        {
            return f1.value;
        }

        public static int operator -(MonitoredByte f1)
        {
            return -f1.value;
        }

        public static MonitoredByte operator ++(MonitoredByte f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredByte operator --(MonitoredByte f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static int operator +(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value + f2.value;
        }

        public static int operator -(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value - f2.value;
        }

        public static int operator *(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value * f2.value;
        }

        public static int operator /(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value / f2.value;
        }

        public static int operator %(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static int operator &(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value & f2.value;
        }

        public static int operator |(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value | f2.value;
        }

        public static int operator ^(MonitoredByte f1, MonitoredByte f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }
}
