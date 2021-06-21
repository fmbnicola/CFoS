using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget2D1 : UIMenu
    {
        [Header("Slider")]
        public CFoS.UI.UISlider1D SliderN1;
        public CFoS.UI.UISlider1D SliderN23;

        [Header("Supershape")]
        public Supershape2DRenderer Renderer;
       

        protected virtual void Start()
        {
            // Render Update
            SliderN1.ValueChangedEvent.AddListener(ValueChangeN1);
            SliderN23.ValueChangedEvent.AddListener(ValueChangeN23);

            ValueChangeN1();
            ValueChangeN23();
        }

        public override void ResetMenu()
        {
            SliderN1.ResetValue();
            SliderN23.ResetValue();
        }

        protected virtual void ValueChangeN1()
        {
            Renderer.Supershape.N1 = SliderN1.Value;
        }

        protected virtual void ValueChangeN23()
        {
            Renderer.Supershape.N2 = SliderN23.Value;
            Renderer.Supershape.N3 = SliderN23.Value;
        }
    }
}