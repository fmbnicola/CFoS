using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Data;
using CFoS.Supershape;

namespace CFoS.Experiments
{
    [System.Serializable]
    public class ExperimentSupershape2D : Experiment 
    {
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
        }
    }
}