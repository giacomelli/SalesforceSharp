using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SalesforceSharp.Serialization;

namespace SalesforceSharp.FunctionalTests.Stubs
{
    public class RecordStub
    {
        public string Id { get; set; }
        [Salesforce(IgnoreUpdate = true)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Salesforce(FieldName = "Phone")]
        public string PhoneCustom { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

