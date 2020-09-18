# VladyslavChyzhevskyi.ASPNET.CQRS

Basic implemtation of CQRS pattern for .NET Core web APIs.

## How to use?

First you need to setup the `ICQRSFeatureProvider` in yours `COnfigureServices` method:

```csharp
services.AddCQRS(options =>
{
    options.BasePath = "/api";
    options.Assemblies = new[] { Assembly.GetExecutingAssembly() };
});
```

Then, in yours Startup's Configure method you need to configure CQRS's feature and CQRS's middleware:

```csharp
app.UseCQRS();
```

The example configuration about will lead queries and commands from the same assembly as yours Startup class.

Once you have done the configuration, you can add your first query:

```csharp
[CQRSRoute("/ping")]
public class PingQuery : IQuery
{
    public Task Execute()
    {
        // noop
        return Task.CompletedTask;
    }
}
```

And then test it with your favourite tool to test web APIs, cor example with `cURL`:

```sh
curl --request GET --url https://localhost:5001/api/ping
```
