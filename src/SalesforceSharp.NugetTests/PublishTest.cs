using NUnit.Framework;
using System;
using SalesforceSharp.Security;
using TestSharp;

namespace SalesforceSharp.NugetTests
{
	/// <summary>
	/// The purpose of these tests is validate if the last SalesforceSharp's Nuget Package has been well built and published.
	/// So, after published a new package version, please, update the SalesforceSharp package in this project and run the tests
	/// below.
	/// <remarks>>
	/// It will use the configurations from SalesforceSharp.FunctionalTests's app.config.
	/// </remarks>
	/// </summary>
	[TestFixture ()]
	public class PublishTest
	{
		[Test ()]
		public void Publish_Package_InstalledAndRunning ()
		{
			var clientId = ConfigHelper.ReadAppSetting ("SalesforceSharp.FunctionalTests", "ClientId");
			var clientSecret = ConfigHelper.ReadAppSetting ("SalesforceSharp.FunctionalTests", "ClientSecret");
			var username = ConfigHelper.ReadAppSetting ("SalesforceSharp.FunctionalTests", "Username");
			var password = ConfigHelper.ReadAppSetting ("SalesforceSharp.FunctionalTests", "Password");
			var flow = new UsernamePasswordAuthenticationFlow (clientId, clientSecret, username, password);

			var client = new SalesforceClient();
			client.Authenticate (flow);

			var users = client.Query<UserStub> ("SELECT Username, Email FROM USER");

			Assert.That (users.Count > 0);
			Assert.IsFalse (String.IsNullOrEmpty (users [0].Username));
			Assert.IsFalse (String.IsNullOrEmpty (users [0].Email));
			Assert.IsTrue (String.IsNullOrEmpty (users [0].Alias));
		}
	}
}

