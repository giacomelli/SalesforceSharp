using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using RestSharp;
using Rhino.Mocks;
using SalesforceSharp.Security;
using SalesforceSharp.Serialization;
using SalesforceSharp.UnitTests.Stubs;
using TestSharp;

namespace SalesforceSharp.UnitTests
{
    [TestFixture]
    public class SalesforceClientTest
    {
        private const string AltUrl = "someurl/somepath";

        #region Constructors
        [Test]
        public void Constructor_NoArgs_DefaultValues()
        {
            var target = new SalesforceClient();
            Assert.AreEqual("v28.0", target.ApiVersion);
            Assert.IsNull(target.InstanceUrl);
            Assert.IsFalse(target.IsAuthenticated);
        }
        #endregion

        #region Authenticate
        [Test]
        public void Authenticate_FlowCannotAuthenticate_Exception()
        {
            var flow = MockRepository.GenerateMock<IAuthenticationFlow>();
            flow.Expect(f => f.Authenticate()).Throw(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failure"));
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failure"), () =>
            {
                target.Authenticate(flow);
            });
        }

        [Test]
        public void Authenticate_FlowCanAuthenticate_IsAuthenticated()
        {
            var flow = MockRepository.GenerateMock<IAuthenticationFlow>();
            flow.Expect(f => f.Authenticate()).Return(new AuthenticationInfo("access", "url"));
            var target = new SalesforceClient();
            
            target.Authenticate(flow);
            Assert.IsTrue(target.IsAuthenticated);
            Assert.AreEqual(target.InstanceUrl, "url");            
        }
        #endregion

        #region Any action
        [Test]
        public void AnyAction_NotAuthentacated_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new InvalidOperationException("Please, execute Authenticate method before call any REST API operation."), () =>
            {
                target.Create("TESTE", "TESTE");
            });

            ExceptionAssert.IsThrowing(new InvalidOperationException("Please, execute Authenticate method before call any REST API operation."), () =>
            {
                target.Delete("TESTE", "TESTE");
            });

            ExceptionAssert.IsThrowing(new InvalidOperationException("Please, execute Authenticate method before call any REST API operation."), () =>
            {
                target.FindById<Exception>("TESTE", "TESTE");
            });
            
            ExceptionAssert.IsThrowing(new InvalidOperationException("Please, execute Authenticate method before call any REST API operation."), () =>
            {
                target.Query<Exception>("TESTE");
            });

