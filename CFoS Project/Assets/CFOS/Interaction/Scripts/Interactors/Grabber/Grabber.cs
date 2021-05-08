using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.Interaction
{
    [RequireComponent(typeof(XRBaseInteractor))]
    public class Grabber : MonoBehaviour
    {
        public Transform Pivot;
        public Transform Volume;
        public Transform Outline;

        [Space(5)]
        public float Size = 0.5f;

        [Space(5)]
        public Data.ColorVariable NormalColor;
        public Data.ColorVariable HoverColor;

        // Cosmetic elements
        private Shapes.Disc outlineRenderer;
        private Renderer volumeRenderer;
        MaterialPropertyBlock propBlock;

        // Properties
        public bool Activated { get; protected set; } = true;
        public void Activate(bool val) { Activated = val; }

        public bool Hovered { get; protected set; } = true;
        public void Hover(bool val) { Hovered = val; }

        public bool Selected { get => (Interactor.selectTarget != null); }

        public XRBaseInteractor Interactor { get; set; }


        // Methods
        private void Awake()
        {
            Interactor = GetComponent<XRBaseInteractor>();

            outlineRenderer = Outline.GetComponent<Shapes.Disc>();
            volumeRenderer = Volume.GetComponent<Renderer>();
            propBlock = new MaterialPropertyBlock();
        }

        private void VisualUpdate()
        {
            // Color or not
            var val = (Hovered || Selected);
            var color = val ? HoverColor.Value : NormalColor.Value;
            propBlock.SetColor("_Color", color);
            outlineRenderer.Color = color;
            volumeRenderer.SetPropertyBlock(propBlock);

            // Visible or not
            outlineRenderer.enabled = Activated;
            volumeRenderer.enabled = Activated;
        }

        void Update()
        {
            var scale = Vector3.one * Size;
            Pivot.transform.localScale = scale;

            // Disable hover/select
            Interactor.allowHover = Activated;
            Interactor.allowSelect = Activated;

            VisualUpdate();
        }

        void LateUpdate()
        {
            var forward = transform.position - Camera.main.transform.position;
            Outline.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }
}