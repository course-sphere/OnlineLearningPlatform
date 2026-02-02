FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore MVC
RUN dotnet publish -o out MVC

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as release
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MVC.dll"]
