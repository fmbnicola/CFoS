using UnityEngine;

namespace CFoS.Data
{
    [System.Serializable]
    public class IntReference
    {
        public bool UseConstant = false;
        public int ConstantValue;
        public IntVariable Variable;

        public float Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }
    }
}
