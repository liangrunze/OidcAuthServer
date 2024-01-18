#  OpenID Connect (OIDC) 服务端

[README](README.md) | [中文文档](READMEZH.md)

## 介绍

- 这个项目是一个使用 .NetCore 实现的 OpenID Connect (OIDC) 服务端。它可以帮助你快速地在你的应用中集成 OIDC 认证。
- 此项目初始目的为配合FRP进行Oidc认证，Frp配置可参见下方：Frp配置示例
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
然后，访问 http://ip/.well-known/openid-configuration来查看oidc的配置信息

### 环境变量
监听https需添加环境变量ENABLE_SSL，并且将certificate文件夹拷贝至服务运行目录（文件中提供了自生成证书和密钥，不推荐用于正式环境），此时服务会监听443和80端口
```bash
set ENABLE_SSL=True
dotnet run
```
####调试
如遇到配置连接等问题，可设置环境变量 ASPNETCORE_ENVIRONMENT来打开调试信息，这时打开http://ip/会显示debug信息
同时可查看控制台输出内容，可配合检查问题
```bash
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```
## Docker

Docker 编译
```bash
docker build -t oidc-server:1.0.0 .
```
此时会生成oidc-server的镜像

快速启动一个oidc服务
```bash
docker run --restart=always -p 80:80  -d  --name oidc-server oidc-server:1.0.0
```
你可以在浏览器中访问 http://ip/.well-known/openid-configuration来查看oidc的配置信息。

### 自定义配置
新建文件 appsettings.json，将仓库中的appsettings.json文件拷贝并修改
```bash
docker run --restart=always -p 80:80  -d -v /you-appsetting-path/appsettings.json:/app/appsettings.json  --name oidc-server oidc-server:1.0.0
```
### 开启调试
这时打开http://ip/会显示debug信息
同时可运行docker logs dockerid，可配合检查问题
```bash
docker run --restart=always -p 80:80  -d -e ASPNETCORE_ENVIRONMENT=Development --name oidc-server oidc-server:1.0.0
```


### 启动HTTPS\http
如果想要只启动HTTP，可设置ENABLE_SSL=false
使用下方命令可启动HTTPS端口，需要将你的证书挂载至指定目录（只支持pfx格式，同时证书名称和密钥文件名称不可修改）
```bash
docker run --restart=always -p 80:80 -p 443:443 -d -e ENABLE_SSL=True -v /you-cert-path/cert:/app/certificate --name oidc-server oidc-server:1.0.0
```

## Frp配置示例

```bash
# frps.toml
#完整配置：
#https://github.com/fatedier/frp/blob/dev/conf/frps_full_example.toml
auth.method = "oidc"
auth.oidc.issuer = "https://localhost:80" #你的oidc-server地址 请注意：结尾 "/" 需要和appsettings.json 中“IssuerUri”保持一致
auth.oidc.audience = "api" #可修改
auth.oidc.skipExpiryCheck = false
auth.oidc.skipIssuerCheck = false
```
```bash
# frpc.toml
#完整配置：
#https://github.com/fatedier/frp/blob/dev/conf/frpc_full_example.toml
auth.method = "oidc"
auth.oidc.clientID = "B90B1793-332A-49E4-8658-5A71503046B8" # Replace with OIDC client ID
auth.oidc.clientSecret = "c4ba5449-b604-49e1-9c8d-b3adb73cf566"
auth.oidc.audience = "api" #可修改
auth.oidc.scope ="api-1"
auth.oidc.tokenEndpointURL = "https://localhost:80/connect/token"#你的oidc-server地址，/connect/token是固定的
```
### frp 运行示例 
frp 获取：https://github.com/fatedier/frp/
运行
```bash
frps -c ./frp-example/frps.toml
frpc -c ./frp-example/frpc.toml
```
或使用docker运行

- 客户端：
https://hub.docker.com/r/snowdreamtech/frpc

- 服务端：
https://hub.docker.com/r/snowdreamtech/frps
#### 参考链接

1.Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

2.ids4 and owin: https://github.com/linianhui/example-oidc
