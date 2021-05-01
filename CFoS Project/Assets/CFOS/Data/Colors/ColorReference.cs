using UnityEngine;

namespace CFoS.Data
{
    [System.Serializable]
    public class ColorReference
    {
        public bool UseConstant = false;
        public Color ConstantValue;
        public ColorVariable Variable;

        public Color Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }
    }
}

