using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.Data
{
    [CreateAssetMenu(fileName = "UserData", menuName = "CFoS/SaveData/UserData")]
    public class UserData : SaveData
    {
        public string deviceId; //android device id?
        
        // age, name, etc...
    }
}