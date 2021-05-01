using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.Interaction
{

    public class HandInteractorManager : MonoBehaviour
    {
        public Grabber Grabber;
        private XRBaseInteractor grabberInteractor;

        public Pointer Pointer;
        private XRBaseInteractor pointerInteractor;

        public enum InteractorState { GrabberMode, PointerMode };
        private InteractorState state;


        private void Start()
        {
            GrabberModeInit();
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
            return (!CheckUINear() && Pointer.Interactor.selectTarget == null);
        }

        void GrabberModeInit()
        {
            Pointer.gameObject.SetActive(false);
            Grabber.gameObject.SetActive(true);
            Grabber.Hover(false);

            state = InteractorState.GrabberMode;
        }


        // Pointer Mode
        bool PointerModeCheck()
        {
            // if we are near UI element (TODO: and we are not currently grabbing)
            return (CheckUINear() && Grabber.Interactor.selectTarget == null);
        }

        void PointerModeInit()
        {
            Grabber.gameObject.SetActive(false);
            Pointer.gameObject.SetActive(true);
            Pointer.Hover(false);

            state = InteractorState.PointerMode;
        }


        // State machine
        void Update()
        {
            switch (state)
            {
                case InteractorState.GrabberMode:

                    if (PointerModeCheck())
                    {
                        PointerModeInit();
                    }
                    break;

                case InteractorState.PointerMode:

                    if (GrabberModeCheck())
                    {
                        GrabberModeInit();
                    }
                    break;
            }
        }
    }
}