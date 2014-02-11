using Newtonsoft.Json;
using RestSharp.Serializers;

namespace SalesforceSharp.Serialization
{
    internal class GenericJsonSerializer : ISerializer
    {
        private SalesForceContractResolver salesForceContractResolver;

        public GenericJsonSerializer(SalesForceContractResolver salesForceContractResolver)
        {
            this.salesForceContractResolver = salesForceContractResolver;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings(){ContractResolver = salesForceContractResolver});
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}