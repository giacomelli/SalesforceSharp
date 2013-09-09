using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HelperSharp;
using Newtonsoft.Json.Linq;
using RestSharp;
using SalesforceSharp.Security;
using SalesforceSharp.Serialization;

namespace SalesforceSharp
{
    /// <summary>
    /// The central point to communicate with Salesforce REST API.
    /// </summary>
    public class SalesforceClient
    {
        #region Fields
        private string m_accessToken;
        private DynamicJsonDeserializer m_deserializer;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        /// <remarks>
        /// The default value is v28.0.
        /// </remarks>
        /// <value>
        /// The API version.
        /// </value>
        public string ApiVersion { get; set; }       

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Gets the instance URL.
        /// </summary>
        /// <value>
        /// The instance URL.
        /// </value>
        public string InstanceUrl { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SalesforceClient"/> class.
        /// </summary>        
        public SalesforceClient()
        {
            ApiVersion = "v28.0";
            m_deserializer = new DynamicJsonDeserializer();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Authenticates the client.
        /// </summary>
        /// <param name="authenticationFlow">The authentication flow which will be used to authenticate on REST API.</param>
        public void Authenticate(IAuthenticationFlow authenticationFlow)
        {
            var info = authenticationFlow.Authenticate();
            m_accessToken = info.AccessToken;
            InstanceUrl = info.InstanceUrl;
            IsAuthenticated = true;
        }
     
        /// <summary>
        /// Executes a SOQL query and returns the result.
        /// </summary>
        /// <param name="query">The SOQL query.</param>
        /// <returns>The API result for the query.</returns>
        public IList<T> Query<T>(string query) where T : new()
        {
            var url = "{0}?q={1}".With(GetUrl("query"), query);
            var response = Request<SalesforceQueryResult<T>>(url);

            return response.Data.Records;
        }

        /// <summary>
        /// Finds a record by Id.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="id">The record id.</param>
        /// <returns>The record with the specified id.</returns>
        public T FindById<T>(string objectName, string id) where T : new()
        {
            var result = Query<T>("SELECT {0} FROM {1} WHERE Id = '{2}'".With(GetRecordProjection(typeof(T)), objectName, id));

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="record">The record to be created.</param>
        /// <returns>The Id of created record.</returns>
        public string Create (string objectName, object record) 
        {
            var response = Post<object>(GetUrl("sobjects"), objectName, record);
            return m_deserializer.Deserialize<dynamic>(response).id.Value;
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="record">The record to be updated.</param>
        public bool Update(string objectName, string recordId, object record)
        {           
            var response = Patch<object>(GetUrl("sobjects"), "{0}/{1}".With(objectName, recordId), record);

            // HTTP status code 204 is returned if an existing record is updated.
            var recordUpdated = response.StatusCode == HttpStatusCode.NoContent;                   
            
            return recordUpdated;
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id which will be deleted.</param>
        /// <returns>True if was deleted, otherwise false.</returns>
        public bool Delete(string objectName, string recordId)
        {
            var response = Del(GetUrl("sobjects"), "{0}/{1}".With(objectName, recordId));

            // HTTP status code 204 is returned if an existing record is deleted.
            var recoredDeleted = response.StatusCode == HttpStatusCode.NoContent;

            return recoredDeleted;
        }
        #endregion

        #region Helpers
        private IRestResponse<T> Patch<T>(string baseUrl, string recordPath, object record) where T : new()
        {
            return Request<T>(baseUrl, recordPath, record, Method.PATCH);
        }

        private IRestResponse<T> Post<T>(string baseUrl, string objectName, object record) where T : new()
        {
            return Request<T>(baseUrl, objectName, record, Method.POST);
        }

        private IRestResponse Del(string baseUrl, string recordPath)
        {
            return Request<object>(baseUrl, recordPath, null, Method.DELETE);
        }

        private IRestResponse<T> Request<T>(string baseUrl, string objectName = null, object record = null, Method method = Method.GET) where T : new()
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(objectName);
            request.RequestFormat = DataFormat.Json;
            request.Method = method;
            request.AddHeader("Authorization", "Bearer " + m_accessToken);

            if (record != null)
            {
                request.AddBody(record);
            }

            var response = client.Execute<T>(request);
            CheckApiException(response);

            return response;
        }

        private string GetUrl(string resourceName)
        {
            return "{0}/services/data/{1}/{2}".With(InstanceUrl, ApiVersion, resourceName);
        }

        private void CheckApiException(IRestResponse response)
        {
            if ((int)response.StatusCode > 299)
            {
                var responseData = m_deserializer.Deserialize<dynamic>(response);

                var error = responseData[0];
                var fieldsArray = error.fields as JArray;

                if (fieldsArray == null)
                {
                    throw new SalesforceException(error.errorCode.Value, error.message.Value);
                }
                else
                {
                    throw new SalesforceException(error.errorCode.Value, error.message.Value, fieldsArray.Select(v => (string)v).ToArray());
                }
            }

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
        }

        private static string GetRecordProjection(Type recordType)
        {
            return String.Join(", ", recordType.GetProperties().Select(p => p.Name));
        }
        #endregion
    }
}
