using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEngine.Networking;

namespace CFoS.UI.Menus
{
    public class UIMenuSubmitData : UIMenu
    {
        [Header("Submit Button")]
        public UIButtonLoad SubmitButton;

        [Header("Exit Button")]
        public UIButton ExitButton;


        protected virtual void Start()
        {
            // Render Update
            SubmitButton.ButtonClickEvent.AddListener(Submit);
            SubmitButton.LoadingFunction = SubmitDataLoadFunction;

            ExitButton.ButtonClickEvent.AddListener(Exit);
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
                SubmitButton.Success();
                SubmitButton.Enable(false);
                ExitButton.Enable(true);
            }
            else
            {
                SubmitButton.Failure();
            }
        }

        protected void Exit()
        {
            CloseMenu();
        }
    }
}