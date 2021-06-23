using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Data;
using CFoS.Supershape;
using CFoS.UI;

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

            StartingRenderer.Supershape.SetData(task.StartingSupershape.GetData());
            TargetRenderer.Supershape = task.TargetSupershape;

            // Init Metrics
            MetricManager.Instance.InitTaskMetrics();

            // Timer
            MetricManager.Instance.StartTimer();
        }

        public override void EndTask()
        {
            base.EndTask();

            // Reset Menus
            var menus = GetComponentsInChildren<UIMenu>();
            foreach(var menu in menus)
            {
                menu.ResetMenu();
            }

            // Register Metrics
            var metricManager = MetricManager.Instance;

            // TaskTime
            var time = metricManager.GetTime();
            metricManager.RegisterTaskMetric("TimeDuration", time);
            metricManager.StopTimer();

            // SelectionError
            var supershape1 = StartingRenderer.Supershape;
            var supershape2 = TargetRenderer.Supershape;
            var error = Supershape2D.CalculateComponentError(supershape1, supershape2);
            metricManager.RegisterTaskMetric("SelectionErrorN1", error.x);
            metricManager.RegisterTaskMetric("SelectionErrorN2", error.y);
            metricManager.RegisterTaskMetric("SelectionErrorN3", error.z);

            // Save Metrics 
            metricManager.SaveTaskMetrics();
        }
    }
}