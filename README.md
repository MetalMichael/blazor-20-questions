# Blazor 20 Questions

:warning: This application is currently non-functional due to a breaking [issue](https://github.com/dotnet/core/issues/2611) with MongoDB.Driver in .NET Core 3.0 preview 5. Waiting on: [this PR](https://github.com/mongodb/mongo-csharp-driver/pull/372) (or [this](https://github.com/rstam/mongo-csharp-driver/pull/110)):warning:

Demo Blazor app implementing a game of [20 Questions](https://en.wikipedia.org/wiki/Twenty_Questions).

Requires .NET Core 3.0 preview 5.

**Stack:**
 - Blazor
 - ASP.NET Core
 - MongoDB

## Docker

The application can be built using `docker-compose`.
Once running, the blazor app can be connected to on port 5000.

This also includes a [mongo-express](http://mongodb-tools.com/tool/mongo-express/) server for debugging.

## Future Improvements

Originally I was hoping to use websockets, to avoid having to poll the API for changes. Looking to add this improvement in the future.

## Screenshots

_Coming soon_