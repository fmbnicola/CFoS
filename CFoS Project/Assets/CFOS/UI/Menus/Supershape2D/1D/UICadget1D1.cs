using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget1D1 : UIMenu
    {
        [Header("Slider")]
        public CFoS.UI.UISlider1D Slider;

        [Header("Supershape")]
        public Supershape2DRenderer Renderer;
       

        protected virtual void Start()
        {
            // Render Update
            Slider.ValueChangedEvent.AddListener(ValueChange);
            ValueChange();
        }

        protected virtual void ValueChange()
        {
            var val = Slider.Value;
            Renderer.Supershape.N1 = val;
            Renderer.Supershape.N2 = val;
            Renderer.Supershape.N3 = val;
        }
    }
}