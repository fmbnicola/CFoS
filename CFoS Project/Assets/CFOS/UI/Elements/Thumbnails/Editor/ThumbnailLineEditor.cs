using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.UI
{
    [CustomEditor(typeof(ThumbnailLine))]
    public class ThumbnailLineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ThumbnailLine line = (ThumbnailLine) target;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Thumbnail", GUILayout.Height(30)))
            {
                line.AddThumbnail();
            }

            if (GUILayout.Button("Remove Thumbnail", GUILayout.Height(30)))
            {
                line.RemoveThumbnail();
            }

            if (GUILayout.Button("Clear", GUILayout.Height(30)))
            {
                line.ClearAllThumbnails();
            }

            GUILayout.EndHorizontal();
           
        }
    }
}