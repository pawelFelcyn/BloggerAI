## Backend 

This directory contains the API source code. 

If you have `dotnet 8.0.12` installed, you can run the project with:

```bash
dotnet run ./BloggerAI.API/BloggerAI.API.csproj
```

You can also build the Docker image:

```bash
docker build -f ./BloggerAI.API/Dockerfile -t <your tag> .
```

### Configuration

In the files `./BloggerAI.API/appsettings.json` and `./BloggerAI.API/appsettings.Development.json`, you can override some configurations. The values from the second file will be applied only in debug mode and have a higher priority than the values from the first file. You can also create a `./BloggerAI.API/appsettings.Development.Local.json` file, which works like `./BloggerAI.API/appsettings.Development.json` (with higher priority), but will not be tracked by Git. The values to be configured:

- `ConnectionStrings__DatabaseConnection`: Connection string to the database (***needs to be configured in Release mode***)
- `AuthenticationSettings__JwtIssuer`: The issuer of the JWT token (***needs to be configured in Release mode***)
- `AuthenticationSettings__JwtExpireDays`: JWT token lifetime in days (***needs to be configured in Release mode***)
- `AuthenticationSettings__JwtKEY`: A private key for generating JWT tokens (***needs to be configured in Release mode***)
- `Keys__OpenAIAPIKey`: OpenAI API key (***needs to be configured in Release mode and in appsettings.Development.Local.json for Debug mode***)
