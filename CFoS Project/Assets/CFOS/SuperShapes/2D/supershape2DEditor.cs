using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Supershape2D))]
    public class Supershaper2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Supershape2D supershape = (Supershape2D) target;

            supershape.A = CustomSlider("A", supershape.A, ref supershape.AMin, ref supershape.AMax);
            supershape.B = CustomSlider("B", supershape.B, ref supershape.BMin, ref supershape.BMax);

            EditorGUILayout.Space(40);
            supershape.M = CustomSlider("M", supershape.M, ref supershape.MMin, ref supershape.MMax);

            EditorGUILayout.Space(40);
            supershape.N1 = CustomSlider("N1", supershape.N1, ref supershape.N1Min, ref supershape.N1Max);
            supershape.N2 = CustomSlider("N2", supershape.N2, ref supershape.N2Min, ref supershape.N2Max);
            supershape.N3 = CustomSlider("N3", supershape.N3, ref supershape.N3Min, ref supershape.N3Max);

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