using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesforceSharp.Models
{
    /// <summary>
    /// SalesForceField
    /// </summary>
    ///
    public class SalesForceField
    {
#pragma warning disable 1591
        public int Length { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public object DefaultValue { get; set; }
        public object InlineHelpText { get; set; }
        public bool WriteRequiresMasterRead { get; set; }
        public int Precision { get; set; }
        public bool Unique { get; set; }
        public object ControllerName { get; set; }
        public bool DeprecatedAndHidden { get; set; }
        public bool Createable { get; set; }
        public bool Updateable { get; set; }
        public bool ExternalId { get; set; }
        public bool IdLookup { get; set; }
        public bool NameField { get; set; }
        public bool Sortable { get; set; }
        public bool Filterable { get; set; }
        public bool RestrictedPicklist { get; set; }
        public bool CaseSensitive { get; set; }
        public string RelationshipName { get; set; }
        public int ByteLength { get; set; }
        public string Label { get; set; }
        public bool Calculated { get; set; }
        public int Scale { get; set; }
        public List<SalesForcePickList> PicklistValues { get; set; }
        public bool AutoNumber { get; set; }
        public bool Nillable { get; set; }
        public bool DisplayLocationInDecimal { get; set; }
        public bool CascadeDelete { get; set; }
        public bool RestrictedDelete { get; set; }
        public object CalculatedFormula { get; set; }
        public object DefaultValueFormula { get; set; }
        public bool DefaultedOnCreate { get; set; }
        public int Digits { get; set; }
        public bool Groupable { get; set; }
        public bool Permissionable { get; set; }
        public List<object> ReferenceTo { get; set; }
        public object RelationshipOrder { get; set; }
        public string SoapType { get; set; }
        public bool DependentPicklist { get; set; }
        public bool NamePointing { get; set; }
        public bool Custom { get; set; }
        public bool HtmlFormatted { get; set; }
#pragma warning restore 1591
    }
}
