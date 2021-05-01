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
        public ThumbnailLine MinimapThumbnails;


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


        protected void InitMinimapThumbnails()
        {
            MinimapThumbnails.SetSampleFunction(MinimapThumbnailSample);
            MinimapThumbnails.SetSelectFunction(ThumbnailSelect);
        }

        protected void UpdateMinimapThumbnails()
        {
            MinimapThumbnails.UpdateSampling();
        }

    }
}