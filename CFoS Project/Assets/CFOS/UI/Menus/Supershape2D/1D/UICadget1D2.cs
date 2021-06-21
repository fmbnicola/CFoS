using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using CFoS.Data;

namespace CFoS.UI.Menus
{
    public class UICadget1D2 : UICadget1D1
    {
        [Header("Thumbnails")]
        public ThumbnailLine Thumbnails;
        public ColorVariable ThumbnailColor1;
        public ColorVariable ThumbnailColor2;


        protected override void Start()
        {
            base.Start();

            // Render
            Renderer.Supershape.OnUpdate += UpdateThumbnails;

            // Thumbnails
            InitThumbnails();
        }

        private void OnDestroy()
        {
            Renderer.Supershape.OnUpdate -= UpdateThumbnails;
        }

    
        // THUMBNAILS
        public Supershape2D.Data ThumbnailSample(Thumbnail thumbnail)
        {
            var data = Renderer.Supershape.GetData();
            var n123 = Slider.WorldCoordsToValue(thumbnail.transform.position);
            data.N1 = n123;
            data.N2 = n123;
            data.N3 = n123;

            return data;
        }

        public void ThumbnailSelect(Thumbnail thumbnail)
        {
            var data = thumbnail.Supershape.GetData();
            Slider.Value = data.N1;
            Slider.ForceValueUpdate();
        }

        public void ThumbnailUpdate(Thumbnail thumbnail)
        {
            var renderer = (Supershape2DQuadRenderer)thumbnail.Renderer;

            Vector3Int i = thumbnail.Index;
            if (i.x % 3 == 0)
            {
                renderer.Color = ThumbnailColor1.Value;
            }
            else
            {
                renderer.Color = ThumbnailColor2.Value;
            }
        }


        protected void InitThumbnails()
        {
            Thumbnails.SetSampleFunction(ThumbnailSample);
            Thumbnails.SetSelectFunction(ThumbnailSelect);
            Thumbnails.SetUpdateFunction(ThumbnailUpdate);
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
    }
}