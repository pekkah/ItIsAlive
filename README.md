# Bootstrapper

NOTE: New version is being planned with support for RTM version of ASP.NET MVC 4 and WebApi

Generic bootstrapper for rapidly building applications. 

_NuGet packages are not ready for publishing yet! You can still download and build them yourself!_

## Getting Started
Install-Package Bootstrapper

```csharp
var bootstrapper = new Setup().Dependencies(from => from.AssemblyOf<YourDependency>()).Start();
```

### NHibernate Extension

Install-Package Bootstrapper.Extensions.NHibernate

```csharp
var bootstrapper = new Configure().ConfigureNHibernate(() => 
{ 
	var configuration = new Configuration();
	// todo configure database, mappings etc.
	return configuration;
 }).Start();
```