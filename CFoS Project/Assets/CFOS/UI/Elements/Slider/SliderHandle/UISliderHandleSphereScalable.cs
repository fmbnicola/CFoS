using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
{

    public class UISliderHandleSphereScalable : UISliderHandleSphere
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
                
                if(!Mathf.Approximately(delta, 0.0f)) SizeChangedEvent.Invoke();

                Size = Mathf.Clamp(Size + delta * SizeChangeRate, MinSize, MaxSize);

                Handle.transform.localScale = Vector3.one * Size;

                //HandleOutline.Height = Size;
                //HandleOutline.Width = Size;
            }
        }

    }
}
