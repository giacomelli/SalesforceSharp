SalesforceSharp
===============

[![Build status](https://ci.appveyor.com/api/projects/status/0tcc8lbbhrqc86hc/branch/master?svg=true)](https://ci.appveyor.com/project/giacomelli/salesforcesharp/branch/master)

An easy-to-use .NET client library for Salesforce REST API

--------

Features
===
 - Create, Update, Delete and Query records.
 - FindById for finding records easily.
 - GetRawContent to get raw string results.
 - GetRawBytes to get raw bytes results, useful for working with blobs.
 - SalesforceAttribute (Ignore, IgnoreUpdate and FieldName).
 - Authentication flows
   	- UsernamePasswordAuthenticationFlow
   	- Others authentication flows can be added by implementing IAuthenticationFlow.
 - Mono support
 - Fully tested on Windows and OSX
 - 100% Unit Tests coveraged 
 - 100% code documentation
 - FxCop validated
 - Good (and well used) design patterns  

--------

Setup
===
PM> Install-Package SalesforceSharp
> 

Usage
===

Authenticating
---
```csharp
var client = new SalesforceClient();
var authFlow = new UsernamePasswordAuthenticationFlow(clientId, clientSecret, username, password);

try 
{
	client.Authenticate(authFlow);
}
catch(SalesforceException ex)
{
	Console.WriteLine("Authentication failed: {0} : {1}", ex.Error, ex.Message);
}
```

Querying records
---
```csharp

public class Account
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

....

var records = client.Query<Account>("SELECT id, name, description FROM Account");

foreach(var r in records)
{
	Console.WriteLine("{0}: {1}", r.Id, r.Name);
}
```

Finding a record by Id
---

```csharp

var record = client.FindById<Account>("Account", "<ID>");
```

Creating a record
---
```csharp

// Using a class. 
client.Create("Account", new Account() 
{ Name = "name created", Description = "description created" }));

// Using an anonymous.
client.Create("Account", new { Name = "name created", Description = "description created" }));
```

Updating a record
---
```csharp

// Using a class. Ever required property should be set.
client.Update("Account", "<record id>", new Account() 
{ Name = "name updated", Description = "description updated" }));

// Using an anonymous. Only required properties will be updated.
client.Update("Account", "<record id>", new { Description = "description updated" }));
```

Deleting a record
---
```csharp

client.Delete("Account", "<ID">);
```

--------

FAQ
-------- 
Having troubles? 
 - Take a look in our [FAQ](https://github.com/giacomelli/SalesforceSharp/wiki/FAQ).
 - Ask on [Stack Overflow](http://stackoverflow.com/search?q=SalesforceSharp)

Roadmap
-------- 
 - Implements others authentcation flows:
 	-  Web server flow, where the server can securely protect the consumer secret.
 	-  User-agent flow, used by applications that cannot securely store the consumer secret.
 
--------

How to improve it?
======

- Create a fork of [SalesforceSharp](https://github.com/giacomelli/SalesforceSharp/fork). 
- Follow our [develoment guidelines](https://github.com/giacomelli/SalesforceSharp/wiki/Development-Guidelines).
- Did you change it? [Submit a pull request](https://github.com/giacomelli/SalesforceSharp/pull/new/master).


License
======

Licensed under the The MIT License (MIT).
In others words, you can use this library for developement any kind of software: open source, commercial, proprietary and alien.