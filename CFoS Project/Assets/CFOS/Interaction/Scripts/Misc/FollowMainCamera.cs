using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class FollowMainCamera : MonoBehaviour
{
    [Header("Horizontal Adjustment")]
    public bool  FollowHorizontal    = true;
    public float HorizontalTolerance = 0.8f;
    public float HorizontalSpeed     = 0.05f;

    [Header("Vertical Adjustment")]
    public bool  FollowVertical    = true;
    public float VerticalTolerance = 0.25f;
    public float VerticalSpeed     = 0.05f;

    // Private vars
    private float yOffset;
    private Vector3 targetPos;

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

        PlayerInputController.Instance.ResetAction.action.performed += ResetAction;
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

        if(xzDist > HorizontalTolerance)
        {
            targetPos.x = camera.position.x;
            targetPos.z = camera.position.z;
        }

        // Y
        var yDist = Mathf.Abs(posDelta.y);
        if(yDist > VerticalTolerance)
        {
            targetPos.y = camera.position.y - yOffset;
        }

    }

    // Adjust position and rotation to meet targets
    void LateUpdate()
    {
        // Position
        if(Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            var pos = transform.position;

            // XZ
            if (FollowHorizontal)
            {
                pos.x = Mathf.Lerp(pos.x, targetPos.x, HorizontalSpeed);
                pos.z = Mathf.Lerp(pos.z, targetPos.z, HorizontalSpeed);
            }

            // Y
            if (FollowVertical)
            {
                pos.y = Mathf.Lerp(pos.y, targetPos.y, VerticalSpeed);
            }

            transform.position = pos;
        }
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
    }
}
