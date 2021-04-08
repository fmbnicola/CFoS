using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CFoS.UI
{
    public class UISliderDiscrete : UISlider
    {

        public float DiscreteInterval = 1.0f;

        protected float ClosestValue(float val)
        {
            float a = val / DiscreteInterval;
            int b   = Mathf.RoundToInt(a);

            return b * DiscreteInterval;
        }

        protected override void Update()
        {
            VisualUpdate();

            // If selecting, handle positions changes value
            if (selected)
            {
                var controllerPos = controller.transform.TransformPoint(selectOffset);
                var handlePos = transform.InverseTransformPoint(controllerPos);

                PositionSlider(handlePos.x);
                Value = HandleToValue(handlePos.x);

                // Discretized!
                Value = ClosestValue(Value);
                PositionSlider(ValueToHandle(Value));
            }
            // If not selecting, value changes handle position
            else
            {
                PositionSlider(ValueToHandle(Value));
            }

            // check for value change (haptics)
            if (!Mathf.Approximately(Value, oldValue))
            {
                ValueChangedEvent.Invoke();
                if (selected) controller.SendHapticImpulse(0.5f, 0.02f);
            }
            oldValue = Value;
        }
    }
}
