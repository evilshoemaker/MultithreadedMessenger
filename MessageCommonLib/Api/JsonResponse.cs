using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCommonLib.Api
{
    public class JsonResponse
    {
        public enum ResponseResults
        {
            Success,
            Error
        }

        public JsonResponse()
        {

        }

        public JsonResponse(ResponseResults result, string description = "", string requestId = "")
        {
            Result = result;
            Description = description;
        }

        public ResponseResults Result { get; set; }
        public string Description { get; set; }
        public string RequestId { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static JsonResponse FromJson(string jsonString)
        {
            return (JsonResponse)JsonConvert.DeserializeObject(jsonString, typeof(JsonResponse));
        }
    }
}
