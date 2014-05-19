
namespace GG.SalesforceSharp.Security
{
    /// <summary>
    /// The informations about the authentication.
    /// </summary>
    public class AuthenticationInfo
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationInfo"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="instanceUrl">The instance URL.</param>
        public AuthenticationInfo(string accessToken, string instanceUrl)
        {
            AccessToken = accessToken;
            InstanceUrl = instanceUrl;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the access token that acts as a session ID that the application
        /// uses for making requests. This token should be protected as
        /// though it were user credentials.
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets the identifies (URL) the Salesforce instance to which API calls should be sent.
        /// </summary>
        public string InstanceUrl { get; private set; }
        #endregion
    }
}
