using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.Experiments
{

    public class ExperimentManager : MonoBehaviour
    {
        public static ExperimentManager Instance { get; private set; }

        [SerializeField]
        public bool LoadOnStart = true;
        public List<Experiment> Experiments;

        [HideInInspector]
        public Experiment LoadedExperiment = null;
        [HideInInspector]
        public int loadedExperimentIndex = -1;

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

        private void Start()
        {
            if (!LoadOnStart) return;
            LoadExperiment(0);
        }

        public void Refresh()
        {
            Experiments = new List<Experiment>();
            foreach(Transform child in transform)
            {
                var exp = child.GetComponent<Experiment>();
                if(exp) Experiments.Add(exp);
            }
        }

        public void Unload()
        {
            if (LoadedExperiment == null) return;

            if (LoadedExperiment.gameObject.activeInHierarchy)
                LoadedExperiment.gameObject.SetActive(false);

            loadedExperimentIndex = -1;
        }

        public void LoadExperiment(string experimentName)
        {
            Unload();

            int index = 0;
            foreach (var exp in Experiments)
            {
                Debug.Log(exp.name);

                if(exp.name.Equals(experimentName) && exp.Included)
                {
                    LoadedExperiment = exp;
                    loadedExperimentIndex = index;

                    LoadedExperiment.gameObject.SetActive(true);
                    LoadedExperiment.Init();
                    return;
                }
                index++;
            }
            Debug.LogError("Experiment \"" + experimentName + "\" Not Found (or not included)");
        }

        public void LoadExperiment(int experimentIndex)
        {
            Unload();

            if (experimentIndex >= 0 && experimentIndex < Experiments.Count)
            {
                var exp = Experiments[experimentIndex];
                if (exp.Included)
                {
                    LoadedExperiment = exp;
                    loadedExperimentIndex = experimentIndex;

                    LoadedExperiment.gameObject.SetActive(true);
                    LoadedExperiment.Init();
                    return;
                }
            } 

            Debug.LogError("Experiment index: " + experimentIndex + " Not Found (or not included)");
        }

        public void NextExperiment()
        {
            LoadExperiment(loadedExperimentIndex + 1);
        }

        public void PreviousExperiment()
        {
            LoadExperiment(loadedExperimentIndex - 1);
        }

        public void NextTask()
        {
            if(LoadedExperiment != null)
            {
                LoadedExperiment.NextTask();
            }
        }
    }
}
