using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Represents the format of a query result.
    /// </summary>
    /// <typeparam name="TRecord">The type of the record.</typeparam>
    public class SalesforceQueryResult<TRecord> where TRecord : new()
    {
        /// <summary>
        /// Gets or sets the records.
        /// </summary>
        /// <value>
        /// The records.
        /// </value>
        public List<TRecord> Records { get; set; }

        /// <summary>
        /// Total records found
        /// </summary>
        /// <value>
        /// totalSize
        /// </value>
        public int TotalSize { get; set; }

        /// <summary>
        /// Got all Records from the query
        /// </summary>
        /// <value>
        /// done
        /// </value>
        public bool Done { get; set; }

        /// <summary>
        /// The Url to get the next/rest batch of records
        /// </summary>
        /// <value>
        /// nextRecordsUrl
        /// </value>
        public string NextRecordsUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryRequestResponse<T>
    {

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public IRestRequest Request { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ContentLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContentEncoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StatusDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] RawBytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Uri ResponseUri { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<RestResponseCookie> Cookies { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<Parameter> Headers { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception ErrorException { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public QueryRequestResponse() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rr"></param>
        public QueryRequestResponse(IRestResponse<T> rr)
        {
            if (rr == null)
            {
                return;
            }
            Request = rr.Request;
            ContentType = rr.ContentType;
            ContentLength = rr.ContentLength;
            ContentEncoding = rr.ContentEncoding;
            Content = rr.Content;
            StatusCode = rr.StatusCode;
            StatusDescription = rr.StatusDescription;
            ResponseUri = rr.ResponseUri;
            Server = rr.Server;
            Cookies = rr.Cookies;
            Headers = rr.Headers;
            ErrorMessage = rr.ErrorMessage;
            ErrorException = rr.ErrorException;
            Data = rr.Data;

        }

        #endregion
    }


}
