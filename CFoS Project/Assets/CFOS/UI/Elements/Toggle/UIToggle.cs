using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
{
    public class UIToggle : UIElement
    {
        public AudioClip ClickAudio;

        [Header("Element")]
        public Shapes.ShapeRenderer CheckedElement;
        public Shapes.ShapeRenderer UncheckedElement;
        private Shapes.ShapeRenderer element;
        public Data.ColorVariable ElementNormalColor;
        public Data.ColorVariable ElementHoverColor;
        public Data.ColorVariable ElementSelectColor;

        [Header("Outline")]
        public Shapes.ShapeRenderer Outline;
        public Data.ColorVariable OutlineColor;

        [Header("Text")]
        public TMPro.TextMeshPro Text;
        public Data.ColorVariable TextColor;

        [Space(10)]
        public float SelectTime = 0.1f;

        public enum ToggleState { Checked, Unchecked };
        private ToggleState state;
        public ToggleState StartState = ToggleState.Unchecked;

        public UnityEvent ToggleCheckedEvent;
        public UnityEvent ToggleUncheckedEvent;


        // Switch States
        void CheckedInit()
        {
            state = ToggleState.Checked;
            element = CheckedElement;

            CheckedElement.gameObject.SetActive(true);
            UncheckedElement.gameObject.SetActive(false);
        }

        void UncheckedInit()
        {
            state = ToggleState.Unchecked;
            element = UncheckedElement;

            UncheckedElement.gameObject.SetActive(true);
            CheckedElement.gameObject.SetActive(false);
        }


        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            CheckedElement.Color = ElementNormalColor.Value;
            UncheckedElement.Color = ElementNormalColor.Value;
            Outline.Color = OutlineColor.Value;
            Text.color = TextColor.Value;
        }

        protected override void Awake()
        {
            base.Awake();

            if (StartState == ToggleState.Checked) CheckedInit();
            else if (StartState == ToggleState.Unchecked) UncheckedInit();

            Outline.gameObject.SetActive(false);

            // Audio
            ToggleCheckedEvent.AddListener(() =>
            {
                UIManager.Instance.PlaySound(ClickAudio);
            });
            ToggleUncheckedEvent.AddListener(() =>
            {
                UIManager.Instance.PlaySound(ClickAudio);
            });
        }



        // Actions
        public override void Select(bool val)
        {
            base.Select(val);

            if (val)
            {
                // Callback
                if (state == ToggleState.Checked)
                {
                    ToggleUncheckedEvent.Invoke();
                    UncheckedInit();
                }
                else if (state == ToggleState.Unchecked)
                {
                    ToggleCheckedEvent.Invoke();
                    CheckedInit();
                }

                // DeSelect
                if (gameObject.activeInHierarchy)
                    StartCoroutine(DeSelect(SelectTime));
            }
        }

        private IEnumerator DeSelect(float time)
        {
            yield return new WaitForSeconds(time);
            Select(false);
        }


        public override void Enable(bool val)
        {
            base.Enable(val);

            if (!val)
            {
                var col = ElementNormalColor.Value; col.a = 0.3f;
                CheckedElement.Color = col;

                col = ElementNormalColor.Value; col.a = 0.3f;
                UncheckedElement.Color = ElementNormalColor.Value;

                col = TextColor.Value; col.a = 0.3f;
                Text.color = TextColor.Value;
            }
            else
            {
                CheckedElement.Color = ElementNormalColor.Value;
                UncheckedElement.Color = ElementNormalColor.Value;
                Text.color = TextColor.Value;
            }
        }


        public void Update()
        {
            if (disabled)
            {
                var col = ElementNormalColor.Value; col.a = 0.3f;
                element.Color = col;
                Outline.gameObject.SetActive(false);
                return;
            }

            // Visual Update
            element.Color = selected ? ElementSelectColor.Value : hovered ? ElementHoverColor.Value : ElementNormalColor.Value;
            Outline.gameObject.SetActive(hovered || selected);
        }

        // TEST CALLBACK
        public void TestCallback()
        {
            Debug.Log("Toggle -> " + gameObject.name + ": " + state);
        }
    }
}