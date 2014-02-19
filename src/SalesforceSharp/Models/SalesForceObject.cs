using System.Collections.Generic;

namespace SalesforceSharp.Models
{
    /// <summary>
    /// SalesForceObject
    /// </summary>
    public class SalesForceObject
    {
#pragma warning disable 1591
        public string Name { get; set; }
        public List<SalesForceField> fields { get; set; }
        public bool CustomSetting { get; set; }
        public bool Undeletable { get; set; }
        public bool Mergeable { get; set; }
        public bool Replicateable { get; set; }
        public bool Triggerable { get; set; }
        public bool FeedEnabled { get; set; }
        public bool Retrieveable { get; set; }
        public bool SearchLayoutable { get; set; }
        public object LookupLayoutable { get; set; }
        public object Listviewable { get; set; }
        public bool DeprecatedAndHidden { get; set; }
        public List<SalesForceRecordTypeInfo> RecordTypeInfos { get; set; }
        public bool Createable { get; set; }
        public bool Deletable { get; set; }
        public bool Updateable { get; set; }
        public bool Queryable { get; set; }
        public bool Layoutable { get; set; }
        public bool Activateable { get; set; }
        public string LabelPlural { get; set; }
        public string KeyPrefix { get; set; }
        public bool Custom { get; set; }
        public bool CompactLayoutable { get; set; }
        public bool Searchable { get; set; }
        public string Label { get; set; }
        public List<SalesForceChildRelationship> ChildRelationships { get; set; }
        public SalesForceObjectUrls Urls { get; set; }
#pragma warning restore 1591
    }
}