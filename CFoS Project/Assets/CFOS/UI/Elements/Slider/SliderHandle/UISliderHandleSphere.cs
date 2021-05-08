using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{

    public class UISliderHandleSphere : UIElement
    {
        public MeshRenderer Handle;
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

        protected MaterialPropertyBlock propBlock;

        private void Start()
        {
            if(propBlock == null) propBlock = new MaterialPropertyBlock();
        }

        [ExecuteAlways]
        protected override void OnValidate()
        {
            // Set Colors
            if (propBlock == null) propBlock = new MaterialPropertyBlock();
            propBlock.SetColor("_HandleColor", HandleNormalColor.Value);
            Handle.SetPropertyBlock(propBlock);

            HandleOutline.Color = HandleOutlineNormalColor.Value;
            HandleText.color = HandleTextColor.Value;

            Handle.transform.localScale = Vector3.one * Size;
            HandleOutline.Height = Size;
            HandleOutline.Width  = Size;
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
            // outline billboard
            var foward = transform.position - Camera.main.transform.position;
            HandleOutline.transform.rotation = Quaternion.LookRotation(foward, Vector3.up);
            HandleText.transform.rotation    = Quaternion.LookRotation(foward, Vector3.up);

            if (disabled)
            {
                var col = HandleNormalColor.Value; col.a = 0.3f;
                propBlock.SetColor("_HandleColor", col);
                Handle.SetPropertyBlock(propBlock);
                return;
            }

            // Visual Update
            var handleColor = selected ? HandleSelectColor.Value : hovered ? HandleHoverColor.Value : HandleNormalColor.Value;
            propBlock.SetColor("_HandleColor", handleColor);
            Handle.SetPropertyBlock(propBlock);

            HandleOutline.Color = selected ? HandleOutlineHoverColor.Value : hovered ? HandleOutlineHoverColor.Value : HandleOutlineNormalColor.Value;
        }

        protected virtual void Update()
        {
            VisualUpdate();
        }

    }
}