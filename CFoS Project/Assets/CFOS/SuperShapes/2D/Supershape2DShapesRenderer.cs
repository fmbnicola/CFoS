using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using UnityEditor;

namespace CFoS.Supershape
{
    [ExecuteAlways]
    public class Supershape2DShapesRenderer : Supershape2DRenderer
    {
        // Render Properties
        [HideInInspector] [SerializeField] protected int samplePoints = 360;
        public int SamplePoints
        {
            get { return samplePoints; }
            set { VarChangeCheck(ref samplePoints, value, Init); }
        }

        [HideInInspector] [SerializeField] protected float lineThickness = 0.05f;
        public float LineThickness
        {
            get { return lineThickness; }
            set { VarChangeCheck(ref lineThickness, value, UpdateRenderProperties); }
        }

        [HideInInspector] [SerializeField] protected Color lineColor = Color.white;
        public Color LineColor
        {
            get { return lineColor; }
            set { VarChangeCheck(ref lineColor, value, UpdateRenderProperties); }

        }

        // Polyline
        public Polyline Polyline;


        // Methods
        protected override void Init()
        {
            base.Init();

            // Init Polyline
            if (Polyline == null)
            {
                Polyline = gameObject.AddComponent(typeof(Polyline)) as Polyline;
                Polyline.Closed = false;
            }
            Polyline.points.Clear();

            if (samplePoints < 10) samplePoints = 10;

            float inc = 2 * Mathf.PI / samplePoints;
            for (float angle = 0; angle < 2 * Mathf.PI; angle += inc)
            {
                var point = supershape.GetCoords(angle);
                Polyline.AddPoint(point);
            }

            Repaint();
        }

        protected override void Clean()
        {
            base.Clean();

            if (Polyline != null)
                Polyline.points.Clear();
        }

        protected override void UpdateRender(Supershape2D.Parameter p)
        {
            base.UpdateRender(p);

            if (Polyline == null) Init();

            int i = 0;
            float inc = 2 * Mathf.PI / samplePoints;
            for (float angle = 0; angle < 2 * Mathf.PI; angle += inc)
            {
                var point = supershape.GetCoords(angle);
                Polyline.SetPointPosition(i++, point);
            }

            Repaint();
        }

        protected override void UpdateRenderProperties()
        {
            // Update Render Properties
            if (Polyline != null)
            {
                Polyline.Color = LineColor;
                Polyline.Thickness = LineThickness;
            }
        }
    }
}