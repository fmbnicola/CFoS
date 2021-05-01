using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIWindow))]
    public class UIWindowEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UIWindow window = (UIWindow) target;

            GUILayout.Space(10);
            if (GUILayout.Button("Rescale Content", GUILayout.Height(30)))
                window.RescaleContent();
        }
    }
}
