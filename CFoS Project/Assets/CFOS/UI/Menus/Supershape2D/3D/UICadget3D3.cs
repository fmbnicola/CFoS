using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using CFoS.Data;
using CFoS.Experimentation;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.UI.Menus
{
    public class UICadget3D3 : UICadget3D2
    {
        [Header("MiniMap")] 
        public ThumbnailCube MinimapThumbnails;
        public ColorVariable MinimapThumbnailColor;

        protected override void Start()
        {
            base.Start();

            // Update Mini-map Thumbnails
            Slider.ValueChangedEvent.AddListener(UpdateMinimapThumbnails);

            var scalableHandle = (UISliderHandleCubeScalable) Slider.Handle;
            scalableHandle.SizeChangedEvent.AddListener(UpdateMinimapThumbnails);
            InitMinimapThumbnails();
        }

        // MINIMAP
        public Supershape2D.Data MinimapThumbnailSample(Thumbnail thumbnail)
        {
            // figure out position on slider based on index and handle size
            int xIndex = thumbnail.Index.x;
            int yIndex = thumbnail.Index.y;
            int zIndex = thumbnail.Index.z;
            int xCount = MinimapThumbnails.Quads[0].Lines[0].Thumbnails.Count;
            int yCount = MinimapThumbnails.Quads[0].Lines.Count;
            int zCount = MinimapThumbnails.Quads.Count;

            float xiOffset = xIndex - ((float)(xCount - 1)) / 2;
            float xOffset = (xCount == 1) ? 0.0f : Slider.Handle.Size / (xCount - 1);

            float yiOffset = yIndex - ((float)(yCount - 1)) / 2;
            float yOffset = (yCount == 1) ? 0.0f : Slider.Handle.Size / (yCount - 1);

            float ziOffset = zIndex - ((float)(zCount - 1)) / 2;
            float zOffset = (zCount == 1) ? 0.0f : Slider.Handle.Size / (zCount - 1);

            Vector3 centerPos = Slider.ValueToWorldCoords(Slider.Value);
            var pos = centerPos + (Vector3.right    * (xiOffset * xOffset))
                                + (Vector3.up       * (yiOffset * yOffset))
                                + (Vector3.forward  * (ziOffset * zOffset));

            // Get parameters from position
            var data = Renderer.Supershape.GetData();
            var n123 = Slider.WorldCoordsToValue(pos);
            data.N1 = n123.x;
            data.N2 = n123.y;
            data.N3 = n123.z;

            return data;
        }

        public void MinimapThumbnailUpdate(Thumbnail thumbnail)
        {
            int i_x = thumbnail.Index.x;
            int i_y = thumbnail.Index.y;
            int i_z = thumbnail.Index.z;

            int xCount = MinimapThumbnails.Quads[0].Lines[0].Thumbnails.Count;
            int yCount = MinimapThumbnails.Quads[0].Lines.Count;
            int zCount = MinimapThumbnails.Quads.Count;

            int i_offset_x = Mathf.Abs(i_x - (xCount - 1)/2);
            int i_offset_y = Mathf.Abs(i_y - (yCount - 1)/2);
            int i_offset_z = Mathf.Abs(i_z - (zCount - 1)/2);
            int i_offset = Mathf.Max(i_offset_x, i_offset_y, i_offset_z);

            var renderer = (Supershape2DQuadRenderer)thumbnail.Renderer;

            var col = MinimapThumbnailColor.Value;
            col.a = 1.0f - (0.6f * i_offset);
            renderer.Color = col;

            var scale = 0.8f - (0.4f * i_offset);
            renderer.Scale = scale;
        }

        public void MinimapThumbnailSelect(Thumbnail thumbnail, XRBaseController controller)
        {
            var data = thumbnail.Supershape.GetData();
            Slider.Value = new Vector3(data.N1, data.N2, data.N3);
            Slider.ForceValueUpdate();

            // Register Thumbnail Click as metric
            if (controller.gameObject.name == "LeftHand Controller")
                MetricManager.Instance.RegisterTaskMetric("MinimapSelectCountL", 1.0f);
            if (controller.gameObject.name == "RightHand Controller")
                MetricManager.Instance.RegisterTaskMetric("MinimapSelectCountR", 1.0f);
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