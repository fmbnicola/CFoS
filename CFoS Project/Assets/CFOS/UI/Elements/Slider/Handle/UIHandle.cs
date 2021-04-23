using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{

    public class UIHandle : UIElement
    {
        public Shapes.ShapeRenderer Handle;
        public Data.ColorVariable HandleNormalColor;
        public Data.ColorVariable HandleHoverColor;
        public Data.ColorVariable HandleSelectColor;

        [Space(10)]
        public Shapes.ShapeRenderer HandleOutline;
        public Data.ColorVariable HandleOutlineColor;

        [Space(10)]
        public TMPro.TextMeshPro HandleText;
        public Data.ColorVariable HandleTextColor;

        [Header("Properties")]
        public float size;

        protected override void OnValidate()
        {
            // TODO
        }
    }
}
