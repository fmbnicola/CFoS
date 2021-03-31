using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
    [ExecuteAlways]
    public class ShapeColorSetter : MonoBehaviour
    {
        public ShapeRenderer ShapeRenderer;
        public CFoS.Data.ColorReference Color;

        public void SetColor()
        {
            ShapeRenderer.Color = Color.Value;
        }

        void OnValidate()
        {
            SetColor();
        }

        void OnEnable()
        {
            Color.Variable.OnUpdate += SetColor;
        }

        void OnDisable()
        {
            Color.Variable.OnUpdate -= SetColor;
        }

        private void OnDestroy()
        {
            Color.Variable.OnUpdate -= SetColor;
        }
    }
}


