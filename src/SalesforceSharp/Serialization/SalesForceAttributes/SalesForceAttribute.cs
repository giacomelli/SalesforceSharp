using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesforceSharp.Serialization.SalesForceAttributes
{
    /// <summary>
    /// Use to manage how each fields are managed when communicate with SalesForce
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SalesForceAttribute : Attribute
    {
        /// <summary>
        /// Exclude this field when pulling or updating to salesforce
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Exclude this field when serializing data to update to salesforce
        /// </summary>
        public bool IgnoreUpdate{ get; set; }
    }
}
