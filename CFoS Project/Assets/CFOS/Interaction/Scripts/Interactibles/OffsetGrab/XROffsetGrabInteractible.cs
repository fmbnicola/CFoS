using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


namespace CFoS.Interaction
{
    public class XROffsetGrabInteractible : XRGrabInteractable
    {
        private Vector3 interactorPosition = Vector3.zero;
        private Quaternion interactorRotation = Quaternion.identity;

        // Select enter and exit events
        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
            StoreInteractor(args.interactor);
            MatchAttachementPoints(args.interactor);
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
            ResetAttachementPoints(args.interactor);
            ClearInteractor(args.interactor);
        }

        // Methods
        private void StoreInteractor(XRBaseInteractor interactor)
        {
            interactorPosition = interactor.attachTransform.localPosition;
            interactorRotation = interactor.attachTransform.localRotation;
        }

        private void MatchAttachementPoints(XRBaseInteractor interactor)
        {
            bool hasAttach = (attachTransform != null);
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        }

        private void ResetAttachementPoints(XRBaseInteractor interactor)
        {
            interactor.attachTransform.localPosition = interactorPosition;
            interactor.attachTransform.localRotation = interactorRotation;
        }

        private void ClearInteractor(XRBaseInteractor interactor)
        {
            interactorPosition = Vector3.zero;
            interactorRotation = Quaternion.identity;
        }
    }
}


