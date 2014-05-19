using System.Collections.Generic;

namespace GG.SalesforceSharp.Serialization
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
    }
}
