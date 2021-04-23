using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget1D2 : UICadget1D1
    {
        [Header("Thumbnails")]
        public Supershape2DRenderer Thumbnail1;
        public Supershape2DRenderer Thumbnail2;
        public Supershape2DRenderer Thumbnail3;
        public Supershape2DRenderer Thumbnail4;
        public Supershape2DRenderer Thumbnail5;
        public Supershape2DRenderer Thumbnail6;
        public Supershape2DRenderer Thumbnail7;

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
            var slider = (UISliderZones) Slider;
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


        protected float[] ParametersFromSlider(Vector3 pos)
        {
            var a = Renderer.Supershape.A;
            var b = Renderer.Supershape.B;
            var m = Renderer.Supershape.M;
            var n123 = Slider.SampleValueWorldCoords(pos);
           
            return new float[] { a, b, m, n123, n123, n123 };
        }

        protected void SetThumbnail(Supershape2D supershape, Vector3 pos)
        {
            var paramameters = ParametersFromSlider(pos);

            supershape.A = paramameters[0];
            supershape.B = paramameters[1];
            supershape.M = paramameters[2];
            supershape.N1 = paramameters[3];
            supershape.N2 = paramameters[4];
            supershape.N3 = paramameters[5];
        }

        protected void UpdateThumbnails(Supershape2D.Parameter p)
        {
            if( p != Supershape2D.Parameter.N1 &&
                p != Supershape2D.Parameter.N2 &&
                p != Supershape2D.Parameter.N3  )
            {
                SetThumbnail(Thumbnail1.Supershape, Thumbnail1.transform.position);
                SetThumbnail(Thumbnail2.Supershape, Thumbnail2.transform.position);
                SetThumbnail(Thumbnail3.Supershape, Thumbnail3.transform.position);
                SetThumbnail(Thumbnail4.Supershape, Thumbnail4.transform.position);
                SetThumbnail(Thumbnail5.Supershape, Thumbnail5.transform.position);
                SetThumbnail(Thumbnail6.Supershape, Thumbnail6.transform.position);
                SetThumbnail(Thumbnail7.Supershape, Thumbnail7.transform.position);
            }
        }

        protected void InitThumbnails()
        {
            Thumbnail1.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Thumbnail3.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Thumbnail4.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Thumbnail2.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Thumbnail5.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Thumbnail6.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Thumbnail7.Supershape = ScriptableObject.CreateInstance<Supershape2D>();

            UpdateThumbnails(Supershape2D.Parameter.Any);
        }

        protected void ZoneChange()
        {
            Starness.color      = TextInactiveColor.Value;
            Roundness.color     = TextInactiveColor.Value;
            Squareness.color    = TextInactiveColor.Value;
            Concave.color       = TextInactiveColor.Value;
            Convex.color        = TextInactiveColor.Value;
            Inscribed.color     = TextInactiveColor.Value;
            Circumscribed.color = TextInactiveColor.Value;

            var slider = (UISliderZones) Slider;
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