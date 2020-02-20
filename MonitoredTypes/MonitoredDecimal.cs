using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored decimal class that can be used such that whenever the decimal value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredDecimal
    {
        private decimal value;

        /// <summary>
        /// Creates a monitored decimal.
        /// </summary>
        /// <param name="val">the initial value of the decimal.</param>
        public MonitoredDecimal(decimal val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredDecimal()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredDecimal> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored decimal, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new decimal value. </param>
        public void SetValue(decimal val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored decimal.
        /// </summary>
        /// <returns>the value of the monitored decimal.</returns>
        public decimal GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the decimal such that the function will be called whenever the decimal is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this decimal as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredDecimal> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredDecimal> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the decimal.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs decimal.Equals(object obj) on the base decimal value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns decimal.GetHashCode(), where the decimal is the base decimal value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static decimal operator +(MonitoredDecimal f1)
        {
            return f1.value;
        }

        public static decimal operator -(MonitoredDecimal f1)
        {
            return -f1.value;
        }

        public static MonitoredDecimal operator ++(MonitoredDecimal f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredDecimal operator --(MonitoredDecimal f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static decimal operator +(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value + f2.value;
        }

        public static decimal operator -(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value - f2.value;
        }

        public static decimal operator *(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value * f2.value;
        }

        public static decimal operator /(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value / f2.value;
        }

        public static decimal operator %(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredDecimal f1, MonitoredDecimal f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #endregion

    }

}
