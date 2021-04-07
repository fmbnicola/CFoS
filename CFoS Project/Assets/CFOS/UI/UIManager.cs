using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public XRRig PlayerRig;

        [Header("Main Menu")]
        public GameObject MainMenuAsset;
        private GameObject mainMenuInstance;

        [Header("Properties")]
        public float UIOffsetDistance = 0.5f;

        [Space(10)]
        [SerializeField]
        [Tooltip("Action to bring up main menu.")]
        private InputActionProperty menuAction;
        public InputActionProperty MenuAction
        {
            get => menuAction;
            set => SetInputActionProperty(ref menuAction, value);
        }

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

            menuAction.action.performed += ToggleMainMenu;
        }


        // Main Menu
        public void ToggleMainMenu(CallbackContext contex)
        {
            if (mainMenuInstance == null)
            {
                mainMenuInstance = OpenMenu(MainMenuAsset);
            }
            else
            {
                CloseMenu(ref mainMenuInstance);
            }
        }


        // Open new Menu
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

        // Close Menu
        public static void CloseMenu(ref GameObject menuInstance)
        {
            var menu = menuInstance.GetComponent<UIMenu>();
            if(menu != null) menu.CloseMenu();

            menuInstance = null;
        }
    }
}
