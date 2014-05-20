namespace SalesforceSharp.Models
{
    /// <summary>
	/// Represents a child relationship on Salesforce.
    /// </summary>
    public class SalesforceChildRelationship
    {
		/// <summary>
		/// Gets or sets the field.
		/// </summary>
		/// <value>The field.</value>
        public string Field { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceChildRelationship"/>
		/// deprecated and hidden.
		/// </summary>
		/// <value><c>true</c> if deprecated and hidden; otherwise, <c>false</c>.</value>
        public bool DeprecatedAndHidden { get; set; }

		/// <summary>
		/// Gets or sets the name of the relationship.
		/// </summary>
		/// <value>The name of the relationship.</value>
        public string RelationshipName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceChildRelationship"/>
		/// cascade delete.
		/// </summary>
		/// <value><c>true</c> if cascade delete; otherwise, <c>false</c>.</value>
        public bool CascadeDelete { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceChildRelationship"/>
		/// restricted delete.
		/// </summary>
		/// <value><c>true</c> if restricted delete; otherwise, <c>false</c>.</value>
        public bool RestrictedDelete { get; set; }

		/// <summary>
		/// Gets or sets the child S object.
		/// </summary>
		/// <value>The child S object.</value>
        public string ChildSObject { get; set; }
    }
}