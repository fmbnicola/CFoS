using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEditor;

namespace CFoS.UI
{
    public class ThumbnailCube : MonoBehaviour
    {
        public GameObject ThumbnailQuadAsset;

        [Header("Properties")]
        public float Width  = 1.0f;
        public float Height = 1.0f;
        public float Depth  = 1.0f;
        public float Scaling = 0.5f;

        [SerializeField]
        public List<ThumbnailQuad> Quads = new List<ThumbnailQuad>();


        // Manage Quads
        public void AddQuad()
        {
#if UNITY_EDITOR
            var newObject = PrefabUtility.InstantiatePrefab(ThumbnailQuadAsset, transform) as GameObject;
#else
            var newObject = Instantiate(ThumbnailQuadAsset, transform);
#endif
            var newQuad = newObject.GetComponent<ThumbnailQuad>();

            // Set Properties
            newQuad.Index   = Quads.Count;
            newQuad.Width   = Width;
            newQuad.Height  = Height;
            newQuad.Scaling = Scaling;

            // Set Lines
            var num = 0;
            if (Quads.Count != 0 && Quads[0].Lines.Count != 0)
            {
                num = Quads[0].Lines[0].Thumbnails.Count;
                newQuad.SetThumbnailNumber(num);
            }

            Quads.Add(newQuad);
            UpdateTransforms();
        }

        public void AddQuads(int num)
        {
            for (var i = 0; i < num; i++)
            {
                AddQuad();
            }
        }

        public void RemoveQuad()
        {
            var count = Quads.Count;
            if (count != 0)
            {
                var quad = Quads[count - 1];
                Quads.Remove(quad);

#if UNITY_EDITOR
                DestroyImmediate(quad.gameObject);
#else
                Destroy(quad.gameObject);
#endif
                UpdateTransforms();
            }
        }

        public void RemoveQuads(int num)
        {
            for (var i = 0; i < num; i++)
            {
                RemoveQuad();
            }
        }

        public void ClearAllQuads()
        {
            foreach (var quad in Quads)
            {
                quad.ClearAllLines();
#if UNITY_EDITOR
                DestroyImmediate(quad.gameObject);
#else
                Destroy(quad.gameObject);
#endif
            }
            Quads.Clear();
        }

        public void SetQuadNumber(int num)
        {
            var count = Quads.Count;
            if (count < num)
            {
                AddQuads(num - count);
            }
            else if (count > num)
            {
                RemoveQuads(count - num);
            }
        }


        // Manage Lines
        public void AddLine()
        {
            foreach (var quad in Quads)
            {
                quad.AddLine();
            }
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
            foreach (var quad in Quads)
            {
                quad.RemoveLine();
            }
        }

        public void RemoveLines(int num)
        {
            for (var i = 0; i < num; i++)
            {
                RemoveLine();
            }
        }

        public void SetLineNumber(int num)
        {
            foreach (var quad in Quads)
            {
                quad.SetLineNumber(num);
            }
        }


        // Manage Thumbnails
        public void AddThumbnail()
        {
            foreach (var quad in Quads)
            {
                quad.AddThumbnail();
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
            foreach (var quad in Quads)
            {
                quad.RemoveThumbnail();
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
            foreach (var quad in Quads)
            {
                quad.SetThumbnailNumber(num);
            }
        }


        // Sampling + Selection
        public void SetSampleFunction(Thumbnail.SamplingFunction function)
        {
            foreach (var quad in Quads)
            {
                quad.SetSampleFunction(function);
            }
        }

        public void UpdateSampling()
        {
            foreach (var quad in Quads)
            {
                quad.UpdateSampling();
            }
        }

        public void SetSelectFunction(Thumbnail.SelectingFunction function)
        {
            foreach (var quad in Quads)
            {
                quad.SetSelectFunction(function);
            }
        }

        public void SetUpdateFunction(Thumbnail.UpdatingFunction function)
        {
            foreach (var quad in Quads)
            {
                quad.SetUpdateFunction(function);
            }
        }


        // Update
        public void UpdateTransforms()
        {
            var count = Quads.Count;
            var offset = (count <= 1) ? 0.0f : Depth / (count - 1);

            for (var i = 0; i < count; i++)
            {
                var quad = Quads[i];
                quad.Width = Width;
                quad.Height = Height;
                quad.Scaling = Scaling;

                // row position
                quad.transform.localPosition = Vector3.forward * (offset * i);

                // line positioning
                quad.UpdateTransforms();
            }
        }

        [ExecuteAlways]
        private void OnValidate()
        {
            UpdateTransforms();
        }

    }
}
