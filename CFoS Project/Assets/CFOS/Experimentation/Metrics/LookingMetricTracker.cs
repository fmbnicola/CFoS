using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.Experimentation
{
    public class LookingMetricTracker : MonoBehaviour
    {
        // Rig
        public XRRig Rig;
        private Transform head;

        private Collider currentTarget;

        private void Awake()
        {
            head = Rig.cameraGameObject.transform;
        }

        private void Update()
        {
            //get the mask to raycast against UI only
            int layer_mask = LayerMask.GetMask("LookTarget");

            //do the raycast specifying the mask
            RaycastHit hit;
            Collider newTarget = null;
            bool raycast = Physics.Raycast(head.transform.position, head.transform.forward, out hit, 200.0f, layer_mask);
            if (raycast)
            {
                newTarget = hit.collider;
                var name = newTarget.gameObject.name;
                MetricManager.Instance.RegisterTaskMetric("TimeLooking" + name, Time.deltaTime);
            }

            // Detect target alternation
            if(newTarget != null && newTarget != currentTarget)
            {
                MetricManager.Instance.RegisterTaskMetric("LookingAlternateCount", 1.0f);
            }
            currentTarget = newTarget;
        }

    }
}