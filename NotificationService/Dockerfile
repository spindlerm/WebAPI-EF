FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /source
COPY  *.sln .

# copy sln and csproj as distinct layers
COPY  CustomerService/*.csproj ./CustomerService/
COPY  Messages/*.csproj ./Messages/
COPY  NotificationService/N*.csproj ./NotificationService/
RUN dotnet restore

#Copy everything else
COPY NotificationService/. ./NotificationService/
COPY Messages/  ./Messages/
WORKDIR /source/NotificationService
RUN dotnet publish  -c Release -o /app 

#Final stsge image
FROM mcr.microsoft.com/dotnet/runtime:5.0  
WORKDIR /app
COPY --from=build-env /app  ./
ENTRYPOINT  ["dotnet", "NotificationService.dll"]