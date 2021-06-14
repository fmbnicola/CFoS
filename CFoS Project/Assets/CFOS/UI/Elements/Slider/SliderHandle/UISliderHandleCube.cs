using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{

    public class UISliderHandleCube : UIElement
    {
        public MeshRenderer Handle;
        public Data.ColorVariable HandleNormalColor;
        public Data.ColorVariable HandleHoverColor;
        public Data.ColorVariable HandleSelectColor;

        [Space(10)]
        public MeshRenderer HandleOutline;
        public Data.ColorVariable HandleOutlineNormalColor;
        public Data.ColorVariable HandleOutlineHoverColor;

        [Space(10)]
        public TMPro.TextMeshPro HandleText;
        public Data.ColorVariable HandleTextColor;

        [Header("Properties")]
        public float Size = 0.03f;

        protected MaterialPropertyBlock handlePropBlock;
        protected MaterialPropertyBlock outlinePropBlock;

        private void Start()
        {
            if(handlePropBlock == null)  handlePropBlock = new MaterialPropertyBlock();
            if(outlinePropBlock == null) outlinePropBlock = new MaterialPropertyBlock();
        }

        [ExecuteAlways]
        protected override void OnValidate()
        {
            // Set Colors
            if (handlePropBlock == null) handlePropBlock = new MaterialPropertyBlock();
            handlePropBlock.SetColor("_HandleColor", HandleNormalColor.Value);
            Handle.SetPropertyBlock(handlePropBlock);

            if (outlinePropBlock == null) outlinePropBlock = new MaterialPropertyBlock();
            outlinePropBlock.SetColor("_OutlineColor", HandleOutlineNormalColor.Value);
            HandleOutline.SetPropertyBlock(handlePropBlock);

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
                handlePropBlock.SetColor("_HandleColor", col);
                Handle.SetPropertyBlock(handlePropBlock);
                return;
            }

            // Visual Update
            var handleColor = selected ? HandleSelectColor.Value : hovered ? HandleHoverColor.Value : HandleNormalColor.Value;
            handlePropBlock.SetColor("_HandleColor", handleColor);
            Handle.SetPropertyBlock(handlePropBlock);

            var outlineColor = selected ? HandleOutlineHoverColor.Value : hovered ? HandleOutlineHoverColor.Value : HandleOutlineNormalColor.Value;
            outlinePropBlock.SetColor("_OutlineColor", outlineColor);
            HandleOutline.SetPropertyBlock(outlinePropBlock);
        }

        protected virtual void Update()
        {
            VisualUpdate();
        }

    }
}