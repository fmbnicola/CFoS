using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEngine.Networking;
using TMPro;

namespace CFoS.UI.Menus
{
    public class UIDisclaimerMenu : UIMenu
    {
        [Header("Agree Toggle")]
        public UIToggle AgreeToggle;

        [Header("Continue Button")]
        public UIButton ContinueButton;

        protected virtual void Start()
        {
            // Callbacks
            AgreeToggle.ToggleCheckedEvent.AddListener(Agree);
            AgreeToggle.ToggleUncheckedEvent.AddListener(Disagree);

            ContinueButton.Enable(false);
            ContinueButton.ButtonClickEvent.AddListener(Continue);
        }

        protected void Agree()
        {
            ContinueButton.Enable(true);
        }

        protected void Disagree()
        {
            ContinueButton.Enable(false);
        }

        protected void Continue()
        {
            NextTask();
        }
    }
}