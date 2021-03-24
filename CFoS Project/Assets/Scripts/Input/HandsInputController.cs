using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;


public class FingerInputState
{
    private UnityEngine.XR.InputDevice? device;

    public enum FingerType { Thumb, Index, Middle, Ring, Pinky};
    public FingerType Type;

    private bool isPressed;
    public bool IsPressed { get { return isPressed; } set { } }

    private bool isTouched;
    public bool IsTouched { get { return isTouched; } set { } }

    private float value;
    public float Value { get { return value; } set { } }

    public FingerInputState(ref UnityEngine.XR.InputDevice? _device, FingerType type)
    {
        device = _device;
        Type = type;

        isPressed = false;
        isTouched = false;
        value = 0.0f;
    }

    public void Copy(FingerInputState other)
    {
        isTouched = other.IsTouched;
        isPressed = other.IsPressed;
        value = other.Value;
    }

    public void Update()
    {
        isPressed = false;
        isTouched = false;
        bool touch;
        bool press;
        float val;

        switch (Type)
        {
            case FingerType.Thumb:

                if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryTouch, out touch))
                {
                    isTouched |= touch;
                }
                else if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryTouch, out touch))
                {
                    isTouched |= touch;
                }
                else if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisTouch, out touch))
                {
                    isTouched |= touch;
                }
              
                if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out press))
                {
                    isPressed |= press;
                }
                else if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out press))
                {
                    isPressed |= press;
                }
                else if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out press))
                {
                    isPressed |= press;
                }

                value = isPressed ? 1.0f : (isTouched ? 0.5f : 0.0f);

                break;


            case FingerType.Index:

                if (device.Value.TryGetFeatureValue(Unity.XR.Oculus.OculusUsages.indexTouch, out touch))
                {
                    isTouched |= touch;
                }

                if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out val))
                {
                    isPressed = Mathf.Approximately(val, 1.0f);
                    val = ExtensionMethods.Remap(val, 0.0f, 1.0f, 0.5f, 1.0f);
                }

                value = isPressed ? 1.0f : (isTouched ? val : 0.0f);

                break;


            case FingerType.Middle:
            case FingerType.Ring:
            case FingerType.Pinky:

                // no sensor so we assume user is touching grip
                isTouched = true;
                value = 0.5f;

                if (device.Value.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out val))
                {
                    isPressed = Mathf.Approximately(val, 1.0f);
                    value = ExtensionMethods.Remap(val, 0.0f, 1.0f, 0.5f, 1.0f);
                }
                break;
        }
    }
}


public class HandInputState
{
    private UnityEngine.XR.InputDevice? device;
    public UnityEngine.XR.InputDevice? Device { get {return device; } set {}}

    public enum HandType { Left, Right };
    public HandType Type;

    private FingerInputState thumb;
    public FingerInputState Thumb { get { return thumb; } set { } }

    private FingerInputState index;
    public FingerInputState Index { get { return index; } set { } }

    private FingerInputState middle;
    public FingerInputState Middle { get { return middle; } set { } }

    private FingerInputState ring;
    public FingerInputState Ring { get { return ring; } set { } }

    private FingerInputState pinky;
    public FingerInputState Pinky { get { return pinky; } set { } }


    public HandInputState(ref UnityEngine.XR.InputDevice? _device, HandType type)
    {
        device = _device;
        Type = type;

        thumb   = new FingerInputState(ref device, FingerInputState.FingerType.Thumb);
        index   = new FingerInputState(ref device, FingerInputState.FingerType.Index);
        middle  = new FingerInputState(ref device, FingerInputState.FingerType.Middle);
        ring    = new FingerInputState(ref device, FingerInputState.FingerType.Ring);
        pinky   = new FingerInputState(ref device, FingerInputState.FingerType.Pinky);
    }

    public void Update()
    {
        thumb.Update();
        index.Update();

        // middle, ring and pinky are the same for common controllers
        middle.Update();
        ring.Copy(middle);
        pinky.Copy(middle);
    }
}


public class HandsInputController : MonoBehaviour
{
    [Header("Controllers")]
    UnityEngine.XR.InputDevice? LeftDevice;
    UnityEngine.XR.InputDevice? RightDevice;

    [SerializeField]
    [Tooltip("Left Hand InputState")]
    private HandInputState leftHand;
    public HandInputState LeftHand { get { return leftHand; } set { } }

    [SerializeField]
    [Tooltip("Right Hand InputState")]
    private HandInputState rightHand;
    public HandInputState RightHand { get { return rightHand; } set { } }


    public void SetupDevices()
    {
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count >= 1)
        {
            LeftDevice = leftHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", LeftDevice.Value.name, LeftDevice.Value.characteristics.ToString()));
        }
        else
        {
            Debug.Log("No device found for left hand!");
        }

        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count >= 1)
        {
            RightDevice = rightHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", RightDevice.Value.name, RightDevice.Value.characteristics.ToString()));
        }
        else
        {
            Debug.Log("No device found for right hand!");
        }
    }

    public void Start()
    {
        SetupDevices();

        leftHand = new HandInputState(ref LeftDevice, HandInputState.HandType.Left);
        rightHand = new HandInputState(ref RightDevice, HandInputState.HandType.Right);
    }

    public void Update()
    {
        if(!LeftDevice.HasValue || !RightDevice.HasValue)
        {
            SetupDevices();
            return;
        }

        leftHand.Update();
        rightHand.Update();
    }
}
