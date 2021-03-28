using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Supershape2D))]
    public class Supershape2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Supershape2D supershape = (Supershape2D)target;

            // Lock/Unlock Button
            GUI.contentColor = supershape.Lock ? new Color(0.0f, 0.8f, 0.0f) : new Color(0.9f, 0.0f, 0.0f);
            if (GUILayout.Button(supershape.Lock ? "UNLOCK" : "LOCK", GUILayout.Height(30)))
                supershape.Lock = !supershape.Lock;
            GUI.contentColor = Color.white;

            // Parameters
            EditorGUI.BeginDisabledGroup(supershape.Lock);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                float a, b, m, n1, n2, n3;
                EditorGUILayout.Space(10);
                a = CustomSlider("A", supershape.A, ref supershape.AMin, ref supershape.AMax);
                b = CustomSlider("B", supershape.B, ref supershape.BMin, ref supershape.BMax);
                EditorGUILayout.Space(40);
                m = CustomSlider("M", supershape.M, ref supershape.MMin, ref supershape.MMax);
                EditorGUILayout.Space(40);
                n1 = CustomSlider("N1", supershape.N1, ref supershape.N1Min, ref supershape.N1Max);
                n2 = CustomSlider("N2", supershape.N2, ref supershape.N2Min, ref supershape.N2Max);
                n3 = CustomSlider("N3", supershape.N3, ref supershape.N3Min, ref supershape.N3Max);

                // Push changes
                if (check.changed)
                {
                    supershape.A = a;
                    supershape.B = b;
                    supershape.M = m;
                    supershape.N1 = n1;
                    supershape.N2 = n2;
                    supershape.N3 = n3;

                    EditorUtility.SetDirty(target);
                }

                if (GUILayout.Button("Randomize", GUILayout.Height(30)))
                {
                    supershape.Randomize();
                    EditorUtility.SetDirty(supershape);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        public float CustomSlider(string label, float val, ref float min, ref float max)
        {
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(label, GUILayout.Width(20));

                GUILayout.BeginVertical();

                    val = EditorGUILayout.Slider(val, min, max);
                    GUILayout.BeginHorizontal();

                        min = EditorGUILayout.FloatField(min, GUILayout.MaxWidth(50));
                        GUILayout.FlexibleSpace();
                        max = EditorGUILayout.FloatField(max, GUILayout.MaxWidth(50));
                        GUILayout.Space(50);

                    GUILayout.EndHorizontal();

                GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            return val;
        }

    }
}