using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;


namespace CFoS.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public XRBaseInteractable Interactable;

        protected ActionBasedController controller = null;

        // Flags
        protected bool hovered = false;
        protected bool selected = false;
        public bool disabled = false;



        // Set Initial properties
        [ExecuteAlways]
        protected abstract void OnValidate();

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

        // Hover 
        public virtual void Hover(bool val)
        {
            // Cant hover if disabled
            if (disabled) return;

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
        
        // Select
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
            // Cant select if disabled
            if (disabled) return;

            // Update state
            selected = val;

            if (!val && !hovered) UnhookController(controller);  
        }

        // Enable
        public virtual void Enable(bool val)
        {
            disabled = !val;
        }
    }
}


