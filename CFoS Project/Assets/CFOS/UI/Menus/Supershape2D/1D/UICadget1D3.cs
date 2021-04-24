using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget1D3 : UICadget1D2
    {
        [Header("Mini-map")]
        public Transform Minimap;
        public Supershape2DRenderer MinimapThumbnail1;
        public Supershape2DRenderer MinimapThumbnail2;
        public Supershape2DRenderer MinimapThumbnail3;
        public Supershape2DRenderer MinimapThumbnail4;
        public Supershape2DRenderer MinimapThumbnail5;


        protected override void Start()
        {
            base.Start();

            // Update Mini-map Thumbnails
            Slider.ValueChangedEvent.AddListener(UpdateMinimapThumbnails);

            var scalableHandle = (UISliderHandleScalable) Slider.Handle;
            scalableHandle.SizeChangedEvent.AddListener(UpdateMinimapThumbnails);
            InitMinimapThumbnails();
        }

        protected void SetMinimapThumbnail(Supershape2D supershape, Vector3 pos)
        {
            var paramameters = ParametersFromSlider(pos);

            supershape.A  = paramameters[0];
            supershape.B  = paramameters[1];
            supershape.M  = paramameters[2];
            supershape.N1 = paramameters[3];
            supershape.N2 = paramameters[4];
            supershape.N3 = paramameters[5];
        }

        protected void UpdateMinimapThumbnails()
        {
            float maxOffset = Slider.Handle.Size;
            float midOffset = maxOffset / 2;

            var slider = Slider.transform;
            Vector3 leftMaxPos  = slider.TransformVector(Vector3.left * maxOffset);
            Vector3 leftMidPos  = slider.TransformVector(Vector3.left * midOffset);
            Vector3 centerPos   = Slider.Handle.transform.position;
            Vector3 rightMidPos = slider.TransformVector(Vector3.right * midOffset);
            Vector3 rightMaxPos = slider.TransformVector(Vector3.right * maxOffset);

            SetMinimapThumbnail(MinimapThumbnail1.Supershape, centerPos + leftMaxPos);
            SetMinimapThumbnail(MinimapThumbnail2.Supershape, centerPos + leftMidPos);
            SetMinimapThumbnail(MinimapThumbnail3.Supershape, centerPos);
            SetMinimapThumbnail(MinimapThumbnail4.Supershape, centerPos + rightMidPos);
            SetMinimapThumbnail(MinimapThumbnail5.Supershape, centerPos + rightMaxPos);
        }

        protected void InitMinimapThumbnails()
        {
            MinimapThumbnail1.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            MinimapThumbnail2.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            MinimapThumbnail3.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            MinimapThumbnail4.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            MinimapThumbnail5.Supershape = ScriptableObject.CreateInstance<Supershape2D>();

            UpdateMinimapThumbnails();
        }
    }


}