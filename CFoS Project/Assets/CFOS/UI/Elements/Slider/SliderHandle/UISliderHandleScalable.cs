using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
{

    public class UISliderHandleScalable : UISliderHandle
    {
        public float MaxSize = 0.1f;
        public float MinSize = 0.01f;
        public float SizeChangeRate = 0.001f;

        public UnityEvent SizeChangedEvent;

        protected override void Update()
        {
            base.Update();


            if (selected)
            {
                var input = controller.translateAnchorAction.action.ReadValue<Vector2>();
                var delta = input.y;
                
                if(!Mathf.Approximately(delta, 0.0f)) SizeChangedEvent.Invoke();

                Size = Mathf.Clamp(Size + delta * SizeChangeRate, MinSize, MaxSize);
                
                Handle.Height = Size;
                Handle.Width = Size;
                HandleOutline.Height = Size;
                HandleOutline.Width = Size;
            }
        }

    }
}
