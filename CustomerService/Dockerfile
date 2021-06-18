FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
#ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /source
COPY  *.sln .

# copy sln and csproj as distinct layers
COPY  CustomerService/*.csproj ./CustomerService/
COPY  Messages/*.csproj ./Messages/
COPY  NotificationService/N*.csproj ./NotificationService/
RUN dotnet restore

#Copy everything else
COPY CustomerService/. ./CustomerService/
COPY Messages/  ./Messages/
WORKDIR /source/CustomerService
RUN dotnet publish  -c Release -o /app 

#Final stsge image
FROM mcr.microsoft.com/dotnet/aspnet:5.0  
WORKDIR /app
COPY --from=build-env /app  ./
ENTRYPOINT  ["dotnet", "CustomerService.dll"]