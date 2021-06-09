using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilboardTransform : MonoBehaviour
{
    void LateUpdate()
    {
        var forward = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
