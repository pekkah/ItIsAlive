# Sakura

Generic bootstrapper for rapidly building applications. 

## Getting Started
Install-Package Sakura
```csharp
var bootstrapper = new Setup().Dependencies(from => from.AssemblyOf<YourDependency>()).Start();
```

NHibernate Extension
Install-Package Sakura.Extensions.NHibernate
```csharp
var bootstrapper = new Setup().ConfigureNHibernate(() => 
{ 
	var configuration = new Configuration();
	// todo configure database, mappings etc.
	return configuration;
 }).Start();
```

ASP.NET MVC 3 Extension
Install-Package Sakura.Extensions.NHibernate

WCF Web Api Extension

NHibernate integration with ASP.NET MVC 3


NHibernate integration with WCF Web Api
 

