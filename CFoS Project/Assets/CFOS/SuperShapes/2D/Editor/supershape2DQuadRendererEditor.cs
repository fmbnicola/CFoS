using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [CustomEditor(typeof(Supershape2DQuadRenderer),true)]
    [CanEditMultipleObjects]
    public class Supershape2DQuadRendererEditor : Supershape2DRendererEditor
    {
        bool showProperties = true;

        SerializedProperty color;
        SerializedProperty scale;

        public void OnEnable()
        {
            color = serializedObject.FindProperty("Color");
            scale = serializedObject.FindProperty("Scale");
        }

        public override void OnInspectorGUI()
        {            
            base.OnInspectorGUI();

            Supershape2DQuadRenderer supershapeRenderer = (Supershape2DQuadRenderer)target;

            // Render Properties
            EditorGUILayout.Space(5);
            showProperties = EditorGUILayout.BeginFoldoutHeaderGroup(showProperties, "Render Properties");

            if (showProperties)
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    supershapeRenderer.Color = EditorGUILayout.ColorField("Color", supershapeRenderer.Color);
                    supershapeRenderer.Scale = EditorGUILayout.FloatField("Scale", supershapeRenderer.Scale);

                    if (check.changed) EditorUtility.SetDirty(supershapeRenderer);
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
                        Supershape2DQuadRenderer renderer = (Supershape2DQuadRenderer)obj;
                        renderer.Color = supershapeRenderer.Color;
                        renderer.Scale = supershapeRenderer.Scale;

                        EditorUtility.SetDirty(renderer);
                    }
                }
                SceneView.RepaintAll();
            }

        }
    }
}