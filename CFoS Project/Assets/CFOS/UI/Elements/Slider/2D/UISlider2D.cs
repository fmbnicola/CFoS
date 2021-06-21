using CFoS.Experimentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UISlider2D : UIElement
    {
        public AudioClip GrabAudio;
        public AudioClip ReleaseAudio;

        [Header("Handle")]
        public UISliderHandle Handle;
       
        [Header("Track")]
        public Shapes.Rectangle Track;
        public Data.ColorVariable TrackColor;

        [Header("Ranges")]
        public TMPro.TextMeshPro RangeXMinText;
        public TMPro.TextMeshPro RangeXMaxText;
        public TMPro.TextMeshPro RangeYMinText;
        public TMPro.TextMeshPro RangeYMaxText;
        public Data.ColorVariable rangeTextColor;

        [Header("Text")]
        public TMPro.TextMeshPro XText;
        public TMPro.TextMeshPro YText;
        public Data.ColorVariable TextColor;

        [Header("Variables")]
        public string StringFormat = "0.00";
        public Vector2 Value = new Vector2(0,0);
        protected Vector2 oldValue;
        public float XMin = 0.0f;
        public float XMax = 1.0f;
        public float YMin = 0.0f;
        public float YMax = 1.0f;

        [Space(10)]
        public UnityEvent ValueChangedEvent;

        protected Vector3 selectOffset = Vector3.zero;

        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            Track.Color = TrackColor.Value;

            RangeXMinText.color = rangeTextColor.Value;
            RangeXMaxText.color = rangeTextColor.Value;
            RangeYMinText.color = rangeTextColor.Value;
            RangeYMaxText.color = rangeTextColor.Value;

            XText.color = TextColor.Value;
            YText.color = TextColor.Value;

            // position range text
            var pos = RangeXMaxText.transform.localPosition;
            pos.x = Track.Width;
            RangeXMaxText.transform.localPosition = pos;

            pos = RangeYMaxText.transform.localPosition;
            pos.y = Track.Height;
            RangeYMaxText.transform.localPosition = pos;

            // manually update slider
            ForceValueUpdate();
        }

        protected override void Awake()
        {
            base.Awake();

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

                // Register selection as metric
                MetricManager.Instance.RegisterTaskMetric("SelectCount", 1.0f);

                UIManager.Instance.PlaySound(GrabAudio);
            }
            else
            {
                UIManager.Instance.PlaySound(ReleaseAudio);
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
                XText.color = col;
                YText.color = col;
            }
            else
            {   
                Track.Color = TrackColor.Value;
                XText.color = TextColor.Value;
                YText.color = TextColor.Value;
            }
            Handle.Enable(val);
        }

        public override void Hover(bool val)
        {
            base.Hover(val);

            Handle.Hover(val);
        }


        // Get Closest Value in slider from a point in space
        public virtual Vector2 WorldCoordsToValue(Vector3 coords)
        {
            Vector3 localPos = transform.InverseTransformPoint(coords);
            return HandleToValue(localPos.x, localPos.y);
        }

        // Get handle world coords from Value
        public virtual Vector3 ValueToWorldCoords(Vector2 value)
        {
            var xy = ValueToHandle(value);
            Vector3 pos = new Vector3(xy.x, xy.y, 0);
            return transform.TransformPoint(pos);
        }

        // converts handle position to slider value
        protected virtual Vector2 HandleToValue(float handleX, float handleY)
        {
            float xValue = ExtensionMethods.Remap(handleX, 0.0f, Track.Width, XMin, XMax);
            float yValue = ExtensionMethods.Remap(handleY, 0.0f, Track.Height, YMin, YMax);
            xValue = Mathf.Clamp(xValue, XMin, XMax);
            yValue = Mathf.Clamp(yValue, YMin, YMax);

            return new Vector2(xValue, yValue);
        }

        // converts slider value to handle position
        protected virtual Vector2 ValueToHandle(Vector2 value)
        {
            float handleX = ExtensionMethods.Remap(value.x, XMin, XMax, 0.0f, Track.Width);
            float handleY = ExtensionMethods.Remap(value.y, YMin, YMax, 0.0f, Track.Height);
            handleX = Mathf.Clamp(handleX, 0.0f, Track.Width);
            handleY = Mathf.Clamp(handleY, 0.0f, Track.Height);

            return new Vector2(handleX, handleY);
        }

        // position handle based on (x,y) val
        protected virtual void PositionSlider(float posx, float posy)
        {
            posx = Mathf.Clamp(posx, 0.0f, Track.Width);
            posy = Mathf.Clamp(posy, 0.0f, Track.Height);

            var pos = Handle.transform.localPosition;
            pos.x = posx;
            pos.y = posy;
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
            RangeXMinText.text = XMin.ToString(StringFormat);
            RangeXMaxText.text = XMax.ToString(StringFormat);
            RangeYMinText.text = YMin.ToString(StringFormat);
            RangeYMaxText.text = YMax.ToString(StringFormat);
        }

        public virtual void ForceValueUpdate()
        {
            oldValue = Value;
            ValueChangedEvent.Invoke();
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
                
                PositionSlider(handlePos.x, handlePos.y);
                Value = HandleToValue(handlePos.x, handlePos.y);

                if(Value != oldValue)
                {
                    ForceValueUpdate();

                    // Register Value change as metric
                    MetricManager.Instance.RegisterTaskMetric("SlideTime", Time.deltaTime);
                }
            }
            // If not selecting, value changes handle position
            else
            {
                var pos = ValueToHandle(Value);
                PositionSlider(pos.x, pos.y);
            }
        }
    }
}