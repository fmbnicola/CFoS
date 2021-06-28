using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.Events;

namespace CFoS.UI.Menus
{
    public class UISubmitDataMenu : UIMenu
    {
        [Header("Submit Button")]
        public UIButtonLoad SubmitButton;

        [Header("Fill Form Button")]
        public UIButton FillFormButton;

        [Header("Info")]
        public Transform InstructionText;
        public Transform UserIDText;

        [Header("Event")]
        public UnityEvent SuccessEvent;

        private string FORM_URL = "https://docs.google.com/forms/d/e/1FAIpQLSdwP_6UTMY10cLCEqS8i9VgPQhkeDNLy1iYfxTjFyy2G7Lekw/viewform?usp=pp_url&entry.1571228065=";

        protected virtual void Start()
        {
            SubmitButton.ButtonClickEvent.AddListener(Submit);
            SubmitButton.LoadingFunction = SubmitDataLoadFunction;

            FillFormButton.ButtonClickEvent.AddListener(FillForm);

            // Text info
            InstructionText.gameObject.SetActive(true);
            UserIDText.gameObject.SetActive(false);
        }

        protected UIButtonLoad.LoadState SubmitDataLoadFunction()
        {
            return UIButtonLoad.LoadState.Loading;
        }

        protected void Submit()
        {
            var manager = SaveData.SaveManager.Instance;
            manager.OnSubmitResponse += ReceiveSubmitResponse;
            manager.SubmitExperimentData();
        }

        protected void ReceiveSubmitResponse(UnityWebRequest request)
        {
            var manager = SaveData.SaveManager.Instance;
            manager.OnSubmitResponse -= ReceiveSubmitResponse;

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Event
                SuccessEvent.Invoke();

                // Toggle Buttons
                SubmitButton.Success();
                SubmitButton.Enable(false);
                FillFormButton.Enable(true);

                // info
                InstructionText.gameObject.SetActive(false);
                UserIDText.gameObject.SetActive(true);

                // User ID
                var idtext = UserIDText.GetComponentInChildren<TextMeshPro>();
                idtext.text = idtext.text.Remove(idtext.text.Length - 3);
                idtext.text += manager.UserId;
            }
            else
            {
                SubmitButton.Failure();
            }
        }

        protected void FillForm()
        {
            var manager = SaveData.SaveManager.Instance;
            var userId = manager.UserId;
            Application.OpenURL(FORM_URL + userId);

            NextTask();
        }
    }
}