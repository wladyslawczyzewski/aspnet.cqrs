# ASPNET.CQRS

![Build RC](https://github.com/vchyzhevskyi/aspnet.cqrs/workflows/Build%20RC/badge.svg)
[![NuGet Badge](https://buildstats.info/nuget/ASPNET.CQRS?includePreReleases=true)](https://www.nuget.org/packages/ASPNET.CQRS/)

Basic implementation of CQRS pattern for .NET Core web APIs.

## Features

`v1.0.0-rc2`:

- Bypass controllers and execute Queries and Commands directly
- Supports simple (no params) and complex (with params) Queries and Commands
- Automatically parses params - Queries uses query string and Commands uses request body (json)
- Supports DI using default IServiceProvider

`v1.0.0.-rc3`:

- Fire & forget command handlers

## How to use?

1. Get the lib package from nuget.org:

```bash
dotnet add package ASPNET.CQRS --version 1.0.0-rc2
```

2. Then you need to setup the `CQRSOptions` in yours `ConfigureServices` method:

```csharp
services.AddCQRS(options =>
{
    options.BasePath = "/api";
    options.Assemblies = new[] { Assembly.GetExecutingAssembly() };
});
```

3. After that, in yours Startup's `Configure` method you need to configure CQRS's feature and CQRS's middleware:

```csharp
app.UseCQRS();
```

The example configuration above will load queries and commands from the same assembly as yours Startup class.

4. Once you have done the configuration, you can add your first query:

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

5. And then test it with your favourite tool to test web APIs, cor example with `cURL`:

```sh
curl --request GET --url https://localhost:5001/api/ping
```
