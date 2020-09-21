# VladyslavChyzhevskyi.ASPNET.CQRS

![Build RC](https://github.com/vchyzhevskyi/aspnet.cqrs/workflows/Build%20RC/badge.svg)

Basic implementation of CQRS pattern for .NET Core web APIs.

## Features

- Bypass controllers and execute Queries and Commands directly
- Supports simple (no params) and complex (with params) Queries and Commands
- Automatically parses params - Queries uses query string and Commands uses request body (json)
- Supports DI using default IServiceProvider

## How to use?

First you need to setup the `CQRSOptions` in yours `ConfigureServices` method:

```csharp
services.AddCQRS(options =>
{
    options.BasePath = "/api";
    options.Assemblies = new[] { Assembly.GetExecutingAssembly() };
});
```

Then, in yours Startup's `Configure` method you need to configure CQRS's feature and CQRS's middleware:

```csharp
app.UseCQRS();
```

The example configuration above will lead queries and commands from the same assembly as yours Startup class.

Once you have done the configuration, you can add your first query:

```csharp
[CQRSRoute("/ping")]
public class PingQuery : IQueryHandler
{
    public Task Handle()
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
