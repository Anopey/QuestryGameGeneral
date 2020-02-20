using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored string class that can be used such that whenever the string value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredString
    {
        private string value;

        /// <summary>
        /// Creates a monitored string.
        /// </summary>
        /// <param name="val">the initial value of the string.</param>
        public MonitoredString(string val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredString()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredString> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored string, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new string value. </param>
        public void SetValue(string val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored string.
        /// </summary>
        /// <returns>the value of the monitored string.</returns>
        public string GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the string such that the function will be called whenever the string is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this string as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredString> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredString> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the string.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs string.Equals(object obj) on the base string value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns string.GetHashCode(), where the string is the base string value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static string operator +(MonitoredString f1)
        {
            return f1.value;
        }


        #endregion

        #region Binary

        public static string operator +(MonitoredString f1, MonitoredString f2)
        {
            return f1.value + f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredString f1, MonitoredString f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredString f1, MonitoredString f2)
        {
            return f1.value != f2.value;
        }

        #endregion

        #endregion

    }
}
