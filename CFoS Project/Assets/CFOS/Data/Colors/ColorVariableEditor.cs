using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Data
{
    [CustomEditor(typeof(ColorVariable))]
    public class Supershape2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ColorVariable color = (ColorVariable) target;

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                Color val = EditorGUILayout.ColorField("Color", color.Value);

                // Push change
                if (check.changed)
                {
                    color.Value = val;
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
