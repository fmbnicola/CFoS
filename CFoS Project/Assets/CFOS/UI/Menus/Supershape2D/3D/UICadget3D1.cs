using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;

namespace CFoS.UI.Menus
{
    public class UICadget3D1 : UIMenu
    {
        [Header("Slider")]
        public CFoS.UI.UISlider1D SliderN1;
        public CFoS.UI.UISlider1D SliderN2;
        public CFoS.UI.UISlider1D SliderN3;

        [Header("Supershape")]
        public Supershape2DRenderer Renderer;
       

        protected virtual void Start()
        {
            // Render Update
            SliderN1.ValueChangedEvent.AddListener(ValueChangeN1);
            SliderN2.ValueChangedEvent.AddListener(ValueChangeN2);
            SliderN3.ValueChangedEvent.AddListener(ValueChangeN3);

            ValueChangeN1();
            ValueChangeN2();
            ValueChangeN3();
        }

        protected virtual void ValueChangeN1()
        {
            Renderer.Supershape.N1 = SliderN1.Value;
        }

        protected virtual void ValueChangeN2()
        {
            Renderer.Supershape.N2 = SliderN2.Value;
        }

        protected virtual void ValueChangeN3()
        {
            Renderer.Supershape.N3 = SliderN3.Value;
        }
    }
}