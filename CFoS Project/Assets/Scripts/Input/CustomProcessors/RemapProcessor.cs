using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class RemapProcessor : InputProcessor<float>
{
    [Header("Original Range")]
    public float fromMin = 0;
    public float fromMax = 1;

    [Header("Target Range")]
    public float toMin = 0;
    public float toMax = 1;

#if UNITY_EDITOR
    static RemapProcessor()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<RemapProcessor>();
    }

    public override float Process(float value, InputControl control)
    {
        return (value - fromMin) / (toMin - fromMin) * (toMax - fromMax) + fromMax;
    }
}
