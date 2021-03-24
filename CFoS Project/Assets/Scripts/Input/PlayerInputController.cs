using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerInputController : MonoBehaviour
{
    #region Input Actions

    [SerializeField]
    [Tooltip("Move Input System Action [Vector2]")]
    private InputActionProperty moveAction;
    public InputActionProperty MoveAction
    {
        get => moveAction;
        set => SetInputActionProperty(ref moveAction, value);
    }

    [SerializeField]
    [Tooltip("Turn Input System Action [Vector2]")]
    InputActionProperty turnAction;
    public InputActionProperty TurnAction
    {
        get => turnAction;
        set => SetInputActionProperty(ref turnAction, value);
    }

    [SerializeField]
    [Tooltip("Crouch Input System Action [Vector2]")]
    private InputActionProperty crouchAction;
    public InputActionProperty CrouchAction
    {
        get => crouchAction;
        set => SetInputActionProperty(ref crouchAction, value);
    }

    [SerializeField]
    [Tooltip("Jump Input System Action [Button Press]")]
    private InputActionProperty jumpAction;
    public InputActionProperty JumpAction
    {
        get => jumpAction;
        set => SetInputActionProperty(ref jumpAction, value);
    }

    [SerializeField]
    [Tooltip("Reset Input System Action [Button Press]")]
    InputActionProperty resetAction;
    public InputActionProperty ResetAction
    {
        get => resetAction;
        set => SetInputActionProperty(ref resetAction, value);
    }

    #endregion

    protected void OnEnable()
    {
        moveAction.EnableDirectAction();
        turnAction.EnableDirectAction();
        crouchAction.EnableDirectAction();
        jumpAction.EnableDirectAction();
        resetAction.EnableDirectAction();
    }

    protected void OnDisable()
    {
        moveAction.DisableDirectAction();
        turnAction.DisableDirectAction();
        crouchAction.DisableDirectAction();
        jumpAction.DisableDirectAction();
        resetAction.DisableDirectAction();
    }

    void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
    {
        if (Application.isPlaying)
            property.DisableDirectAction();

        property = value;

        if (Application.isPlaying && isActiveAndEnabled)
            property.EnableDirectAction();
    }
}
