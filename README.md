#  OpenID Connect (OIDC) Server

[README](README.md) | [中文文档](READMEZH.md)

## Introduction

- This project is an OpenID Connect (OIDC) server implemented using .NetCore. It can help you quickly integrate OIDC authentication into your application.
- The initial purpose of this project is to perform Oidc authentication in conjunction with FRP.For details about Frp configuration, see the following: Examples of Frp configuration.
## Environment Requirements

- .NET Core 3.1 

## Installation

First, you need to clone this repository to your local environment:

```bash
git clone https://github.com/liangrunze/OidcAuthServer.git
```
Then, go to the project directory and build it using .NET CLI:
```bash
cd ./OidcAuthServer
dotnet build
```
## Configuration and Running
You need to set your  OIDC parameters in the appsettings.json file.

```JSON
{
  "IssuerUri": "https://localhost",//issuer address, needs to be consistent with the external access address
  "OidcClients": [
    {
      "ClientWebHomepage": "",  //Oidc authorized client homepage
      "ClientId": "B90B1793-332A-49E4-8658-5A71503046B8",  //client id, can be customized
      "ClientName": "frpc-proxy", //client name, can be customized
      "AllowedGrantTypes": "client_credentials", //allowed authorization types
      "RequireConsent": false,
      "AllowedScopes": [ "profile", "openid", "email", "api-1" ], //allowed access domains
      "FrontChannelLogoutSessionRequired": true,
      "RequirePkce": false,
      "ClientSecrets": "c4ba5449-b604-49e1-9c8d-b3adb73cf566", //client's Secrets, the registered client needs to be consistent
      "OidcLoginCallback": "/oidc/login-callback",
      "OidcFrontChannelLogoutCallback": "/oidc/front-channel-logout-callback"
    }
  ]
}
```
Quick Run: 
You can use the following command to run your application:
```bash
dotnet run
```
Then, you can  Visit http://ip/.well-known/openid-configuration to view the oidc configuration information.

### Environment Variables
To listen to https, you need to add the environment variable ENABLE_SSL, and copy the certificate folder to the service running directory (the self-generated certificate and key are provided in the file, not recommended for formal environment), at this time the service will listen to ports 443 and 80.
```bash
set ENABLE_SSL=True
dotnet run
```
####Debugging 
If you encounter configuration connection and other issues, you can set the environment variable ASPNETCORE_ENVIRONMENT to open debug information, this is to open http://ip/ will display debug information. You can also check the console output content, which can be used to check the problem.
```bash
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```
## Docker

Docker compilation
```bash
docker build -t oidc-server:1.0.0 .
```
The image of the oidc-server is generated

Quickly start an oidc service
```bash
docker run --restart=always -p 80:80  -d  --name oidc-server oidc-server:1.0.0
```
You can view your app by visiting   http://ip/.well-known/openid-configuration for oidc configuration information.

### Custom configuration
Create a file appsettings.json, copy and modify the appsettings.json file in the warehouse
```bash
docker run --restart=always -p 80:80  -d -v /you-appsetting-path/appsettings.json:/app/appsettings.json  --name oidc-server oidc-server:1.0.0
```
### Enable debugging
When you open http://ip/, the debug message is displayed
At the same time, docker logs dockerid can be run to check problems
```bash
docker run --restart=always -p 80:80  -d -e ASPNETCORE_ENVIRONMENT=Development --name oidc-server oidc-server:1.0.0
```


### HTTPS\HTTP
To enable only HTTP, set ENABLE_SSL=false
Use the following command to start the HTTPS port, you need to mount your certificate to the specified directory (pfx format only, while the certificate name and key file name cannot be modified)
```bash
docker run --restart=always -p 80:80 -p 443:443 -d -e ENABLE_SSL=True -v /you-cert-path/cert:/app/certificate --name oidc-server oidc-server:1.0.0
```

## Frp configuration example
https://github.com/fatedier/frp/
```bash
# frps.toml
# Complete configuration:
#https://github.com/fatedier/frp/blob/dev/conf/frps_full_example.toml
auth.method = "oidc"
Aut.oidc.issuer = "https://localhost:80" # Your oidc-server address Note that the ending "/" must be the same as "IssuerUri" in appsettings.json
auth.oidc.audience = "api" # Modifiable
auth.oidc.skipExpiryCheck = false
auth.oidc.skipIssuerCheck = false
```

```bash
# frpc.toml
# Complete configuration:
#https://github.com/fatedier/frp/blob/dev/conf/frpc_full_example.toml
auth.method = "oidc"
auth.oidc.clientID = "B90B1793-332A-49E4-8658-5A71503046B8" # Replace with OIDC client ID
auth.oidc.clientSecret = "c4ba5449-b604-49e1-9c8d-b3adb73cf566"
auth.oidc.audience = "api" # Modifiable
auth.oidc.scope ="api-1"
Auth.Oidc.TokenEndpointURL = "https://localhost:80/connect/token" # your oidc - server address/connect/token is fixed
```
### frp run example
FRP : https://github.com/fatedier/frp/
Run
```bash
frps -c ./frp-example/frps.toml
frpc -c ./frp-example/frpc.toml
```
Or run with docker

- Client:
https://hub.docker.com/r/snowdreamtech/frpc

- Server:
https://hub.docker.com/r/snowdreamtech/frps

#### Reference Links

1.Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

2.ids4 and owin: https://github.com/linianhui/example-oidc