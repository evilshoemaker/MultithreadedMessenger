using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MessageCommonLib.Api
{
    public class JsonRequest
    {
        public string Method { get; set; }
        public string RequestId { get; set; }

        public JsonRequest()
        {
            RequestId = Guid.NewGuid().ToString();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static T FromJson<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject(json, typeof(T));
        }

        public static object FromJson(string jsonString)
        {
            JObject jsonObj = JObject.Parse(jsonString);

            if (!jsonObj.ContainsKey("Method"))
                throw new Exception("Incorrect request");

            string methodName = (string)jsonObj["Method"];

            switch (methodName)
            {
                case "login":
                    return JsonConvert.DeserializeObject(jsonString, typeof(LoginRequest));
                case "logout":
                    return JsonConvert.DeserializeObject(jsonString, typeof(LogoutRequest));
                case "send":
                    return JsonConvert.DeserializeObject(jsonString, typeof(SendMessageRequest));
                case "clientListBroadcast":
                    return JsonConvert.DeserializeObject(jsonString, typeof(ClientListBroadcast));
                default:
                    return null;
            }
        }
    }
}
