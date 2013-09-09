using System;
using TestSharp;

namespace SalesforceSharp.FunctionalTests
{
    /// <summary>
    /// Test's configurations.
    /// <remarks>
    /// PLEASE, DO NOT USE PRODUCTION DATA FOR TESTS. SANDBOX EXISTS FOR THIS PURPOSE.
    /// </remarks>
    /// </summary>
    public static class TestConfig
    {
        #region Constructors
        static TestConfig()
        {
            try
            {
                TokenRequestEndpointUrl = ConfigHelper.ReadAppSetting("SalesforceSharp.FunctionalTests", "TokenRequestEndpointUrl");
                ClientId = ConfigHelper.ReadAppSetting("SalesforceSharp.FunctionalTests", "ClientId");
                ClientSecret = ConfigHelper.ReadAppSetting("SalesforceSharp.FunctionalTests", "ClientSecret");
                Username = ConfigHelper.ReadAppSetting("SalesforceSharp.FunctionalTests", "Username");
                Password = ConfigHelper.ReadAppSetting("SalesforceSharp.FunctionalTests", "Password");
                ObjectName = ConfigHelper.ReadAppSetting("SalesforceSharp.FunctionalTests", "ObjectName");                
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Please, check the Salesforce.FunctionalTests' App.config and define the test configurations.", ex);
            }

            if (String.IsNullOrWhiteSpace(TokenRequestEndpointUrl)
            || String.IsNullOrWhiteSpace(ClientId)
            || String.IsNullOrWhiteSpace(ClientSecret)
            || String.IsNullOrWhiteSpace(Username)
            || String.IsNullOrWhiteSpace(Password)
            || String.IsNullOrWhiteSpace(ObjectName))
            {
                throw new InvalidOperationException("Please, check the Salesforce.FunctionalTests' App.config and define ALL the test configurations.");
            }           
        }
        #endregion

        #region Properties
        public static string TokenRequestEndpointUrl { get; set; }
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string ObjectName { get; set; }
        #endregion
    }
}
