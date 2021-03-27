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
        // Supershape Reference
        
        private Supershape2D supershape;
        public Supershape2D Supershape
        {
            get { return supershape; }
            set { if (supershape != value) { Clean(); supershape = value; Init(); } }
        }

        // Render Properties
        private int samplePoints = 360;
        public int SamplePoints
        {
            get { return samplePoints; }
            set { if (samplePoints!= value) { samplePoints = value; Init(); } }
        }

        [HideInInspector] public float LineThickness = 2.0f;
        [HideInInspector] public Color LineColor = Color.white;

        // Polyline
        protected PolylinePath polyline;

        // Debug
        public bool DebugEnabled = false;



        // Unity Events
        public override void OnEnable()
        {
            base.OnEnable();

            Init();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            Clean();
        }

        void OnDestroy()
        {
            Clean();
        }



        // Methods
        protected void Init()
        {
            if(DebugEnabled) Debug.Log("Init");

            // Init Supershape
            if (supershape == null)
            {
                if (DebugEnabled) Debug.Log("New Supershape");
                supershape = ScriptableObject.CreateInstance<Supershape2D>();
            }
            supershape.OnUpdate -= UpdateRender;
            supershape.OnUpdate += UpdateRender;

            // Init Polyline
            if (polyline != null)
            {
                polyline.Dispose();
            }
            polyline = new PolylinePath();

            if (samplePoints < 10) samplePoints = 10;

            float inc = 2 * Mathf.PI / samplePoints;
            for (float angle = 0; angle < 2 * Mathf.PI; angle += inc)
            {
                var point = supershape.GetCoords(angle);
                polyline.AddPoint(point);
            }

            // Force SceneView update
            #if UNITY_EDITOR
            SceneView.RepaintAll();
            #endif
        }

        protected void Clean()
        {
            if (DebugEnabled) Debug.Log("Clean");

            if (supershape != null)
                supershape.OnUpdate -= UpdateRender;

            if (polyline != null)
                polyline.Dispose();
        }

        protected void UpdateRender()
        {
            if (supershape == null || polyline == null) Init();

            if (DebugEnabled) Debug.Log("Update: " + supershape);

            float inc = 2 * Mathf.PI / samplePoints;
            int i = 0;
            for (float angle = 0; angle < 2 * Mathf.PI; angle += inc)
            {
                var point = supershape.GetCoords(angle);
                polyline.SetPoint(i++, point);
            }

            // Force SceneView update
            #if UNITY_EDITOR
            SceneView.RepaintAll();
            #endif
        }

        public override void DrawShapes(Camera cam)
        {
            if (supershape == null || polyline == null) Init();

            using (Draw.Command(cam))
            {
                // set up static parameters. these are used for all following Draw.Line calls
                Draw.PolylineGeometry = PolylineGeometry.Flat2D;
                Draw.LineThicknessSpace = ThicknessSpace.Meters;
                Draw.DetailLevel = DetailLevel.Medium;
                Draw.PolylineJoins = PolylineJoins.Miter;

                // set properties
                Draw.LineThickness = LineThickness;
                Draw.Color = LineColor;

                // set static parameter to draw in the local space of this object
                Draw.Matrix = transform.localToWorldMatrix;

                // draw SShape
                Draw.Polyline(polyline, closed: true);

            }
        }
    }
}

