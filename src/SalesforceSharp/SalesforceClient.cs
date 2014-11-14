﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HelperSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using SalesforceSharp.Models;
using SalesforceSharp.Security;
using SalesforceSharp.Serialization;
using RestSharp.Extensions;

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
        private IRestClient m_restClient;
        private GenericJsonDeserializer genericJsonDeserializer;
        private GenericJsonSerializer updateJsonSerializer;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SalesforceClient"/> class.
        /// </summary>        
        public SalesforceClient()
            : this(new RestClient())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesforceClient"/> class.
        /// </summary>
        /// <param name="restClient">The rest client.</param>
        protected internal SalesforceClient(IRestClient restClient)
        {
            m_restClient = restClient;
            ApiVersion = "v28.0";
            m_deserializer = new DynamicJsonDeserializer();
            genericJsonDeserializer = new GenericJsonDeserializer(new SalesforceContractResolver(false));
            updateJsonSerializer = new GenericJsonSerializer(new SalesforceContractResolver(true));
        }
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
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The API result for the query.</returns>
        public IList<T> Query<T>(string query, string altUrl = "") where T : new()
        {
            return QueryActionBatch<T>(query, s => { }, altUrl);
        }


        /// <summary>
        /// Executes a SOQL query and returns the result.
        /// </summary>
        /// <param name="query">The SOQL query.</param>
        /// <param name="action">Action to call after getting a non error response.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The API result for the query.</returns>
        public IList<T> QueryActionBatch<T>(string query, Action<IList<T>> action, string altUrl = "") where T : new()
        {
            if (action == null) throw new ArgumentNullException("action");

            ExceptionHelper.ThrowIfNullOrEmpty("query", query);

            var escapedQuery = query.UrlEncode();

            var url = "{0}?q={1}".With(string.IsNullOrEmpty(altUrl) ? GetUrl("query") : GetAltUrl(altUrl), escapedQuery);

            var returns = new List<T>();
            IRestResponse<SalesforceQueryResult<T>> response = null;

            do
            {
                if (response != null)
                {
                    url = GetNextRecordsUrl(response);
                    response = null;
                }

                if (string.IsNullOrEmpty(url))
                {
                    break;
                }

                response = Request<SalesforceQueryResult<T>>(url);
                if (response == null || response.Data == null) continue;

                if (!response.Data.Records.Any()) continue;
                try
                {
                    var customResponse = genericJsonDeserializer.Deserialize<SalesforceQueryResult<T>>(response);
                    if (customResponse == null) continue;
                    action(customResponse.Records);
                    returns.AddRange(customResponse.Records);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            } while (response != null && response.Data != null && !response.Data.Done && !string.IsNullOrEmpty(response.Data.NextRecordsUrl));

            return returns;
        }


        private string GetNextRecordsUrl<T>(IRestResponse<SalesforceQueryResult<T>> previousResponse) where T : new()
        {
            if (previousResponse == null || previousResponse.Data == null ||
                string.IsNullOrEmpty(previousResponse.Data.NextRecordsUrl))
            {
                return string.Empty;
            }
            return InstanceUrl + previousResponse.Data.NextRecordsUrl;

        }

        /// <summary>
        /// Finds a record by Id.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The record with the specified id.</returns>
        public T FindById<T>(string objectName, string recordId, string altUrl) where T : new()
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);
            ExceptionHelper.ThrowIfNullOrEmpty("altUrl", altUrl);

            var result = Query<T>("SELECT {0} FROM {1} WHERE Id = '{2}'".With(GetRecordProjection(typeof(T)), objectName, recordId), altUrl);

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Finds a record by Id.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <returns>The record with the specified id.</returns>
        public T FindById<T>(string objectName, string recordId) where T : new()
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);

            var result = Query<T>("SELECT {0} FROM {1} WHERE Id = '{2}'".With(GetRecordProjection(typeof(T)), objectName, recordId));

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Obtains a JSON representation of fields an meta data for a given object type
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <returns></returns>
        public string ReadMetaData(string objectName)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);

            var response = Request<object>(GetUrl("sobjects"), string.Format("{0}/describe/", objectName));

            return response.Content;
        }

        /// <summary>
        /// Creates a record
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="record">The record to be created.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The Id of created record.</returns>
        public string Create(string objectName, object record, string altUrl)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNull("record", record);
            ExceptionHelper.ThrowIfNullOrEmpty("altUrl", altUrl);

            var response = Request<object>(GetAltUrl(altUrl), objectName, record, Method.POST);
            return m_deserializer.Deserialize<dynamic>(response).id.Value;
        }

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="record">The record to be created.</param>
        /// <returns>The Id of created record.</returns>
        public string Create(string objectName, object record)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNull("record", record);

            var response = Request<object>(GetUrl("sobjects"), objectName, record, Method.POST);
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
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);
            ExceptionHelper.ThrowIfNull("record", record);

            var response = Request<object>(GetUrl("sobjects"), "{0}/{1}".With(objectName, recordId), record, Method.PATCH);

            // HTTP status code 204 is returned if an existing record is updated.
            var recordUpdated = response.StatusCode == HttpStatusCode.NoContent;

            return recordUpdated;
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="record">The record to be updated.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        public bool Update(string objectName, string recordId, object record, string altUrl)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);
            ExceptionHelper.ThrowIfNull("record", record);
            ExceptionHelper.ThrowIfNullOrEmpty("altUrl", altUrl);

            var response = Request<object>(GetAltUrl(altUrl), "{0}/{1}".With(objectName, recordId), record, Method.PATCH);

            // HTTP status code 204 is returned if an existing record is updated.
            var recordUpdated = response.StatusCode == HttpStatusCode.NoContent;

            return recordUpdated;
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id which will be deleted.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>True if was deleted, otherwise false.</returns>
        public bool Delete(string objectName, string recordId, string altUrl)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);
            ExceptionHelper.ThrowIfNullOrEmpty("altUrl", altUrl);

            var response = Request<object>(GetAltUrl(altUrl), "{0}/{1}".With(objectName, recordId), null, Method.DELETE);

            // HTTP status code 204 is returned if an existing record is deleted.
            var recoredDeleted = response.StatusCode == HttpStatusCode.NoContent;

            return recoredDeleted;
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id which will be deleted.</param>
        /// <returns>True if was deleted, otherwise false.</returns>
        public bool Delete(string objectName, string recordId)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);

            var response = Request<object>(GetUrl("sobjects"), "{0}/{1}".With(objectName, recordId), null, Method.DELETE);

            // HTTP status code 204 is returned if an existing record is deleted.
            var recoredDeleted = response.StatusCode == HttpStatusCode.NoContent;

            return recoredDeleted;
        }

        /// <summary>
        /// Get sObject Details.
        /// </summary>
        /// <param name="sobjectApiName">object Api Id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns></returns>
        public SalesforceObject GetSObjectDetail(string sobjectApiName, string altUrl = "")
        {
            var url = "{0}/{1}/describe".With(string.IsNullOrEmpty(altUrl) ? GetUrl("sobjects") : GetAltUrl(altUrl), sobjectApiName);

            var response = Request<SalesforceObject>(url);
            return response.Data;
        }

        /// <summary>
        /// Returns the raw content of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The returned content as a string</returns>
        public string GetRawContent(string objectName, string recordId, string altUrl)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);
            ExceptionHelper.ThrowIfNullOrEmpty("altUrl", altUrl);

            var response = RequestRaw(GetAltUrl(altUrl), "{0}/{1}".With(objectName, recordId));

            return response.Content;
        }

        /// <summary>
        /// Returns the raw content of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <returns>The returned content as a string</returns>
        public string GetRawContent(string objectName, string recordId)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);

            var response = RequestRaw(GetUrl("sobjects"), "{0}/{1}".With(objectName, recordId));

            return response.Content;
        }

        /// <summary>
        /// Returns the raw byte array of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The returned binary content as a byte array</returns>
        public byte[] GetRawBytes(string objectName, string recordId, string altUrl)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);
            ExceptionHelper.ThrowIfNullOrEmpty("altUrl", altUrl);

            var response = RequestRaw(GetAltUrl(altUrl), "{0}/{1}".With(objectName, recordId));

            return response.RawBytes;
        }

        /// <summary>
        /// Returns the raw byte array of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <returns>The returned binary content as a byte array</returns>
        public byte[] GetRawBytes(string objectName, string recordId)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("objectName", objectName);
            ExceptionHelper.ThrowIfNullOrEmpty("recordId", recordId);

            var response = RequestRaw(GetUrl("sobjects"), "{0}/{1}".With(objectName, recordId));

            return response.RawBytes;
        }

        #endregion

        #region Requests
        /// <summary>
        /// Perform the request against Salesforce's REST API.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="objectName">The Name of the object.</param>
        /// <param name="record">The record.</param>
        /// <param name="method">The http method.</param>
        /// <exception cref="System.InvalidOperationException">Please, execute Authenticate method before call any REST API operation.</exception>
        protected IRestResponse<T> Request<T>(string baseUrl, string objectName = null, object record = null, Method method = Method.GET) where T : new()
        {
            if (!IsAuthenticated)
            {
                throw new InvalidOperationException("Please, execute Authenticate method before call any REST API operation.");
            }

            var request = BuildRequest(baseUrl, objectName, record, method);

            var response = m_restClient.Execute<T>(request);
            CheckApiException(response);

            return response;
        }

        /// <summary>
        /// Perform the request against Salesforce's REST API.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="objectName">The Name of the object.</param>
        /// <param name="record">The record.</param>
        /// <param name="method">The http method.</param>
        /// <exception cref="System.InvalidOperationException">Please, execute Authenticate method before call any REST API operation.</exception>
        protected IRestResponse RequestRaw(string baseUrl, string objectName = null, object record = null, Method method = Method.GET)
        {
            if (!IsAuthenticated)
            {
                throw new InvalidOperationException("Please, execute Authenticate method before call any REST API operation.");
            }

            var request = BuildRequest(baseUrl, objectName, record, method);

            var response = m_restClient.Execute(request);
            CheckApiException(response);

            return response;
        }

        /// <summary>
        /// Builds a rest request
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="objectName">The Name of the object.</param>
        /// <param name="record">The record.</param>
        /// <param name="method">The http method.</param>
        /// <returns>A rest request object</returns>
        private RestRequest BuildRequest(string baseUrl, string objectName, object record, Method method)
        {
            Uri uri = new Uri(baseUrl);
            m_restClient.BaseUrl = uri;
            var request = new RestRequest(objectName)
            {
                RequestFormat = DataFormat.Json,
                Method = method
            };
            request.AddHeader("Authorization", "Bearer " + m_accessToken);

            if (record != null)
            {
                request.AddParameter("application/json; charset=utf-8", updateJsonSerializer.Serialize(record), ParameterType.RequestBody);
            }
            return request;
        }

        /// <summary>
        /// Checks if an API exception was throw in the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="SalesforceException">
        /// </exception>
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
                var ex = new FormatException(string.Format("{0}{1}{2}", response.ErrorException.Message, Environment.NewLine, response.Content));
                throw ex;
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Gets the record projection fields.
        /// </summary>
        /// <param name="recordType">Type of the record.</param>
        /// <returns></returns>
        public static string GetRecordProjection(Type recordType)
        {
            var propNames = new List<string>();

            var props = recordType.GetProperties();
            foreach (var prop in props)
            {
                var sfAttrs = prop.GetCustomAttributes(typeof (SalesforceAttribute), true);
                // If Ignore then we shouldn't include it.
                if (sfAttrs.Any())
                {
                    var sfAttr = sfAttrs.FirstOrDefault() as SalesforceAttribute;
                    if (sfAttr != null)
                    {
                        if (sfAttr.Ignore)
                        {
                            continue;
                        }
                        if (!string.IsNullOrEmpty(sfAttr.FieldName))
                        {
                            propNames.Add(sfAttr.FieldName);
                            continue;
                        }
                    }
                }

                propNames.Add(prop.Name);
            }

            return String.Join(", ", propNames);
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        protected string GetUrl(string resourceName)
        {
            return "{0}/services/data/{1}/{2}".With(InstanceUrl, ApiVersion, resourceName);
        }

        /// <summary>
        /// Gets URL of use with custom restful endpoint.
        /// </summary>
        /// <param name="url">URL of alternate service</param>
        /// <returns></returns>
        protected string GetAltUrl(string url)
        {
            return "{0}/{1}".With(InstanceUrl, url);
        }
        #endregion
    }
}
