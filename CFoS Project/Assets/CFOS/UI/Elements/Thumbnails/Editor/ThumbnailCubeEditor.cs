using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.UI
{
    [CustomEditor(typeof(ThumbnailCube))]
    public class ThumbnailCubeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ThumbnailCube cube = (ThumbnailCube) target;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Quad", GUILayout.Height(30)))
            {
                cube.AddQuad();
            }
            if (GUILayout.Button("Remove Quad", GUILayout.Height(30)))
            {
                cube.RemoveQuad();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Line", GUILayout.Height(30)))
            {
                cube.AddLine();
            }
            if (GUILayout.Button("Remove Line", GUILayout.Height(30)))
            {
                cube.RemoveLine();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Thumbnail", GUILayout.Height(30)))
            {
                cube.AddThumbnail();
            }
            if (GUILayout.Button("Remove Thumbnail", GUILayout.Height(30)))
            {
                cube.RemoveThumbnail();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Height(30)))
            {
                cube.ClearAllQuads();
            }
            GUILayout.EndHorizontal();
           
        }
    }
}