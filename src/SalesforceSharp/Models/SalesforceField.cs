using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesforceSharp.Models
{
    /// <summary>
	/// Represents a field on Salesforce.
    /// </summary>
    ///
    public class SalesforceField
    {
		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>The length.</value>
        public int Length { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
        public string Type { get; set; }

		/// <summary>
		/// Gets or sets the default value.
		/// </summary>
		/// <value>The default value.</value>
        public object DefaultValue { get; set; }

		/// <summary>
		/// Gets or sets the inline help text.
		/// </summary>
		/// <value>The inline help text.</value>
        public object InlineHelpText { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> write requires
		/// master read.
		/// </summary>
		/// <value><c>true</c> if write requires master read; otherwise, <c>false</c>.</value>
        public bool WriteRequiresMasterRead { get; set; }

		/// <summary>
		/// Gets or sets the precision.
		/// </summary>
		/// <value>The precision.</value>
        public int Precision { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is unique.
		/// </summary>
		/// <value><c>true</c> if unique; otherwise, <c>false</c>.</value>
        public bool Unique { get; set; }

		/// <summary>
		/// Gets or sets the name of the controller.
		/// </summary>
		/// <value>The name of the controller.</value>
        public object ControllerName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> deprecated and hidden.
		/// </summary>
		/// <value><c>true</c> if deprecated and hidden; otherwise, <c>false</c>.</value>
        public bool DeprecatedAndHidden { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is createable.
		/// </summary>
		/// <value><c>true</c> if createable; otherwise, <c>false</c>.</value>
        public bool Createable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is updateable.
		/// </summary>
		/// <value><c>true</c> if updateable; otherwise, <c>false</c>.</value>
        public bool Updateable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> external identifier.
		/// </summary>
		/// <value><c>true</c> if external identifier; otherwise, <c>false</c>.</value>
        public bool ExternalId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> identifier lookup.
		/// </summary>
		/// <value><c>true</c> if identifier lookup; otherwise, <c>false</c>.</value>
        public bool IdLookup { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> name field.
		/// </summary>
		/// <value><c>true</c> if name field; otherwise, <c>false</c>.</value>
        public bool NameField { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is sortable.
		/// </summary>
		/// <value><c>true</c> if sortable; otherwise, <c>false</c>.</value>
        public bool Sortable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is filterable.
		/// </summary>
		/// <value><c>true</c> if filterable; otherwise, <c>false</c>.</value>
        public bool Filterable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> restricted picklist.
		/// </summary>
		/// <value><c>true</c> if restricted picklist; otherwise, <c>false</c>.</value>
        public bool RestrictedPicklist { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> case sensitive.
		/// </summary>
		/// <value><c>true</c> if case sensitive; otherwise, <c>false</c>.</value>
        public bool CaseSensitive { get; set; }

		/// <summary>
		/// Gets or sets the name of the relationship.
		/// </summary>
		/// <value>The name of the relationship.</value>
        public string RelationshipName { get; set; }

		/// <summary>
		/// Gets or sets the length of the byte.
		/// </summary>
		/// <value>The length of the byte.</value>
        public int ByteLength { get; set; }

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>The label.</value>
        public string Label { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is calculated.
		/// </summary>
		/// <value><c>true</c> if calculated; otherwise, <c>false</c>.</value>
        public bool Calculated { get; set; }

		/// <summary>
		/// Gets or sets the scale.
		/// </summary>
		/// <value>The scale.</value>
        public int Scale { get; set; }

		/// <summary>
		/// Gets or sets the picklist values.
		/// </summary>
		/// <value>The picklist values.</value>
        public List<SalesforcePickList> PicklistValues { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> auto number.
		/// </summary>
		/// <value><c>true</c> if auto number; otherwise, <c>false</c>.</value>
        public bool AutoNumber { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is nillable.
		/// </summary>
		/// <value><c>true</c> if nillable; otherwise, <c>false</c>.</value>
        public bool Nillable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> display location
		/// in decimal.
		/// </summary>
		/// <value><c>true</c> if display location in decimal; otherwise, <c>false</c>.</value>
        public bool DisplayLocationInDecimal { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> cascade delete.
		/// </summary>
		/// <value><c>true</c> if cascade delete; otherwise, <c>false</c>.</value>
        public bool CascadeDelete { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> restricted delete.
		/// </summary>
		/// <value><c>true</c> if restricted delete; otherwise, <c>false</c>.</value>
        public bool RestrictedDelete { get; set; }

		/// <summary>
		/// Gets or sets the calculated formula.
		/// </summary>
		/// <value>The calculated formula.</value>
        public object CalculatedFormula { get; set; }

		/// <summary>
		/// Gets or sets the default value formula.
		/// </summary>
		/// <value>The default value formula.</value>
        public object DefaultValueFormula { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> defaulted on create.
		/// </summary>
		/// <value><c>true</c> if defaulted on create; otherwise, <c>false</c>.</value>
        public bool DefaultedOnCreate { get; set; }

		/// <summary>
		/// Gets or sets the digits.
		/// </summary>
		/// <value>The digits.</value>
        public int Digits { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is groupable.
		/// </summary>
		/// <value><c>true</c> if groupable; otherwise, <c>false</c>.</value>
        public bool Groupable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is permissionable.
		/// </summary>
		/// <value><c>true</c> if permissionable; otherwise, <c>false</c>.</value>
        public bool Permissionable { get; set; }

		/// <summary>
		/// Gets or sets the reference to.
		/// </summary>
		/// <value>The reference to.</value>
        public List<object> ReferenceTo { get; set; }

		/// <summary>
		/// Gets or sets the relationship order.
		/// </summary>
		/// <value>The relationship order.</value>
        public object RelationshipOrder { get; set; }

		/// <summary>
		/// Gets or sets the type of the SOAP.
		/// </summary>
		/// <value>The type of the SOAP.</value>
        public string SoapType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> dependent picklist.
		/// </summary>
		/// <value><c>true</c> if dependent picklist; otherwise, <c>false</c>.</value>
        public bool DependentPicklist { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> name pointing.
		/// </summary>
		/// <value><c>true</c> if name pointing; otherwise, <c>false</c>.</value>
        public bool NamePointing { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> is custom.
		/// </summary>
		/// <value><c>true</c> if custom; otherwise, <c>false</c>.</value>
        public bool Custom { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SalesforceSharp.Models.SalesforceField"/> html formatted.
		/// </summary>
		/// <value><c>true</c> if html formatted; otherwise, <c>false</c>.</value>
        public bool HtmlFormatted { get; set; }
    }
}
