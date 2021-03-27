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

            serializedObject.Update();
            Supershape2DRenderer supershapeRenderer = (Supershape2DRenderer) target;

            // Supershape Reference
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginHorizontal();
            supershapeRenderer.Supershape = (Supershape2D) EditorGUILayout.ObjectField("Supershape Reference", supershapeRenderer.Supershape, typeof(Supershape2D), true);
            if (GUILayout.Button("Edit", GUILayout.Width(50))) Selection.activeObject = supershapeRenderer.Supershape;
            EditorGUILayout.EndHorizontal();
            
            // Supershape Parameters quick-edit
            var supershape = supershapeRenderer.Supershape;
            if (supershape != null)
            {
                EditorGUILayout.Space(5);
                showParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showParameters, "Parameters");
                if (showParameters)
                {
                    supershapeRenderer.Supershape.A = EditorGUILayout.Slider("A", supershape.A, supershape.AMin, supershape.AMax);
                    supershapeRenderer.Supershape.B = EditorGUILayout.Slider("B", supershape.B, supershape.BMin, supershape.BMax);

                    supershapeRenderer.Supershape.M = EditorGUILayout.Slider("M", supershape.M, supershape.MMin, supershape.MMax);

                    supershapeRenderer.Supershape.N1 = EditorGUILayout.Slider("N1", supershape.N1, supershape.N1Min, supershape.N1Max);
                    supershapeRenderer.Supershape.N2 = EditorGUILayout.Slider("N2", supershape.N2, supershape.N2Min, supershape.N2Max);
                    supershapeRenderer.Supershape.N3 = EditorGUILayout.Slider("N3", supershape.N3, supershape.N3Min, supershape.N3Max);

                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            // Render Properties
            EditorGUILayout.Space(5);
            showProperties = EditorGUILayout.BeginFoldoutHeaderGroup(showProperties, "Render Properties");

            if (showProperties)
            {
                supershapeRenderer.SamplePoints = EditorGUILayout.IntField("Sample Points", supershapeRenderer.SamplePoints);
                EditorGUILayout.PropertyField(thickness);
                EditorGUILayout.PropertyField(color);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            //Propagate to multi-obj editing
            foreach (var obj in targets)
            {
                Supershape2DRenderer renderer = (Supershape2DRenderer)obj;
                renderer.SamplePoints = supershapeRenderer.SamplePoints;
                renderer.Supershape = supershapeRenderer.Supershape;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}