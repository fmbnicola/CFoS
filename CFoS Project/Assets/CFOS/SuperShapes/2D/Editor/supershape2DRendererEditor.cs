using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [CustomEditor(typeof(Supershape2DRenderer))]
    [CanEditMultipleObjects]
    public class Supershape2DRendererEditor : Editor
    {
        bool showParameters = false;
        bool showProperties = true;

        SerializedProperty thickness;
        SerializedProperty color;

        public void OnEnable()
        {
            thickness = serializedObject.FindProperty("LineThickness");
            color = serializedObject.FindProperty("LineColor");
        }

        public override void OnInspectorGUI()
        {            
            base.OnInspectorGUI();

            Supershape2DRenderer supershapeRenderer = (Supershape2DRenderer) target;

            EditorGUI.BeginChangeCheck();

            // Supershape Reference
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                supershapeRenderer.Supershape = (Supershape2D)EditorGUILayout.ObjectField("Supershape Reference", supershapeRenderer.Supershape, typeof(Supershape2D), true);
                if (GUILayout.Button("Edit", GUILayout.Width(50))) Selection.activeObject = supershapeRenderer.Supershape;
                EditorGUILayout.EndHorizontal();

                if (check.changed && targets.Length > 1)
                {
                    foreach (Supershape2DRenderer obj in targets)
                    {
                        obj.Supershape = supershapeRenderer.Supershape;
                    }
                }
            }
            
            // Supershape Parameters quick-edit
            var supershape = supershapeRenderer.Supershape;
            if (supershape != null)
            {

                EditorGUILayout.Space(5);
                showParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showParameters, "Parameters");
                EditorGUI.BeginDisabledGroup(supershape.Lock);
                if (showParameters)
                {

                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        float a, b, m, n1, n2, n3;
                        a = EditorGUILayout.Slider("A", supershape.A, supershape.AMin, supershape.AMax);
                        b = EditorGUILayout.Slider("B", supershape.B, supershape.BMin, supershape.BMax);
                        m = EditorGUILayout.Slider("M", supershape.M, supershape.MMin, supershape.MMax);
                        n1 = EditorGUILayout.Slider("N1", supershape.N1, supershape.N1Min, supershape.N1Max);
                        n2 = EditorGUILayout.Slider("N2", supershape.N2, supershape.N2Min, supershape.N2Max);
                        n3 = EditorGUILayout.Slider("N3", supershape.N3, supershape.N3Min, supershape.N3Max);

                        if (check.changed)
                        {
                            supershapeRenderer.Supershape.A = a;
                            supershapeRenderer.Supershape.B = b;
                            supershapeRenderer.Supershape.M  = m;
                            supershapeRenderer.Supershape.N1 = n1;
                            supershapeRenderer.Supershape.N2 = n2;
                            supershapeRenderer.Supershape.N3 = n3;
                        }   
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUI.EndDisabledGroup();
            }

            // Render Properties
            EditorGUILayout.Space(5);
            showProperties = EditorGUILayout.BeginFoldoutHeaderGroup(showProperties, "Render Properties");

            if (showProperties)
            {
                supershapeRenderer.SamplePoints = EditorGUILayout.IntField("Sample Points", supershapeRenderer.SamplePoints);
                supershapeRenderer.LineThickness = EditorGUILayout.FloatField("Line Thickness", supershapeRenderer.LineThickness);
                supershapeRenderer.LineColor = EditorGUILayout.ColorField("Line Color", supershapeRenderer.LineColor);
                
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            //Propagate to multi-obj editing
            if (EditorGUI.EndChangeCheck())
            {
                if(targets.Length > 1)
                {
                    foreach (var obj in targets)
                    {
                        Supershape2DRenderer renderer = (Supershape2DRenderer)obj;
                        renderer.SamplePoints = supershapeRenderer.SamplePoints;
                        renderer.LineThickness = supershapeRenderer.LineThickness;
                        renderer.LineColor = supershapeRenderer.LineColor;
                    }
                }
                SceneView.RepaintAll();
            }

        }
    }
}