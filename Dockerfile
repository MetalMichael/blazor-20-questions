FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview5-buster AS build-env
WORKDIR /app
# Copy everything and build
COPY . ./
RUN dotnet restore "./Blazor20Questions.Server/Blazor20Questions.Server.csproj"
RUN dotnet publish "./Blazor20Questions.Server/Blazor20Questions.Server.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview5-buster-slim
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Blazor20Questions.Server.dll"]