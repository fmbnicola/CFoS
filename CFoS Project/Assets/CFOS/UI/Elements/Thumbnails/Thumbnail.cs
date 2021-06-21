using CFoS.Experimentation;
using CFoS.Supershape;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CFoS.UI
{
    public class Thumbnail : UIElement
    {
        public AudioClip ClickAudio;

        [Header("Renderer")]
        public Supershape2DRenderer Renderer;
        public Supershape2D Supershape { get; private set; }

        [Header("Selection")]
        public Shapes.Rectangle Selection;
        public Data.ColorVariable SelectionHoverColor;

        public delegate Supershape2D.Data SamplingFunction(Thumbnail thumbnail);
        public SamplingFunction SampleFunction;

        public delegate void SelectingFunction(Thumbnail thumbnail);
        public SelectingFunction SelectFunction;

        public delegate void UpdatingFunction(Thumbnail thumbnail);
        public UpdatingFunction UpdateFunction = delegate (Thumbnail thumbnail) { };

        // Position index in line, quad or cube
        [Space(10)][SerializeField]
        public Vector3Int Index = Vector3Int.zero;

        // Init
        protected override void Awake()
        {
            base.Awake();

            Renderer.Supershape = ScriptableObject.CreateInstance<Supershape2D>();
            Supershape = Renderer.Supershape;
        }

        [ExecuteAlways]
        protected override void OnValidate()
        {
            //Reset visuals
            Selection.enabled = false;
            Selection.Color = SelectionHoverColor.Value;
        }

        // Sampling
        public void UpdateSampling()
        {
            var data = SampleFunction(this);
            Supershape.SetData(data);
        }


        // Selection
        public override void Select(bool val)
        {
            base.Select(val);

            if (val)
            {
                // Evoke select function and Schedule Deselect
                SelectFunction(this);

                // Play audio
                UIManager.Instance.PlaySound(ClickAudio);

                if (gameObject.activeInHierarchy)
                    StartCoroutine(DeSelect(0.1f));

                // Register Selection as Metric
                MetricManager.Instance.RegisterTaskMetric("SelectCount", 1.0f);
            }
        }

        private IEnumerator DeSelect(float time)
        {
            yield return new WaitForSeconds(time);
            Select(false);
        }

        public void Update()
        {
            Selection.enabled = (hovered || selected);

            UpdateFunction(this);
        }
    }
}