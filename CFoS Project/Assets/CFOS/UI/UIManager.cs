using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public XRRig PlayerRig;

        [Header("Main Menu")]
        public GameObject MainMenuAsset;
        private GameObject mainMenuInstance;
        public bool MainMenuStartOpen = true;
        public bool MainMenuEnabled = true;

        [Header("Properties")]
        public float UIOffsetDistance = 0.5f;

        // INPUT
        [Space(10)]
        [SerializeField]
        [Tooltip("Action to bring up main menu.")]
        private InputActionProperty menuAction;
        public InputActionProperty MenuAction
        {
            get => menuAction;
            set => SetInputActionProperty(ref menuAction, value);
        }

        protected AudioSource AudioSource;

        void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
        {
            if (Application.isPlaying)
                property.DisableDirectAction();

            property = value;

            if (Application.isPlaying && isActiveAndEnabled)
                property.EnableDirectAction();
        }

        protected void OnEnable()
        {
            menuAction.EnableDirectAction();
        }

        protected void OnDisable()
        {
            menuAction.DisableDirectAction();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            AudioSource = GetComponent<AudioSource>();

            menuAction.action.performed += ToggleMainMenu;
        }

        private void Start()
        {
            if (MainMenuStartOpen)
            {
                OpenMainMenu();
            }
        }


        // Main Menu
        public void CloseMainMenu()
        {
            CloseMenu(ref mainMenuInstance);
        }

        public void OpenMainMenu()
        {
            if (mainMenuInstance != null) return;
            mainMenuInstance = OpenMenu(MainMenuAsset);
        }

        public void ToggleMainMenu(CallbackContext contex)
        {
            if (!MainMenuEnabled) return;

            if (mainMenuInstance == null)
            {
                OpenMainMenu();
            }
            else
            {
                CloseMainMenu();
            }
        }


        // Open new menu
        public static GameObject OpenMenu(GameObject menuObj)
        {
            // position window in front of player
            var cam = Instance.PlayerRig.cameraGameObject;
            var pos = cam.transform.position;

            var look = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
            var offset = look * Instance.UIOffsetDistance;
            var rotation = Quaternion.LookRotation(look);

            GameObject obj;
            obj = Instantiate(menuObj, Instance.transform);
            obj.transform.position = pos + offset;
            obj.transform.rotation = rotation;

            return obj;
        }

        // Close existing menu
        public static void CloseMenu(ref GameObject menuInstance)
        {
            var menu = menuInstance.GetComponent<UIMenu>();
            if(menu != null) menu.CloseMenu();

            menuInstance = null;
        }

        public void PlaySound(AudioClip clip)
        {
            // audio pitch
            var volume = 1.0f - (Random.Range(-0.1f, 0.1f));
            var pitch  = 1.0f - (Random.Range(-0.1f, 0.1f));

            AudioSource.volume = volume;
            AudioSource.pitch = pitch;
            AudioSource.PlayOneShot(clip);
        }
    }
}
