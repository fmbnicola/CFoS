using CFoS.Experimentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UISlider3DDiscrete : UISlider3D
    {

        public float DiscreteInterval = 1.0f;

        protected Vector3 ClosestValue(Vector3 val)
        {
            float a; int b;

            a = val.x / DiscreteInterval;
            b = Mathf.RoundToInt(a);
            float xx = b * DiscreteInterval;

            a = val.y / DiscreteInterval;
            b = Mathf.RoundToInt(a);
            float yy = b * DiscreteInterval;

            a = val.z / DiscreteInterval;
            b = Mathf.RoundToInt(a);
            float zz = b * DiscreteInterval;

            return new Vector3(xx, yy, zz);
        }

        protected override void Update()
        {
            VisualUpdate();

            // If selecting, handle positions changes value
            if (selected)
            {
                var controllerPos = controller.transform.TransformPoint(selectOffset);
                var handlePos = transform.InverseTransformPoint(controllerPos);

                PositionSlider(handlePos.x, handlePos.y, handlePos.z);
                Value = HandleToValue(handlePos.x, handlePos.y, handlePos.z);

                // Discretized!
                Value = ClosestValue(Value);

                var pos = ValueToHandle(Value);
                PositionSlider(pos.x, pos.y, pos.z);
            }
            // If not selecting, value changes handle position
            else
            {
                var pos = ValueToHandle(Value);
                PositionSlider(pos.x, pos.y, pos.z);
            }

            // check for value change (haptics)
            if (Value != oldValue)
            {
                ValueChangedEvent.Invoke();
                if (selected) controller.SendHapticImpulse(0.5f, 0.02f);

                // Register Value change as metric
                MetricManager.Instance.RegisterTaskMetric("TimeSliding", Time.deltaTime);
            }
            oldValue = Value;
        }
    }
}