FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app
# Copy everything and build
COPY . ./
RUN dotnet restore "./Blazor20Questions.Server/Blazor20Questions.Server.csproj"
RUN dotnet publish "./Blazor20Questions.Server/Blazor20Questions.Server.csproj" -c Release -o out
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Blazor20Questions.Server.dll"]