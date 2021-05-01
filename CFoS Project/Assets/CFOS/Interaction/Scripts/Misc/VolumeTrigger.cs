using UnityEngine;
using UnityEngine.Events;

namespace CFoS.Interaction
{
    public class VolumeTrigger : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerEnter;
        public UnityEvent<Collider> onTriggerExit;
        public UnityEvent<Collider> onTriggerStay;

        private void OnTriggerEnter(Collider other)
        {
            if (onTriggerEnter != null) onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (onTriggerExit != null) onTriggerExit.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (onTriggerStay != null) onTriggerStay.Invoke(other);
        }
    }
}


