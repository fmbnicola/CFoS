using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [ExecuteAlways]
    public class Supershape2DRenderer : MonoBehaviour
    {
        public void VarChangeCheck<T>(ref T var, T val, Action func)
        {
            if (!var.Equals(val)) { var = val; func?.Invoke(); }
        }

        // Supershape Reference
        [SerializeReference] [HideInInspector]
        private Supershape2D supershape;
        public Supershape2D Supershape
        {
            get { return supershape; }
            set { VarChangeCheck(ref supershape, value, () => { Clean(); Init(); }); }
        }

        // Render Properties
        private int samplePoints = 360;
        public int SamplePoints
        {
            get { return samplePoints; }
            set { VarChangeCheck(ref samplePoints, value, Init); }
        }

        private float lineThickness = 0.05f;
        public float LineThickness
        {
            get { return lineThickness; }
            set { VarChangeCheck(ref lineThickness, value, UpdateProperties); }
        }

        private Color lineColor = Color.white;
        public Color LineColor
        {
            get { return lineColor; }
            set { VarChangeCheck(ref lineColor, value, UpdateProperties); }

        }

        // Polyline
        public Polyline Polyline;

        // Debug
        public bool DebugEnabled = false;



        // Unity Events
        public void OnEnable()
        {
            Init();
        }

        public void OnDisable()
        {
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

            if (Polyline != null)
                Polyline.points.Clear();
        }

        protected void UpdateRender()
        {
            if (supershape == null || Polyline == null) Init();

            if (DebugEnabled) Debug.Log("Update: " + supershape);

            int i = 0;
            float inc = 2 * Mathf.PI / samplePoints;
            for (float angle = 0; angle < 2 * Mathf.PI; angle += inc)
            {
                var point = supershape.GetCoords(angle);
                Polyline.SetPointPosition(i++, point);
            }

            // Force SceneView update
            #if UNITY_EDITOR
            SceneView.RepaintAll();
            #endif
        }

        protected void UpdateProperties()
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

