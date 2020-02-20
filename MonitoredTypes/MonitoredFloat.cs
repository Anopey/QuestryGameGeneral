using System.Collections;
using System.Collections.Generic;
using System;

namespace QuestryGameGeneral.MonitoredTypes
{

    /// <summary>
    /// A monitored float class that can be used such that whenever the float value changes all the subscribers will be notified.
    /// </summary>
    public class MonitoredFloat
    {
        private float value;

        /// <summary>
        /// Creates a monitored float.
        /// </summary>
        /// <param name="val">the initial value of the float.</param>
        public MonitoredFloat(float val)
        {
            value = val;
        }

        /// <summary>
        /// Upon destruction, nullifies all 
        /// </summary>
        ~MonitoredFloat()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredFloat> ValueChanged;

        /// <summary>
        /// Sets the value of the monitored float, notifying subscribed functions if the value is not the same.
        /// </summary>
        /// <param name="val"> the new float value. </param>
        public void SetValue(float val)
        {
            if (value == val)
                return;
            value = val;
            onValueChange();
        }

        /// <summary>
        /// Gets the value of the monitored float.
        /// </summary>
        /// <returns>the value of the monitored float.</returns>
        public float GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        /// <summary>
        /// Subscribes the given function to the float such that the function will be called whenever the float is changed.
        /// </summary>
        /// <param name="action"> the function to be called that shall accept this float as its parameter. </param>
        public void SubscribeValueChange(Action<MonitoredFloat> action)
        {
            ValueChanged += action;
        }

        /// <summary>
        /// Unsubscribes the input function if it was subscribed in the first place.
        /// </summary>
        /// <param name="action"> the function to be potentially unsubscribed. </param>
        public void UnSubscribeValueChange(Action<MonitoredFloat> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        #region Misc

        /// <summary>
        /// returns the string value of the float.
        /// </summary>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// performs float.Equals(object obj) on the base float value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// returns float.GetHashCode(), where the float is the base float value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion

        #region Unary

        public static float operator +(MonitoredFloat f1)
        {
            return f1.value;
        }

        public static float operator -(MonitoredFloat f1)
        {
            return -f1.value;
        }

        public static MonitoredFloat operator ++(MonitoredFloat f1)
        {
            f1.value++;
            return f1;
        }

        public static MonitoredFloat operator --(MonitoredFloat f1)
        {
            f1.value--;
            return f1;
        }

        #endregion

        #region Binary

        public static float operator +(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value + f2.value;
        }

        public static float operator -(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value - f2.value;
        }

        public static float operator *(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value * f2.value;
        }

        public static float operator /(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value / f2.value;
        }

        public static float operator %(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value % f2.value;
        }

        #endregion

        #region Comparisons

        public static bool operator ==(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value == f2.value;
        }

        public static bool operator !=(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value != f2.value;
        }

        public static bool operator >(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value > f2.value;
        }

        public static bool operator <(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value < f2.value;
        }

        public static bool operator >=(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value >= f2.value;
        }

        public static bool operator <=(MonitoredFloat f1, MonitoredFloat f2)
        {
            return f1.value >= f2.value;
        }


        #endregion

        #endregion

    }


}
