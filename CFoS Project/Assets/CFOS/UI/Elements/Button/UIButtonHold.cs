using CFoS.Experimentation;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
    {
    public class UIButtonHold : UIButton
    {
        public AudioClip FinishAudio;

        [Header("HoldBar")]
        public Shapes.Rectangle HoldBar;
        public Data.ColorVariable HoldBarColor;

        [Space(5)]
        public float HoldTime = 2.0f;
        private float animProgress = 0.0f; 

        [Space(5)]
        public UnityEvent ButtonStartHoldEvent;
        public UnityEvent ButtonStopHoldEvent;


        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            base.OnValidate();

            var ele = (Shapes.Rectangle) Element;
            var pos = HoldBar.transform.localPosition;
            pos.x = -ele.Width / 2;
            pos.y = -ele.Height / 2;
            HoldBar.transform.localPosition = pos;
            HoldBar.Color = HoldBarColor.Value;
        }

        protected override void Awake()
        {
            base.Awake();

            // HoldBar Animation
            animProgress = 0.0f;
            HoldBar.Width = 0.0f;

            // Audio
            ButtonStartHoldEvent.AddListener(() =>
            {
                UIManager.Instance.PlaySound(ClickAudio);
            });
            ButtonClickEvent.RemoveAllListeners();
            ButtonClickEvent.AddListener(() =>
            {
                UIManager.Instance.PlaySound(FinishAudio);
            });
        }

        public override void Update()
        {
            base.Update();

            var sign  = (selected) ? 1 : -1;
            var delta = (sign) * (Time.deltaTime / HoldTime);

            // if maxed out -> select!
            if (animProgress != 1 && (animProgress + delta) >= 1)
            {
                ButtonClickEvent.Invoke();

                if (controller) controller.SendHapticImpulse(0.5f, 0.02f);

                if (gameObject.activeInHierarchy)
                    StartCoroutine(DeSelect(SelectTime));

                animProgress = 0.0f;
            }

            // Animate
            animProgress += delta;
            animProgress = Mathf.Clamp01(animProgress);

            var ele = (Shapes.Rectangle)Element;
            HoldBar.Width = animProgress * ele.Width;
        }


        public override void SelectAux(bool val)
        {
            if (val)
            {
                ButtonStartHoldEvent.Invoke();
            }
            else
            {
                ButtonStopHoldEvent.Invoke();
            }
        }

        // TEST CALLBACKS
        public void TestStartHoldCallback()
        {
            Debug.Log("Button Start Hold -> " + gameObject.name);
        }

        public void TestStopHoldCallback()
        {
            Debug.Log("Button Stop Hold -> " + gameObject.name);
        }
    }
}