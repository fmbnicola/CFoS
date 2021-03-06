using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.Supershape
{
    [CreateAssetMenu(fileName = "Supershape2D",menuName ="CFoS/Supershape/Supershape2D")]
    public class Supershape2D : ScriptableObject
    {
        // Check that parameter was changed and evoke update
        public enum Parameter { A, B, M, N1, N2, N3, Any};
        public delegate void OnUpdateDelegate(Parameter p);
        public event OnUpdateDelegate OnUpdate;
        
        // Data structure to be used as a representation of a SShape2D
        public struct Data
        {
            public float A;
            public float B;
            public float M;
            public float N1;
            public float N2;
            public float N3;

            public Data(float a, float b, float m, float n1, float n2, float n3)
            {
                A = a;
                B = b;
                M = m;
                N1 = n1;
                N2 = n2;
                N3 = n3;
            }
        }

        public void VarChangeCheck(Parameter p, ref float var, float val)
        {
            if (Lock)
            {
                Debug.LogError("Supershape " + name + " is locked! [Trying to set parameter]");
                return;
            }
            if (var != val)
            {
                var = val;
                OnUpdate?.Invoke(p);
            }
        }

        [HideInInspector] public bool Lock = false;

        // Parameters and Ranges
        [HideInInspector] [SerializeField] private float a = 1.0f;
        [HideInInspector] public float AMin = 0, AMax = 1;
        public float A { get { return a; } set { VarChangeCheck(Parameter.A, ref a, value); } }

        [HideInInspector] [SerializeField] private float b = 1.0f;
        [HideInInspector] public float BMin = 0, BMax = 1;
        public float B { get { return b; } set { VarChangeCheck(Parameter.B, ref b, value); } }

        [HideInInspector] [SerializeField] private float m = 0.0f;
        [HideInInspector] public float MMin = 0, MMax = 50;
        public float M { get { return m; } set { VarChangeCheck(Parameter.M, ref m, value); } }

        [HideInInspector] [SerializeField] private float n1 = 1.0f;
        [HideInInspector] public float N1Min = 0, N1Max = 2;
        public float N1 { get { return n1; } set { VarChangeCheck(Parameter.N1, ref n1, value); } }

        [HideInInspector] [SerializeField] private float n2 = 1.0f;
        [HideInInspector] public float N2Min = 0, N2Max = 2;
        public float N2 { get { return n2; } set { VarChangeCheck(Parameter.N2, ref n2, value); } }

        [HideInInspector] [SerializeField] private float n3 = 1.0f;
        [HideInInspector] public float N3Min = 0, N3Max = 2;
        public float N3 { get { return n3; } set { VarChangeCheck(Parameter.N3, ref n3, value); } }


        // Methods
        public void SetData(Data data)
        {
            if (Lock)
            {
                Debug.LogError("Supershape " + name + " is locked! [Trying to set parameters]");
                return;
            }

            a = data.A;
            b = data.B;
            m = data.M;
            n1 = data.N1;
            n2 = data.N2;
            n3 = data.N3;

            OnUpdate?.Invoke(Parameter.Any);
        }

        public Data GetData()
        {
            return new Data(A, B, M, N1, N2, N3);
        }

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

            OnUpdate?.Invoke(Parameter.Any);

            Debug.Log("Randomize");
        }

        // Static Methods
        public static float CalculateCumulativeError(Supershape2D s1, Supershape2D s2)
        {
            var a  = Mathf.Abs(s1.A - s2.A);
            var b  = Mathf.Abs(s1.B - s2.b);
            var n1 = Mathf.Abs(s1.N1 - s2.N1);
            var n2 = Mathf.Abs(s1.N2 - s2.N2);
            var n3 = Mathf.Abs(s1.N3 - s2.N3);
            var m  = Mathf.Abs(s1.M - s2.M);

            return a + b + n1 + n2 + n3 + m;
        }

        public static Vector3 CalculateComponentError(Supershape2D s1, Supershape2D s2)
        {
            var n1 = Mathf.Abs(s1.N1 - s2.N1);
            var n2 = Mathf.Abs(s1.N2 - s2.N2);
            var n3 = Mathf.Abs(s1.N3 - s2.N3);

            return new Vector3(n1, n2, n3);
        }

        public static float CalculateGreatestValue(Supershape2D s)
        {
            float maxVal = 0.0f;

            float maxTheta  = (2 * Mathf.PI) / s.M;
            float increment = maxTheta / 100.0f;
            for (float theta = 0.0f; theta < maxTheta; theta += increment)
            {
                var val = s.GetValue(theta);
                if(val > maxVal) maxVal = val;
            }
            return maxVal;
        }
    }
}