using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.Experimentation
{
    [System.Serializable]
    public class Metric
    {
        public string Name = "Metric";
        public bool enabled = true;

        public enum UpdateType { Set, Add, Min, Max }
        public UpdateType UpdateMethod = UpdateType.Set;

        [HideInInspector] public float Value;

        public void Init()
        {
            Value = 0.0f;
        }

        public void Update(float value)
        {
            switch (UpdateMethod)
            {
                case UpdateType.Set:
                    Value = value;
                    break;

                case UpdateType.Add:
                    Value += value;
                    break;

                case UpdateType.Min:
                    if (value < Value) 
                        Value = value;
                    break;

                case UpdateType.Max:

                    if (value > Value)
                        Value = value;
                    break;
            }
        }

    }

    public class MetricManager : MonoBehaviour
    {
        public static MetricManager Instance { get; private set; }

        // Metrics
        [Header("Tracked Metrics")]
        public bool SaveMetrics = true;
        [SerializeField]
        public List<Metric> Metrics;
        private Dictionary<string, Metric> metricDict;

        // Timer
        protected float ElapsedTime = 0.0f;
        protected enum TimerState {Stopped, Paused, Counting};
        protected TimerState TimeState = TimerState.Stopped;


        // Unity Events
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
            // Save metric by name for easier access
            metricDict = new Dictionary<string, Metric>();
            foreach (var metric in Metrics)
            {
                metricDict[metric.Name] = metric;
            }
        }

        private void Update()
        {
            TimerUpdate();
        }


        // Task Metrics
        public void InitTaskMetrics()
        {
            if (!SaveMetrics) return;

            foreach (var metric in Metrics)
            {
                metric.Init();
            }
        }

        public void RegisterTaskMetric(string metricName, float value)
        {
            if (!SaveMetrics) return;

            // look up metric in the dictionary and update
            if (metricDict.ContainsKey(metricName))
            {
                var metric = metricDict[metricName];

                if (metric.enabled)
                    metric.Update(value); 
            }
        }

        public void SaveTaskMetrics()
        {
            if (!SaveMetrics) return;

            var expManager = ExperimentManager.Instance;
            string expName = expManager.LoadedExperiment.name;
            int taskIndex = expManager.LoadedExperiment.LoadedTaskIndex;
            string ExpTask = expName + " Task " + taskIndex.ToString();

            var data = new SaveData.SaveData();
            foreach (var metric in Metrics)
            {
                if (!metric.enabled) continue;

                var key = ExpTask + " " + metric.Name;
                var value = metric.Value.ToString();
                data.Add(key, value);
            }

            var sameManager = SaveData.SaveManager.Instance;
            sameManager.SaveData(data);
        }


        // Timer
        protected void TimerUpdate()
        {
            switch (TimeState)
            {
                case TimerState.Counting:
                    ElapsedTime += Time.deltaTime;
                    break;

                case TimerState.Stopped:
                case TimerState.Paused:
                    break;
            }
        }

        public float GetTime()
        {
            return ElapsedTime;
        }

        public void StopTimer()
        {
            TimeState = TimerState.Stopped;
            ElapsedTime = 0.0f;
        }

        public void StartTimer()
        {
            TimeState = TimerState.Counting;
        }

        public void PauseTimer()
        {
            TimeState = TimerState.Paused;
        }
    }
}
