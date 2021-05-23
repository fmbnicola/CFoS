using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFoS.Supershape;
using UnityEngine.Networking;
using CFoS.Experimentation;

namespace CFoS.UI.Menus
{
    public class UIDoneMenu : UIMenu
    {
        [Header("Done Button")]
        public UIButtonHold DoneButton;


        protected virtual void Start()
        {
            // Render Update
            DoneButton.ButtonClickEvent.AddListener(Click);

            DoneButton.ButtonStartHoldEvent.AddListener(StartHold);
            DoneButton.ButtonStopHoldEvent.AddListener(StopHold);
        }

        public void Click()
        {
            this.NextTask();
        }

        public void StartHold()
        {
            MetricManager.Instance.PauseTimer();
        }

        public void StopHold()
        {
            MetricManager.Instance.StartTimer();
        }

    }
}