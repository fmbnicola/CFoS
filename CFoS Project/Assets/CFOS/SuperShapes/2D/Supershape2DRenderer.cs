using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [ExecuteAlways]
    public class Supershape2DRenderer : ImmediateModeShapeDrawer
    {
        public Supershape2D Supershape;

        protected PolylinePath polyline;

        [Header("Render Properties")]
        public int SamplePoints = 360;
        public float LineThickness = 2.0f;
        public Color LineColor = Color.white;


        public void UpdateRender()
        {
            if (Supershape == null || polyline == null) Init();

            //Debug.Log(Supershape);

            float inc = 2 * Mathf.PI / SamplePoints;
            int i = 0;
            for (float angle = 0; angle < 2 * Mathf.PI; angle += inc)
            {
                var point = Supershape.GetCoords(angle);
                polyline.SetPoint(i++, point);
            }

            SceneView.RepaintAll();
        }

        public void Init()
        {
            // Init Supershape
            if (Supershape == null)
            {
                Supershape = ScriptableObject.CreateInstance<Supershape2D>();
                Supershape.OnUpdate += UpdateRender;
            }

            // Init Polyline
            if (polyline != null)
            {
                polyline.Dispose();
            }
            polyline = new PolylinePath();

            float inc = 2 * Mathf.PI / SamplePoints;
            for (float angle = 0; angle < 2 * Mathf.PI; angle += inc)
            {
                var point = Supershape.GetCoords(angle);
                polyline.AddPoint(point);
            }
        }

        public override void DrawShapes(Camera cam)
        {
            if (Supershape == null || polyline == null) Init();

            using (Draw.Command(cam))
            {

                // set up static parameters. these are used for all following Draw.Line calls
                Draw.LineGeometry = LineGeometry.Flat2D;
                Draw.LineThicknessSpace = ThicknessSpace.Pixels;

                // set properties
                Draw.LineThickness = LineThickness;
                Draw.Color = LineColor;

                // set static parameter to draw in the local space of this object
                Draw.Matrix = transform.localToWorldMatrix;

                // draw SShape
                Draw.Polyline(polyline, closed: true);

            }
        }

        void OnDestroy()
        {
            if (polyline != null)
                polyline.Dispose();
        }

    }
}

