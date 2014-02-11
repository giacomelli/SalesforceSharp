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
        
        #region Methods
        /// <summary>
        /// Deserializes the specified response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
        #endregion
    }
}