# Sakura

Generic bootstrapper for rapidly building applications. 

_NuGet packages are not ready for publishing yet! You can still download and build them yourself!_

## Getting Started
Install-Package Sakura

```csharp
var bootstrapper = new Setup().Dependencies(from => from.AssemblyOf<YourDependency>()).Start();
```

### NHibernate Extension

Install-Package Sakura.Extensions.NHibernate

```csharp
var bootstrapper = new Setup().ConfigureNHibernate(() => 
{ 
	var configuration = new Configuration();
	// todo configure database, mappings etc.
	return configuration;
 }).Start();
```

### ASP.NET MVC 3 Extension

Install-Package Sakura.Extensions.Mvc

```csharp
var bootstrapper = new Setup()
	.Dependencies(from => from.AssemblyOf<MyController>())
	.ConfigureMvc(() => 
	{ 
		var routes = RouteTable.Routes;
		routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

		routes.MapRoute(
                	"Default", 
                	"{controller}/{action}/{id}", 
                	new { controller = "Home", action = "Index", id = UrlParameter.Optional });
	}).Start();
```

### WCF Web Api Extension

Install-Package Sakura.Extensions.WebApi

```csharp
var bootstrapper = new Setup()
	.Dependencies(from => from.AssemblyOf<ContactsApi>())
	.ConfigureWebApi(configurationFactory => 
	{ 
		var routes = RouteTable.Routes;
		var configuration = configurationFactory();
		routes.SetDefaultHttpConfiguration(configuration);
		routes.MapServiceRoute<ContactsApi>("api/contacts");
	}).Start();
```

### NHibernate integration with ASP.NET MVC 3

Install-Package Sakura.Extensions.NHibernateMvc

```csharp
var bootstrapper = new Setup()
	.Dependencies(from => from.AssemblyOf<MyController>())
	.ConfigureMvc(() => 
	{ 
		var routes = RouteTable.Routes;
		routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

		routes.MapRoute(
                	"Default", 
                	"{controller}/{action}/{id}", 
                	new { controller = "Home", action = "Index", id = UrlParameter.Optional });
	})
	.ConfigureNHibernate(ConfigureNHibernate)
	.EnableMvcUnitOfWork()
	.Start();
	
public class ContactsController : Controller // Sakura will automatically register IController -types
{
	public ActionResult Contacts(IUnitOfWork unitOfWork)
	{
		var contacts = unitOfWork.QueryOver<Contact>().Take(100).List();
		
		return View(contacts);
	}
}
```

Your controller action methods will be automatically wrapped in transaction when ```IUnitOfWork``` is added as parameter.

- If your method is successfull then transaction is automatically committed,
- If your method fails by throwing an exception the transaction is automatically rolled back.

### NHibernate integration with WCF Web Api

Install-Package Sakura.Extensions.NHibernateWebApi

```csharp
var bootstrapper = new Setup()
	.Dependencies(from => from.AssemblyOf<ContactsApi>())
	.ConfigureWebApi(configurationFactory => 
	{ 
		var routes = RouteTable.Routes;
		var configuration = configurationFactory();
		routes.SetDefaultHttpConfiguration(configuration);
		routes.MapServiceRoute<ContactsApi>("api/contacts");
	})
	.ConfigureNHibernate(ConfigureNHibernate)
	.EnableWebApiUnitOfWork()
	.Start();
	
[ServiceContract]
public class ContactsApi
{
	[WebGet]
	public IEnumerable<ContactDto> Contacts(IUnitOfWork unitOfWork)
	{
		var contacts = unitOfWork.QueryOver<Contact>().Take(100).List();
		return contacts.Select(contact => new ContactDto() { Name = contact.Name });
	}
}
```

Your service action methods will be automatically wrapped in transaction when ```IUnitOfWork``` is added as parameter.

- If your method is successfull then transaction is automatically committed,
- If your method fails by throwing an exception the transaction is automatically rolled back.
 

