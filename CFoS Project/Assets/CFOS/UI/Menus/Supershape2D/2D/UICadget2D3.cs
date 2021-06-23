using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using CFoS.Data;
using UnityEngine.XR.Interaction.Toolkit;
using CFoS.Experimentation;

namespace CFoS.UI.Menus
{
    public class UICadget2D3 : UICadget2D2
    {
        [Header("MiniMap")] 
        public ThumbnailQuad MinimapThumbnails;
        public ColorVariable MinimapThumbnailColor;

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

        public void MinimapThumbnailSelect(Thumbnail thumbnail, XRBaseController controller)
        {
            var data = thumbnail.Supershape.GetData();
            Slider.Value = new Vector2(data.N1, data.N2);
            Slider.ForceValueUpdate();

            // Register Thumbnail Click as metric
            if (controller.gameObject.name == "LeftHand Controller")
                MetricManager.Instance.RegisterTaskMetric("MinimapSelectCountL", 1.0f);
            if (controller.gameObject.name == "RightHand Controller")
                MetricManager.Instance.RegisterTaskMetric("MinimapSelectCountR", 1.0f);
        }

        public void MinimapThumbnailUpdate(Thumbnail thumbnail)
        {
            int i_x = thumbnail.Index.x;
            int i_y = thumbnail.Index.y;
            int count = MinimapThumbnails.Lines[0].Thumbnails.Count;
            var half = (count - 1) / 2;
            int i_offset_x = Mathf.Abs(i_x - half);
            int i_offset_y = Mathf.Abs(i_y - half);

            int i_offset = Mathf.Max(i_offset_x, i_offset_y);

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
            MinimapThumbnails.SetSelectFunction(MinimapThumbnailSelect);
        }

        protected void UpdateMinimapThumbnails()
        {
            MinimapThumbnails.UpdateSampling();
        }

    }
}