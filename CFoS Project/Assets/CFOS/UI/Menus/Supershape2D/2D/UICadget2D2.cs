using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEngine.XR.Interaction.Toolkit;
using CFoS.Experimentation;

namespace CFoS.UI.Menus
{
    public class UICadget2D2 : UIMenu
    {
        [Header("Supershape")]
        public Supershape2DRenderer Renderer;

        [Header("2D Slider")]
        public CFoS.UI.UISlider2D Slider;

        [Header("Thumbnails")]
        public ThumbnailQuad Thumbnails;

        // Init
        protected virtual void Start()
        {
            // Render Update
            Slider.ValueChangedEvent.AddListener(ValueChange);
            Renderer.Supershape.OnUpdate += UpdateThumbnails;
            
            // Thumbnails
            InitThumbnails();

            ValueChange();
        }

        public override void ResetMenu()
        {
            Slider.ResetValue();
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
            Renderer.Supershape.N3 = Slider.Value.y;
        }


        // THUMBNAILS
        public Supershape2D.Data ThumbnailSample(Thumbnail thumbnail)
        {
            var data = Renderer.Supershape.GetData();
            var n123 = Slider.WorldCoordsToValue(thumbnail.transform.position);
            data.N1 = n123.x;
            data.N2 = n123.y;
            data.N3 = n123.y;

            return data;
        }

        public void ThumbnailSelect(Thumbnail thumbnail, XRBaseController controller)
        {
            var data = thumbnail.Supershape.GetData();
            Slider.Value = new Vector2(data.N1, data.N2);
            Slider.ForceValueUpdate();

            // Register Thumbnail Click as metric
            if (controller.gameObject.name == "LeftHand Controller")
                MetricManager.Instance.RegisterTaskMetric("ThumbnailSelectCountL", 1.0f);
            if (controller.gameObject.name == "RightHand Controller")
                MetricManager.Instance.RegisterTaskMetric("ThumbnailSelectCountR", 1.0f);
        }

        /*
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
        }*/

        protected void InitThumbnails()
        {
            Thumbnails.SetSampleFunction(ThumbnailSample);
            //Thumbnails.SetUpdateFunction(ThumbnailUpdate);
            Thumbnails.SetSelectFunction(ThumbnailSelect);
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