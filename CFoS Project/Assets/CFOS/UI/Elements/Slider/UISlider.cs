using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UISlider : UIElement
    {
        [Header("Handle")]
        public Shapes.ShapeRenderer Handle;
        public Data.ColorVariable HandleNormalColor;
        public Data.ColorVariable HandleHoverColor;
        public Data.ColorVariable HandleSelectColor;

        [Space(10)]
        public Shapes.ShapeRenderer HandleOutline;
        public Data.ColorVariable HandleOutlineColor;

        [Space(10)]
        public TMPro.TextMeshPro HandleText;
        public Data.ColorVariable HandleTextColor;

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
        public float Min = 0.0f;
        public float Max = 1.0f;
        protected float oldValue;

        [Space(10)]
        public UnityEvent ValueChangedEvent;


        protected Vector3 selectOffset = Vector3.zero;

        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            // Set Colors
            Handle.Color = HandleNormalColor.Value;
            HandleOutline.Color = HandleOutlineColor.Value;
            HandleText.color = HandleTextColor.Value;

            Track.Color = TrackColor.Value;

            RangeMinText.color = rangeTextColor.Value;
            RangeMaxText.color = rangeTextColor.Value;

            Text.color = TextColor.Value;

            // position range text
            var pos = RangeMaxText.transform.localPosition;
            pos.x = 0.0f;
            RangeMaxText.transform.localPosition = pos;

            pos = RangeMaxText.transform.localPosition;
            pos.x = Track.End.x;
            RangeMaxText.transform.localPosition = pos; 
        }

        protected virtual void Awake()
        {
            HandleOutline.gameObject.SetActive(false);

            oldValue = Value;
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
            }
        }

        public override void Enable(bool val)
        {
            base.Enable(val);

            if (!val)
            {
                var col = HandleTextColor.Value; col.a = 0.3f;
                HandleText.color = col;

                col = TrackColor.Value; col.a = 0.3f;
                Track.Color = col;

                col = TextColor.Value; col.a = 0.3f;
                Text.color = col;
            }
            else
            {
                HandleText.color = HandleTextColor.Value;
                Track.Color = TrackColor.Value;
                Text.color = TextColor.Value;
            }
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
                var col = HandleNormalColor.Value; col.a = 0.3f;
                Handle.Color = col;
                HandleOutline.gameObject.SetActive(false);

                return;
            }

            // Visual Update
            Handle.Color = selected ? HandleSelectColor.Value : hovered ? HandleHoverColor.Value : HandleNormalColor.Value;
            HandleOutline.gameObject.SetActive(hovered || selected);

            HandleText.text = Value.ToString(StringFormat);
            RangeMinText.text = Min.ToString(StringFormat);
            RangeMaxText.text = Max.ToString(StringFormat);
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
                    ValueChangedEvent.Invoke();
                }
                oldValue = Value;
            }
            // If not selecting, value changes handle position
            else
            {
                PositionSlider(ValueToHandle(Value));
            }
        }


    }
}