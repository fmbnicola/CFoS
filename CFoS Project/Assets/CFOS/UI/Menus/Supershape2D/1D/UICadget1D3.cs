using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using CFoS.Data;

namespace CFoS.UI.Menus
{
    public class UICadget1D3 : UICadget1D2
    {
        [Header("Mini-map")]
        public ThumbnailLine MinimapThumbnails;
        public ColorVariable MinimapThumbnailColor;

        protected override void Start()
        {
            base.Start();

            // Update Mini-map Thumbnails
            Slider.ValueChangedEvent.AddListener(UpdateMinimapThumbnails);

            var scalableHandle = (UISliderHandleScalable) Slider.Handle;
            scalableHandle.SizeChangedEvent.AddListener(UpdateMinimapThumbnails);
            InitMinimapThumbnails();
        }

        // MINIMAP
        public Supershape2D.Data MinimapThumbnailSample(Thumbnail thumbnail)
        {
            // figure out position on slider based on index and handle size
            int index = thumbnail.Index.x;
            int count = MinimapThumbnails.Thumbnails.Count;

            float i_offset = index - ((float)(count - 1))/2;
            float offset = (count == 1) ? 0.0f : Slider.Handle.Size / (count - 1);

            Vector3 centerPos = Slider.ValueToWorldCoords(Slider.Value);
            var pos = centerPos + (Vector3.right * (i_offset * offset));

            // Get parameters from position
            var data = Renderer.Supershape.GetData();
            var n123 = Slider.WorldCoordsToValue(pos);
            data.N1 = n123;
            data.N2 = n123;
            data.N3 = n123;

            return data;
        }

        public void MinimapThumbnailUpdate(Thumbnail thumbnail)
        {
            int index = thumbnail.Index.x;
            int count = MinimapThumbnails.Thumbnails.Count;
            float i_offset = Mathf.Abs(index - ((float)(count - 1)) / 2);

            var renderer = (Supershape2DQuadRenderer)thumbnail.Renderer;

            var col = MinimapThumbnailColor.Value;
            col.a = 1.0f - (0.4f * i_offset);
            renderer.Color = col;

            var scale = 0.8f - (0.2f * i_offset);
            renderer.Scale = scale;
        }


        protected void InitMinimapThumbnails()
        {
            MinimapThumbnails.SetSampleFunction(MinimapThumbnailSample);
            MinimapThumbnails.SetUpdateFunction(MinimapThumbnailUpdate);
            MinimapThumbnails.SetSelectFunction(ThumbnailSelect);
        }

        protected void UpdateMinimapThumbnails()
        {
            MinimapThumbnails.UpdateSampling();
        }

    }
}