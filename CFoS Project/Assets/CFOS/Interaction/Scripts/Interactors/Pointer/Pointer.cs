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

        public float Size = 0.5f;
        public float PointerDistance = 0.5f;

        public Data.ColorReference HoverColor;

        // Cosmetic elements
        private Shapes.Line outlineRenderer;
        private Renderer volumeRenderer;
        MaterialPropertyBlock propBlock;


        // Interactor
        public XRBaseInteractor Interactor { get; set; }

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
        }

        // Methods
        public void Hover(bool val)
        {
            var color = val ? HoverColor.Value : Color.white;
            propBlock.SetColor("_Color", color);

            outlineRenderer.Color = color;
            volumeRenderer.SetPropertyBlock(propBlock);
        }

        public void Show(bool val)
        {
            outlineRenderer.enabled = val;
            volumeRenderer.enabled = val;
        }
    }
}
