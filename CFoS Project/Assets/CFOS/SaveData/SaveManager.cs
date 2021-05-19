using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CFoS.SaveData
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        // CONSTANTS
        private const string URI = "https://api.pageclip.co/data";
        private const string FORM = "UserTesting_0";
        private const string API_KEY = "api_sNcXtXUXzzFu47paLJXHYeGHLYM7PCAz";

        // Save data structure
        [Header("Save Data")]
        [SerializeField]
        protected List<Data.SaveData> SaveData;

        // Response Delegate
        public delegate void OnSubmitResponseDelegate(UnityWebRequest request);
        public event OnSubmitResponseDelegate OnSubmitResponse;


        // Unity Events
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
        }

        private void Start()
        {
            RegisterUserData();
        }


        // Register Data
        protected void ClearAllData()
        {
            foreach (var data in SaveData)
            {
                Destroy(data);
            }
            SaveData.Clear();
        }

        protected void RegisterUserData()
        {
            var userData = ScriptableObject.CreateInstance<Data.UserData>();
            userData.deviceId = "1"; // TODO: get unique id
            SaveData.Add(userData);
        }

        protected void RegisterTaskData(Data.TaskData taskData)
        {
            SaveData.Add(taskData);
        }


        // Public Methods
        public void Reset()
        {
            ClearAllData();
            RegisterUserData();
        }

        public void SaveTask(Data.TaskData taskData)
        {
            RegisterTaskData(taskData);
        }


        // Submit Data To Server
        protected string Authenticate(string username, string password)
        {
            string auth = username + ":" + password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }

        protected string SerializeData()
        {
            string serializedData = "";
            foreach(var obj in SaveData)
            {
                serializedData += JsonUtility.ToJson(obj);
            }
            return serializedData;
        }

        protected IEnumerator DoSubmitData()
        {
            string uri = URI + "/" + FORM;
            var data = SerializeData();
            var auth = Authenticate(API_KEY, "");

            UnityWebRequest webRequest = UnityWebRequest.Put(uri, data);
            webRequest.SetRequestHeader("Authorization", auth);
            webRequest.SetRequestHeader("Content-Type", "application/json");

            using (webRequest)
            {
                // Request and wait for response
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Data Processing Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.Log(pages[page] + ": Connection Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;
                }

                OnSubmitResponse?.Invoke(webRequest);
            }
        }

        public void SubmitData()
        {
            StartCoroutine(DoSubmitData());
        }

    }
}
