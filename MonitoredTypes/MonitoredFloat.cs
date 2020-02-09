using System.Collections;
using System.Collections.Generic;
using System;

namespace QuestryGameGeneral.MonitoredTypes
{

    public class MonitoredFloat
    {
        private float value;

        public MonitoredFloat(float val)
        {
            value = val;
        }

        ~MonitoredFloat()
        {
            ValueChanged = null;
        }

        #region Monitoring

        private event Action<MonitoredFloat> ValueChanged;

        public void SetValue(float val)
        {
            value = val;
            onValueChange();
        }

        public float GetValue()
        {
            return value;
        }

        private void onValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        public void SubscribeValueChange(Action<MonitoredFloat> action)
        {
            ValueChanged += action;
        }

        public void UnSubscribeValueChange(Action<MonitoredFloat> action)
        {
            ValueChanged -= action;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return value.ToString();
        }

        #endregion

    }


}
