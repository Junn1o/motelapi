# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 9023
EXPOSE 9024

RUN mkdir -p /app/wwwroot/images/user && \
    chown -R $APP_UID:$APP_UID /app && \
    chmod -R 755 /app/wwwroot
USER $APP_UID
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY motel.sln ./
COPY ["motel.csproj", "./"]
RUN dotnet restore "./motel.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "./motel.csproj" -c $BUILD_CONFIGURATION -o /app/build



# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./motel.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
USER root
COPY --from=publish /app/publish .
RUN chown -R $APP_UID:$APP_UID /app && \
    chmod -R 755 /app/wwwroot
COPY ./wwwroot/images/user/noavt.png /app/wwwroot/images/user/noavt.png
USER $APP_UID
ENTRYPOINT ["dotnet", "motel.dll"]