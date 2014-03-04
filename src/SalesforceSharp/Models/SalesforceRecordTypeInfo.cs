namespace SalesforceSharp.Models
{
    /// <summary>
	/// Represents a RecordTypeInfo on Salesforce.
    /// </summary>
    public class SalesforceRecordTypeInfo
    {
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the record type identifier.
		/// </summary>
		/// <value>The record type identifier.</value>
        public string RecordTypeId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceRecordTypeInfo"/> is available.
		/// </summary>
		/// <value><c>true</c> if available; otherwise, <c>false</c>.</value>
        public bool Available { get; set; }

		/// <summary>
		/// Gets or sets the urls.
		/// </summary>
		/// <value>The urls.</value>
        public SalesforceRecordTypeInfoLayoutUrl Urls { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceRecordTypeInfo"/> default
		/// record type mapping.
		/// </summary>
		/// <value><c>true</c> if default record type mapping; otherwise, <c>false</c>.</value>
        public bool DefaultRecordTypeMapping { get; set; }
    }
}