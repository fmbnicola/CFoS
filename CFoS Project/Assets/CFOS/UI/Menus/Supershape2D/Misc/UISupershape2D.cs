using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI.Menus
{
    public class UISupershape2D : UIMenu
    {
        public CFoS.Supershape.Supershape2DRenderer Renderer;

        [Space(10)]
        public CFoS.UI.UISlider1D N1Slider;
        public CFoS.UI.UISlider1D N2Slider;
        public CFoS.UI.UISlider1D N3Slider;
        [Space(5)]
        public CFoS.UI.UISlider1D MSlider;

        protected bool needUpdate = false;

        protected virtual void Start()
        {
            // Init sliders
            N1Slider.Min = Renderer.Supershape.N1Min;
            N1Slider.Max = Renderer.Supershape.N1Max;
            N1Slider.Value = Renderer.Supershape.N1;

            N2Slider.Min = Renderer.Supershape.N2Min;
            N2Slider.Max = Renderer.Supershape.N2Max;
            N2Slider.Value = Renderer.Supershape.N2;

            N3Slider.Min = Renderer.Supershape.N3Min;
            N3Slider.Max = Renderer.Supershape.N3Max;
            N3Slider.Value = Renderer.Supershape.N3;

            MSlider.Min = Renderer.Supershape.MMin;
            MSlider.Max = Renderer.Supershape.MMax;
            MSlider.Value = Renderer.Supershape.M;

            // hook to slider events
            N1Slider.ValueChangedEvent.AddListener(UpdateValues);
            N2Slider.ValueChangedEvent.AddListener(UpdateValues);
            N3Slider.ValueChangedEvent.AddListener(UpdateValues);
            MSlider.ValueChangedEvent.AddListener(UpdateValues);
        }

        protected virtual void Update()
        {
            if (needUpdate)
            {
                Renderer.Supershape.N1 = N1Slider.Value;
                Renderer.Supershape.N2 = N2Slider.Value;
                Renderer.Supershape.N3 = N3Slider.Value;
                Renderer.Supershape.M = MSlider.Value;
            }
            needUpdate = false;
        }

        protected void UpdateValues()
        {
            needUpdate = true;
        }
    }
}