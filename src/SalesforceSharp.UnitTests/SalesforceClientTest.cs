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
using TestSharp;

namespace SalesforceSharp.UnitTests
{
    [TestFixture]
    public class SalesforceClientTest
    {
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
        #endregion
    }
}
