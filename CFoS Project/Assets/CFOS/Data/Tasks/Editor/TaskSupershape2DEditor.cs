using CFoS.Supershape;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Data
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TaskSupershape2D))]
    public class TaskSupershape2DEditor : Editor
    {
        bool showStartingParameters = false;
        bool showTargetParameters = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TaskSupershape2D task = (TaskSupershape2D) target;

            EditorGUILayout.BeginHorizontal();
            task.StartingSupershape = (Supershape2D)EditorGUILayout.ObjectField("Starting Supershape", task.StartingSupershape, typeof(Supershape2D), true);
            if (GUILayout.Button("Edit", GUILayout.Width(50))) Selection.activeObject = task.StartingSupershape;
            EditorGUILayout.EndHorizontal();
            SupershapeEdit(task.StartingSupershape, ref showStartingParameters);

            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            task.TargetSupershape = (Supershape2D)EditorGUILayout.ObjectField("Target Supershape", task.TargetSupershape, typeof(Supershape2D), true);
            if (GUILayout.Button("Edit", GUILayout.Width(50))) Selection.activeObject = task.TargetSupershape;
            EditorGUILayout.EndHorizontal();
            SupershapeEdit(task.TargetSupershape, ref showTargetParameters);
        }

        private void SupershapeEdit(Supershape2D supershape, ref bool showParams)
        {
            if (supershape != null)
            {
                showParams = EditorGUILayout.BeginFoldoutHeaderGroup(showParams, "Parameters");
                EditorGUI.BeginDisabledGroup(supershape.Lock);
                if (showParams)
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
                            supershape.A = a;
                            supershape.B = b;
                            supershape.M = m;
                            supershape.N1 = n1;
                            supershape.N2 = n2;
                            supershape.N3 = n3;

                            EditorUtility.SetDirty(supershape);
                        }
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUI.EndDisabledGroup();
            }
        }

    }
}