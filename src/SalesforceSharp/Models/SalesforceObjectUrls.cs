namespace SalesforceSharp.Models
{
    /// <summary>
	/// Represents an ObjectUrls on Salesforce.
    /// </summary>
    public class SalesforceObjectUrls
    {
		/// <summary>
		/// Gets or sets the user interface edit template.
		/// </summary>
		/// <value>The user interface edit template.</value>
        public string UiEditTemplate { get; set; }

		/// <summary>
		/// Gets or sets the sobject.
		/// </summary>
		/// <value>The sobject.</value>
        public string Sobject { get; set; }

		/// <summary>
		/// Gets or sets the quick actions.
		/// </summary>
		/// <value>The quick actions.</value>
        public string QuickActions { get; set; }

		/// <summary>
		/// Gets or sets the user interface detail template.
		/// </summary>
		/// <value>The user interface detail template.</value>
        public string UiDetailTemplate { get; set; }

		/// <summary>
		/// Gets or sets the describe.
		/// </summary>
		/// <value>The describe.</value>
        public string Describe { get; set; }

		/// <summary>
		/// Gets or sets the row template.
		/// </summary>
		/// <value>The row template.</value>
        public string RowTemplate { get; set; }

		/// <summary>
		/// Gets or sets the layouts.
		/// </summary>
		/// <value>The layouts.</value>
        public string Layouts { get; set; }

		/// <summary>
		/// Gets or sets the compact layouts.
		/// </summary>
		/// <value>The compact layouts.</value>
        public string CompactLayouts { get; set; }

		/// <summary>
		/// Gets or sets the user interface new record.
		/// </summary>
		/// <value>The user interface new record.</value>
        public string UiNewRecord { get; set; }
    }
}