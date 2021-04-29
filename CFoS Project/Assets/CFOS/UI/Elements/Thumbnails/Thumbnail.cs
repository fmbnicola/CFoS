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

        public Supershape2D Supershape { get; private set; }

        public delegate Supershape2D.Data SamplingFunction(Thumbnail thumbnail);
        public SamplingFunction Function;

        // Position index in line, quad or cube
        public Vector3 Index = Vector3.zero;

        void Awake()
        {
            supershapeRenderer = GetComponent<Supershape2DRenderer>();
            supershapeRenderer.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Supershape = supershapeRenderer.Supershape;
        }

        public void UpdateSampling()
        {
            var data = Function(this);
            supershapeRenderer.Supershape.SetData(data);
        }
    }
}