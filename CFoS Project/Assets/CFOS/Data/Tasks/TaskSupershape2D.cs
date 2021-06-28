using UnityEngine;
using CFoS.Supershape;

namespace CFoS.Data
{
    [CreateAssetMenu(fileName = "TaskSupershape2D", menuName = "CFoS/Task/TaskSupershape2D")]
    public class TaskSupershape2D : Task
    {
        [SerializeField]
        public Supershape2D StartingSupershape;

        [SerializeField]
        public Supershape2D TargetSupershape;
    }
}