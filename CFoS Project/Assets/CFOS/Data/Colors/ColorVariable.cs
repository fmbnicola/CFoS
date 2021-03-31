using UnityEngine;

namespace CFoS.Data
{
    [CreateAssetMenu]
    public class ColorVariable : ScriptableObject
    {
        public delegate void OnUpdateDelegate();
        public event OnUpdateDelegate OnUpdate;

        public void VarChangeCheck(ref Color var, Color val)
        {
            if (!var.Equals(val))
            {
                var = val;
                OnUpdate?.Invoke();
            }
        }

        [HideInInspector] [SerializeField] private Color val;
        public Color Value
        {
            get { return val; }
            set { VarChangeCheck(ref val, value); }
        }
    }
}

