using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI
{
    public class ThumbnailLine : MonoBehaviour
    {
        [SerializeField]
        public List<Supershape2DRenderer> Thumbnails = new List<Supershape2DRenderer>();

        public GameObject ThumbnailAsset;

        protected delegate Supershape2D.Data samplingFunction(float pos);

        protected samplingFunction function;



        void Start()
        {

        }



        protected void AddThumbnail()
        {
            var newObject       = Instantiate(ThumbnailAsset);
            var newThumbnail    = newObject.GetComponent<Supershape2DRenderer>();
            Thumbnails.Add(newThumbnail);
        }

        protected void AddThumbnails(int num)
        {
            for (var i = 0; i < num; i++)
            {
                AddThumbnail();
            }
        }

        protected void RemoveThumbnail()
        {
            var count = Thumbnails.Count;
            if (count != 0)
            {
                var thumbnail = Thumbnails[count - 1];
                Destroy(thumbnail.gameObject);
            }
        }

        protected void RemoveThumbnails(int num)
        {
            for (var i = 0; i < num; i++)
            {
                RemoveThumbnail();
            }
        }

        protected void ClearAllThumbnails()
        {
            foreach (var thumbnail in Thumbnails)
            {
                Destroy(thumbnail.gameObject);
            }
            Thumbnails.Clear();
        }

        protected void SetThumbnailNumber(int num)
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

        void Update()
        {

        }


    }
}
