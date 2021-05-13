using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEditor;

namespace CFoS.UI
{
    public class ThumbnailQuad : MonoBehaviour
    {
        public GameObject ThumbnailLineAsset;

        [Header("Properties")]
        public float Width = 1.0f;
        public float Height = 1.0f;
        public float Scaling = 0.5f;

        [Space(10)][SerializeField]
        public int Index = 0;

        [SerializeField]
        public List<ThumbnailLine> Lines = new List<ThumbnailLine>();

        // Manage Lines
        public void AddLine()
        {
#if UNITY_EDITOR
            var newObject = PrefabUtility.InstantiatePrefab(ThumbnailLineAsset, transform) as GameObject;
#else
            var newObject = Instantiate(ThumbnailLineAsset, transform);
#endif
            var newLine = newObject.GetComponent<ThumbnailLine>();

            // Set Properties
            newLine.Index = new Vector2Int(Lines.Count, Index);
            newLine.Length = Width;
            newLine.Scaling = Scaling;

            // Set Thumbnails
            var num = (Lines.Count == 0) ? 0 : Lines[0].Thumbnails.Count;
            newLine.SetThumbnailNumber(num);

            Lines.Add(newLine);
            UpdateTransforms();
        }

        public void AddLines(int num)
        {
            for (var i = 0; i < num; i++)
            {
                AddLine();
            }
        }

        public void RemoveLine()
        {
            var count = Lines.Count;
            if (count != 0)
            {
                var line = Lines[count - 1];
                Lines.Remove(line);

#if UNITY_EDITOR
                DestroyImmediate(line.gameObject);
#else
                Destroy(line.gameObject);
#endif
                UpdateTransforms();
            }
        }

        public void RemoveLines(int num)
        {
            for (var i = 0; i < num; i++)
            {
                RemoveLine();
            }
        }

        public void ClearAllLines()
        {
            foreach (var line in Lines)
            {
                line.ClearAllThumbnails();
#if UNITY_EDITOR
                DestroyImmediate(line.gameObject);
#else
                Destroy(line.gameObject);
#endif
            }
            Lines.Clear();
        }

        public void SetLineNumber(int num)
        {
            var count = Lines.Count;
            if (count < num)
            {
                AddLines(num - count);
            }
            else if (count > num)
            {
                RemoveLines(count - num);
            }
        }


        // Manage Thumbnails
        public void AddThumbnail()
        {
            foreach(var line in Lines)
            {
                line.AddThumbnail();
            }
        }

        public void AddThumbnails(int num)
        {
            for (var i = 0; i < num; i++)
            {
                AddThumbnail();
            }
        }

        public void RemoveThumbnail()
        {
            foreach (var line in Lines)
            {
                line.RemoveThumbnail();
            }
        }

        public void RemoveThumbnails(int num)
        {
            for (var i = 0; i < num; i++)
            {
                RemoveThumbnail();
            }
        }

        public void SetThumbnailNumber(int num)
        {
            foreach (var line in Lines)
            {
                line.SetThumbnailNumber(num);
            }
        }


        // Sampling + Selection
        public void SetSampleFunction(Thumbnail.SamplingFunction function)
        {
            foreach (var line in Lines)
            {
                line.SetSampleFunction(function);
            }
        }

        public void UpdateSampling()
        {
            foreach (var line in Lines)
            {
                line.UpdateSampling();
            }
        }

        public void SetSelectFunction(Thumbnail.SelectingFunction function)
        {
            foreach (var line in Lines)
            {
                line.SetSelectFunction(function);
            }
        }

        public void SetUpdateFunction(Thumbnail.UpdatingFunction function)
        {
            foreach (var line in Lines)
            {
                line.SetUpdateFunction(function);
            }
        }


        // Update
        public void UpdateTransforms()
        {
            var count = Lines.Count;
            var offset = (count <= 1) ? 0.0f : Height / (count - 1);

            for (var i = 0; i < count; i++)
            {
                var line = Lines[i];
                line.Length = Width;
                line.Scaling = Scaling;

                // row position
                line.transform.localPosition = Vector3.up * (offset * i);

                // line positioning
                line.UpdateTransforms();
            }
        }

        [ExecuteAlways]
        private void OnValidate()
        {
            UpdateTransforms();
        }

    }
}
