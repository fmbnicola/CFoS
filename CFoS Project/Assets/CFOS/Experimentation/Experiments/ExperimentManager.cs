using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.Experimentation
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

        [Header("Randomization")]
        public bool RandomizeExperimentOrder = false;
        public int RandomizeStartIndex = 0;
        public int RandomizeEndIndex = 0;

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
            // Randomize Experiment Order
            if(RandomizeExperimentOrder)
                ExtensionMethods.Shuffle(Experiments, RandomizeStartIndex, RandomizeEndIndex);

            // Randomize Task Order
            foreach(var experiment in Experiments)
            {
                if (experiment.RandomizeTaskOrder)
                    ExtensionMethods.Shuffle(experiment.Tasks);
            }

            // Save Experiment and Task order
            SaveTaskOrder();

            // Init
            if (!LoadOnStart) return;
            LoadExperiment(0);
        }

        private void Update()
        {
            if (LoadedExperiment != null)
            {
                LoadedExperiment.UpdateExperiment();
            }
        }

        // Save task order
        public void SaveTaskOrder()
        {
            var data = new SaveData.SaveData();

            var key = "ExperimentOrder";
            var value = "";

            for(int i = RandomizeStartIndex; i <= RandomizeEndIndex; i++)
            {
                var exp = Experiments[i];
                value += exp.name + ",";

                foreach(var task in exp.Tasks)
                {
                    value += task.name + ",";
                }
            }
            data.Add(key, value);

            var sameManager = SaveData.SaveManager.Instance;
            sameManager.SaveData(data);
        }


        // Experiments
        public void Refresh()
        {
            Experiments = new List<Experiment>();
            foreach (Transform child in transform)
            {
                var exp = child.GetComponent<Experiment>();
                if (exp && exp.Included) Experiments.Add(exp);
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

                if (exp.name.Equals(experimentName) && exp.Included)
                {
                    LoadedExperiment = exp;
                    loadedExperimentIndex = index;

                    LoadedExperiment.gameObject.SetActive(true);
                    LoadedExperiment.InitExperiment();
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
                    LoadedExperiment.InitExperiment();
                    return;
                }
            }

            if (experimentIndex >= Experiments.Count)
            {
                Unload();
                return;
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

        // Tasks
        public void NextTask()
        {
            if (LoadedExperiment != null)
            {
                LoadedExperiment.NextTask();
            }
        }

    }
}
