using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesforceSharp;
using SalesforceSharp.Security;


//Questions?
//Scott VandenElzen
//Email: scott@vandenelzen.com
//Twitter: @scottvelzen


//Note - the version of Json package manager pulled down with SalesForceSharp didn't work, I neded to do PM>Install-Package Newtonsoft.Json to make this work

namespace SalesForceSharpSample
{

    // sample class used for queries 
    public class SFCase
    {
        public string id { get; set; }              // internal ID Number for the case which isn't shown on the case form
        public int CaseNumber { get; set; }         // notice i have CaseNumber which I can only read (not write)
        public string Subject { get; set; }
        public string Description { get; set; }
        public decimal? Rank__c { get; set; }       // note the __c for a custom field in SF -- it's nullable
    }

    // sample class used to update -- not i can't have ID or CaseNumber in it because they are read-only fields
    public class SFCaseUpdate
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public decimal? Rank__c { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {

            // See the SalesforceSetupWalkthrough.doc for information on how to configure your SalesForce account

            // from your setup >> create >> apps >> connected apps settings in SalesForce
            const string sfdcConsumerKey = "3MVG9JZ_r.QzrS7gHCcJexYMP2UL45ZgzaagHsVXfYSjWwlhU7n2uaxzfsuBNvwjofV70lM9QtA_xYLTxXjgf";
            const string sfdcConsumerSecret = "2968907211720668629";

            // your user credentials in salesforce
            const string sfdcUserName = "SalesforceDemoAcct@gmail.com";
            const string sfdcPassword = "Thi$IsMyPassw0rd!";

            // your security token form salesforce.  Name >> My Settings >> Personal >>  Reset My Security Token
            const string sfdcToken = "w7UGcyJnOaWX8U7XRCEbIfLYw";

            var client = new SalesforceClient();
            var authFlow = new UsernamePasswordAuthenticationFlow(sfdcConsumerKey, sfdcConsumerSecret, sfdcUserName, sfdcPassword + sfdcToken);
            
            // all actions should be in a try-catch - i'll just do the authentication one for an example
            try
            {
                client.Authenticate(authFlow);
            }
            catch (SalesforceException ex)
            {
                Console.WriteLine("Authentication failed: {0} : {1}", ex.Error, ex.Message);
            }

            // create a record using a class instance
            SFCaseUpdate myCase = new SFCaseUpdate();
            myCase.Subject = "This is the subject of my salesforce case";
            myCase.Description = "This is the description of my salesforce case";
            myCase.Rank__c = 5;
            client.Create("Case", myCase);

            // create a record using an anonymous class and returns the ID
            string resultID = client.Create("Case", new { Subject = "This is the subject of another salesforce case", Description = "This is the description of that other salesforce case", Rank__c = 5 });

            // query records
            var records = client.Query<SFCase>("SELECT id, CaseNumber, Subject, Description, Rank__c FROM Case");
            foreach (var r in records)
            {
                Console.WriteLine("Query Records {0}: {1} {2}", r.id, r.CaseNumber, r.Subject);
            }

            // find the record we just added by the ID we captured above in resultID
            var record = client.FindById<SFCase>("Case", resultID);
            Console.WriteLine("\n\nRead this record {0} {1} {2} {3} {4}\n\n", record.id, record.CaseNumber, record.Subject, record.Description, record.Rank__c);

            // update that record and set the custom field rank to 1 using an anonymous class.
            client.Update("Case", resultID, new { Rank__c = 1 });

            // update that record and set the custom field rank to 9001 using a class instance, note that i have to fill in every property from the record or it will push back nulls 
            SFCaseUpdate myupdate = new SFCaseUpdate();
            myupdate.Rank__c = 9001;
            myupdate.Description = record.Description;
            myupdate.Subject = record.Subject;
            client.Update("Case", resultID, myupdate);

            // re-read it to see if it updated correctly, rank should = 9001
            var record2 = client.FindById<SFCase>("Case", resultID);
            Console.WriteLine("Read this record again {0} {1} {2} {3} {4}\n\n", record2.id, record2.CaseNumber, record2.Subject, record2.Description, record2.Rank__c);

            // now delete the record I added
            client.Delete("Case", resultID);


            Console.WriteLine("Hit Enter to Exit");
            Console.ReadKey();
        }
    }
}
