using System;
using NUnit.Framework;
using SalesforceSharp.Security;
using SalesforceSharp.UnitTests.Stubs;
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
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failure"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret,  "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidPassword_InvalidPassword()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidPassword, "authentication failure - invalid password"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, "invalid password"));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidClientId_InvalidClientId()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "client identifier invalid"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow("Invalid client id", TestConfig.ClientSecret, "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidClientSecret_InvalidClientSecret()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "invalid client credentials"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, "invalid client secret", "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }
    
        [Test]
        public void Authenticate_ValidCredentials_Authenticated()
        {
            var target = new SalesforceClient();
            target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password));
            Assert.IsTrue(target.IsAuthenticated);
        }
        #endregion    

        #region Query
        [Test]
        public void Query_InvalidQuery_Exception()
        {
            var target = CreateClientAndAuth();

            ExceptionAssert.IsThrowing(typeof(SalesforceException), () =>
            {
                target.Query<RecordStub>("SELECT id, name, FROM " + TestConfig.ObjectName);
            });            
        }

        [Test]
        public void Query_ValidQueryWithObject_Result()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name FROM " + TestConfig.ObjectName);
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                Assert.IsNotNullOrEmpty(actual[0].Id);
                Assert.IsNotNullOrEmpty(actual[0].Name);
            }
        }
        #endregion

        #region Update
        [Test]
        public void Update_InvalidId_Exception()
        {
            var target = CreateClientAndAuth();
            
            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.NotFound, "Provided external ID field does not exist or is not accessible: INVALID ID"), () => {
                target.Update(TestConfig.ObjectName, "INVALID ID", new { Name = "TEST" });            
            });
        }

        [Test]
        public void Update_ValidRecordWithAnonymous_Updated()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name, description FROM Account");
            Assert.IsNotNull(actual);
            
            if (actual.Count > 0)
            {
                Assert.IsTrue(target.Update("Account", actual[0].Id, new { Description = DateTime.Now + " UPDATED" }));
            }
        }

        [Test]
        public void Update_ValidRecordWithClass_Updated()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name, description FROM Account");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                Assert.IsTrue(target.Update("Account", actual[0].Id, new RecordStub {Name = actual[0].Name, Description = DateTime.Now + " UPDATED" }));
            }
        }
        #endregion

        #region Helpers
        private SalesforceClient CreateClientAndAuth()
        {
            return CreateClientAndAuth(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password);
        }

        private UsernamePasswordAuthenticationFlow CreateAuthenticationFlow(string clientId, string clientSecret, string username, string password)
        {
            var flow = new UsernamePasswordAuthenticationFlow(clientId, clientSecret, username, password);
            flow.TokenRequestEndpointUrl = TestConfig.TokenRequestEndpointUrl;

            return flow;
        }

        private SalesforceClient CreateClientAndAuth(
            string clientId, 
            string clientSecret, 
            string username, 
            string password)
        {
            var client = new SalesforceClient();
            var authenticationFlow = CreateAuthenticationFlow(clientId, clientSecret, username, password);            

            client.Authenticate(authenticationFlow);

            return client;
        }        
        #endregion
    }
}