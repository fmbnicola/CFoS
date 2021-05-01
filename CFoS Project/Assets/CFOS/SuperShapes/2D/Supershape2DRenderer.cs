using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CFoS.Supershape
{
    [ExecuteAlways]
    public abstract class Supershape2DRenderer : MonoBehaviour
    {
        public void VarChangeCheck<T>(ref T var, T val, Action func)
        {
            if (!var.Equals(val)) { var = val; func?.Invoke(); }
        }

        // Supershape Reference
        [SerializeReference] [HideInInspector]
        protected Supershape2D supershape;
        public Supershape2D Supershape
        {
            get { return supershape; }
            set { VarChangeCheck(ref supershape, value, () => { Clean(); Init(); }); }
        }

        // Debug
        public bool DebugEnabled = false;


        // Unity Events
        public void OnEnable()
        {
            Init();
        }

        public void OnDisable()
        {
            Clean();
        }

        public void OnDestroy()
        {
            Clean();
        }


        // Methods
        protected virtual void Init()
        {
            if(DebugEnabled) Debug.Log("Init");

            // Init Supershape
            if (supershape == null)
            {
                if (DebugEnabled) Debug.Log("New Supershape");
                supershape = ScriptableObject.CreateInstance<Supershape2D>();
            }
            supershape.OnUpdate -= UpdateRender;
            supershape.OnUpdate += UpdateRender;
        }

        protected virtual void Clean()
        {
            if (DebugEnabled) Debug.Log("Clean");

            if (supershape != null)
                supershape.OnUpdate -= UpdateRender;
        }

        protected virtual void UpdateRender(Supershape2D.Parameter p)
        {
            if (DebugEnabled) Debug.Log("Update: " + supershape);

            if (supershape == null) Init();
        }

        protected void Repaint()
        {
            #if UNITY_EDITOR
            SceneView.RepaintAll();
            #endif
        }

        protected abstract void UpdateRenderProperties();

    }
}

