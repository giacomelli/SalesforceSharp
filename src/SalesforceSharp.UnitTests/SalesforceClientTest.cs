using System;
using NUnit.Framework;
using TestSharp;

namespace SalesforceSharp.UnitTests
{
    [TestFixture]
    public class SalesforceClientTest
    {
        #region Authenticate
        [Test]
        public void Authenticate_InvalidUsername_AuthenticationFailure()
        {
            var target = CreateClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failure"), () =>
            {
                target.Authenticate("invalid user name", TestConfig.Password);
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidPassword_InvalidPassword()
        {
            var target = CreateClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidPassword, "authentication failure - invalid password"), () =>
            {
                target.Authenticate(TestConfig.Username, "invalid password");
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidClientId_InvalidClientId()
        {
            var target = CreateClient("Invalid client id", TestConfig.ClientSecret);

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "client identifier invalid"), () =>
            {
                target.Authenticate(TestConfig.Username, TestConfig.Password);
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidClientSecret_InvalidClientSecret()
        {
            var target = CreateClient(TestConfig.ClientId, "invalid client secret");

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "invalid client credentials"), () =>
            {
                target.Authenticate(TestConfig.Username, TestConfig.Password);
            });

            Assert.IsFalse(target.IsAuthenticated);
        }
    
        [Test]
        public void Authenticate_ValidCredentials_Authenticated()
        {
            var target = CreateClient(TestConfig.ClientId, TestConfig.ClientSecret);
            Assert.IsTrue(target.Authenticate(TestConfig.Username, TestConfig.Password));
            Assert.IsTrue(target.IsAuthenticated);
        }
        #endregion

        #region Get
        [Test]
        public void GetObject_ValidObjectName_Objects()
        {
            var target = CreateClientAndAuth();
            target.GetObject(TestConfig.ObjectName);
        }
        #endregion

        #region Helpers
        private SalesforceClient CreateClient()
        {
            return CreateClient(TestConfig.ClientId, TestConfig.ClientSecret);
        }

        private SalesforceClient CreateClient(string clientId, string clientSecret)
        {
            var client = new SalesforceClient(clientId, clientSecret);
            client.AuthorizeUrl = TestConfig.AuthorizeUrl;

            return client;
        }

        private SalesforceClient CreateClientAndAuth()
        {
            return CreateClientAndAuth(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password);
        }

        private SalesforceClient CreateClientAndAuth(
            string clientId, 
            string clientSecret, 
            string username, 
            string password)
        {
            var client = CreateClient(clientId, clientSecret);
            client.Authenticate(username, password);

            return client;
        }        
        #endregion
    }
}