using CFoS.Supershape;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CFoS.UI
{
    [RequireComponent(typeof(Supershape2DRenderer))]
    public class Thumbnail : MonoBehaviour
    {
        private Supershape2DRenderer supershapeRenderer;

        void Start()
        {
            supershapeRenderer = GetComponent<Supershape2DRenderer>();
            supershapeRenderer.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
        }

        public void UpdateParameters(Supershape2D.Data data)
        {
            supershapeRenderer.Supershape.SetData(data);
        }
    }
}