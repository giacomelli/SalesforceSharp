using System.Collections.Generic;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using HelperSharp;

namespace SalesforceSharp
{
    /// <summary>
    /// The central point to communicate with Salesforce REST API.
    /// </summary>
    public class SalesforceClient
    {
        #region Fields
        private string m_accessToken;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        /// <remarks>
        /// The default value is v28.0.
        /// </remarks>
        /// <value>
        /// The API version.
        /// </value>
        public string ApiVersion { get; set; }


        /// <summary>
        /// Gets or sets the authorize URL.
        /// </summary>
        /// <remarks>
        /// The default value is https://login.salesforce.com/services/oauth2/token.
        /// For sandbox use "https://test.salesforce.com/services/oauth2/token.
        /// </remarks>
        /// <value>
        /// The authorize URL.
        /// </value>
        public string AuthorizeUrl { get; set; }


        /// <summary>
        /// Gets the client id.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId { get; private set; }


        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Gets the instance URL.
        /// </summary>
        /// <value>
        /// The instance URL.
        /// </value>
        public string InstanceUrl { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SalesforceClient"/> class.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        public SalesforceClient(string clientId, string clientSecret)
        {
            ApiVersion = "v28.0";
            AuthorizeUrl = "https://login.salesforce.com/services/oauth2/token";            
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Authenticates the client.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="SalesforceException"></exception>
        public bool Authenticate(string username, string password)
        {            
            var restClient = new RestClient();
            restClient.BaseUrl = AuthorizeUrl;            

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", ClientId);
            request.AddParameter("client_secret", ClientSecret);                        
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            var response = restClient.Post(request);
            IsAuthenticated = response.StatusCode == HttpStatusCode.OK;

            var deserializer = new JsonDeserializer();
            var responseData = deserializer.Deserialize<Dictionary<string, string>>(response);
                                       
            if (IsAuthenticated)
            {
                m_accessToken = responseData["access_token"];
                InstanceUrl = responseData["instance_url"];
            }
            else
            {
                throw new SalesforceException(responseData["error"], responseData["error_description"]);
            }
             
            return IsAuthenticated;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        public void GetObject(string objectName)
        {
            var client = new RestClient(GetObjectsUrl());
            var request = new RestRequest(objectName);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + m_accessToken);

            var response = client.Get(request);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Gets the objects URL.
        /// </summary>
        /// <returns></returns>
        private string GetObjectsUrl()
        {
            return "{0}/services/data/{1}/sobjects".With(InstanceUrl, ApiVersion);
        }
        #endregion
    }
}
