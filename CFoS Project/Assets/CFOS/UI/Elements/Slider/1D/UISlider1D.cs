using CFoS.Experimentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UISlider1D : UIElement
    {

        [Header("Handle")]
        public UISliderHandle Handle;
       
        [Header("Track")]
        public Shapes.Line Track;
        public Data.ColorVariable TrackColor;

        [Header("Range")]
        public TMPro.TextMeshPro RangeMinText;
        public TMPro.TextMeshPro RangeMaxText;
        public Data.ColorVariable rangeTextColor;

        [Header("Text")]
        public TMPro.TextMeshPro Text;
        public Data.ColorVariable TextColor;

        [Header("Variables")]
        public string StringFormat = "0.0";

        public float Value = 0.0f;
        protected float oldValue;
        protected float defaultValue;

        public float Min = 0.0f;
        public float Max = 1.0f;

        [Space(10)]
        public UnityEvent ValueChangedEvent;


        protected Vector3 selectOffset = Vector3.zero;

        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            Track.Color = TrackColor.Value;

            RangeMinText.color = rangeTextColor.Value;
            RangeMaxText.color = rangeTextColor.Value;

            Text.color = TextColor.Value;

            // position range text
            var pos = RangeMaxText.transform.localPosition;
            pos.x = Track.End.x;
            RangeMaxText.transform.localPosition = pos;

            // manually update slider
            ForceValueUpdate();
        }

        protected override void Awake()
        {
            base.Awake();

            oldValue = Value;
            defaultValue = Value;
        }


        // Slider Functions
        public override void Select(bool val)
        {
            base.Select(val);

            if (val)
            {
                // save local offset from controller to handle
                var handlePos = Handle.transform.position;
                selectOffset = controller.transform.InverseTransformPoint(handlePos);

                // Register selection as metric
                if (controller.gameObject.name == "LeftHand Controller")
                    MetricManager.Instance.RegisterTaskMetric("SliderSelectCountL", 1.0f);

                if (controller.gameObject.name == "RightHand Controller")
                    MetricManager.Instance.RegisterTaskMetric("SliderSelectCountR", 1.0f);
            }
        }

        public override void Enable(bool val)
        {
            base.Enable(val);

            if (!val)
            {
                var col = TrackColor.Value; col.a = 0.3f;
                Track.Color = col;

                col = TextColor.Value; col.a = 0.3f;
                Text.color = col;
            }
            else
            {   
                Track.Color = TrackColor.Value;
                Text.color = TextColor.Value;
            }
            Handle.Enable(val);
        }

        public override void Hover(bool val)
        {
            base.Hover(val);

            Handle.Hover(val);
        }


        // Get Closest Value in slider from a point in space
        public virtual float WorldCoordsToValue(Vector3 coords)
        {
            Vector3 localPos = transform.InverseTransformPoint(coords);
            return HandleToValue(localPos.x);
        }

        // Get handle world coords from Value
        public virtual Vector3 ValueToWorldCoords(float value)
        {
            var x = ValueToHandle(value);
            Vector3 pos = new Vector3(x,0,0);
            return transform.TransformPoint(pos);
        }


        // converts handle position to slider value
        protected virtual float HandleToValue(float handlePos)
        {
            float value = ExtensionMethods.Remap(handlePos, 0.0f, Track.End.x, Min, Max);
            return Mathf.Clamp(value, Min, Max);
        }

        // converts slider value to handle position
        protected virtual float ValueToHandle(float value)
        {
            float handlePos = ExtensionMethods.Remap(value, Min, Max, 0.0f, Track.End.x);
            return Mathf.Clamp(handlePos, 0.0f, Track.End.x);
        }

        // position handle based on x val
        protected virtual void PositionSlider(float posx)
        {
            posx = Mathf.Clamp(posx, 0.0f, Track.End.x);

            var pos = Handle.transform.localPosition;
            pos.x = posx;
            Handle.transform.localPosition = pos;
        }


        protected void VisualUpdate()
        {
            if (disabled)
            {
                return;
            }

            // Visual Update
            Handle.HandleText.text = Value.ToString(StringFormat);
            RangeMinText.text = Min.ToString(StringFormat);
            RangeMaxText.text = Max.ToString(StringFormat);
        }

        public virtual void ForceValueUpdate()
        {
            oldValue = Value;
            ValueChangedEvent.Invoke();
        }

        public virtual void ResetValue()
        {
            Value = defaultValue;
            ForceValueUpdate();
        }

        protected virtual void Update()
        {
            VisualUpdate();

            // If selecting, handle positions changes value
            if (selected)
            {
                // apply selectionOffset to controllerPosition and get x value of local coords
                var controllerPos = controller.transform.TransformPoint(selectOffset);
                var handlePos = transform.InverseTransformPoint(controllerPos);
                
                PositionSlider(handlePos.x);
                Value = HandleToValue(handlePos.x);

                if(!Mathf.Approximately(Value, oldValue))
                {
                    ForceValueUpdate();

                    // Register Value change as metric
                    MetricManager.Instance.RegisterTaskMetric("TimeSliding", Time.deltaTime);
                }
            }
            // If not selecting, value changes handle position
            else
            {
                PositionSlider(ValueToHandle(Value));
            }
        }

    }
}