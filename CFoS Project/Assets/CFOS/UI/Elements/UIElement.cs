using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;


namespace CFoS.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public XRBaseInteractable Interactable;

        protected ActionBasedController controller;

        protected bool hovered;
        protected bool selected;


        // Set Initial properties
        [ExecuteAlways]
        protected abstract void OnValidate();

        // Take care of controllers
        public void Start()
        {
            controller = null;
            hovered = false;
            selected = false;
        }

        public void OnDestroy()
        {
            UnhookController(controller);
        }


        // Controller hook/unhooking
        private void HookController(ActionBasedController controller)
        {
            if (controller != null)
            {
                controller.uiPressAction.action.performed += DoSelect;
                controller.uiPressAction.action.canceled += DoDeSelect;
            }
        }

        private void UnhookController(ActionBasedController controller)
        {
            if (controller != null)
            {
                controller.uiPressAction.action.performed -= DoSelect;
                controller.uiPressAction.action.canceled -= DoDeSelect;
            }
        }


        // UI Element Functions
        public virtual void Hover(bool val)
        {
            // Update state
            hovered = val;

            if (selected) return;

            // clear old controller
            UnhookController(controller);

            // Hover Start
            if (val)
            {
                // get new controller 
                var interactor = (XRBaseControllerInteractor)Interactable.hoveringInteractors[0];
                controller = (ActionBasedController)interactor.xrController;

                // and hook delegates
                HookController(controller);
            }
        }
        
        protected void DoSelect(CallbackContext contex)
        {
            Select(true);
        }

        protected void DoDeSelect(CallbackContext contex)
        {
            Select(false);
        }

        public virtual void Select(bool val)
        {
            // Update state
            selected = val;

            if (!val && !hovered) UnhookController(controller);  
        }
    }
}


