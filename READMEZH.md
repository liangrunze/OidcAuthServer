#  OpenID Connect (OIDC) 服务端

[README](README.md) | [中文文档](READMEZH.md)

## 介绍

- 这个项目是一个使用 .NetCore 实现的 OpenID Connect (OIDC) 服务端。它可以帮助你快速地在你的应用中集成 OIDC 认证。
- 此项目初始目的为配合FRP进行Oidc认证
## 环境要求

- .NET Core 3.1 

## 安装

首先，你需要克隆这个仓库到你的本地环境：

```bash
git clone https://github.com/liangrunze/OidcAuth.git
```
然后，进入项目目录并使用 .NET CLI 进行构建：
```bash
cd ./OidcAuthServer
dotnet build
```
## 配置和运行
你需要在 appsettings.json 文件中设置你的数据库连接字符串和 OIDC 参数。

```JSON
{
  "IssuerUri": "https://localhost",//issuer地址，需要和外部访问地址一致
  "OidcClients": [
    {
      "ClientWebHomepage": "", //Oidc授权客户端主页
      "ClientId": "B90B1793-332A-49E4-8658-5A71503046B8", //客户端id，可自定义
      "ClientName": "frpc-proxy", //客户端名称，可自定义
      "AllowedGrantTypes": "client_credentials", //允许的授权类型
      "RequireConsent": false,
      "AllowedScopes": [ "profile", "openid", "email", "api-1" ], //允许访问的域
      "FrontChannelLogoutSessionRequired": true,
      "RequirePkce": false,
      "ClientSecrets": "c4ba5449-b604-49e1-9c8d-b3adb73cf566", //客户端的Secrets，注册的客户端需要保持一致
      "OidcLoginCallback": "/oidc/login-callback",
      "OidcFrontChannelLogoutCallback": "/oidc/front-channel-logout-callback"
    }
  ]
}
```
快速运行
你可以使用以下命令来运行你的应用：
```bash
dotnet run
```
然后，你可以在浏览器中访问 http://ip/ 来查看你的应用。访问 http://ip/.well-known/openid-configuration来查看oidc的配置信息

### 环境变量
监听https需添加环境变量ENABLE_SSL，并且将certificate文件夹拷贝至服务运行目录（文件中提供了自生成证书和密钥，不推荐用于正式环境），此时服务会监听443和80端口
```bash
set ENABLE_SSL=True
dotnet run
```
####调试
如遇到配置连接等问题，可设置环境变量 ASPNETCORE_ENVIRONMENT来打开调试信息，这是打开http://ip/会显示debug信息
同时可查看控制台输出内容，可配合检查问题
```bash
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```
## Docker


#### 参考链接

1.Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

2.ids4 and owin: https://github.com/linianhui/example-oidc
