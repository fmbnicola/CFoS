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
        public Transform ConnectInternetText;
        public Transform UserIDText;

        [Header("Event")]
        public UnityEvent SuccessEvent;

        private string FORM_URL = "https://docs.google.com/forms/d/e/1FAIpQLSdERtJnntOfx1RE8yUPkNdeVYRazF3oPjvxENarYG48ZGDE_A/viewform?usp=pp_url&entry.1625732217=";

        protected virtual void Start()
        {
            // Render Update
            SubmitButton.ButtonClickEvent.AddListener(Submit);
            SubmitButton.LoadingFunction = SubmitDataLoadFunction;

            FillFormButton.ButtonClickEvent.AddListener(FillForm);

            // Text info
            ConnectInternetText.gameObject.SetActive(true);
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
            manager.SubmitData();
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
                ConnectInternetText.gameObject.SetActive(false);
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