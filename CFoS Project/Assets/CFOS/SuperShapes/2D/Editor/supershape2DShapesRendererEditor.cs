using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [CustomEditor(typeof(Supershape2DShapesRenderer),true)]
    [CanEditMultipleObjects]
    public class Supershape2DShapesRendererEditor : Supershape2DRendererEditor
    {
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

            Supershape2DShapesRenderer supershapeRenderer = (Supershape2DShapesRenderer)target;

            // Render Properties
            EditorGUILayout.Space(5);
            showProperties = EditorGUILayout.BeginFoldoutHeaderGroup(showProperties, "Render Properties");

            if (showProperties)
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    supershapeRenderer.SamplePoints = EditorGUILayout.IntField("Sample Points", supershapeRenderer.SamplePoints);
                    supershapeRenderer.LineThickness = EditorGUILayout.FloatField("Line Thickness", supershapeRenderer.LineThickness);
                    supershapeRenderer.LineColor = EditorGUILayout.ColorField("Line Color", supershapeRenderer.LineColor);

                    if(check.changed) EditorUtility.SetDirty(supershapeRenderer);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            //Propagate to multi-obj editing
            if (EditorGUI.EndChangeCheck())
            {
                if(targets.Length > 1)
                {
                    foreach (var obj in targets)
                    {
                        Supershape2DShapesRenderer renderer = (Supershape2DShapesRenderer)obj;
                        renderer.SamplePoints = supershapeRenderer.SamplePoints;
                        renderer.LineThickness = supershapeRenderer.LineThickness;
                        renderer.LineColor = supershapeRenderer.LineColor;

                        EditorUtility.SetDirty(renderer);
                    }
                }
                SceneView.RepaintAll();
            }

        }
    }
}