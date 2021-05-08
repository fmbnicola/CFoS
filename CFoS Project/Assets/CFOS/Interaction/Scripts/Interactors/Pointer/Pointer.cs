using CFoS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.Interaction
{
    [RequireComponent(typeof(XRBaseInteractor))]
    public class Pointer : MonoBehaviour
    {
        public Transform Pivot;
        public Transform Volume;
        public Transform Outline;

        [Space(5)]
        public float Size = 0.5f;
        public float PointerDistance = 0.5f;

        [Space(5)]
        public Data.ColorVariable NormalColor;
        public Data.ColorVariable HoverColor;

        // Cosmetic elements
        private Shapes.Line outlineRenderer;
        private Renderer volumeRenderer;
        MaterialPropertyBlock propBlock;

        // Properties
        public bool Activated { get; protected set; } = true;
        public void Activate(bool val) { Activated = val; }

        public bool Hovered { get; protected set; } = true;
        public void Hover(bool val){ Hovered = val; }

        public UIElement SelectedElement { get; set; } = null;
        public bool Selected { get => (SelectedElement != null); }
        public void Select(UIElement element) { SelectedElement = element; }

        public XRBaseInteractor Interactor { get; set; }

        // Methods
        private void Awake()
        {
            Interactor = GetComponent<XRBaseInteractor>();

            outlineRenderer = Outline.GetComponent<Shapes.Line>();
            volumeRenderer = Volume.GetComponent<Renderer>();
            propBlock = new MaterialPropertyBlock();
        }

        private void VisualUpdate()
        {
            // Color or not
            var val = ( Hovered || Selected );
            var color = val ? HoverColor.Value : NormalColor.Value;
            propBlock.SetColor("_Color", color);
            outlineRenderer.Color = color;
            volumeRenderer.SetPropertyBlock(propBlock);
           
            // Visible or not
            outlineRenderer.enabled = Activated;
            volumeRenderer.enabled = Activated;
        }

        private void Update()
        {
            var scale = Vector3.one * Size;
            Pivot.transform.localScale = scale;

            // disable hover/select
            var val = (Activated && !Selected);
            Interactor.allowHover = val;
            Interactor.allowSelect = val;

            VisualUpdate();
        }
    }
}
