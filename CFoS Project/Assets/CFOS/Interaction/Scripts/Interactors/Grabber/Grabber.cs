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

        public float Size = 0.5f;

        public Color HoverColor;

        // Cosmetic elements
        private Shapes.Disc outlineRenderer;
        private Renderer volumeRenderer;
        MaterialPropertyBlock propBlock;

        // Interactor
        public XRBaseInteractor Interactor { get; set; }

        private void Awake()
        {
            Interactor = GetComponent<XRBaseInteractor>();

            outlineRenderer = Outline.GetComponent<Shapes.Disc>();
            volumeRenderer = Volume.GetComponent<Renderer>();
            propBlock = new MaterialPropertyBlock();
        }

        void Update()
        {
            var scale = Vector3.one * Size;
            Pivot.transform.localScale = scale;
        }

        void LateUpdate()
        {
            var foward = transform.position - Camera.main.transform.position;
            Outline.rotation = Quaternion.LookRotation(foward, Vector3.up);
        }


        // Methods
        public void Hover(bool val)
        {
            var color = val ? HoverColor : Color.white;
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