using System.Net;
using HelperSharp;
using RestSharp;
using SalesforceSharp.Serialization;
using System;

namespace SalesforceSharp.Security
{
    /// <summary>
    /// The username-password authentication flow can be used to authenticate when the consumer already has the user’s credentials.
    /// In this flow, the user’s credentials are used by the application to request an access token as shown in the following steps.
    /// <remarks>
    /// Warning: This OAuth authentication flow involves passing the user’s credentials back and forth. Use this
    /// authentication flow only when necessary. No refresh token will be issued.
    /// 
    /// You should only use the password access grant type in situations such as an AUTONOMOUS CLIENT, where a user cannot be present 
    /// at application startup. In this instance, you should carefully set the API user's permissions to minimize its access as far as possible, 
    /// and protect the API user's stored credentials from unauthorized access.
    /// 
    /// More info at:
    /// http://wiki.developerforce.com/page/Digging_Deeper_into_OAuth_2.0_on_Force.com
    /// </remarks>
    /// </summary>
    public class UsernamePasswordAuthenticationFlow : IAuthenticationFlow
    {
        #region Fields
        private IRestClient m_restClient;
        private string m_clientId;
        private string m_clientSecret;
        private string m_username;
        private string m_password;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePasswordAuthenticationFlow"/> class.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public UsernamePasswordAuthenticationFlow(string clientId, string clientSecret, string username, string password) : 
            this(new RestClient(), clientId, clientSecret, username, password)
        {            
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePasswordAuthenticationFlow"/> class.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="tokenRequestEndpointUrl">The token request endpoint url.</param>
        public UsernamePasswordAuthenticationFlow(string clientId, string clientSecret, string username, string password, string tokenRequestEndpointUrl) : 
            this(new RestClient(), clientId, clientSecret, username, password, tokenRequestEndpointUrl)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePasswordAuthenticationFlow"/> class.
        /// </summary>
        /// <param name="restClient">The REST client which will be used.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="tokenRequestEndpointUrl">The token request endpoint url.</param>
        internal UsernamePasswordAuthenticationFlow (IRestClient restClient, string clientId, string clientSecret, string username, string password, string tokenRequestEndpointUrl = "https://login.salesforce.com/services/oauth2/token")
        {
            ExceptionHelper.ThrowIfNull("restClient", restClient);
            ExceptionHelper.ThrowIfNullOrEmpty("clientId", clientId);
            ExceptionHelper.ThrowIfNullOrEmpty("clientSecret", clientSecret);
            ExceptionHelper.ThrowIfNullOrEmpty("username", username);
            ExceptionHelper.ThrowIfNullOrEmpty("password", password);

            m_restClient = restClient;
            m_clientId = clientId;
            m_clientSecret = clientSecret;
            m_username = username;
            m_password = password;
            TokenRequestEndpointUrl = tokenRequestEndpointUrl;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the token request endpoint url.
        /// </summary>
        /// <remarks>
        /// The default value is https://login.salesforce.com/services/oauth2/token.
        /// For sandbox use "https://test.salesforce.com/services/oauth2/token.
        /// </remarks>
        public string TokenRequestEndpointUrl { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Authenticate in the Salesforce REST's API.
        /// </summary>
        /// <returns>
        /// The authentication info with access token and instance url for futher API calls.
        /// </returns>
        /// <remarks>
        /// If authentiaction fails an SalesforceException will be throw.
        /// </remarks>
        public AuthenticationInfo Authenticate()
        {
            Uri uri = new Uri(TokenRequestEndpointUrl);
            m_restClient.BaseUrl = uri;

            var request = new RestRequest (Method.POST) {
                RequestFormat = DataFormat.Json
            };
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", m_clientId);
            request.AddParameter("client_secret", m_clientSecret);
            request.AddParameter("username", m_username);
            request.AddParameter("password", m_password);

            var response = m_restClient.Post(request);
            var isAuthenticated = response.StatusCode == HttpStatusCode.OK;

            var deserializer = new GenericJsonDeserializer (new SalesforceContractResolver (false));
            var responseData = deserializer.Deserialize<dynamic>(response);

            if (isAuthenticated)
            {
                return new AuthenticationInfo(responseData.access_token.Value, responseData.instance_url.Value);
            }
            else
            {
                throw new SalesforceException(responseData.error.Value, responseData.error_description.Value);
            }            
        }
        #endregion
    }
}
