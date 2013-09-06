using System;
using TestSharp;

namespace SalesforceSharp.UnitTests
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
                TokenRequestEndpointUrl = ConfigHelper.ReadAppSetting("SalesforceSharp.UnitTests", "TokenRequestEndpointUrl");
                ClientId = ConfigHelper.ReadAppSetting("SalesforceSharp.UnitTests", "ClientId");
                ClientSecret = ConfigHelper.ReadAppSetting("SalesforceSharp.UnitTests", "ClientSecret");
                Username = ConfigHelper.ReadAppSetting("SalesforceSharp.UnitTests", "Username");
                Password = ConfigHelper.ReadAppSetting("SalesforceSharp.UnitTests", "Password");
                ObjectName = ConfigHelper.ReadAppSetting("SalesforceSharp.UnitTests", "ObjectName");                
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Please, check the Salesforce.UnitTests' App.config and define the test configurations.");
            }

            if (String.IsNullOrWhiteSpace(TokenRequestEndpointUrl)
            || String.IsNullOrWhiteSpace(ClientId)
            || String.IsNullOrWhiteSpace(ClientSecret)
            || String.IsNullOrWhiteSpace(Username)
            || String.IsNullOrWhiteSpace(Password)
            || String.IsNullOrWhiteSpace(ObjectName))
            {
                throw new InvalidOperationException("Please, check the Salesforce.UnitTests' App.config and define ALL the test configurations.");
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
