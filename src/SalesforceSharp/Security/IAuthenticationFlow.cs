using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceSharp.Security
{
    /// <summary>
    /// Defines a OAuth 2.0 authentication flow.
    /// <remarks>
    /// Salesforce uses authentication to allow users to securely access data without having to reveal username and password credentials.
    ///
    /// Before making REST API calls, you must authenticate the user using OAuth 2.0. To do so, you’ll need to:
    ///     • Set up a remote access application definition in Salesforce.
    ///     • Determine the correct OAuth endpoint to use.
    ///     • Authenticate the user via one of several different OAuth 2.0 authentication flows. An OAuth authentication flow defines
    ///     a series of steps used to coordinate the authentication process between your application and Salesforce. Supported OAuth
    ///     flows include:
    ///         ◊ Web server flow, where the server can securely protect the consumer secret.
    ///         ◊ User-agent flow, used by applications that cannot securely store the consumer secret.
    ///         ◊ Username-password flow, where the application has direct access to user credentials.
    ///         
    /// After successfully authenticating the user, you’ll receive an access token which can be used to make authenticated REST API calls.
    /// 
    /// More info at: 
    /// Digging Deeper into OAuth 2.0 on Force.com: http://wiki.developerforce.com/page/Digging_Deeper_into_OAuth_2.0_on_Force.com
    /// Force.com REST API Developer's Guide: docs/Force.com REST API Developer's Guide.pdf
    /// </remarks>
    /// </summary>
    public interface IAuthenticationFlow
    {
        /// <summary>
        /// Authenticate in the Salesforce REST's API.
        /// </summary>
        /// <remarks>
        /// If authentiaction fails an SalesforceException will be throw.
        /// </remarks>
        /// <returns>The authentication info with access token and instance url for futher API calls.</returns>
        AuthenticationInfo Authenticate();
    }
}
