using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.Interaction
{

    public class HandInteractorManager : MonoBehaviour
    {
        public Grabber Grabber;

        public Pointer Pointer;

        public enum InteractorState { GrabberMode, PointerMode };
        private InteractorState state;


        private void Start()
        {
            GrabberModeInit();
            PointerModeEnd();
        }


        // Raycast to find UI elements
        bool CheckUINear()
        {
            //get the mask to raycast against UI only
            int layer_mask = LayerMask.GetMask("UI");

            //do the raycast specifying the mask
            return Physics.Raycast(Pointer.transform.position, Pointer.transform.up, Pointer.PointerDistance, layer_mask);
        }


        // Grabber Mode
        bool GrabberModeCheck()
        {
            // if not near UI element and not currently selecting UI element
            return (!CheckUINear() && Pointer.SelectedElement == null);
        }

        void GrabberModeInit()
        {
            Grabber.Activate(true);
            Grabber.Hover(false);

            state = InteractorState.GrabberMode;
        }

        void GrabberModeEnd()
        {
            Grabber.Activate(false);
        }


        // Pointer Mode
        bool PointerModeCheck()
        {
            // if we are near UI element (TODO: and we are not currently grabbing)
            return (CheckUINear() && Grabber.Interactor.selectTarget == null);
        }

        void PointerModeInit()
        {
            Pointer.Activate(true);
            Pointer.Hover(false);

            state = InteractorState.PointerMode;
        }

        void PointerModeEnd()
        {
            Pointer.Activate(false);
        }


        // State machine
        void Update()
        {
            switch (state)
            {
                case InteractorState.GrabberMode:

                    if (PointerModeCheck())
                    {
                        GrabberModeEnd();
                        PointerModeInit();
                    }
                    break;

                case InteractorState.PointerMode:

                    if (GrabberModeCheck())
                    {
                        PointerModeEnd();
                        GrabberModeInit();
                    }
                    break;
            }
        }
    }
}