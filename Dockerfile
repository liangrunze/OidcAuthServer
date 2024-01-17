# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

COPY . /OidcAuthServer

WORKDIR /OidcAuthServer

RUN dotnet publish ./OidcAuthServer/OidcAuthServer.csproj --output /publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80
EXPOSE 443

# Set the environment variable, if it is Development, to enable the Debug option for easy debugging and configuration
ENV ASPNETCORE_ENVIRONMENT=Production

# To enable https, you need to configure the certificate address and password
ENV ENABLE_SSL=false 

COPY ./certificate /app/certificate

ENTRYPOINT ["/bin/bash", "-c", "dotnet OidcAuthServer.dll  --environment $ASPNETCORE_ENVIRONMENT"]

