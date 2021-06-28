using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.Events;

namespace CFoS.UI.Menus
{
    public class UIDisclaimerMenu : UIMenu
    {
        [Header("Generate UserId Button ")]
        public UIButtonLoad GenerateIDButton;

        [Header("Fill Form Button")]
        public UIButton FillFormButton;

        [Header("Info")]
        public Transform InstructionText;
        public Transform UserIDText;

        [Header("Event")]
        public UnityEvent SuccessEvent;

        private string FORM_URL = "https://docs.google.com/forms/d/e/1FAIpQLSeSDHG_aQw-DmIZIQzvNLcTzGldryI5a1fT_qYaQ_6MpaEWyQ/viewform?usp=pp_url&entry.113491696=";

        protected virtual void Start()
        {
            GenerateIDButton.ButtonClickEvent.AddListener(GenerateId);
            GenerateIDButton.LoadingFunction = GenerateIdLoadFunction;

            FillFormButton.ButtonClickEvent.AddListener(FillForm);

            // Text info
            InstructionText.gameObject.SetActive(true);
            UserIDText.gameObject.SetActive(false);
        }

        protected UIButtonLoad.LoadState GenerateIdLoadFunction()
        {
            return UIButtonLoad.LoadState.Loading;
        }

        protected void GenerateId()
        {
            var manager = SaveData.SaveManager.Instance;
            manager.OnSubmitResponse += ReceiveGenerateIdResponse;
            manager.GetUserId();
        }

        protected void ReceiveGenerateIdResponse(UnityWebRequest request)
        {
            var manager = SaveData.SaveManager.Instance;
            manager.OnSubmitResponse -= ReceiveGenerateIdResponse;

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Event
                SuccessEvent.Invoke();

                // Toggle Buttons
                GenerateIDButton.Success();
                GenerateIDButton.Enable(false);
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
                GenerateIDButton.Failure();
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