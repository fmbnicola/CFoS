using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget2D2 : UIMenu
    {
        [Header("2D Slider")]
        public CFoS.UI.UISlider2D SliderN1_N23;

        [Header("Supershape")]
        public Supershape2DRenderer Renderer;


        protected void Start()
        {
            // Render Update
            SliderN1_N23.ValueChangedEvent.AddListener(ValueChangeN123);

        }


        // SLIDER 
        protected virtual void ValueChangeN123()
        {
            Renderer.Supershape.N1 = SliderN1_N23.Value.x;
            Renderer.Supershape.N2 = SliderN1_N23.Value.y;
            Renderer.Supershape.N3 = SliderN1_N23.Value.y;
        }


    }
}