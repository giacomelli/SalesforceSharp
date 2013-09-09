using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace SalesforceSharp.UnitTests
{
    [TestFixture]
    public class SalesforceExceptionTest
    {
        [Test]
        public void Constructor_UnknowErrorString_Unknow()
        {
            var target = new SalesforceException("TEST1", "description 1");
            Assert.AreEqual(SalesforceError.Unknown, target.Error);
            Assert.AreEqual("description 1", target.Message);
            Assert.AreEqual(0, target.Fields.Length);

            target = new SalesforceException("TEST2", "description 2");
            Assert.AreEqual(SalesforceError.Unknown, target.Error);
            Assert.AreEqual("description 2", target.Message);
            Assert.AreEqual(0, target.Fields.Length);
        }

        [Test]
        public void Constructor_KnownErrorString_Parsed()
        {
            var target = new SalesforceException("String_Too_Long", "description 1");
            Assert.AreEqual(SalesforceError.StringTooLong, target.Error);
            Assert.AreEqual("description 1", target.Message);
            Assert.AreEqual(0, target.Fields.Length);

            target = new SalesforceException("Invalid_Password", "description 2");
            Assert.AreEqual(SalesforceError.InvalidPassword, target.Error);
            Assert.AreEqual("description 2", target.Message);
            Assert.AreEqual(0, target.Fields.Length);
        }

        [Test]
        public void Constructor_WithFields_Fields()
        {
            var target = new SalesforceException("String_Too_Long", "description 1", new string[] { "Field 1", "Field 2" });
            Assert.AreEqual(SalesforceError.StringTooLong, target.Error);
            Assert.AreEqual("description 1", target.Message);
            Assert.AreEqual(2, target.Fields.Length);
            Assert.AreEqual("Field 1", target.Fields[0]);
            Assert.AreEqual("Field 2", target.Fields[1]);
        }

        [Test]
        public void GetObjectData_Args_DataAdded()
        {
            var target = new SalesforceException("String_Too_Long", "description 1", new string[] { "Field 1", "Field 2" });
            var info = new SerializationInfo(typeof(SalesforceException), new FormatterConverter());
            target.GetObjectData(info, new StreamingContext());
            Assert.AreEqual(SalesforceError.StringTooLong,  info.GetValue("Error", typeof(SalesforceError)));
            Assert.AreEqual(target.Fields, info.GetValue("Fields", typeof(string[])));
        }
    }
}