using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.Experimentation
{
    public class XRRigMetricTracker : MonoBehaviour
    {
        // Rig
        public XRRig Rig;
        private Transform head;
        private Transform leftHand;
        private Transform rightHand;

        // Translation
        protected Vector3 HeadLastPos;
        protected Vector3 LeftHandLastPos;
        protected Vector3 RightHandLastPos;

        // Rotation
        protected Quaternion HeadLastRot;
        protected Quaternion LeftHandLastRot;
        protected Quaternion RightHandLastRot;

        private void Awake()
        {
            head        = Rig.cameraGameObject.transform;
            leftHand    = Rig.transform.Find("LeftHand Controller");
            rightHand   = Rig.transform.Find("RightHand Controller");
        }

        void Start()
        {
            UpdateLastVals();
        }

        
        void Update()
        {
            RegisterDiffs();
            UpdateLastVals();
        }


        protected void RegisterDiffs()
        {
            // Translation
            var translationH = Vector3.Distance(head.localPosition, HeadLastPos);
            MetricManager.Instance.RegisterTaskMetric("AverageTranslationH", translationH);

            var translationL = Vector3.Distance(leftHand.localPosition, LeftHandLastPos);
            MetricManager.Instance.RegisterTaskMetric("AverageTranslationL", translationL);

            var translationR = Vector3.Distance(rightHand.localPosition, RightHandLastPos);
            MetricManager.Instance.RegisterTaskMetric("AverageTranslationR", translationR);

            // Rotation
            var rotationH = Quaternion.Angle(head.localRotation, HeadLastRot);
            MetricManager.Instance.RegisterTaskMetric("AverageRotationH", rotationH);

            var rotationL = Quaternion.Angle(leftHand.localRotation, LeftHandLastRot);
            MetricManager.Instance.RegisterTaskMetric("AverageRotationL", rotationL);

            var rotationR = Quaternion.Angle(rightHand.localRotation, RightHandLastRot);
            MetricManager.Instance.RegisterTaskMetric("AverageRotationR", rotationR);
        }

        protected void UpdateLastVals()
        {
            HeadLastPos         = head.localPosition;
            LeftHandLastPos     = leftHand.localPosition;
            RightHandLastPos    = rightHand.localPosition;

            HeadLastRot         = head.localRotation;
            LeftHandLastRot     = leftHand.localRotation;
            RightHandLastRot    = rightHand.localRotation;
        }
    }
}