namespace SalesforceSharp.Models
{
    /// <summary>
    /// SalesForceChildRelationship
    /// </summary>
    public class SalesForceChildRelationship
    {
#pragma warning disable 1591
        public string Field { get; set; }
        public bool DeprecatedAndHidden { get; set; }
        public string RelationshipName { get; set; }
        public bool CascadeDelete { get; set; }
        public bool RestrictedDelete { get; set; }
        public string ChildSObject { get; set; }
#pragma warning restore 1591
    }
}