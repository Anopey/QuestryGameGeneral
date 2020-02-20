using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored bool class that can be used such that whenever the bool value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredBool
    {
        private bool value;

        /// <summary>
        /// Creates a monitored bool.
        /// </summary>
        /// <param name="val">the initial value of the bool.</param>
        public MonitoredBool(bool val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredBool()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredBool> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored bool, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new bool value. </param>
        public void SetValue(bool val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored bool.
        /// </summary>
        /// <returns>the value of the monitored bool.</returns>
        public bool GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the bool such that the function will be called whenever the bool is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this bool as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredBool> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredBool> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the bool.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs bool.Equals(object obj) on the base bool value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns bool.GetHashCode(), where the bool is the base bool value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static bool operator +(MonitoredBool f1)
        {
            return f1.value;
        }

        public static bool operator !(MonitoredBool f1)
        {
            return !f1.value;
        }


        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredBool f1, MonitoredBool f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredBool f1, MonitoredBool f2)
        {
            return f1.value != f2.value;
        }

        #endregion

        #region Bitwise Operators

        public static bool operator &(MonitoredBool f1, MonitoredBool f2)
        {
            return f1.value & f2.value;
        }

        public static bool operator |(MonitoredBool f1, MonitoredBool f2)
        {
            return f1.value | f2.value;
        }

        public static bool operator ^(MonitoredBool f1, MonitoredBool f2)
        {
            return f1.value ^ f2.value;
        }

        #endregion

        #endregion

    }
}
