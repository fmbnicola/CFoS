using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UIButton : MonoBehaviour
    {
        public XRBaseInteractable Interactable;

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
        public float SelectTime = 0.2f;

        //Offsets
        private float normalOffsetAmmount;
        private float hoverOffsetAmmount;

        private float textOffset;
        private float offset;
        
        [Space(5)]
        public UnityEvent ButtonClickEvent;
        private ActionBasedController controller;

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
            Element.transform.localPosition = Vector3.forward * offset;
            Text.transform.localPosition = Vector3.forward * (offset + textOffset);
        }

        // Button Functions
        public void Hover(bool val)
        {
            // Visual Update
            Element.Color = val ? ElementHoverColor.Value : ElementNormalColor.Value;
            Text.color = val ? TextHoverColor.Value : TextNormalColor.Value;
            offset = val? hoverOffsetAmmount : normalOffsetAmmount;

            // unhook 
            if(controller != null)
            {
                controller.uiPressAction.action.performed -= Select;
            }

            // hook UI press action
            if (val)
            {
                var interactor = (XRBaseControllerInteractor) Interactable.hoveringInteractors[0];
                controller = (ActionBasedController) interactor.xrController;
                controller.uiPressAction.action.performed += Select;
            }
        }

        private void Select(CallbackContext contex)
        {
            // Visual Update
            Element.Color =  ElementSelectColor.Value;
            Text.color = TextSelectColor.Value;

            // Callback
            ButtonClickEvent.Invoke();

            // DeSelect
            StartCoroutine(DeSelect(SelectTime));
        }

        private IEnumerator DeSelect(float time)
        {
            yield return new WaitForSeconds(time);

            Element.Color = ElementNormalColor.Value;
            Text.color = TextNormalColor.Value;
            offset = normalOffsetAmmount;
        }


        // TEST CALLBACK
        public void TestCallback()
        {
            Debug.Log("Button Click -> " + gameObject.name);
        }
    }
}