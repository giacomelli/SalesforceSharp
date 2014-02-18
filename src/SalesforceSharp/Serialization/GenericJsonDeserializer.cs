using System;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Json Deserializar using dynamic.
    /// <remarks>
    /// Original source code from http://www.csharpcity.com/2013/deserializing-to-dynamic-with-restsharp/.
    /// </remarks>
    /// </summary>
    internal class GenericJsonDeserializer : IDeserializer
    {
        #region Properties
        /// <summary>
        /// Gets or sets the root element.
        /// </summary>
        public string RootElement { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        public string DateFormat { get; set; }
        #endregion

        private SalesForceContractResolver salesForceContractResolver;

        public GenericJsonDeserializer(SalesForceContractResolver salesForceContractResolver)
        {
            if (salesForceContractResolver == null) throw new ArgumentNullException("salesForceContractResolver");
            this.salesForceContractResolver = salesForceContractResolver;
        }

        #region Methods
        /// <summary>
        /// Deserializes the specified response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings(){ContractResolver = salesForceContractResolver});
        }
        #endregion
    }
}