using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.Experiments
{

    public class ExperimentManager : MonoBehaviour
    {
        public static ExperimentManager Instance { get; private set; }

        [SerializeField]
        public List<Experiment> Experiments;

        [HideInInspector]
        public Experiment LoadedExperiment = null;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }

        public void Refresh()
        {
            Experiments = new List<Experiment>();
            foreach(Transform child in transform)
            {
                var exp = child.GetComponent<Experiment>();
                Experiments.Add(exp);
            }
        }

        public void Unload()
        {
            if (LoadedExperiment == null) return;

            if (LoadedExperiment.gameObject.activeInHierarchy)
                LoadedExperiment.gameObject.SetActive(false);
        }

        public void LoadExperiment(string experimentName)
        {
            Unload();

            foreach (var exp in Experiments)
            {
                Debug.Log(exp.name);

                if(exp.name.Equals(experimentName) && exp.Included)
                {
                    LoadedExperiment = exp;
                    LoadedExperiment.gameObject.SetActive(true);
                    return;
                }
            }
            Debug.LogError("Experiment \"" + experimentName + "\" Not Found (or not included)");
        }
    }
}
