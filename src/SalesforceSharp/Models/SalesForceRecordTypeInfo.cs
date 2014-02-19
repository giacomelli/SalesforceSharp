namespace SalesforceSharp.Models
{
    /// <summary>
    /// SalesForceRecordTypeInfo
    /// </summary>
    public class SalesForceRecordTypeInfo
    {
#pragma warning disable 1591
        public string Name { get; set; }
        public string RecordTypeId { get; set; }
        public bool Available { get; set; }
        public SalesForceRecordTypeInfoLayoutUrl Urls { get; set; }
        public bool DefaultRecordTypeMapping { get; set; }
#pragma warning restore 1591
    }
}