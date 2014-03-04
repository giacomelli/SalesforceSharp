using System.Collections.Generic;

namespace SalesforceSharp.Models
{
    /// <summary>
	/// Represents an Object on Salesforce.
    /// </summary>
    public class SalesforceObject
    {
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the fields.
		/// </summary>
		/// <value>The fields.</value>
        public List<SalesforceField> Fields { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> custom setting.
		/// </summary>
		/// <value><c>true</c> if custom setting; otherwise, <c>false</c>.</value>
        public bool CustomSetting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is undeletable.
		/// </summary>
		/// <value><c>true</c> if undeletable; otherwise, <c>false</c>.</value>
        public bool Undeletable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is mergeable.
		/// </summary>
		/// <value><c>true</c> if mergeable; otherwise, <c>false</c>.</value>
        public bool Mergeable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is replicateable.
		/// </summary>
		/// <value><c>true</c> if replicateable; otherwise, <c>false</c>.</value>
        public bool Replicateable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is triggerable.
		/// </summary>
		/// <value><c>true</c> if triggerable; otherwise, <c>false</c>.</value>
        public bool Triggerable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> feed enabled.
		/// </summary>
		/// <value><c>true</c> if feed enabled; otherwise, <c>false</c>.</value>
        public bool FeedEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is retrieveable.
		/// </summary>
		/// <value><c>true</c> if retrieveable; otherwise, <c>false</c>.</value>
        public bool Retrieveable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> search layoutable.
		/// </summary>
		/// <value><c>true</c> if search layoutable; otherwise, <c>false</c>.</value>
        public bool SearchLayoutable { get; set; }

		/// <summary>
		/// Gets or sets the lookup layoutable.
		/// </summary>
		/// <value>The lookup layoutable.</value>
        public object LookupLayoutable { get; set; }

		/// <summary>
		/// Gets or sets the listviewable.
		/// </summary>
		/// <value>The listviewable.</value>
        public object ListViewable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> deprecated and hidden.
		/// </summary>
		/// <value><c>true</c> if deprecated and hidden; otherwise, <c>false</c>.</value>
        public bool DeprecatedAndHidden { get; set; }

		/// <summary>
		/// Gets or sets the record type infos.
		/// </summary>
		/// <value>The record type infos.</value>
        public List<SalesforceRecordTypeInfo> RecordTypeInfos { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is createable.
		/// </summary>
		/// <value><c>true</c> if createable; otherwise, <c>false</c>.</value>
        public bool Createable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is deletable.
		/// </summary>
		/// <value><c>true</c> if deletable; otherwise, <c>false</c>.</value>
        public bool Deletable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is updateable.
		/// </summary>
		/// <value><c>true</c> if updateable; otherwise, <c>false</c>.</value>
        public bool Updateable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is queryable.
		/// </summary>
		/// <value><c>true</c> if queryable; otherwise, <c>false</c>.</value>
        public bool Queryable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is layoutable.
		/// </summary>
		/// <value><c>true</c> if layoutable; otherwise, <c>false</c>.</value>
        public bool Layoutable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is activateable.
		/// </summary>
		/// <value><c>true</c> if activateable; otherwise, <c>false</c>.</value>
        public bool Activateable { get; set; }

		/// <summary>
		/// Gets or sets the label plural.
		/// </summary>
		/// <value>The label plural.</value>
        public string LabelPlural { get; set; }

		/// <summary>
		/// Gets or sets the key prefix.
		/// </summary>
		/// <value>The key prefix.</value>
        public string KeyPrefix { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is custom.
		/// </summary>
		/// <value><c>true</c> if custom; otherwise, <c>false</c>.</value>
        public bool Custom { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> compact layoutable.
		/// </summary>
		/// <value><c>true</c> if compact layoutable; otherwise, <c>false</c>.</value>
        public bool CompactLayoutable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceObject"/> is searchable.
		/// </summary>
		/// <value><c>true</c> if searchable; otherwise, <c>false</c>.</value>
        public bool Searchable { get; set; }

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>The label.</value>
        public string Label { get; set; }

		/// <summary>
		/// Gets or sets the child relationships.
		/// </summary>
		/// <value>The child relationships.</value>
        public List<SalesforceChildRelationship> ChildRelationships { get; set; }

		/// <summary>
		/// Gets or sets the urls.
		/// </summary>
		/// <value>The urls.</value>
        public SalesforceObjectUrls Urls { get; set; }
    }
}