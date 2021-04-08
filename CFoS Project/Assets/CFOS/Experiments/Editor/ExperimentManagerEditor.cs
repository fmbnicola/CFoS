using UnityEditor;
using UnityEngine;

namespace CFoS.Experiments
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ExperimentManager))]
    public class ExperimentManagerEditor : Editor
    {
        ExperimentManager manager;

        public void OnEnable()
        {
            manager = (ExperimentManager) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Refresh Button
            if (GUILayout.Button("Refresh", GUILayout.Height(30)))
            {
                manager.Refresh();

                EditorUtility.SetDirty(target);
            }
        }
    }
}
