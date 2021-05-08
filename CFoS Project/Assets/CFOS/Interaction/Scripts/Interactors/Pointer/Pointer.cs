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
        public XRBaseInteractor Interactor { get; set; }
        public UIElement SelectedElement { get; set; } = null;

        private void Awake()
        {
            Interactor = GetComponent<XRBaseInteractor>();

            outlineRenderer = Outline.GetComponent<Shapes.Line>();
            volumeRenderer = Volume.GetComponent<Renderer>();
            propBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            var scale = Vector3.one * Size;
            Pivot.transform.localScale = scale;

            //if not activated or selecting, disable hover/select
            if(Activated && SelectedElement == null)
            { 
                Interactor.allowHover = true;
                Interactor.allowSelect = true;
            }
            else
            {
                Interactor.allowHover = false;
                Interactor.allowSelect = false;
            }
        }

        // Methods
        public void Hover(bool val)
        {
            var color = val ? HoverColor.Value : NormalColor.Value;
            propBlock.SetColor("_Color", color);

            outlineRenderer.Color = color;
            volumeRenderer.SetPropertyBlock(propBlock);
        }

        public void Show(bool val)
        {
            outlineRenderer.enabled = val;
            volumeRenderer.enabled = val;
        }

        public void Activate(bool val)
        {
            Activated = val;
        }
    }
}
