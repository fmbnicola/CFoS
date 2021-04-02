using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{
    public class UIWindow : MonoBehaviour
    {
        [Header("Handle")]
        public Shapes.ShapeRenderer Handle;
        public Data.ColorVariable HandleNormalColor;
        public Data.ColorVariable HandleHoverColor;

        [Header("Panel")]
        public Shapes.ShapeRenderer Panel;
        public Data.ColorVariable PanelColor;

        // Init
        [ExecuteAlways]
        private void OnValidate()
        {
            Handle.Color = HandleNormalColor.Value;
            Panel.Color = PanelColor.Value;
        }

        public void Hover(bool val)
        {
            Handle.Color = val ? HandleHoverColor.Value: HandleNormalColor.Value;
        }
    }
}





