using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
{
    public class UIButton : UIElement
    {

        [Header("Element")]
        public Shapes.ShapeRenderer Element;
        public Data.ColorVariable ElementNormalColor;
        public Data.ColorVariable ElementHoverColor;
        public Data.ColorVariable ElementSelectColor;

        [Header("Text")]
        public TMPro.TextMeshPro Text;
        public Data.ColorVariable TextNormalColor;
        public Data.ColorVariable TextHoverColor;
        public Data.ColorVariable TextSelectColor;

        [Space(5)]
        public float SelectTime = 0.1f;

        [Space(5)]
        public UnityEvent ButtonClickEvent;

        //Offsets
        private float normalOffsetAmmount;
        private float hoverOffsetAmmount;

        private float textOffset;
        private float offset;
        

        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            Element.Color = ElementNormalColor.Value;
            Text.color = TextNormalColor.Value;
        }

        public void Awake()
        {
            // Offsets
            normalOffsetAmmount = Element.transform.localPosition.z; ;
            hoverOffsetAmmount = normalOffsetAmmount * 0.2f;

            textOffset = Text.transform.localPosition.z - normalOffsetAmmount;
            offset = normalOffsetAmmount;
        }

        public void Update()
        {
            // Visual Update
            Element.transform.localPosition = Vector3.forward * offset;
            Text.transform.localPosition = Vector3.forward * (offset + textOffset);

            Element.Color = selected ? ElementSelectColor.Value : hovered ? ElementHoverColor.Value : ElementNormalColor.Value;
            Text.color = selected ? TextNormalColor.Value : hovered ? TextHoverColor.Value : TextNormalColor.Value;
            offset = hovered? hoverOffsetAmmount : normalOffsetAmmount;
        }

        public override void Select(bool val)
        {
            base.Select(val);

            if (val)
            {
                // Callback and Schedule Deselect
                ButtonClickEvent.Invoke();
                StartCoroutine(DeSelect(SelectTime));
            }
        }

        private IEnumerator DeSelect(float time)
        {
            yield return new WaitForSeconds(time);
            Select(false);
        }


        // TEST CALLBACK
        public void TestCallback()
        {
            Debug.Log("Button Click -> " + gameObject.name);
        }
    }
}