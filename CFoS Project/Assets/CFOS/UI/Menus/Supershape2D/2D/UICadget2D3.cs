using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget2D3 : UICadget2D2
    {
        [Header("MiniMap")] 
        public ThumbnailQuad MinimapThumbnails;


        protected override void Start()
        {
            base.Start();

            // Update Mini-map Thumbnails
            Slider.ValueChangedEvent.AddListener(UpdateMinimapThumbnails);

            var scalableHandle = (UISliderHandleScalable)Slider.Handle;
            scalableHandle.SizeChangedEvent.AddListener(UpdateMinimapThumbnails);
            InitMinimapThumbnails();
        }

        // MINIMAP
        public Supershape2D.Data MinimapThumbnailSample(Thumbnail thumbnail)
        {
            // figure out position on slider based on index and handle size
            int xIndex = thumbnail.Index.x;
            int yIndex = thumbnail.Index.y;
            int xCount = MinimapThumbnails.Lines[0].Thumbnails.Count;
            int yCount = MinimapThumbnails.Lines.Count;

            float xiOffset = xIndex - ((float)(xCount - 1)) / 2;
            float xOffset = (xCount == 1) ? 0.0f : Slider.Handle.Size / (xCount - 1);

            float yiOffset = yIndex - ((float)(yCount - 1)) / 2;
            float yOffset = (yCount == 1) ? 0.0f : Slider.Handle.Size / (yCount - 1);

            Vector3 centerPos = Slider.ValueToWorldCoords(Slider.Value);
            var pos = centerPos + (Vector3.right * (xiOffset * xOffset))
                                + (Vector3.up    * (yiOffset * yOffset));

            // Get parameters from position
            var data = Renderer.Supershape.GetData();
            var n123 = Slider.WorldCoordsToValue(pos);
            data.N1 = n123.x;
            data.N2 = n123.y;
            data.N3 = n123.y;

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