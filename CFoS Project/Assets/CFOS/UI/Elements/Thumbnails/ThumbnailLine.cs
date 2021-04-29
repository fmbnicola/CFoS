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

        [SerializeField]
        public List<Thumbnail> Thumbnails = new List<Thumbnail>();
        private bool countChanged = false;

        protected delegate Supershape2D.Data samplingFunction(float pos);
        protected samplingFunction function;


        void Start()
        {

        }

        public void AddThumbnail()
        {
            var newObject       = Instantiate(ThumbnailAsset, transform);
            var newThumbnail    = newObject.GetComponent<Thumbnail>();
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

        public void PositionThumbnails()
        {
            var count  = Thumbnails.Count;
            var space  = (Spacing * transform.localScale);
            var offset = -(space * (count - 1)) / 2;

            for (var i = 0; i < count; i++)
            {
                var thumbnail = Thumbnails[i];
                thumbnail.transform.localPosition = Vector3.Scale(Vector3.right, (offset + (space * i)));
            }
        }

        public void UpdatePosition()
        {
            if (countChanged || transform.hasChanged)
            {
                PositionThumbnails();
            }
        }

        public void UpdateSampling()
        {
            foreach (var thumbnail in Thumbnails)
            {
                var parameters = function(thumbnail.transform.localPosition.x);
                thumbnail.UpdateParameters(parameters);
            }
        }

        [ExecuteAlways]
        private void OnValidate()
        {
            UpdatePosition();
        }

        void Update()
        {
            UpdatePosition();
            UpdateSampling();
        }


    }
}
