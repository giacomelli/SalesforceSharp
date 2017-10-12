using SalesforceSharp.Models;
using SalesforceSharp.Security;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SalesforceSharp
{
    /// <summary>
    /// The central point to communicate with Salesforce REST API.
    /// </summary>
    public interface ISalesforceClient
    {
        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        /// <remarks>
        /// The default value is v40.0.
        /// </remarks>
        /// <value>
        /// The API version.
        /// </value>
        string ApiVersion { get; set; }

        /// <summary>
        /// Gets the instance URL.
        /// </summary>
        /// <value>
        /// The instance URL.
        /// </value>
        string InstanceUrl { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Authenticates the client.
        /// </summary>
        /// <param name="authenticationInfo">Authenticates the client with the given information.</param>
        void Authenticate(AuthenticationInfo authenticationInfo);

        /// <summary>
        /// Authenticates the client.
        /// </summary>
        /// <param name="authenticationFlow">The authentication flow which will be used to authenticate on REST API.</param>
        void Authenticate(IAuthenticationFlow authenticationFlow);

        /// <summary>
        /// Authenticates the client.
        /// </summary>
        /// <param name="authenticationFlow">The authentication flow which will be used to authenticate on REST API.</param>
        /// <param name="cancellationToken"/>
        Task AuthenticateAsync(IAuthenticationFlow authenticationFlow, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="record">The record to be created.</param>
        /// <returns>The Id of created record.</returns>
        string Create(string objectName, object record);

        /// <summary>
        /// Creates a record
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="record">The record to be created.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The Id of created record.</returns>
        string Create(string objectName, object record, string altUrl);

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="record">The record to be created.</param>
        /// <param name="cancellationToken"/>
        /// <returns>The Id of created record.</returns>
        Task<string> CreateAsync(string objectName, object record, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a record
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="record">The record to be created.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns>The Id of created record.</returns>
        Task<string> CreateAsync(string objectName, object record, string altUrl, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id which will be deleted.</param>
        /// <returns>True if was deleted, otherwise false.</returns>
        bool Delete(string objectName, string recordId);

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id which will be deleted.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>True if was deleted, otherwise false.</returns>
        bool Delete(string objectName, string recordId, string altUrl);

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id which will be deleted.</param>
        /// <param name="cancellationToken"/>
        /// <returns>True if was deleted, otherwise false.</returns>
        Task<bool> DeleteAsync(string objectName, string recordId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id which will be deleted.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns>True if was deleted, otherwise false.</returns>
        Task<bool> DeleteAsync(string objectName, string recordId, string altUrl, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Finds a record by Id.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <returns>The record with the specified id.</returns>
        T FindById<T>(string objectName, string recordId) where T : new();

        /// <summary>
        /// Finds a record by Id.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The record with the specified id.</returns>
        T FindById<T>(string objectName, string recordId, string altUrl) where T : new();

        /// <summary>
        /// Finds a record by Id.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="cancellationToken"/>
        /// <returns>The record with the specified id.</returns>
        Task<T> FindByIdAsync<T>(string objectName, string recordId, CancellationToken cancellationToken = default(CancellationToken)) where T : new();

        /// <summary>
        /// Finds a record by Id.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns>The record with the specified id.</returns>
        Task<T> FindByIdAsync<T>(string objectName, string recordId, string altUrl, CancellationToken cancellationToken = default(CancellationToken)) where T : new();

        /// <summary>
        /// Returns the raw byte array of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <returns>The returned binary content as a byte array</returns>
        byte[] GetRawBytes(string objectName, string recordId);

        /// <summary>
        /// Returns the raw byte array of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The returned binary content as a byte array</returns>
        byte[] GetRawBytes(string objectName, string recordId, string altUrl);

        /// <summary>
        /// Returns the raw byte array of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="cancellationToken"/>
        /// <returns>The returned binary content as a byte array</returns>
        Task<byte[]> GetRawBytesAsync(string objectName, string recordId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the raw byte array of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns>The returned binary content as a byte array</returns>
        Task<byte[]> GetRawBytesAsync(string objectName, string recordId, string altUrl, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the raw content of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <returns>The returned content as a string</returns>
        string GetRawContent(string objectName, string recordId);

        /// <summary>
        /// Returns the raw content of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The returned content as a string</returns>
        string GetRawContent(string objectName, string recordId, string altUrl);

        /// <summary>
        /// Returns the raw content of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="cancellationToken"/>
        /// <returns>The returned content as a string</returns>
        Task<string> GetRawContentAsync(string objectName, string recordId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the raw content of a GET request to the given object
        /// </summary>
        /// <param name="objectName">The object name</param>
        /// <param name="recordId">The record id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns>The returned content as a string</returns>
        Task<string> GetRawContentAsync(string objectName, string recordId, string altUrl, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get sObject Details.
        /// </summary>
        /// <param name="sobjectApiName">object Api Id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns></returns>
        SalesforceObject GetSObjectDetail(string sobjectApiName, string altUrl = "");

        /// <summary>
        /// Get sObject Details.
        /// </summary>
        /// <param name="sobjectApiName">object Api Id</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns></returns>
        Task<SalesforceObject> GetSObjectDetailAsync(string sobjectApiName, string altUrl = "", CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes a SOQL query and returns the result.
        /// </summary>
        /// <param name="query">The SOQL query.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The API result for the query.</returns>
        IList<T> Query<T>(string query, string altUrl = "") where T : new();

        /// <summary>
        /// Executes a SOQL query and returns the result.
        /// </summary>
        /// <param name="query">The SOQL query.</param>
        /// <param name="action">Action to call after getting a non error response.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <returns>The API result for the query.</returns>
        IList<T> QueryActionBatch<T>(string query, Action<IList<T>> action, string altUrl = "") where T : new();

        /// <summary>
        /// Executes a SOQL query and returns the result.
        /// </summary>
        /// <param name="query">The SOQL query.</param>
        /// <param name="action">Action to call after getting a non error response.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns>The API result for the query.</returns>
        Task<IList<T>> QueryActionBatchAsync<T>(string query, Action<IList<T>> action, string altUrl = "", CancellationToken cancellationToken = default(CancellationToken)) where T : new();

        /// <summary>
        /// Executes a SOQL query and returns the result.
        /// </summary>
        /// <param name="query">The SOQL query.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        /// <returns>The API result for the query.</returns>
        Task<IList<T>> QueryAsync<T>(string query, string altUrl = "", CancellationToken cancellationToken = default(CancellationToken)) where T : new();

        /// <summary>
        /// Obtains a JSON representation of fields an meta data for a given object type
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <returns></returns>
        string ReadMetaData(string objectName);

        /// <summary>
        /// Obtains a JSON representation of fields an meta data for a given object type
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="cancellationToken"/>
        /// <returns></returns>
        Task<string> ReadMetaDataAsync(string objectName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="record">The record to be updated.</param>
        bool Update(string objectName, string recordId, object record);

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="record">The record to be updated.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        bool Update(string objectName, string recordId, object record, string altUrl);

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="record">The record to be updated.</param>
        /// <param name="cancellationToken"/>
        Task<bool> UpdateAsync(string objectName, string recordId, object record, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="objectName">The name of the object in Salesforce.</param>
        /// <param name="recordId">The record id.</param>
        /// <param name="record">The record to be updated.</param>
        /// <param name="altUrl">The url to use without the instance url</param>
        /// <param name="cancellationToken"/>
        Task<bool> UpdateAsync(string objectName, string recordId, object record, string altUrl, CancellationToken cancellationToken = default(CancellationToken));
    }
}