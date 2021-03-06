using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{

    public class UISliderHandle : UIElement
    {
        public Shapes.Rectangle Handle;
        public Data.ColorVariable HandleNormalColor;
        public Data.ColorVariable HandleHoverColor;
        public Data.ColorVariable HandleSelectColor;

        [Space(10)]
        public Shapes.Rectangle HandleOutline;
        public Data.ColorVariable HandleOutlineNormalColor;
        public Data.ColorVariable HandleOutlineHoverColor;

        [Space(10)]
        public TMPro.TextMeshPro HandleText;
        public Data.ColorVariable HandleTextColor;

        [Header("Properties")]
        public float Size = 0.03f;

        [ExecuteAlways]
        protected override void OnValidate()
        {
            // Set Colors
            var col = HandleNormalColor.Value;
            col.a = 0.5f;
            Handle.Color = col;
            HandleOutline.Color = HandleOutlineNormalColor.Value;
            HandleText.color = HandleTextColor.Value;

            Handle.Height = Size;
            Handle.Width  = Size;
            HandleOutline.Height = Size;
            HandleOutline.Width  = Size;
        }

        public override void Enable(bool val)
        {
            base.Enable(val);

            if (!val)
            {
                var col = HandleTextColor.Value;
                col.a = 0.3f;
                HandleText.color = col;
            }
            else
            {
                HandleText.color = HandleTextColor.Value;
            }
        }

        protected void VisualUpdate()
        {
            if (disabled)
            {
                var disabledCol = HandleNormalColor.Value;
                disabledCol.a = 0.3f;
                Handle.Color = disabledCol;
                return;
            }

            // Visual Update
            var handleCol = selected ? HandleSelectColor.Value : hovered ? HandleHoverColor.Value : HandleNormalColor.Value;
            handleCol.a = 0.5f;
            Handle.Color = handleCol;

            HandleOutline.Color = selected ? HandleOutlineHoverColor.Value : hovered ? HandleOutlineHoverColor.Value : HandleOutlineNormalColor.Value;
        }

        protected virtual void Update()
        {
            VisualUpdate();
        }

    }
}