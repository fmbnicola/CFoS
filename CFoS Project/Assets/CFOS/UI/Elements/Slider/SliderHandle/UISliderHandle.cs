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
        public Data.ColorVariable HandleOutlineColor;

        [Space(10)]
        public TMPro.TextMeshPro HandleText;
        public Data.ColorVariable HandleTextColor;

        [Header("Properties")]
        public float Size = 0.03f;

        [ExecuteAlways]
        protected override void OnValidate()
        {
            // Set Colors
            Handle.Color = HandleNormalColor.Value;
            HandleOutline.Color = HandleOutlineColor.Value;
            HandleText.color = HandleTextColor.Value;

            Handle.Height = Size;
            Handle.Width  = Size;
            HandleOutline.Height = Size;
            HandleOutline.Height = Size;
        }

        protected virtual void Awake()
        {
            HandleOutline.gameObject.SetActive(false);
        }

        public override void Enable(bool val)
        {
            base.Enable(val);

            if (!val)
            {
                var col = HandleTextColor.Value; col.a = 0.3f;
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
                var col = HandleNormalColor.Value; col.a = 0.3f;
                Handle.Color = col;
                HandleOutline.gameObject.SetActive(false);
                return;
            }

            // Visual Update
            Handle.Color = selected ? HandleSelectColor.Value : hovered ? HandleHoverColor.Value : HandleNormalColor.Value;
            HandleOutline.gameObject.SetActive(hovered || selected);
        }

        protected virtual void Update()
        {
            VisualUpdate();
        }

    }
}