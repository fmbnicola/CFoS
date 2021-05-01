using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget1D2 : UICadget1D1
    {
        [Header("Thumbnails")]
        public ThumbnailLine Thumbnails;

        [Header("Zones")]
        public Data.ColorVariable TextActiveColor;
        public Data.ColorVariable TextInactiveColor;
        [Space(20)]
        public TMPro.TextMeshPro Starness;
        public TMPro.TextMeshPro Roundness;
        public TMPro.TextMeshPro Squareness;
        [Space(10)]
        public TMPro.TextMeshPro Concave;
        public TMPro.TextMeshPro Convex;
        [Space(10)]
        public TMPro.TextMeshPro Inscribed;
        public TMPro.TextMeshPro Circumscribed;


        protected override void Start()
        {
            base.Start();

            // Zones
            var slider = (UISlider1DZones) Slider;
            slider.ZoneChangedEvent.AddListener(ZoneChange);
            ZoneChange();

            // Render
            Slider.ValueChangedEvent.AddListener(ValueChange);
            Renderer.Supershape.OnUpdate += UpdateThumbnails;

            // Thumbnails
            InitThumbnails();
        }

        private void OnDestroy()
        {
            Renderer.Supershape.OnUpdate -= UpdateThumbnails;
        }

    
        // THUMBNAILS
        public Supershape2D.Data SampleSlider(Thumbnail thumbnail)
        {
            var data = Renderer.Supershape.GetData();
            var n123 = Slider.SampleValueWorldCoords(thumbnail.transform.position);
            data.N1 = n123;
            data.N2 = n123;
            data.N3 = n123;

            return data;
        }

        protected void InitThumbnails()
        {
            Thumbnails.Function = SampleSlider;
            UpdateThumbnails(Supershape2D.Parameter.Any);
        }

        protected void UpdateThumbnails(Supershape2D.Parameter p)
        {
            if (p != Supershape2D.Parameter.N1 &&
                p != Supershape2D.Parameter.N2 &&
                p != Supershape2D.Parameter.N3)
            {
                Thumbnails.UpdateSampling();
            }
        }


        // ZONES
        protected void ZoneChange()
        {
            Starness.color      = TextInactiveColor.Value;
            Roundness.color     = TextInactiveColor.Value;
            Squareness.color    = TextInactiveColor.Value;
            Concave.color       = TextInactiveColor.Value;
            Convex.color        = TextInactiveColor.Value;
            Inscribed.color     = TextInactiveColor.Value;
            Circumscribed.color = TextInactiveColor.Value;

            var slider = (UISlider1DZones) Slider;
            switch (slider.ZoneIndex)
            {
                case 0:
                    Starness.color = TextActiveColor.Value;
                    Concave.color = TextActiveColor.Value;
                    Inscribed.color = TextActiveColor.Value;
                    break;

                case 1:
                    Roundness.color = TextActiveColor.Value;
                    Convex.color = TextActiveColor.Value;
                    Inscribed.color = TextActiveColor.Value;
                    break;

                case 2:
                    Squareness.color = TextActiveColor.Value;
                    Convex.color = TextActiveColor.Value;
                    Circumscribed.color = TextActiveColor.Value;
                    break;
            }
        }

    }
}