#  OpenID Connect (OIDC) Server

[README](README.md) | [中文文档](READMEZH.md)

## Introduction

- This project is an OpenID Connect (OIDC) server implemented using .NetCore. It can help you quickly integrate OIDC authentication into your application.
- The initial purpose of this project is to perform Oidc authentication in conjunction with FRP.
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
Then, you can visit http://ip/ in your browser to view your application. Visit http://ip/.well-known/openid-configuration to view the oidc configuration information.

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


#### Reference Links

1.Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

2.ids4 and owin: https://github.com/linianhui/example-oidc