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
}
