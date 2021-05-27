using CFoS.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CFoS.Experimentation
{
    [System.Serializable]
    public class Experiment : MonoBehaviour
    {
        public bool Included = true;

        [SerializeField]
        public List<Task> Tasks;

        [HideInInspector]
        public Task LoadedTask = null;
        [HideInInspector]
        public int LoadedTaskIndex = -1; 


        // EXPERIMENT EVENTS
        public virtual void Init()
        {
            Debug.Log("Experiment " + name + " Init." );

            // Load the first task
            if(Tasks.Count != 0)
            {
                LoadTask(0);
            }
        }

        public virtual void End()
        {
            Debug.Log("Experiment " + name + " End.");

            // Go to next experiment
            var manager = ExperimentManager.Instance;
            manager.NextExperiment();
        }


        // TASKS
        public void LoadTask(int taskIndex)
        {
            // End Loaded Task
            if (LoadedTask != null)
            {
                EndTask();
            }

            // Init next task
            if (taskIndex >= 0 && taskIndex < Tasks.Count)
            {
                var task = Tasks[taskIndex];

                LoadedTask = task;
                LoadedTaskIndex = taskIndex;

                InitTask();
                return;
            }

            // if no more tasks, end experiment
            if (taskIndex >= Tasks.Count)
            {
                End();
                return;
            }

            Debug.LogError("Task index: " + taskIndex + " Not Found");
        }

        public void NextTask()
        {
            LoadTask(LoadedTaskIndex + 1);
        }

        public void PreviousTask()
        {
            LoadTask(LoadedTaskIndex - 1);
        }

        public virtual void InitTask()
        {
            Debug.Log("Task index " + LoadedTaskIndex + " Init.");
        }

        public virtual void EndTask()
        {
            Debug.Log("Task index " + LoadedTaskIndex + " Ended.");
        }
    }
}