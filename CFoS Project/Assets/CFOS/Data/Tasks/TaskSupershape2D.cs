using UnityEngine;
using CFoS.Supershape;

namespace CFoS.Data
{
    [CreateAssetMenu(fileName = "TaskSupershape2D", menuName = "CFoS/Task/TaskSupershape2D")]
    public class TaskSupershape2D : Task
    {
        [SerializeField][HideInInspector]
        public Supershape2D StartingSupershape;

        [SerializeField][HideInInspector]
        public Supershape2D TargetSupershape;
    }
}