using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
    {
    public class UIButtonLoad : UIButton
    {
        public AudioClip FailureAudio;
        public AudioClip SuccessAudio;

        [Header("Icons")]
        public Shapes.ShapeRenderer LoadIcon;
        public Data.ColorVariable LoadIconColor;
        public Shapes.ShapeRenderer SuccessIcon;
        public Data.ColorVariable SuccessIconColor;
        public Shapes.ShapeRenderer FailureIcon1;
        public Shapes.ShapeRenderer FailureIcon2;
        public Data.ColorVariable FailureIconColor;

        [Header("Loading")]
        public float LoadIconSpeed = -500.0f;

        [Header("Text")]
        public string LoadingMessage;
        public string FailureMessage;
        public string SuccessMessage;
        protected string defaultMessage;

        [Space(5)]
        public UnityEvent ButtonSuccessEvent;
        public UnityEvent ButtonFailureEvent;

        public enum LoadState { Normal, Loading, Failure, Success};
        protected LoadState state = LoadState.Normal;

        public delegate LoadState LoadFunction();
        public LoadFunction LoadingFunction = delegate () { return LoadState.Loading; };

        // Init
        [ExecuteAlways]
        protected override void OnValidate()
        {
            base.OnValidate();

            LoadIcon.Color     = LoadIconColor.Value;
            SuccessIcon.Color  = SuccessIconColor.Value;
            FailureIcon1.Color = FailureIconColor.Value;
            FailureIcon2.Color = FailureIconColor.Value;
        }

        protected override void Awake()
        {
            base.Awake();

            defaultMessage = Text.text;
            ResetState();

            ButtonClickEvent.AddListener(Load);

            // Sound
            ButtonSuccessEvent.AddListener(() =>
            {
                UIManager.Instance.PlaySound(SuccessAudio);
            });
            ButtonFailureEvent.AddListener(() =>
            {
                UIManager.Instance.PlaySound(FailureAudio);
            });
        }


        public override void Update()
        {
            // Loading
            if (state == LoadState.Loading)
            {
                if (disabled)
                {
                    var col = LoadIconColor.Value; col.a = 0.3f;
                    LoadIcon.Color = col;
                    col = SuccessIconColor.Value; col.a = 0.3f;
                    SuccessIcon.Color = col;
                    col = FailureIconColor.Value; col.a = 0.3f;
                    FailureIcon1.Color = col;
                    FailureIcon2.Color = col;
                }
                else
                {
                    LoadIcon.Color = LoadIconColor.Value;
                    SuccessIcon.Color = SuccessIconColor.Value;
                    FailureIcon1.Color = FailureIconColor.Value;
                    FailureIcon2.Color = FailureIconColor.Value;
                }

                // animation
                var rotation = LoadIcon.transform.localEulerAngles;
                rotation.z += LoadIconSpeed * Time.deltaTime;
                LoadIcon.transform.localRotation = Quaternion.Euler(rotation);

                // update based on loading function
                var loadResult = LoadingFunction();
                if (loadResult == LoadState.Failure) Failure();
                else if (loadResult == LoadState.Success) Success();
                else if (loadResult == LoadState.Normal) ResetState();
            }

            base.Update();
        }

        // Actions
        public void Load()
        {
            state = LoadState.Loading;

            Enable(false);
            StartCoroutine(DeSelect(SelectTime));

            // Visual Update
            LoadIcon.enabled = true;
            SuccessIcon.enabled = false;
            FailureIcon1.enabled = false;
            FailureIcon2.enabled = false;
            Text.text = LoadingMessage;
        }

        public void Failure()
        {
            state = LoadState.Failure;
            ButtonFailureEvent.Invoke();

            Enable(true);

            // Visual Update
            LoadIcon.enabled = false;
            SuccessIcon.enabled = false;
            FailureIcon1.enabled = true;
            FailureIcon2.enabled = true;
            LoadIcon.transform.localRotation= Quaternion.identity;
            Text.text = FailureMessage;
        }

        public void Success()
        {
            state = LoadState.Success;
            ButtonSuccessEvent.Invoke();

            Enable(true);

            // Visual Update
            LoadIcon.enabled = false;
            SuccessIcon.enabled = true;
            FailureIcon1.enabled = false;
            FailureIcon2.enabled = false;
            LoadIcon.transform.localRotation = Quaternion.identity;
            Text.text = SuccessMessage;
        }

        public void ResetState()
        {
            state = LoadState.Normal;

            Enable(true);

            // Visual Update
            LoadIcon.enabled = false;
            SuccessIcon.enabled = false;
            FailureIcon1.enabled = false;
            FailureIcon2.enabled = false;
            LoadIcon.transform.rotation = Quaternion.identity;
            Text.text = defaultMessage;
        }

        // TEST CALLBACKS
        public void TestSuccessCallback()
        {
            Debug.Log("Button Load Success -> " + gameObject.name);
        }

        public void TestFailureCallback()
        {
            Debug.Log("Button Load Failure -> " + gameObject.name);
        }
    }
}