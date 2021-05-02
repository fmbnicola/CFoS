using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.UI
{
    [CustomEditor(typeof(ThumbnailQuad))]
    public class ThumbnailQuadEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ThumbnailQuad quad = (ThumbnailQuad) target;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Line", GUILayout.Height(30)))
            {
                quad.AddLine();
            }
            if (GUILayout.Button("Remove Line", GUILayout.Height(30)))
            {
                quad.RemoveLine();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Thumbnail", GUILayout.Height(30)))
            {
                quad.AddThumbnail();
            }
            if (GUILayout.Button("Remove Thumbnail", GUILayout.Height(30)))
            {
                quad.RemoveThumbnail();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Height(30)))
            {
                quad.ClearAllLines();
            }
            GUILayout.EndHorizontal();
           
        }
    }
}