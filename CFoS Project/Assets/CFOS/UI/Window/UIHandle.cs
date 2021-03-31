using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{
    public class UIHandle : MonoBehaviour
    {
        public Shapes.ShapeRenderer ShapeRenderer;
        public Data.ColorReference HoverColor;

        public void Hover(bool val)
        {
            ShapeRenderer.Color = val ? HoverColor.Value: Color.white;
        }
    }
}





