using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.Interaction
{
    public class CustomXRDirectInteractor : XRDirectInteractor
    {
        // Reference to XRController
        [SerializeField]
        XRBaseController m_ControllerRef;
        public XRBaseController ControllerRef
        {
            get => m_ControllerRef;
            set => m_ControllerRef = value;
        }

        // Reference to Trigger Collider
        [SerializeField]
        VolumeTrigger m_TriggerRef;
        public VolumeTrigger TriggerRef
        {
            get => m_TriggerRef;
            set => m_TriggerRef = value;
        }

        protected override void Awake()
        {
            // Run base Awake and suppress warnings
            Debug.unityLogger.filterLogType = LogType.Assert;
            base.Awake();
            Debug.unityLogger.filterLogType = LogType.Log;

            // set Controller
            xrController = m_ControllerRef;

            // hook events to trigger volume
            m_TriggerRef.onTriggerEnter.AddListener(OnTriggerEnter);
            m_TriggerRef.onTriggerExit.AddListener(OnTriggerExit);
        }

    }
}

