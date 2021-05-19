using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.Data
{
    [CreateAssetMenu(fileName = "TaskData", menuName = "CFoS/SaveData/TaskData")]
    public class TaskData : SaveData
    {
        public float taskDuration;
        public float selectionError;

        // number of clicks, travel distance, head rotation etc...
    }
}