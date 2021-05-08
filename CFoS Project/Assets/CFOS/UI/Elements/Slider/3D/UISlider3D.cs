using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UISlider3D : UIElement
    {
        [Header("Handle")]
        public UISliderHandleSphere Handle;
       
        [Header("Track")]
        public Transform Track;
        
        [Header("Ranges")]
        public TMPro.TextMeshPro RangeXMinText;
        public TMPro.TextMeshPro RangeXMaxText;
        public TMPro.TextMeshPro RangeYMinText;
        public TMPro.TextMeshPro RangeYMaxText;
        public TMPro.TextMeshPro RangeZMinText;
        public TMPro.TextMeshPro RangeZMaxText;
        public Data.ColorVariable rangeTextColor;

        [Header("Text")]
        public TMPro.TextMeshPro XText;
        public TMPro.TextMeshPro YText;
        public TMPro.TextMeshPro ZText;
        public Data.ColorVariable TextColor;

        [Header("Variables")]
        public string StringFormat = "0.00";
        public Vector3 Value = new Vector3(0,0,0);
        protected Vector3 oldValue;
        public float XMin = 0.0f;
        public float XMax = 1.0f;
        public float YMin = 0.0f;
        public float YMax = 1.0f;
        public float ZMin = 0.0f;
        public float ZMax = 1.0f;

        [Space(10)]
        public UnityEvent ValueChangedEvent;

        protected Vector3 selectOffset = Vector3.zero;

        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            RangeXMinText.color = rangeTextColor.Value;
            RangeXMaxText.color = rangeTextColor.Value;
            RangeYMinText.color = rangeTextColor.Value;
            RangeYMaxText.color = rangeTextColor.Value;

            XText.color = TextColor.Value;
            YText.color = TextColor.Value;

            // position range text
            var pos = RangeXMaxText.transform.localPosition;
            pos.x = Track.localScale.x;
            RangeXMaxText.transform.localPosition = pos;

            pos = RangeYMaxText.transform.localPosition;
            pos.y = Track.localScale.y;
            RangeYMaxText.transform.localPosition = pos;

            pos = RangeZMaxText.transform.localPosition;
            pos.z = Track.localScale.z;
            RangeZMaxText.transform.localPosition = pos;

            // manually update slider
            ForceValueUpdate();
        }

        protected virtual void Awake()
        {
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
                var col = TextColor.Value; col.a = 0.3f;
                XText.color = col;
                YText.color = col;
                ZText.color = col;
            }
            else
            {   
                XText.color = TextColor.Value;
                YText.color = TextColor.Value;
                ZText.color = TextColor.Value;
            }
            Handle.Enable(val);
        }

        public override void Hover(bool val)
        {
            base.Hover(val);

            Handle.Hover(val);
        }


        // Get Closest Value in slider from a point in space
        public virtual Vector3 WorldCoordsToValue(Vector3 coords)
        {
            Vector3 localPos = transform.InverseTransformPoint(coords);
            return HandleToValue(localPos.x, localPos.y, localPos.z);
        }

        // Get handle world coords from Value
        public virtual Vector3 ValueToWorldCoords(Vector3 value)
        {
            var xyz = ValueToHandle(value);
            Vector3 pos = new Vector3(xyz.x, xyz.y, xyz.z);
            return transform.TransformPoint(pos);
        }

        // converts handle position to slider value
        protected virtual Vector3 HandleToValue(float handleX, float handleY, float handleZ)
        {
            float xValue = ExtensionMethods.Remap(handleX, 0.0f, Track.localScale.x, XMin, XMax);
            float yValue = ExtensionMethods.Remap(handleY, 0.0f, Track.localScale.y, YMin, YMax);
            float zValue = ExtensionMethods.Remap(handleZ, 0.0f, Track.localScale.z, ZMin, ZMax);
            xValue = Mathf.Clamp(xValue, XMin, XMax);
            yValue = Mathf.Clamp(yValue, YMin, YMax);
            zValue = Mathf.Clamp(zValue, ZMin, ZMax);

            return new Vector3(xValue, yValue, zValue);
        }

        // converts slider value to handle position
        protected virtual Vector3 ValueToHandle(Vector3 value)
        {
            float handleX = ExtensionMethods.Remap(value.x, XMin, XMax, 0.0f, Track.localScale.x);
            float handleY = ExtensionMethods.Remap(value.y, YMin, YMax, 0.0f, Track.localScale.y);
            float handleZ = ExtensionMethods.Remap(value.z, ZMin, ZMax, 0.0f, Track.localScale.z);
            handleX = Mathf.Clamp(handleX, 0.0f, Track.localScale.x);
            handleY = Mathf.Clamp(handleY, 0.0f, Track.localScale.y);
            handleZ = Mathf.Clamp(handleZ, 0.0f, Track.localScale.z);

            return new Vector3(handleX, handleY, handleZ);
        }

        // position handle based on (x,y) val
        protected virtual void PositionSlider(float posx, float posy, float posz)
        {
            posx = Mathf.Clamp(posx, 0.0f, Track.localScale.x);
            posy = Mathf.Clamp(posy, 0.0f, Track.localScale.y);
            posz = Mathf.Clamp(posz, 0.0f, Track.localScale.z);

            var pos = Handle.transform.localPosition;
            pos.x = posx;
            pos.y = posy;
            pos.z = posz;
            Handle.transform.localPosition = pos;
        }


        protected void VisualUpdate()
        {
            var foward = transform.position - Camera.main.transform.position;
            XText.transform.rotation = Quaternion.LookRotation(foward, Vector3.up);
            YText.transform.rotation = Quaternion.LookRotation(foward, Vector3.up);
            ZText.transform.rotation = Quaternion.LookRotation(foward, Vector3.up);

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
            RangeZMinText.text = ZMin.ToString(StringFormat);
            RangeZMaxText.text = ZMax.ToString(StringFormat);
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
                
                PositionSlider(handlePos.x, handlePos.y, handlePos.z);
                Value = HandleToValue(handlePos.x, handlePos.y, handlePos.z);

                if(Value != oldValue)
                {
                    ForceValueUpdate();
                }
            }
            // If not selecting, value changes handle position
            else
            {
                var pos = ValueToHandle(Value);
                PositionSlider(pos.x, pos.y, pos.z);
            }
        }
    }
}