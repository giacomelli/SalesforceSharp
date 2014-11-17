using System;
using System.Net;
using NUnit.Framework;
using RestSharp;
using Rhino.Mocks;
using SalesforceSharp.Security;
using TestSharp;

namespace SalesforceSharp.UnitTests.Security
{
    [TestFixture]
    public class UsernamePasswordAuthenticationFlowTest
    {
        [Test]
        public void Constructor_NoRestClient_DefaultValues()
        {
            var target = new UsernamePasswordAuthenticationFlow("clientId", "clientSecret", "username", "password");
            Assert.IsNotNull(target);
        }

        [Test]
        public void Constructor_InvalidArgs_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("restClient"), () =>
            {
                new UsernamePasswordAuthenticationFlow(null, "clientId", "clientSecret", "username", "password");
            });

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'clientId' can't be empty.", "clientId"), () =>
            {
                new UsernamePasswordAuthenticationFlow(new RestClient(), "", "clientSecret", "username", "password");
            });

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'clientSecret' can't be empty.", "clientSecret"), () =>
            {
                new UsernamePasswordAuthenticationFlow(new RestClient(), "clientId", "", "username", "password");
            });

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'username' can't be empty.", "username"), () =>
            {
                new UsernamePasswordAuthenticationFlow(new RestClient(), "clientId", "clientSecret", "", "password");
            });

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'password' can't be empty.", "password"), () =>
            {
                new UsernamePasswordAuthenticationFlow(new RestClient(), "clientId", "clientSecret", "username", "");
            });
        }

        [Test]
        public void Authenticate_Failed_Exception()
        {
            var response = MockRepository.GenerateMock<IRestResponse>();
            response.Expect(r => r.Content).Return("{ error: 'authentication failure', error_description: 'authentication failed' }");
            response.Expect(r => r.StatusCode).Return(HttpStatusCode.BadRequest);

            var restClient = MockRepository.GenerateMock<IRestClient>();
            restClient.Expect(r => r.BaseUrl).SetPropertyWithArgument(new Uri("http://tokenUrl"));
            restClient.Expect(r => r.Execute(null)).IgnoreArguments().Return(response);

            var target = new UsernamePasswordAuthenticationFlow(restClient, "clientId", "clientSecret", "userName", "password");
            target.TokenRequestEndpointUrl = "http://tokenUrl";

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failed"), () =>
            {
                target.Authenticate();
            });
        }

        [Test]
        public void Authenticate_Success_AuthenticationInfo()
        {
            var response = MockRepository.GenerateMock<IRestResponse>();
            response.Expect(r => r.Content).Return("{ access_token: 'access token 1', instance_url: 'instance url 2' }");
            response.Expect(r => r.StatusCode).Return(HttpStatusCode.OK);

            var restClient = MockRepository.GenerateMock<IRestClient>();
            restClient.Expect(r => r.BaseUrl).SetPropertyWithArgument(new Uri("https://login.salesforce.com/services/oauth2/token"));
            restClient.Expect(r => r.Execute(null)).IgnoreArguments().Return(response);

            var target = new UsernamePasswordAuthenticationFlow(restClient, "clientId", "clientSecret", "userName", "password");
            var actual = target.Authenticate();
            Assert.AreEqual("access token 1", actual.AccessToken);
            Assert.AreEqual("instance url 2", actual.InstanceUrl);
        }
    }
}