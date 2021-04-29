using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI
{
    public class ThumbnailLine : MonoBehaviour
    {
        public GameObject ThumbnailAsset;

        public float Spacing = 1.0f;
        public float Scaling = 0.5f;

        [SerializeField]
        public List<Thumbnail> Thumbnails = new List<Thumbnail>();
        private bool countChanged = false;

        public Thumbnail.SamplingFunction Function;


        public void AddThumbnail()
        {
            var newObject       = Instantiate(ThumbnailAsset, transform);
            var newThumbnail    = newObject.GetComponent<Thumbnail>();

            newThumbnail.Index = new Vector3(Thumbnails.Count, 0 , 0);
            Thumbnails.Add(newThumbnail);

            countChanged = true;
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

                countChanged = true;
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

            countChanged = true;
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

        public void UpdateTransforms()
        {
            var count = Thumbnails.Count;
            var offset = -(Spacing * (count - 1)) / 2;

            for (var i = 0; i < count; i++)
            {
                var thumbnail = Thumbnails[i];

                // position
                thumbnail.transform.localPosition = Vector3.right * (offset + (Spacing * i));

                // scale
                thumbnail.transform.localScale = Vector3.one * Scaling;
            }
        }

        public void UpdateSampling()
        {
            foreach (var thumbnail in Thumbnails)
            {
                thumbnail.Function = Function;
                thumbnail.UpdateSampling();
            }
        }

        [ExecuteAlways]
        private void OnValidate()
        {
            UpdateTransforms();
        }
    }
}
