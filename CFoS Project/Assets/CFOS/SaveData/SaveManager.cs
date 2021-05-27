using CFoS.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

 
namespace CFoS.SaveData
{
    [System.Serializable]
    public class SaveData
    {
        [SerializeField]
        public List<KeyValuePair<string, string>> SavedData;

        public SaveData()
        {
            SavedData = new List<KeyValuePair<string, string>>();
        }

        public void Add(string key, string val)
        {
            SavedData.Add(new KeyValuePair<string, string>(key, val));
        }

        public void ConcatenateData(SaveData data)
        {
            SavedData.AddRange(data.SavedData);
        }

        public void Clear()
        {
            SavedData.Clear();
        }

        public string Serialize()
        {
            var serializedData = "{";
            for(int i = 0; i < SavedData.Count; i ++)
            {
                var pair = SavedData[i];
                serializedData += "\"" + pair.Key + "\": \"" + pair.Value + "\"";
                if(i < SavedData.Count - 1)
                {
                    serializedData += ",";
                }
            }
            serializedData += "}";
            return serializedData;
        }
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        // CONSTANTS
        private const string URI = "https://api.pageclip.co/data";
        private const string FORM = "UserTesting_0";
        private const string API_KEY = "api_sNcXtXUXzzFu47paLJXHYeGHLYM7PCAz";

        // Save data structure
        [SerializeField]
        protected SaveData SavedData;

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
            SavedData = new SaveData();
            RegisterUserData();
        }


        // Register Data
        protected void ClearAllData()
        {
            SavedData.Clear();
        }

        protected void RegisterData(SaveData data)
        {
            SavedData.ConcatenateData(data);
        }

        protected void RegisterUserData()
        {
            var userData = new SaveData();
            var uniqueId = SystemInfo.deviceUniqueIdentifier;
            userData.Add("DeviceId", uniqueId);
            RegisterData(userData);
        }


        // Public Methods
        public void ResetData()
        {
            ClearAllData();
            RegisterUserData();
        }

        public void SaveData(SaveData data)
        {
            RegisterData(data);
        }


        // Submit Data To Server
        protected string Authenticate(string username, string password)
        {
            string auth = username + ":" + password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }

        protected IEnumerator DoSubmitData()
        {
            string uri = URI + "/" + FORM;
            var data = SavedData.Serialize();
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
