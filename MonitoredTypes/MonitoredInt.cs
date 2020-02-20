using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored int class that can be used such that whenever the int value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredInt
    {
        private int value;

        /// <summary>
        /// Creates a monitored int.
        /// </summary>
        /// <param name="val">the initial value of the int.</param>
        public MonitoredInt(int val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredInt()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredInt> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored int, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new int value. </param>
        public void SetValue(int val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored int.
        /// </summary>
        /// <returns>the value of the monitored int.</returns>
        public int GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the int such that the function will be called whenever the int is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this int as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredInt> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredInt> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the int.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs int.Equals(object obj) on the base int value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns int.GetHashCode(), where the int is the base int value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static int operator +(MonitoredInt f1)
        {
            return f1.value;
        }

        public static int operator -(MonitoredInt f1)
        {
            return -f1.value;
        }

        public static MonitoredInt operator ++(MonitoredInt f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredInt operator --(MonitoredInt f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static int operator +(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value + f2.value;
        }

        public static int operator -(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value - f2.value;
        }

        public static int operator *(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value * f2.value;
        }

        public static int operator /(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value / f2.value;
        }

        public static int operator %(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #region Bitwise Operators

        public static int operator &(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value & f2.value;
        }

        public static int operator |(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value | f2.value;
        }

        public static int operator ^(MonitoredInt f1, MonitoredInt f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }

}
