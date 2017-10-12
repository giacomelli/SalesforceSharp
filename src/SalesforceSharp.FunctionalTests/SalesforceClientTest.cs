using Newtonsoft.Json;
using NUnit.Framework;
using SalesforceSharp.FunctionalTests.Stubs;
using SalesforceSharp.Security;
using SalesforceSharp.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestSharp;

namespace SalesforceSharp.FunctionalTests
{
    [TestFixture]
    public class SalesforceClientTest
    {
        #region Authenticate

        [Test]
        public void Authenticate_InvalidUsername_AuthenticationFailure()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failure"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidPassword_InvalidPassword()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidPassword, "authentication failure - invalid password"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, "invalid password"));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidClientId_InvalidClientId()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "client identifier invalid"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow("Invalid client id", TestConfig.ClientSecret, "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidClientSecret_InvalidClientSecret()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "invalid client credentials"), () =>
            {
                target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, "invalid client secret", "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public void Authenticate_ValidCredentials_Authenticated()
        {
            var target = new SalesforceClient();
            target.Authenticate(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password));
            Assert.IsTrue(target.IsAuthenticated);
        }

        #endregion Authenticate

        #region AuthenticateAsync

        [Test]
        public async Task AuthenticateAsync_InvalidUsername_AuthenticationFailure()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.AuthenticationFailure, "authentication failure"), async () =>
            {
                await target.AuthenticateAsync(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public async Task AuthenticateAsync_InvalidPassword_InvalidPassword()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidPassword, "authentication failure - invalid password"), async () =>
            {
                await target.AuthenticateAsync(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, "invalid password"));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public async Task AuthenticateAsync_InvalidClientId_InvalidClientId()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "client identifier invalid"), async () =>
            {
                await target.AuthenticateAsync(CreateAuthenticationFlow("Invalid client id", TestConfig.ClientSecret, "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public async Task AuthenticateAsync_InvalidClientSecret_InvalidClientSecret()
        {
            var target = new SalesforceClient();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidClient, "invalid client credentials"), async () =>
            {
                await target.AuthenticateAsync(CreateAuthenticationFlow(TestConfig.ClientId, "invalid client secret", "invalid user name", TestConfig.Password));
            });

            Assert.IsFalse(target.IsAuthenticated);
        }

        [Test]
        public async Task AuthenticateAsync_ValidCredentials_Authenticated()
        {
            var target = new SalesforceClient();
            await target.AuthenticateAsync(CreateAuthenticationFlow(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password));
            Assert.IsTrue(target.IsAuthenticated);
        }

        #endregion AuthenticateAsync

        #region Query

        [Test]
        public void Query_InvalidQuery_Exception()
        {
            var target = CreateClientAndAuth();

            ExceptionAssert.IsThrowing(typeof(SalesforceException), () =>
            {
                target.Query<RecordStub>("SELECT id, name, FROM " + TestConfig.ObjectName);
            });
        }

        [Test]
        public void Query_ValidQueryWithObject_Result()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name FROM Account");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                TextAssert.IsNotNullOrEmpty(actual[0].Id);
                TextAssert.IsNotNullOrEmpty(actual[0].Name);
            }

            actual = target.Query<RecordStub>("SELECT id, name FROM Account WHERE LastModifiedDate = 2013-12-01T12:00:00+00:00");
            Assert.IsNotNull(actual);
        }

        [Test]
        public void Query_ValidQueryWithJsonAttributeObject_Result()
        {
            var target = CreateClientAndAuth();
            var actual = target.QueryActionBatch<RecordStub>("SELECT  id, name, Phone from Account where Phone != '' LIMIT 3 ",
                (a) => { });
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                TextAssert.IsNotNullOrEmpty(actual[0].Id);
                TextAssert.IsNotNullOrEmpty(actual[0].Name);
                TextAssert.IsNotNullOrEmpty(actual[0].PhoneCustom);
            }
        }

        /// <summary>
        /// To validate this issue: https://github.com/giacomelli/SalesforceSharp/issues/4.
        /// </summary>
        [Test]
        public void Query_ValidQueryWithSpecialChars_Result()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name, description FROM Account WHERE LastModifiedDate >= 2013-12-01T12:00:00+00:00");
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// To validate this issue: https://github.com/giacomelli/SalesforceSharp/issues/6.
        /// </summary>
        [Test]
        public void Query_ValidQueryClassWithFields_ResultNoFieldsBind()
        {
            var target = CreateClientAndAuth();

            // Public FIELDS are supported.
            var actual1 = target.Query<ContactStubWithFields>("SELECT Id, Name, Email FROM Contact LIMIT 1 OFFSET 0");
            Assert.AreEqual(1, actual1.Count);

            var first1 = actual1[0];
            TextAssert.IsNotNullOrEmpty(first1.Id);
            TextAssert.IsNotNullOrEmpty(first1.Name);

            // Public PROPERTIES are supported.
            var actual2 = target.Query<ContactStub>("SELECT Id, Name, Email FROM Contact LIMIT 1 OFFSET 0");
            Assert.AreEqual(1, actual2.Count);

            var first2 = actual2[0];
            TextAssert.IsNotNullOrEmpty(first2.Id);
            TextAssert.IsNotNullOrEmpty(first2.Name);
        }

        [Test]
        public void Query_ValidQueryWithObjectWrongPropertyTypes_Exception()
        {
            var target = CreateClientAndAuth();

            ExceptionAssert.IsThrowing(typeof(JsonReaderException), () =>
            {
                target.Query<WrongRecordStub>("SELECT IsDeleted FROM Account");
            });
        }

        #endregion Query

        #region QueryAsync

        [Test]
        public async Task QueryAsync_InvalidQueryAsync_Exception()
        {
            var target = await CreateClientAndAuthAsync();

            ExceptionAssert.IsThrowing(typeof(SalesforceException), async () =>
            {
                await target.QueryAsync<RecordStub>("SELECT id, name, FROM " + TestConfig.ObjectName);
            });
        }

        [Test]
        public async Task QueryAsync_ValidQueryWithObject_Result()
        {
            var target = await CreateClientAndAuthAsync();
            var actual = await target.QueryAsync<RecordStub>("SELECT id, name FROM Account");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                TextAssert.IsNotNullOrEmpty(actual[0].Id);
                TextAssert.IsNotNullOrEmpty(actual[0].Name);
            }

            actual = await target.QueryAsync<RecordStub>("SELECT id, name FROM Account WHERE LastModifiedDate = 2013-12-01T12:00:00+00:00");
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task QueryAsync_ValidQueryWithJsonAttributeObject_Result()
        {
            var target = await CreateClientAndAuthAsync();
            var actual = await target.QueryActionBatchAsync<RecordStub>("SELECT  id, name, Phone from Account where Phone != '' LIMIT 3 ",
                (a) => { });
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                TextAssert.IsNotNullOrEmpty(actual[0].Id);
                TextAssert.IsNotNullOrEmpty(actual[0].Name);
                TextAssert.IsNotNullOrEmpty(actual[0].PhoneCustom);
            }
        }

        /// <summary>
        /// To validate this issue: https://github.com/giacomelli/SalesforceSharp/issues/4.
        /// </summary>
        [Test]
        public async Task QueryAsync_ValidQueryWithSpecialChars_Result()
        {
            var target = await CreateClientAndAuthAsync();
            var actual = await target.QueryAsync<RecordStub>("SELECT id, name, description FROM Account WHERE LastModifiedDate >= 2013-12-01T12:00:00+00:00");
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// To validate this issue: https://github.com/giacomelli/SalesforceSharp/issues/6.
        /// </summary>
        [Test]
        public async Task QueryAsync_ValidQueryClassWithFields_ResultNoFieldsBind()
        {
            var target = await CreateClientAndAuthAsync();

            // Public FIELDS are supported.
            var actual1 = await target.QueryAsync<ContactStubWithFields>("SELECT Id, Name, Email FROM Contact LIMIT 1 OFFSET 0");
            Assert.AreEqual(1, actual1.Count);

            var first1 = actual1[0];
            TextAssert.IsNotNullOrEmpty(first1.Id);
            TextAssert.IsNotNullOrEmpty(first1.Name);

            // Public PROPERTIES are supported.
            var actual2 = await target.QueryAsync<ContactStub>("SELECT Id, Name, Email FROM Contact LIMIT 1 OFFSET 0");
            Assert.AreEqual(1, actual2.Count);

            var first2 = actual2[0];
            TextAssert.IsNotNullOrEmpty(first2.Id);
            TextAssert.IsNotNullOrEmpty(first2.Name);
        }

        [Test]
        public async Task QueryAsync_ValidQueryWithObjectWrongPropertyTypes_Exception()
        {
            var target = await CreateClientAndAuthAsync();

            ExceptionAssert.IsThrowing(typeof(JsonReaderException), async () =>
            {
                await target.QueryAsync<WrongRecordStub>("SELECT IsDeleted FROM Account");
            });
        }

        #endregion QueryAsync

        #region QueryBatch

        [Test]
        public void QueryActionBatch_ValidQuery_AllRecords()
        {
            var target = CreateClientAndAuth();
            var queryString = "SELECT id, name, description ";
            queryString += " FROM Account";

            var totalRecords = 0;

            var actual = target.QueryActionBatch<RecordStub>(queryString, s =>
            {
                totalRecords += s.Count;
            });

            Assert.IsNotNull(totalRecords);
            Assert.AreNotEqual(0, totalRecords);
            Assert.AreEqual(totalRecords, actual.Count);
        }

        #endregion QueryBatch

        #region QueryBatchAsync

        [Test]
        public async Task QueryActionBatchAsync_ValidQuery_AllRecords()
        {
            var target = await CreateClientAndAuthAsync();
            var queryString = "SELECT id, name, description ";
            queryString += " FROM Account";

            var totalRecords = 0;

            var actual = await target.QueryActionBatchAsync<RecordStub>(queryString, s =>
            {
                totalRecords += s.Count;
            });

            Assert.IsNotNull(totalRecords);
            Assert.AreNotEqual(0, totalRecords);
            Assert.AreEqual(totalRecords, actual.Count);
        }

        #endregion QueryBatchAsync

        #region GetSOjbect

        [Test]
        public void Get_SOjbectDetail_work()
        {
            var target = CreateClientAndAuth();
            var accObject = target.GetSObjectDetail("account");
            Assert.IsNotNull(accObject);
            Assert.AreEqual(accObject.Name, "Account");
            Assert.IsNotNull(accObject.Fields);
            Assert.IsNotEmpty(accObject.Fields);
            Assert.IsNotNull(accObject.Fields.FirstOrDefault(x => x.Name == "Id"));

            var industryField = accObject.Fields.FirstOrDefault(x => x.Name == "Industry");
            Assert.IsNotNull(industryField);
            Assert.IsNotNull(industryField.PicklistValues);
            Assert.IsNotEmpty(industryField.PicklistValues);
            Assert.IsNotNull(industryField.PicklistValues.FirstOrDefault(y => y.Value == "Engineering"));
        }

        #endregion GetSOjbect

        #region GetSOjbectAsync

        [Test]
        public async Task Get_SOjbectDetailAsync_work()
        {
            var target = await CreateClientAndAuthAsync();
            var accObject = await target.GetSObjectDetailAsync("account");
            Assert.IsNotNull(accObject);
            Assert.AreEqual(accObject.Name, "Account");
            Assert.IsNotNull(accObject.Fields);
            Assert.IsNotEmpty(accObject.Fields);
            Assert.IsNotNull(accObject.Fields.FirstOrDefault(x => x.Name == "Id"));

            var industryField = accObject.Fields.FirstOrDefault(x => x.Name == "Industry");
            Assert.IsNotNull(industryField);
            Assert.IsNotNull(industryField.PicklistValues);
            Assert.IsNotEmpty(industryField.PicklistValues);
            Assert.IsNotNull(industryField.PicklistValues.FirstOrDefault(y => y.Value == "Engineering"));
        }

        #endregion GetSOjbectAsync

        #region GetRaw

        [Test]
        public void GetRawContent_ValidRecord()
        {
            var target = CreateClientAndAuth();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = target.Create("Contact", record);
            var actual = target.GetRawContent("Contact", id);

            Assert.IsNotNull(actual);
            Assert.That(actual.Contains(string.Format("\"FirstName\":\"{0}\"", record.FirstName)));
            Assert.That(actual.Contains(string.Format("\"LastName\":\"{0}\"", record.LastName)));
        }

        [Test]
        public void GetRawBytes_ValidRecord()
        {
            var target = CreateClientAndAuth();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = target.Create("Contact", record);
            var bytes = target.GetRawBytes("Contact", id);

            string actual = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.IsNotNull(actual);
            Assert.That(actual.Contains(string.Format("\"FirstName\":\"{0}\"", record.FirstName)));
            Assert.That(actual.Contains(string.Format("\"LastName\":\"{0}\"", record.LastName)));
        }

        #endregion GetRaw

        #region GetRawAsync

        [Test]
        public async Task GetRawContentAsync_ValidRecord()
        {
            var target = await CreateClientAndAuthAsync();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = await target.CreateAsync("Contact", record);
            var actual = await target.GetRawContentAsync("Contact", id);

            Assert.IsNotNull(actual);
            Assert.That(actual.Contains(string.Format("\"FirstName\":\"{0}\"", record.FirstName)));
            Assert.That(actual.Contains(string.Format("\"LastName\":\"{0}\"", record.LastName)));
        }

        [Test]
        public async Task GetRawBytesAsync_ValidRecord()
        {
            var target = await CreateClientAndAuthAsync();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = await target.CreateAsync("Contact", record);
            var bytes = target.GetRawBytes("Contact", id);

            string actual = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.IsNotNull(actual);
            Assert.That(actual.Contains(string.Format("\"FirstName\":\"{0}\"", record.FirstName)));
            Assert.That(actual.Contains(string.Format("\"LastName\":\"{0}\"", record.LastName)));
        }

        #endregion GetRawAsync

        #region FindById

        [Test]
        public void FindById_NotExistingID_Null()
        {
            var target = CreateClientAndAuth();
            Assert.IsNull(target.FindById<RecordStub>("Contact", "003i000000K2BP0AAM"));
        }

        [Test]
        public void FindById_ValidId_Record()
        {
            var target = CreateClientAndAuth();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = target.Create("Contact", record);
            var actual = target.FindById<ContactStub>("Contact", id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(record.FirstName, actual.FirstName);
            Assert.AreEqual(record.LastName, actual.LastName);
        }

        #endregion FindById

        #region FindByIdAsync

        [Test]
        public async Task FindByIdAsync_NotExistingID_Null()
        {
            var target = await CreateClientAndAuthAsync();
            Assert.IsNull(await target.FindByIdAsync<RecordStub>("Contact", "003i000000K2BP0AAM"));
        }

        [Test]
        public async Task FindByIdAsync_ValidId_Record()
        {
            var target = await CreateClientAndAuthAsync();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = await target.CreateAsync("Contact", record);
            var actual = target.FindById<ContactStub>("Contact", id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(record.FirstName, actual.FirstName);
            Assert.AreEqual(record.LastName, actual.LastName);
        }

        #endregion FindByIdAsync

        #region ReadMetaData

        [Test]
        public void ReadMetaData_ValidObjectName_Metadata()
        {
            var target = CreateClientAndAuth();

            string result = target.ReadMetaData("Account");

            TextAssert.IsNotNullOrEmpty(result);
        }

        #endregion ReadMetaData

        #region ReadMetaDataAsync

        [Test]
        public async Task ReadMetaDataAsync_ValidObjectName_Metadata()
        {
            var target = await CreateClientAndAuthAsync();

            string result = await target.ReadMetaDataAsync("Account");

            TextAssert.IsNotNullOrEmpty(result);
        }

        #endregion ReadMetaDataAsync

        #region Create

        [Test]
        public void Create_ValidRecordWithAnonymous_Created()
        {
            var target = CreateClientAndAuth();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = target.Create("Contact", record);
            Assert.IsFalse(String.IsNullOrWhiteSpace(id));
        }

        [Test]
        public void Create_ValidRecordWithClassWithWrongProperties_Exception()
        {
            var target = CreateClientAndAuth();
            var record = new
            {
                FirstName1 = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidField, "No such column 'FirstName1' on sobject of type Contact"), () =>
            {
                target.Create("Contact", record);
            });
        }

        #endregion Create

        #region CreateAsync

        [Test]
        public async Task CreateAsync_ValidRecordWithAnonymous_Created()
        {
            var target = await CreateClientAndAuthAsync();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = await target.CreateAsync("Contact", record);
            Assert.IsFalse(String.IsNullOrWhiteSpace(id));
        }

        [Test]
        public async Task CreateAsync_ValidRecordWithClassWithWrongProperties_Exception()
        {
            var target = await CreateClientAndAuthAsync();
            var record = new
            {
                FirstName1 = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidField, "No such column 'FirstName1' on sobject of type Contact"), async () =>
            {
                await target.CreateAsync("Contact", record);
            });
        }

        #endregion CreateAsync

        #region Update

        [Test]
        public void Update_InvalidId_Exception()
        {
            var target = CreateClientAndAuth();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.NotFound, "Provided external ID field does not exist or is not accessible: INVALID ID"), () =>
            {
                target.Update(TestConfig.ObjectName, "INVALID ID", new { Name = "TEST" });
            });
        }

        [Test]
        public void Update_ValidRecordWithAnonymous_Updated()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name, description FROM Account");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                Assert.IsTrue(target.Update("Account", actual[0].Id, new { Description = DateTime.Now + " UPDATED" }));
            }
        }

        [Test]
        public void Update_ValidRecordWithClass_Updated()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name, Phone, FirstName, LastName, description FROM Contact LIMIT 10");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                Assert.IsTrue(target.Update("Contact", actual[0].Id, new RecordStub { FirstName = actual[0].FirstName, LastName = actual[0].LastName, PhoneCustom = actual[0].PhoneCustom, Description = DateTime.Now + " UPDATED" }));
            }
        }

        [Test]
        public void Update_ValidRecordWithClassWithWrongProperties_Exception()
        {
            var target = CreateClientAndAuth();
            var actual = target.Query<RecordStub>("SELECT id, name, description FROM Account");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidFieldForInsertUpdate, "Unable to create/update fields: IsDeleted. Please check the security settings of this field and verify that it is read/write for your profile or permission set."), () =>
                {
                    target.Update("Account", actual[0].Id, new WrongRecordStub { Name = actual[0].Name, Description = DateTime.Now + " UPDATED" });
                });
            }
        }

        #endregion Update

        #region UpdateAsync

        [Test]
        public async Task UpdateAsync_InvalidId_Exception()
        {
            var target = await CreateClientAndAuthAsync();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.NotFound, "Provided external ID field does not exist or is not accessible: INVALID ID"), async () =>
            {
                await target.UpdateAsync(TestConfig.ObjectName, "INVALID ID", new { Name = "TEST" });
            });
        }

        [Test]
        public async Task UpdateAsync_ValidRecordWithAnonymous_Updated()
        {
            var target = await CreateClientAndAuthAsync();
            var actual = await target.QueryAsync<RecordStub>("SELECT id, name, description FROM Account");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                Assert.IsTrue(await target.UpdateAsync("Account", actual[0].Id, new { Description = DateTime.Now + " UPDATED" }));
            }
        }

        [Test]
        public async Task UpdateAsync_ValidRecordWithClass_Updated()
        {
            var target = await CreateClientAndAuthAsync();
            var actual = await target.QueryAsync<RecordStub>("SELECT id, name, Phone, FirstName, LastName, description FROM Contact LIMIT 10");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                Assert.IsTrue(await target.UpdateAsync("Contact", actual[0].Id, new RecordStub { FirstName = actual[0].FirstName, LastName = actual[0].LastName, PhoneCustom = actual[0].PhoneCustom, Description = DateTime.Now + " UPDATED" }));
            }
        }

        [Test]
        public async Task UpdateAsync_ValidRecordWithClassWithWrongProperties_Exception()
        {
            var target = await CreateClientAndAuthAsync();
            var actual = await target.QueryAsync<RecordStub>("SELECT id, name, description FROM Account");
            Assert.IsNotNull(actual);

            if (actual.Count > 0)
            {
                ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.InvalidFieldForInsertUpdate, "Unable to create/update fields: IsDeleted. Please check the security settings of this field and verify that it is read/write for your profile or permission set."), async () =>
                {
                    await target.UpdateAsync("Account", actual[0].Id, new WrongRecordStub { Name = actual[0].Name, Description = DateTime.Now + " UPDATED" });
                });
            }
        }

        #endregion UpdateAsync

        #region Delete

        [Test]
        public void Delete_MalFormedId_Exception()
        {
            var target = CreateClientAndAuth();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.EntityIsDeleted, "malformed id 003i000000K27rxAAC"), () =>
            {
                target.Delete("Contact", "003i000000K27rxAAC");
            });
        }

        [Test]
        public void Delete_AlreadyDeleted_Exception()
        {
            var target = CreateClientAndAuth();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = target.Create("Contact", record);

            Assert.IsTrue(target.Delete("Contact", id));

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.EntityIsDeleted, "Entity is deleted"), () =>
            {
                target.Delete("Contact", id);
            });
        }

        [Test]
        public void Delete_ExistingId_Deleted()
        {
            var target = CreateClientAndAuth();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = target.Create("Contact", record);

            Assert.IsTrue(target.Delete("Contact", id));
        }

        #endregion Delete

        #region DeleteAsync

        [Test]
        public async Task DeleteAsync_MalFormedId_Exception()
        {
            var target = await CreateClientAndAuthAsync();

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.EntityIsDeleted, "malformed id 003i000000K27rxAAC"), async () =>
            {
                await target.DeleteAsync("Contact", "003i000000K27rxAAC");
            });
        }

        [Test]
        public async Task DeleteAsync_AlreadyDeleted_Exception()
        {
            var target = await CreateClientAndAuthAsync();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = await target.CreateAsync("Contact", record);

            Assert.IsTrue(await target.DeleteAsync("Contact", id));

            ExceptionAssert.IsThrowing(new SalesforceException(SalesforceError.EntityIsDeleted, "Entity is deleted"), async () =>
            {
                await target.DeleteAsync("Contact", id);
            });
        }

        [Test]
        public async Task DeleteAsync_ExistingId_Deleted()
        {
            var target = await CreateClientAndAuthAsync();
            var record = new
            {
                FirstName = "Name " + DateTime.Now.Ticks,
                LastName = "Last name"
            };

            var id = await target.CreateAsync("Contact", record);

            Assert.IsTrue(await target.DeleteAsync("Contact", id));
        }

        #endregion DeleteAsync

        #region ClassHelper

        [Test]
        public void GetRecordProjection_Result()
        {
            var jSonPropertyString = SalesforceClient.GetRecordProjection(typeof(TestJson));
            Assert.IsTrue(jSonPropertyString.Contains("Id"));
            Assert.IsTrue(jSonPropertyString.Contains("JsonName"));
            Assert.IsFalse(jSonPropertyString.Contains("JsonIgnoreMe"));
        }

        #endregion ClassHelper

        #region Helpers

        private SalesforceClient CreateClientAndAuth()
        {
            return CreateClientAndAuth(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password);
        }

        private Task<SalesforceClient> CreateClientAndAuthAsync()
        {
            return CreateClientAndAuthAsync(TestConfig.ClientId, TestConfig.ClientSecret, TestConfig.Username, TestConfig.Password);
        }

        private UsernamePasswordAuthenticationFlow CreateAuthenticationFlow(string clientId, string clientSecret, string username, string password)
        {
            var flow = new UsernamePasswordAuthenticationFlow(clientId, clientSecret, username, password);
            flow.TokenRequestEndpointUrl = TestConfig.TokenRequestEndpointUrl;

            return flow;
        }

        private SalesforceClient CreateClientAndAuth(
            string clientId,
            string clientSecret,
            string username,
            string password)
        {
            var client = new SalesforceClient();
            var authenticationFlow = CreateAuthenticationFlow(clientId, clientSecret, username, password);

            client.Authenticate(authenticationFlow);

            return client;
        }

        private async Task<SalesforceClient> CreateClientAndAuthAsync(
           string clientId,
           string clientSecret,
           string username,
           string password)
        {
            var client = new SalesforceClient();
            var authenticationFlow = CreateAuthenticationFlow(clientId, clientSecret, username, password);

            await client.AuthenticateAsync(authenticationFlow);

            return client;
        }

        public class TestJson
        {
            public int Id { get; set; }

            [Salesforce(Ignore = true)]
            public string JsonIgnoreMe { get; set; }

            [Salesforce(FieldName = "JsonName")]
            public string JsonRenameMe { get; set; }
        }

        #endregion Helpers
    }
}