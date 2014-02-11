using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SalesforceSharp.FunctionalTests.Stubs
{
    public class RecordStub
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty("Phone")]
        public string PhoneCustom { get; set; }
    }
}

