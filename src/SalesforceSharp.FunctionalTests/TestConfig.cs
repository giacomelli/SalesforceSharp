using System;
using System.Configuration;
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
                var reader = new AppSettingsReader ();
                TokenRequestEndpointUrl = reader.GetValue("TokenRequestEndpointUrl", typeof(string)) as string;
                ClientId = reader.GetValue("ClientId", typeof (string)) as string;
                ClientSecret = reader.GetValue("ClientSecret", typeof (string)) as string;
                Username = reader.GetValue("Username", typeof (string)) as string;
                Password = reader.GetValue("Password", typeof (string)) as string;
                ObjectName = reader.GetValue("ObjectName", typeof (string)) as string;
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
