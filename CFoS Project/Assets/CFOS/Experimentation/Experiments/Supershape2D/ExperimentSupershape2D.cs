using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Data;
using CFoS.Supershape;

namespace CFoS.Experimentation
{
    [System.Serializable]
    public class ExperimentSupershape2D : Experiment 
    {
        [Header("Renderers")]
        public Supershape2DRenderer StartingRenderer;
        public Supershape2DRenderer TargetRenderer;

        public override void InitTask()
        {
            base.InitTask();

            var task = (TaskSupershape2D) LoadedTask;
            if(task == null)
            {
                Debug.LogError("Experiment Task not of type TaskSupershape2D!");
                return;
            }

            var startingSupershape = ScriptableObject.CreateInstance<Supershape2D>();
            startingSupershape.SetData(task.StartingSupershape.GetData());
            StartingRenderer.Supershape = startingSupershape;

            TargetRenderer.Supershape = task.TargetSupershape;

            // Init Metrics
            MetricManager.Instance.InitTaskMetrics();

            // Timer
            MetricManager.Instance.StartTimer();
        }

        public override void EndTask()
        {
            base.EndTask();

            // Register Metrics
            var metricManager = MetricManager.Instance;

            // TaskTime
            var time = metricManager.GetTime();
            metricManager.RegisterTaskMetric("TimeDuration", time);
            metricManager.StopTimer();

            // SelectionError
            var supershape1 = StartingRenderer.Supershape;
            var supershape2 = TargetRenderer.Supershape;
            var error = Supershape2D.CalculateCumulativeError(supershape1, supershape2);
            metricManager.RegisterTaskMetric("SelectionError", error);

            // Save Metrics 
            metricManager.SaveTaskMetrics();
        }
    }
}