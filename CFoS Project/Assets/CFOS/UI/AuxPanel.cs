using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.UI;
using CFoS.Supershape;

public class AuxPanel : MonoBehaviour
{
    [Header("Supershape")]
    public List<Supershape2DQuadRenderer> Renderers;
    public ParticleSystem Confetti;

    [Header("Sliders")]
    public UISlider1D ASlider;
    public UISlider1D BSlider;
    public UISlider1D MSlider;
    public UISlider2D HSSlider;

    [Space(5)]
    public UIButton ResetButton;

    private Supershape2D.Data defaultData;

    // Start is called before the first frame update
    void Start()
    {
        defaultData = Renderers[0].Supershape.GetData();

        ASlider.ValueChangedEvent.AddListener(ValueChange);
        BSlider.ValueChangedEvent.AddListener(ValueChange);
        MSlider.ValueChangedEvent.AddListener(ValueChange);
        HSSlider.ValueChangedEvent.AddListener(HueChange);

        ResetButton.ButtonClickEvent.AddListener(Reset);
    }

    private void ValueChange()
    {
        var data = Renderers[0].Supershape.GetData();
        data.A = ASlider.Value;
        data.B = BSlider.Value;
        data.M = MSlider.Value;
        Renderers[0].Supershape.SetData(data);
    }

    private void HueChange()
    {
        foreach(var renderer in Renderers)
        {
            renderer.Color = Color.HSVToRGB(HSSlider.Value.x, HSSlider.Value.y, 1.0f);
        }
    }

    private void Reset()
    {
        ASlider.ResetValue();
        BSlider.ResetValue();
        MSlider.ResetValue();
        HSSlider.ResetValue();

        Confetti.Play();
        Renderers[0].Supershape.SetData(defaultData);
    }
}
