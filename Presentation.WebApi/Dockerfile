#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation.WebApi/Presentation.WebApi.csproj", "Presentation.WebApi/"]
COPY ["BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["Data.Postgres/Data.Postgres.csproj", "Data.Postgres/"]
COPY ["Notification.Email/Notification.Email.csproj", "Notification.Email/"]
COPY ["Data.Proxy/Data.Proxy.csproj", "Data.Proxy/"]
COPY ["Data.Redis.Common/Data.Redis.Common.csproj", "Data.Redis.Common/"]
COPY ["Data.Redis.Specific/Data.Redis.Specific.csproj", "Data.Redis.Specific/"]
COPY ["Data.SqlServer.Specific/Data.SqlServer.Specific.csproj", "Data.SqlServer.Specific/"]
COPY ["Notification.Email.AWS/Notification.Email.AWS.csproj", "Notification.Email.AWS/"]
COPY ["Notification.SMS/Notification.SMS.csproj", "Notification.SMS/"]
RUN dotnet restore "./Presentation.WebApi/./Presentation.WebApi.csproj"
COPY . .
WORKDIR "/src/Presentation.WebApi"
RUN dotnet build "./Presentation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Presentation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.WebApi.dll"]