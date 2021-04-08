using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI.Menus
{
    public class UISupershape2DLamegons : UIMenu
    {
        public CFoS.Supershape.Supershape2DRenderer Renderer;

        [Space(10)]
        public CFoS.UI.UISlider N1Slider;
        public CFoS.UI.UISlider N23Slider;

        [Space(5)]
        public CFoS.UI.UISlider MSlider;

        protected bool N1changed = false;
        protected bool N23changed = false;
        protected bool needUpdate = false;

        protected virtual void Start()
        {
            // Init sliders

            // N1
            N1Slider.Min = Renderer.Supershape.N1Min;
            N1Slider.Max = Renderer.Supershape.N1Max;
            N1Slider.Value = Renderer.Supershape.N1;

            // N23
            N23Slider.Min = Mathf.Min(Renderer.Supershape.N2Min, Renderer.Supershape.N3Min);
            Renderer.Supershape.N2Min = N23Slider.Min;
            Renderer.Supershape.N3Min = N23Slider.Min;

            N23Slider.Max = Mathf.Max(Renderer.Supershape.N2Max, Renderer.Supershape.N3Max);
            Renderer.Supershape.N2Max = N23Slider.Max;
            Renderer.Supershape.N3Max = N23Slider.Max;

            N23Slider.Value = Renderer.Supershape.N2;
            Renderer.Supershape.N2 = N23Slider.Value;
            Renderer.Supershape.N3 = N23Slider.Value;

            // M
            MSlider.Min = Renderer.Supershape.MMin;
            MSlider.Max = Renderer.Supershape.MMax;
            MSlider.Value = Renderer.Supershape.M;

            // hook to slider events
            N1Slider.ValueChangedEvent.AddListener(UpdateN1);
            N1Slider.ValueChangedEvent.AddListener(UpdateN23);
            N23Slider.ValueChangedEvent.AddListener(UpdateN23);
            MSlider.ValueChangedEvent.AddListener(UpdateValues);
        }

        protected virtual void Update()
        {
            if (N1changed)
            {
                var m   = MSlider.Value;
                var n1  = N1Slider.Value;
                var div = Mathf.Pow(m / 4, 2);
                div = Mathf.Approximately(div, 0.0f) ? Mathf.Epsilon : div;
                var n23 = n1 / div;

                MSlider.Value = m;
                N1Slider.Value = n1;
                N23Slider.Value = n23;

                Renderer.Supershape.N1  = n1;
                Renderer.Supershape.N2  = n23;
                Renderer.Supershape.N3  = n23;
                Renderer.Supershape.M   = m;
            }
            else if (N23changed)
            {
                var m   = MSlider.Value;
                var n23 = N23Slider.Value;
                var n1  = Mathf.Pow(m / 4, 2) * n23;

                MSlider.Value = m;
                N1Slider.Value = n1;
                N23Slider.Value = n23;

                Renderer.Supershape.N1  = n1;
                Renderer.Supershape.N2  = n23;
                Renderer.Supershape.N3  = n23;
                Renderer.Supershape.M   = m;
            }
            else if (needUpdate)
            {
                Renderer.Supershape.N1 = N1Slider.Value;
                Renderer.Supershape.N2 = N23Slider.Value;
                Renderer.Supershape.N3 = N23Slider.Value;
                Renderer.Supershape.M = MSlider.Value;
            }
            N1changed = false;
            N23changed = false;
            needUpdate = false;
        }


        protected void UpdateN1()
        {
            N1changed = true;
        }

        protected void UpdateN23()
        {
            N23changed = true;
        }

        protected void UpdateValues()
        {
            needUpdate = true;
        }
    }
}
