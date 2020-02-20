using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.MonitoredTypes
{
    /// <summary>
    /// A monitored object class that can be used such that whenever the object value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredObject
    {
        private object value;

        /// <summary>
        /// Creates a monitored object.
        /// </summary>
        /// <param name="val">the initial value of the object.</param>
        public MonitoredObject(object val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredObject()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredObject> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored object, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new object value. </param>
        public void SetValue(object val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored object.
        /// </summary>
        /// <returns>the value of the monitored object.</returns>
        public object GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the object such that the function will be called whenever the object is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this object as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredObject> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredObject> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the object value of the object.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs object.Equals(object obj) on the base object value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns object.GetHashCode(), where the object is the base object value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static object operator +(MonitoredObject f1)
        {
            return f1.value;
        }


        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredObject f1, MonitoredObject f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredObject f1, MonitoredObject f2)
        {
            return f1.value != f2.value;
        }

        #endregion

        #endregion

    }
}
