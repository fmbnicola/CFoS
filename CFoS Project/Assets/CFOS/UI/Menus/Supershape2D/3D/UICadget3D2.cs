using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using CFoS.Data;

namespace CFoS.UI.Menus
{
    public class UICadget3D2 : UIMenu
    {
        [Header("Supershape")]
        public Supershape2DRenderer Renderer;

        [Header("3D Slider")]
        public CFoS.UI.UISlider3D Slider;

        [Header("Thumbnails")]
        public ThumbnailQuad Thumbnails;
        public ColorVariable ThumbnailColor1;
        public ColorVariable ThumbnailColor2;

        // Init
        protected virtual void Start()
        {
            // Render Update
            Slider.ValueChangedEvent.AddListener(ValueChange);
            Renderer.Supershape.OnUpdate += UpdateThumbnails;

            // Thumbnails
            InitThumbnails();
        }

        private void OnDestroy()
        {
            Renderer.Supershape.OnUpdate -= UpdateThumbnails;
        }


        // SLIDER 
        protected virtual void ValueChange()
        {
            Renderer.Supershape.N1 = Slider.Value.x;
            Renderer.Supershape.N2 = Slider.Value.y;
            Renderer.Supershape.N3 = Slider.Value.z;

            UpdateThumbnailsPosition();
            UpdateThumbnails(Supershape2D.Parameter.Any);
        }


        // THUMBNAILS
        public Supershape2D.Data ThumbnailSample(Thumbnail thumbnail)
        {
            var data = Renderer.Supershape.GetData();
            var n123 = Slider.WorldCoordsToValue(thumbnail.transform.position);
            data.N1 = n123.x;
            data.N2 = n123.y;
            data.N3 = n123.z;

            return data;
        }

        public void ThumbnailSelect(Thumbnail thumbnail)
        {
            var data = thumbnail.Supershape.GetData();
            Slider.Value = new Vector3(data.N1, data.N2, data.N3);
            Slider.ForceValueUpdate();
        }

        public void ThumbnailUpdate(Thumbnail thumbnail)
        {
            var renderer = (Supershape2DQuadRenderer) thumbnail.Renderer;

            Vector3Int i = thumbnail.Index;
            if (i.x % 3 == 0 && i.y % 3 == 0)
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

        protected void UpdateThumbnailsPosition()
        {
            var pos = Thumbnails.transform.position;
            pos.z = Slider.Handle.transform.position.z;
            Thumbnails.transform.position = pos;
        }

        protected void UpdateThumbnails(Supershape2D.Parameter p)
        {
            Thumbnails.UpdateSampling();
        }
    }
}