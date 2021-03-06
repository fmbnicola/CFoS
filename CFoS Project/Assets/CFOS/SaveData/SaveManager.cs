using CFoS.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

 
namespace CFoS.SaveData
{
    // Save Data Structure
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


    // JSON API Payload Structure
    [System.Serializable]
    public class Payload
    {
        public string UserId;
        public string DeviceId;
    }

    [System.Serializable]
    public class Item
    {
        public string itemEid;
        public Payload payload;
        public string createdAt;
        public string archivedAt;
    }

    [System.Serializable]
    public class Form
    {
        public string form;
        public string dataType;
        [SerializeField]
        public List<Item> data;
    }


    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        // CONSTANTS
        private const string URI = "https://api.pageclip.co/data";
        private const string DATA_FORM = "";
        private const string IDS_FORM  = "";
        private const string API_KEY = "";

        // List of words to filter out (for politeness sake)
        private readonly List<string> badWords = new List<string>()
        {
            "ass","fuc","fuk","fuq","fux","fck","coc","cok","coq","kox","koc",
            "kok","koq","cac","cak","caq","kac","kak","kaq","dic","dik","diq",
            "dix","dck","pns","psy","fag","fgt","ngr","nig","cnt","knt","sht",
            "dsh","twt","bch","cum","clt","kum","klt","suc","suk","suq","sck",
            "lic","lik","liq","lck","jiz","jzz","gay","gey","gei","gai","vag",
            "vgn","sjv","fap","prn","lol","jew","joo","gvr","pus","pis","pss",
            "snm","tit","fku","fcu","fqu","hor","slt","jap","wop","kik","kyk",
            "kyc","kyq","dyk","dyq","dyc","kkk","jyz","prk","prc","prq","mic",
            "mik","miq","myc","myk","myq","guc","guk","guq","giz","gzz","sex",
            "sxx","sxi","sxe","sxy","xxx","wac","wak","waq","wck","pot","thc",
            "vaj","vjn","nut","std","lsd","poo","azn","pcp","dmn","orl","anl",
            "ans","muf","mff","phk","phc","phq","xtc","tok","toc","toq","mlf",
            "rac","rak","raq","rck","sac","sak","saq","pms","nad","ndz","nds",
            "wtf","sol","sob","fob","sfu","wap","crl","fdp","fds"
        };

        // Save data structure
        [SerializeField]
        protected SaveData SavedData;
    
        public Form receivedData;
        public string UserId { get;  protected set; }
        
        // Response Delegate
        public delegate void OnSubmitResponseDelegate(UnityWebRequest request);
        public event OnSubmitResponseDelegate OnSubmitResponse;


        // Init
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

            SavedData = new SaveData();
        }


        // Communicate with API 
        protected string Authenticate(string username, string password)
        {
            string auth = username + ":" + password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }


        // Get UserId
        protected string GenerateCode(int numChars)
        {
            var code = "";
            for (int i = 0; i < numChars; i++)
            {
                char c = (char)('A' + Random.Range(0, 26));
                code += c;
            }
            return code;
        }

        protected IEnumerator DoReceiveUsedIds()
        {
            string uri = URI + "/" + IDS_FORM + "?archived=false";
            var data = SavedData.Serialize();
            var auth = Authenticate(API_KEY, "");

            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
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

                // If successful Save all user ids
                if(webRequest.result == UnityWebRequest.Result.Success)
                {
                    string jsonData = webRequest.downloadHandler.text;
                    receivedData = JsonUtility.FromJson<Form>(jsonData);
                }
                // Otherwise abort request
                else
                {
                    OnSubmitResponse?.Invoke(webRequest);
                }
               
            }
        }

        protected IEnumerator DoGetUserId()
        {
            // populate submitedData structure
            yield return DoReceiveUsedIds();

            // generate valid code 
            int repeatTolerance = 5; // just to prevent infinite loop (probably will never happen tho)
            string id = "";
            while (true)
            {
                id = GenerateCode(3);

                var repeated = false;
                foreach(var item in receivedData.data)
                {
                    if(item.payload.UserId == id)
                    {
                        repeated = true;
                        break;
                    }
                }

                // if id is truly unique we are done
                if ((!badWords.Contains(id) && !repeated) || 
                    repeatTolerance == 0)
                {
                    //Save Id in database
                    UserId = id;
                    yield return DoSubmitUserID();

                    yield break;
                }

                repeatTolerance--;
                yield return null;
            }
        }

        protected IEnumerator DoSubmitUserID()
        {
            // Submit user id to server
            string uri = URI + "/" + IDS_FORM;
            var data = "{\"UserId\": \"" + UserId + "\"}";
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

        public void GetUserId()
        {
            StartCoroutine(DoGetUserId());
        }


        // Submit Experiment Data
        protected IEnumerator DoRegisterUserData()
        {
            // Save DeviceID and UserId
            var userData = new SaveData();

            var deviceId = SystemInfo.deviceUniqueIdentifier;
            userData.Add("DeviceId", deviceId);
            userData.Add("UserId", UserId);

            RegisterData(userData);

            yield return null;
        }

        protected IEnumerator DoSubmitxperimentData()
        {
            yield return StartCoroutine(DoRegisterUserData());

            // Submit all data to server
            string uri = URI + "/" + DATA_FORM;
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

        public void SubmitExperimentData()
        {
            StartCoroutine(DoSubmitxperimentData());
        }


        // Data Methods
        protected void RegisterData(SaveData data)
        {
            SavedData.ConcatenateData(data);
        }

        public void ResetData()
        {
            SavedData.Clear();
        }

        public void SaveData(SaveData data)
        {
            RegisterData(data);
        }
    }
}
