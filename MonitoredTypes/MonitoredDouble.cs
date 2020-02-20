using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored double class that can be used such that whenever the double value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredDouble
    {
        private double value;

        /// <summary>
        /// Creates a monitored double.
        /// </summary>
        /// <param name="val">the initial value of the double.</param>
        public MonitoredDouble(double val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredDouble()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredDouble> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored double, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new double value. </param>
        public void SetValue(double val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored double.
        /// </summary>
        /// <returns>the value of the monitored double.</returns>
        public double GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the double such that the function will be called whenever the double is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this double as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredDouble> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredDouble> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the double.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs double.Equals(object obj) on the base double value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns double.GetHashCode(), where the double is the base double value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static double operator +(MonitoredDouble f1)
        {
            return f1.value;
        }

        public static double operator -(MonitoredDouble f1)
        {
            return -f1.value;
        }

        public static MonitoredDouble operator ++(MonitoredDouble f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredDouble operator --(MonitoredDouble f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static double operator +(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value + f2.value;
        }

        public static double operator -(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value - f2.value;
        }

        public static double operator *(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value * f2.value;
        }

        public static double operator /(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value / f2.value;
        }

        public static double operator %(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredDouble f1, MonitoredDouble f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #endregion

    }

}
