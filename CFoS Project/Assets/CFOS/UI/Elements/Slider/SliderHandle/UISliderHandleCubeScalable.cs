using CFoS.Experimentation;
using CFoS.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
{

    public class UISliderHandleCubeScalable : UISliderHandleCube
    {
        public float MaxSize = 0.1f;
        public float MinSize = 0.01f;
        public float SizeChangeRate = 0.001f;

        public UnityEvent SizeChangedEvent;

        [ExecuteAlways]
        protected override void OnValidate()
        {
            base.OnValidate();

            SizeChangedEvent.Invoke();
        }

        protected override void Update()
        {
            base.Update();

            if (selected)
            {
                var input = controller.translateAnchorAction.action.ReadValue<Vector2>();
                var delta = input.y;

                if (!Mathf.Approximately(delta, 0.0f))
                {
                    SizeChangedEvent.Invoke();

                    // Register Value change as metric
                    MetricManager.Instance.RegisterTaskMetric("TimeRescaling", Time.deltaTime);
                }

                Size = Mathf.Clamp(Size + delta * SizeChangeRate, MinSize, MaxSize);

                Handle.transform.localScale = Vector3.one * Size;
            }
        }

        // Controller hinting
        protected override void RegisterSelection(bool val)
        {
            if (controller != null)
            {
                var hints = controller.GetComponentInChildren<ControllerHints>();
                hints.JoystickHighlight(val);
            }
        }
    }
}
