using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{

    public class UISliderHandleCube : UIElement
    {
        public OutlineCubeSimple  Handle;
        public Data.ColorVariable HandleNormalColor;
        public Data.ColorVariable HandleHoverColor;
        public Data.ColorVariable HandleSelectColor;
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
            Handle.SetColor(HandleNormalColor.Value);
            Handle.SetLinesColor(HandleOutlineNormalColor.Value);

            Handle.transform.localScale = Vector3.one * Size;
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
            // make text face user
            var foward = transform.position - Camera.main.transform.position;
            HandleText.transform.rotation    = Quaternion.LookRotation(foward, Vector3.up);

            if (disabled)
            {
                var col = HandleNormalColor.Value; col.a = 0.3f;
                Handle.SetColor(col);
                return;
            }

            // Visual Update
            var handleColor = selected ? HandleSelectColor.Value : hovered ? HandleHoverColor.Value : HandleNormalColor.Value;
            Handle.SetColor(handleColor);

            var outlineColor = selected ? HandleOutlineHoverColor.Value : hovered ? HandleOutlineHoverColor.Value : HandleOutlineNormalColor.Value;
            Handle.SetLinesColor(outlineColor);
        }

        protected virtual void Update()
        {
            VisualUpdate();
        }

    }
}