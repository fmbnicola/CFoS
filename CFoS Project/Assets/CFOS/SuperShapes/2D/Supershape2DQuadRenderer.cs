using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.Supershape
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshRenderer))]
    public class Supershape2DQuadRenderer : Supershape2DRenderer
    {

        // Render Properties
        [HideInInspector] [SerializeField] protected Color color = Color.white;
        public Color Color
        {
            get { return color; }
            set { VarChangeCheck(ref color, value, UpdateRenderProperties); }
        }

        [HideInInspector] [SerializeField] protected float scale = 0.5f;
        public float Scale
        {
            get { return scale; }
            set { VarChangeCheck(ref scale, value, UpdateRenderProperties); }
        }

        // Mesh Renderer
        public MeshRenderer MeshRenderer;
        protected Material material;
        protected MaterialPropertyBlock propBlock;

        // Methods
        protected override void Init()
        {
            base.Init();

            if (MeshRenderer == null)
            {
                MeshRenderer = gameObject.GetComponent<MeshRenderer>();
            }
            List<Material> mats = new List<Material>();
            MeshRenderer.GetSharedMaterials(mats);

            material = mats[0];

            if(propBlock == null)
            {
                propBlock = new MaterialPropertyBlock();
            }

            propBlock.SetColor("_Color", color);
            propBlock.SetFloat("_Scale", scale);

            propBlock.SetFloat("_A",supershape.A);
            propBlock.SetFloat("_B",supershape.B);
            propBlock.SetFloat("_N1",supershape.N1);
            propBlock.SetFloat("_N2",supershape.N2);
            propBlock.SetFloat("_N3",supershape.N3);
            propBlock.SetFloat("_M",supershape.M);
            MeshRenderer.SetPropertyBlock(propBlock);

            Repaint();
        }

        protected override void Clean()
        {
            base.Clean();
        }

        protected override void UpdateRender(Supershape2D.Parameter p)
        {
            base.UpdateRender(p);

            if (MeshRenderer == null) Init();

            propBlock.SetFloat("_A", supershape.A);
            propBlock.SetFloat("_B", supershape.B);
            propBlock.SetFloat("_N1", supershape.N1);
            propBlock.SetFloat("_N2", supershape.N2);
            propBlock.SetFloat("_N3", supershape.N3);
            propBlock.SetFloat("_M", supershape.M);
            MeshRenderer.SetPropertyBlock(propBlock);

            Repaint();
        }

        protected override void UpdateRenderProperties()
        {
            if (MeshRenderer != null && propBlock != null)
            {
                propBlock.SetColor("_Color", color);
                propBlock.SetFloat("_Scale", scale);
                MeshRenderer.SetPropertyBlock(propBlock);
            }
        }
    }
}