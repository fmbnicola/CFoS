using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CFoS.Experiments
{
    [System.Serializable]
    public class Experiment : MonoBehaviour
    {
        public bool Included = true;

        public virtual void Init()
        {
            Debug.Log("Experiment " + name + " Init." );
        }
    }
}