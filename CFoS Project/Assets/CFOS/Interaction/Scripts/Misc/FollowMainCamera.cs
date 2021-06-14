using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class FollowMainCamera : MonoBehaviour
{
    [Header("Input")]
    public PlayerInputController Input;

    [Header("Tolerances")]
    public float TranslationTolerance  = 1.0f;
    public float HeightChangeTolerance = 0.6f;
    public float RotationTolerance     = 90.0f;

    [Header("Speeds")]
    public float PosSpeed = 0.1f;
    public float RotSpeed = 1.0f;

    // Offsets
    private float yOffset;

    // Targets
    private Vector3 targetPos;
    private Quaternion targetRot;

    // Conserve vertical offset
    private void Awake()
    {
        var camera = Camera.main.transform;
        yOffset = (camera.position.y - transform.position.y);
    }

    // Init position and rotation targets
    void Start()
    {
        targetPos = transform.position;
        targetRot = transform.rotation;

        Input.ResetAction.action.performed += ResetAction;
    }

    // Find targets
    private void Update()
    {
        var camera = Camera.main.transform;

        // Calculate Position Delta
        var pos = transform.position;
        pos.y += yOffset;
        var posDelta = camera.position - pos;

        // XZ
        var xzDelta = Vector3.ProjectOnPlane(posDelta, Vector3.up);
        var xzDist  = xzDelta.magnitude;

        if(xzDist > TranslationTolerance)
        {
            targetPos.x = camera.position.x;
            targetPos.z = camera.position.z;
        }

        // Y
        var yDist = Mathf.Abs(posDelta.y);
        if(yDist > HeightChangeTolerance)
        {
            targetPos.y = camera.position.y - yOffset;
        }

        // Calculate Rotation delta on y axis
        var rotDelta = Mathf.Abs(camera.eulerAngles.y - transform.eulerAngles.y);
        if(rotDelta > RotationTolerance)
        {
            var foward = Vector3.ProjectOnPlane(camera.forward, Vector3.up);
            targetRot = Quaternion.LookRotation(foward, Vector3.up);
        }

    }

    // Adjust position and rotation to meet targets
    void LateUpdate()
    {
        // Position
        if(Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, PosSpeed);
        }

        // Rotation
        /*
        if (Quaternion.Angle(transform.rotation, targetRot) > 0.01f) {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, RotSpeed/360);
        }
        */
    }

    protected void ResetAction(InputAction.CallbackContext context)
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        Debug.Log("Reset Position");

        var camera = Camera.main.transform;

        // reset position
        targetPos.x = camera.position.x;
        targetPos.z = camera.position.z;
        targetPos.y = camera.position.y - yOffset;

        transform.position = targetPos;

        // reset rotation
        var foward = Vector3.ProjectOnPlane(camera.forward, Vector3.up);
        targetRot = Quaternion.LookRotation(foward, Vector3.up);

        transform.rotation = targetRot;
    }
}
