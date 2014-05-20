using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Use to manage how each fields are managed when communicate with Salesforce
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SalesforceAttribute : Attribute
    {
        /// <summary>
        /// Exclude this field when pulling or updating to salesforce
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Exclude this field when serializing data to update to salesforce
        /// </summary>
        public bool IgnoreUpdate{ get; set; }

        /// <summary>
        /// FieldId in Salesforce for this property.  If none, then it will use the property name.
        /// </summary>
        public string FieldName { get; set; }
    }
}