            ExceptionAssert.IsThrowing(new InvalidOperationException("Please, execute Authenticate method before call any REST API operation."), () =>
            {
                target.Update("TESTE", "TESTE", "TESTE");
            });
        }
        #endregion

        #region Create
        [Test]
        public void Create_NullOrEmptyObjectName_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'objectName' can't be empty.", "objectName"), () =>
            {
                target.Create("", "TESTE");
            });
        }

        [Test]
        public void Create_NullRecord_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentNullException("record"), () =>
            {
                target.Create("TESTE", null);
            });
        }

        [Test]
        public void Create_ErrorReceived_Exception()
        {
            var target = CreateClientWithResponseError<object>();
         
            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.Unknown, "error"), () =>
            {
                target.Create("TESTE", "TESTE");
            });
        }

        [Test]
        public void Create_NoErrorReceived_Created()
        {
            var target = CreateClientWithResponseOk<object>(HttpStatusCode.NoContent, null);
            var actual = target.Create("TESTE", "TESTE");
            Assert.AreEqual("id", actual);
        }  

        [Test]
        public void CreateWithOverrideUrl_NoErrorReceived_Created()
        {
            string addedUrl = string.Empty;
            var target = CreateAltUrlClientWithResponseOk<object>(HttpStatusCode.NoContent, null, (urlParam) => addedUrl = urlParam);
            var actual = target.Create("TESTE", "TESTE", AltUrl);
            Assert.AreEqual("id", actual);
            Assert.That(addedUrl.IndexOf(string.Format("url/{0}", AltUrl)), Is.EqualTo(0));
        }
        #endregion

        #region Delete
        [Test]
        public void Delete_NullOrEmptyObjectName_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'objectName' can't be empty.", "objectName"), () =>
            {
                target.Delete("", "TESTE");
            });
        }

        [Test]
        public void Delete_NullOrEmptyId_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'recordId' can't be empty.", "recordId"), () =>
            {
                target.Delete("TESTE", "");
            });
        }

        [Test]
        public void Delete_ErrorReceived_Exception()
        {
            var target = CreateClientWithResponseError<object>();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.Unknown, "error"), () =>
            {
                target.Delete("TESTE", "TESTE");
            });
        }

        [Test]
        public void Delete_NoErrorReceived_Deleted()
        {
            var target = CreateClientWithResponseOk<object>(HttpStatusCode.NoContent, null);

            Assert.IsTrue(target.Delete("TESTE", "TESTE"));            
        }

        [Test]
        public void DeleteWithOverrideUrl_NoErrorReceived_Deleted()
        {
            string addedUrl = string.Empty;
            var target = CreateAltUrlClientWithResponseOk<object>(HttpStatusCode.NoContent, null, (urlParam) => addedUrl = urlParam);

            Assert.IsTrue(target.Delete("TESTE", "TESTE", AltUrl));
            Assert.That(addedUrl.IndexOf(string.Format("url/{0}", AltUrl)), Is.EqualTo(0));

        }
        #endregion

        #region FindById
        [Test]
        public void FindById_NullOrEmptyObjectName_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'objectName' can't be empty.", "objectName"), () =>
            {
                target.FindById<Exception>("", "TESTE");
            });
        }

        [Test]
        public void FindById_NullOrEmptyId_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'recordId' can't be empty.", "recordId"), () =>
            {
                target.FindById<Exception>("TESTE", "");
            });
        }

        [Test]
        public void FindById_ErrorReceived_Exception()
        {
            var target = CreateClientWithResponseError<SalesforceQueryResult<Exception>>();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.Unknown, "error"), () =>
            {
                target.FindById<Exception>("TESTE", "TESTE");
            });
        }

        [Test]
        public void FindById_NoErrorReceived_Record()
        {
            var target = CreateClientWithResponseOk<SalesforceQueryResult<RecordStub>>(HttpStatusCode.OK, new SalesforceQueryResult<RecordStub>() { Records = new List<RecordStub>() { new RecordStub() } });

            Assert.IsNotNull(target.FindById<RecordStub>("TESTE", "TESTE"));           
        }

        [Test]
        public void FindByIdWithOverrideUrl_NoErrorReceived_Record()
        {
            string addedUrl = string.Empty;
            var target = CreateAltUrlClientWithResponseOk<SalesforceQueryResult<RecordStub>>(HttpStatusCode.OK, new SalesforceQueryResult<RecordStub>() { Records = new List<RecordStub>() { new RecordStub() } }, (urlParam) => addedUrl = urlParam);

            Assert.IsNotNull(target.FindById<RecordStub>("TESTE", "TESTE", AltUrl));
            Assert.That(addedUrl.IndexOf(string.Format("url/{0}", AltUrl)), Is.EqualTo(0));
        }
        #endregion

        #region Query
        [Test]
        public void Query_NullOrEmptyObjectName_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'query' can't be empty.", "query"), () =>
            {
                target.Query<Exception>("");
            });
        }

        [Test]
        public void Query_ErrorReceived_Exception()
        {
            var target = CreateClientWithResponseError<SalesforceQueryResult<Exception>>();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.Unknown, "error"), () =>
            {
                target.Query<Exception>("TESTE");
            });
        }

        [Test]
        public void Query_NoErrorReceived_Records()
        {
            var target = CreateClientWithResponseOk<SalesforceQueryResult<RecordStub>>(HttpStatusCode.OK, new SalesforceQueryResult<RecordStub>() { Records = new List<RecordStub>() { new RecordStub(), new RecordStub() } });
            var actual = target.Query<RecordStub>("TESTE");
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void QueryAltUrl_NoErrorReceived_Records()
        {
            string addedUrl = string.Empty;
            var target = CreateAltUrlClientWithResponseOk<SalesforceQueryResult<RecordStub>>(HttpStatusCode.OK, new SalesforceQueryResult<RecordStub>() { Records = new List<RecordStub>() { new RecordStub(), new RecordStub() } }, (urlParam) => addedUrl = urlParam);
            var actual = target.Query<RecordStub>("TESTE", AltUrl);
            Assert.AreEqual(2, actual.Count);
            Assert.That(addedUrl.IndexOf(string.Format("url/{0}", AltUrl)), Is.EqualTo(0));
        }
        #endregion

        #region Update
        [Test]
        public void Update_NullOrEmptyObjectName_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'objectName' can't be empty.", "objectName"), () =>
            {
                target.Update("", "TESTE", "TESTE");
            });
        }

        [Test]
        public void Update_NullOrEmptyRecordId_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentException("Argument 'recordId' can't be empty.", "recordId"), () =>
            {
                target.Update("TESTE", "", "TESTE");
            });
        }

        [Test]
        public void Update_NullRecord_Exception()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new ArgumentNullException("record"), () =>
            {
                target.Update("TESTE", "TESTE", null);
            });
        }

        [Test]
        public void Update_ErrorReceived_Exception()
        {
            var target = CreateClientWithResponseError<object>();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.Unknown, "error"), () =>
            {
                target.Update("TESTE", "TESTE", "TESTE");
            });
        }

        [Test]
        public void Update_ErrorReceivedWithInvalidFields_Exception()
        {
            var response = MockRepository.GenerateMock<IRestResponse<object>>();
            response.Expect(r => r.Content).Return("[{ errorCode: 'error', message: 'error', fields: ['field1', 'field2'] }]");
            response.Expect(r => r.StatusCode).Return(HttpStatusCode.BadRequest);

            var restClient = MockRepository.GenerateMock<IRestClient>();
            restClient.Expect(r => r.BaseUrl).SetPropertyWithArgument("tokenUrl");
            restClient.Expect(r => r.Execute<object>(null)).IgnoreArguments().Return(response);

            var flow = MockRepository.GenerateMock<IAuthenticationFlow>();
            flow.Expect(f => f.Authenticate()).Return(new AuthenticationInfo("access", "url"));

            var target = new SalesforceClient(restClient);
            target.Authenticate(flow);

            ExceptionAssert.IsThrowing(new SalesforceException("TESTE", "error", new string[] { "field1", "field2" }), () =>
            {
                target.Update("TESTE", "TESTE", "TESTE");
            });
        }

        [Test]
        public void Update_StatusCodeOkButErrorException_Exception()
        {
            var response = MockRepository.GenerateMock<IRestResponse<object>>();
            response.Expect(r => r.Content).Return("");
            response.Expect(r => r.StatusCode).Return(HttpStatusCode.NoContent);
            response.Expect(r => r.ErrorException).Return(new Exception("TESTE"));

            var restClient = MockRepository.GenerateMock<IRestClient>();
            restClient.Expect(r => r.BaseUrl).SetPropertyWithArgument("tokenUrl");
            restClient.Expect(r => r.Execute<object>(null)).IgnoreArguments().Return(response);

            var flow = MockRepository.GenerateMock<IAuthenticationFlow>();
            flow.Expect(f => f.Authenticate()).Return(new AuthenticationInfo("access", "url"));

            var target = new SalesforceClient(restClient);
            target.Authenticate(flow);

            ExceptionAssert.IsThrowing(new FormatException("TESTE"+ Environment.NewLine), () =>
            {
                target.Update("TESTE", "TESTE", "TESTE");
            });
        }

        [Test]
        public void Update_NoErrorReceived_Updated()
        {
            var target = CreateClientWithResponseOk<object>(HttpStatusCode.NoContent, null);
            Assert.IsTrue(target.Update("TESTE", "TESTE", "TESTE"));
        }

        [Test]
        public void UpdateAltUrl_NoErrorReceived_Updated()
        {
            string addedUrl = string.Empty;
            var target = CreateAltUrlClientWithResponseOk<object>(HttpStatusCode.NoContent, null, (urlParam) => addedUrl = urlParam);
            Assert.IsTrue(target.Update("TESTE", "TESTE", "TESTE", AltUrl));
            Assert.That(addedUrl.IndexOf(string.Format("url/{0}", AltUrl)), Is.EqualTo(0));
        }
        #endregion

        #region Helpers
        private SalesforceClient CreateClientWithResponseError<T>() where T:new()
        {
            var response = MockRepository.GenerateMock<IRestResponse<T>>();
            response.Expect(r => r.Content).Return("[{ errorCode: 'error', message: 'error' }]");
            response.Expect(r => r.StatusCode).Return(HttpStatusCode.BadRequest);

            var restClient = MockRepository.GenerateMock<IRestClient>();
            restClient.Expect(r => r.BaseUrl).SetPropertyWithArgument("tokenUrl");
            restClient.Expect(r => r.Execute<T>(null)).IgnoreArguments().Return(response);

            var flow = MockRepository.GenerateMock<IAuthenticationFlow>();
            flow.Expect(f => f.Authenticate()).Return(new AuthenticationInfo("access", "url"));

            var target = new SalesforceClient(restClient);
            target.Authenticate(flow);

            return target;
        }

        private SalesforceClient CreateClientWithResponseOk<T>(HttpStatusCode statusCode, T data) where T : new()
        {
            var response = MockRepository.GenerateMock<IRestResponse<T>>();
            response.Expect(r => r.Content).Return("{\"id\":\"id\",\"records\":[{\"Id\": \"1\"},{\"Id\":\"2\"}]}");
            response.Expect(r => r.StatusCode).Return(statusCode);
            response.Expect(r => r.ErrorException).Return(null);
            response.Expect(r => r.Data).Return(data);

            var restClient = MockRepository.GenerateMock<IRestClient>();
            //restClient.Expect(r => r.BaseUrl).SetPropertyWithArgument("url/services/data/v28.0/query?q=SELECT Id FROM TESTE WHERE Id = 'TESTE'");
            restClient.Expect(r => r.Execute<T>(null)).IgnoreArguments().Return(response);


            var flow = MockRepository.GenerateMock<IAuthenticationFlow>();
            flow.Expect(f => f.Authenticate()).Return(new AuthenticationInfo("access", "url"));

            var target = new SalesforceClient(restClient);
            target.Authenticate(flow);

            return target;
        }

        private SalesforceClient CreateAltUrlClientWithResponseOk<T>(HttpStatusCode statusCode, T data, Action<string> recordUrl) where T : new()
        {
            var response = MockRepository.GenerateMock<IRestResponse<T>>();
            response.Expect(r => r.Content).Return("{\"id\":\"id\",\"records\":[{\"Id\": \"1\"},{\"Id\":\"2\"}]}");
            response.Expect(r => r.StatusCode).Return(statusCode);
            response.Expect(r => r.ErrorException).Return(null);
            response.Expect(r => r.Data).Return(data);

            var restClient = MockRepository.GenerateMock<IRestClient>();
            restClient.Expect(r => r.BaseUrl = string.Format("url{0}/TESTE", AltUrl)).IgnoreArguments().Do(recordUrl);
            restClient.Expect(r => r.Execute<T>(null)).IgnoreArguments().Return(response);
            
            var flow = MockRepository.GenerateMock<IAuthenticationFlow>();
            flow.Expect(f => f.Authenticate()).Return(new AuthenticationInfo("access", "url"));

            var target = new SalesforceClient(restClient);
            target.Authenticate(flow);

            return target;
        }
        #endregion
    }
}
