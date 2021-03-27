using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.Supershape
{
    [CreateAssetMenu(fileName = "Supershape2D",menuName ="CFoS/Supershape2D")]
    public class Supershape2D : ScriptableObject
    {
        // Check that parameter was changed and evoke update
        public delegate void OnUpdateDelegate();
        public event OnUpdateDelegate OnUpdate;

        public void VarChangeCheck(ref float var, float val)
        {
            if (Lock)
            {
                Debug.LogError("Supershape " + name + " is locked! [Trying to set parameter]");
                return;
            }
            if (var != val)
            {
                var = val;
                OnUpdate?.Invoke();
            }
        }

        [HideInInspector] public bool Lock = false;

        // Parameters and Ranges
        private float a = 1.0f;
        [HideInInspector] public float AMin = -1, AMax = 1;
        public float A { get { return a; } set { VarChangeCheck(ref a, value); } }

        private float b = 1.0f;
        [HideInInspector] public float BMin = -1, BMax = 1;
        public float B { get { return b; } set { VarChangeCheck(ref b, value); } }

        private float m  = 0.0f;
        [HideInInspector] public float MMin = 0, MMax = 50;
        public float M { get { return m; } set { VarChangeCheck(ref m, value); } }

        private float n1 = 1.0f;
        [HideInInspector] public float N1Min = 0, N1Max = 2;
        public float N1 { get { return n1; } set { VarChangeCheck(ref n1, value); } }

        private float n2 = 1.0f;
        [HideInInspector] public float N2Min = 0, N2Max = 2;
        public float N2 { get { return n2; } set { VarChangeCheck(ref n2, value); } }

        private float n3 = 1.0f;
        [HideInInspector] public float N3Min = 0, N3Max = 2;
        public float N3 { get { return n3; } set { VarChangeCheck(ref n3, value); } }


        // Methods
        public override string ToString()
        {
            return "Supershape2D { a: " + a + ", b: " + b + ", m: " + m + ", n1: " + n1 + ", n2: " + n2 + ", n3: " + n3 + " }";
        }

        public float GetValue(float theta)
        {
            float part1 = (1.0f / a) * Mathf.Cos(theta * m / 4.0f);
            part1 = Mathf.Abs(part1);
            part1 = Mathf.Pow(part1, n2);

            float part2 = (1.0f / b) * Mathf.Sin(theta * m / 4.0f);
            part2 = Mathf.Abs(part2);
            part2 = Mathf.Pow(part2, n3);

            float part3 = Mathf.Pow(part1 + part2, 1.0f / n1);

            if (part3 == 0.0f) return 0.0f;

            return (1.0f / part3);
        }

        public Vector2 GetCoords(float theta)
        {
            float r = GetValue(theta);

            float x = r * Mathf.Cos(theta);
            float y = r * Mathf.Sin(theta);

            return new Vector2(x, y);
        }

        public void Randomize()
        {
            if (Lock)
            {
                Debug.LogError("Supershape " + name + " is locked! [Trying to randomize parameters]");
                return;
            }

            a  = Random.Range(AMin, AMax);
            b  = Random.Range(BMin, BMax);
            m  = Random.Range(MMin, MMax);
            n1 = Random.Range(N1Min, N1Max);
            n2 = Random.Range(N2Min, N2Max);
            n3 = Random.Range(N3Min, N3Max);

            OnUpdate?.Invoke();

            Debug.Log("Randomize");
        }
    }
}