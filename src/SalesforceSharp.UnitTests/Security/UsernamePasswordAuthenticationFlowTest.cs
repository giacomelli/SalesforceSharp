using System;
using NUnit.Framework;
using SalesforceSharp.Security;
using TestSharp;

namespace SalesforceSharp.UnitTests.Security
{
    [TestFixture]
    public class UsernamePasswordAuthenticationFlowTest
    {
        [Test]
        public void Authenticate_InvalidUsername_AuthenticationFailure()
        {
            var target = new UsernamePasswordAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, "invalid user name", TestConfig.Password);
            target.TokenRequestEndpointUrl = TestConfig.TokenRequestEndpointUrl;

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failure"), () =>
            {
                target.Authenticate();
            });
        }

        [Test]
        public void RequestAccessToken_InvalidPassword_InvalidPassword()
        {
            var target = new UsernamePasswordAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, "invalid password");
            target.TokenRequestEndpointUrl = TestConfig.TokenRequestEndpointUrl;

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidPassword, "authentication failure - invalid password"), () =>
            {
                target.Authenticate();
            });
        }

        [Test]
        public void RequestAccessToken_InvalidClientId_InvalidClientId()
        {            
            var target = new UsernamePasswordAuthenticationFlow("Invalid client id", TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password);
            target.TokenRequestEndpointUrl = TestConfig.TokenRequestEndpointUrl;

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "client identifier invalid"), () =>
            {
                target.Authenticate();
            });
        }

        [Test]
        public void RequestAccessToken_InvalidClientSecret_InvalidClientSecret()
        {
            var target = new UsernamePasswordAuthenticationFlow(TestConfig.ClientId, "invalid client secret", TestConfig.Username, TestConfig.Password);
            target.TokenRequestEndpointUrl = TestConfig.TokenRequestEndpointUrl;

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "invalid client credentials"), () =>
            {
                target.Authenticate();
            });
        }

        [Test]
        public void RequestAccessToken_ValidCredentials_Authenticated()
        {
            var target = new UsernamePasswordAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password);
            target.TokenRequestEndpointUrl = TestConfig.TokenRequestEndpointUrl;

            var actual = target.Authenticate();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.AccessToken.Length > 0);
            Assert.IsTrue(actual.InstanceUrl.Length > 0);
            Assert.IsTrue(Uri.IsWellFormedUriString(actual.InstanceUrl, UriKind.Absolute));
        }
    }
}
