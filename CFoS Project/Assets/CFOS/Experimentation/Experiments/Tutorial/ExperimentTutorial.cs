using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Data;
using CFoS.Supershape;

namespace CFoS.Experimentation
{
    [System.Serializable]
    public class ExperimentTutorial : Experiment 
    {
        [Header("Renderer")]
        public Supershape2DRenderer StartingRenderer;

        public override void InitTask()
        {
            base.InitTask();

            // Init Supershape
            // var startingSupershape = ScriptableObject.CreateInstance<Supershape2D>();
            // StartingRenderer.Supershape = startingSupershape;

            /* No need to record metrics for tutorial tasks
             
            // Init Metrics
            MetricManager.Instance.InitTaskMetrics();

            // Timer
            MetricManager.Instance.StartTimer();

            //*/
        }

        public override void EndTask()
        {
            base.EndTask();

            /* No need to record metrics for tutorial tasks

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

            //*/
        }
    }
}
 