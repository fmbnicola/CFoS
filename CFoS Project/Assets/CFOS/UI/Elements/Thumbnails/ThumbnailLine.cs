using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEditor;

namespace CFoS.UI
{
    public class ThumbnailLine : MonoBehaviour
    {
        public GameObject ThumbnailAsset;

        [Header("Properties")]
        public float Length  = 1.0f;
        public float Scaling = 0.5f;

        [Space(10)][SerializeField]
        public Vector2Int Index = Vector2Int.zero;

        [SerializeField]
        public List<Thumbnail> Thumbnails = new List<Thumbnail>();

        // Manage Thumbnails
        public void AddThumbnail()
        {
#if UNITY_EDITOR
            var newObject       = PrefabUtility.InstantiatePrefab(ThumbnailAsset, transform) as GameObject;
#else
            var newObject       = Instantiate(ThumbnailAsset, transform);
#endif
            var newThumbnail    = newObject.GetComponent<Thumbnail>();

            newThumbnail.Index = new Vector3Int(Thumbnails.Count, Index.x, Index.y);
            Thumbnails.Add(newThumbnail);

            UpdateTransforms();
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
            var count = Thumbnails.Count;
            if (count != 0)
            {
                var thumbnail = Thumbnails[count - 1];
                Thumbnails.Remove(thumbnail);

#if UNITY_EDITOR
                DestroyImmediate(thumbnail.gameObject);
#else
                Destroy(thumbnail.gameObject);
#endif
                UpdateTransforms();
            }
        }

        public void RemoveThumbnails(int num)
        {
            for (var i = 0; i < num; i++)
            {
                RemoveThumbnail();
            }
        }

        public void ClearAllThumbnails()
        {
            foreach (var thumbnail in Thumbnails)
            {

#if UNITY_EDITOR
                DestroyImmediate(thumbnail.gameObject);
#else
                Destroy(thumbnail.gameObject);
#endif
            }
            Thumbnails.Clear();
        }

        public void SetThumbnailNumber(int num)
        {
            var count = Thumbnails.Count;
            if ( count < num )
            {
                AddThumbnails(num - count);
            }
            else if ( count > num )
            {
                RemoveThumbnails(count - num);
            }
        }


        // Sampling + Selection + Update
        public void SetSampleFunction(Thumbnail.SamplingFunction function)
        {
            foreach (var thumbnail in Thumbnails)
            {
                thumbnail.SampleFunction = function;
                thumbnail.UpdateSampling();
            }
        }

        public void UpdateSampling()
        {
            foreach (var thumbnail in Thumbnails)
            {
                thumbnail.UpdateSampling();
            }
        }

        public void SetSelectFunction(Thumbnail.SelectingFunction function)
        {
            foreach (var thumbnail in Thumbnails)
            {
                thumbnail.SelectFunction = function;
            }
        }

        public void SetUpdateFunction(Thumbnail.UpdatingFunction function)
        {
            foreach (var thumbnail in Thumbnails)
            {
                thumbnail.UpdateFunction = function;
            }
        }

        // Update
        public void UpdateTransforms()
        {
            var count = Thumbnails.Count;
            var offset = (count <= 1) ? 0.0f : Length / (count - 1);

            for (var i = 0; i < count; i++)
            {
                var thumbnail = Thumbnails[i];

                // position
                thumbnail.transform.localPosition = Vector3.right * (offset * i);

                // scale
                thumbnail.transform.localScale = Vector3.one * Scaling;
            }
        }

        [ExecuteAlways]
        private void OnValidate()
        {
            if (Thumbnails.Count == 0) return;
            UpdateTransforms();
        }

    }
}
