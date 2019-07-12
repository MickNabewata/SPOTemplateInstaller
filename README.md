# For Japanese 日本語

## 概要
当リポジトリで公開するツールは、PnPプロビジョニングテンプレートを手軽に利用できるようにするものです。  
ツールを使ってSharePointサイトの定義をXMLファイルにエクスポートしたり、  
それを別のサイトにインポートしたりすることができます。  
リポジトリ内には以下２つのツールがあります。
  - SPOTemplateInstaller : 画面上で操作できるWPFアプリケーションです。
  - SPOTemplateInstallBat : 画面の無いコンソールアプリケーションです。簡易的な実行や自動化に向いています。

## SPOTemplateInstaller

### ツールの概要

このツールを起動すると、最初にOffice 365テナントへのログインを求められます。  
ログインに成功すると、テナント内のSharePointサイトを検索し、一覧表示することができます。  
検索結果からサイトを１つ選択して、サイト定義をXMLファイルにエクスポートしたり  
サイトにXMLファイルをインポートしたりすることができます。  
  
![画面](https://cdn-ak.f.st-hatena.com/images/fotolife/m/micknabewata/20190217/20190217174310.png)
  
### インストール方法

[modules](https://github.com/MickNabewata/SPOTemplateInstaller/tree/master/SPOTemplateInstaller/modules)フォルダからビルド済モジュールをダウンロードして任意のフォルダに配置します。  
ダウンロードしたフォルダ内のSPOTemplateInstaller.exeを実行すると画面が起動します。

## SPOTemplateInstallBat

### ツールの概要

簡易的に実行したり、自動化を行う場合にはこちらをご利用ください。  

### インストール方法

[modules](https://github.com/MickNabewata/SPOTemplateInstaller/tree/master/SPOTemplateInstallBat/modules)フォルダからビルド済モジュールをダウンロードして任意のフォルダに配置します。

### 実行方法

コマンドラインからSPOTemplateInstallBat.exeを実行します。
  
    SET MODE="GET"
    SET SITE="https://contoso.sharepoint.com/sites/sample"
    SET ACCOUNT="user@contoso.onmicrosoft.com"
    SET PASSWORD="password"
    SET FOLDER="C:\work"
    SET FILE="sample.xml"
    
    SPOTemplateInstallBat.exe %MODE% %SITE% %ACCOUNT% %PASSWORD% %FOLDER% %FILE%

### パラメータ一覧

| 位置 | 説明 | 例 |
----|---- | ----
| 1 | 実行モードです。GET または APPLYを指定します。 | GET |
| 2 | サイトURLです。 | https://contoso.sharepoint.com/sites/sample |
| 3 | サイトにアクセスするためのアカウントです。 | user@contoso.onmicrosoft.com |
| 4 | 上記アカウントのパスワードです。 | password |
| 5 | XMLファイルの入出力先となるフォルダパスです。 | c:\work |
| 6 | XMLファイル名です。 | sample.xml |

# For English

## Overview
This repository tool helps you work with PnP Provisioning Template.  
You can export the definision of a SharePoint site to an xml file, and apply it to other sites.  
There are two tools in this repository.
 - SPOTemplateInstaller : WPF application with interface for operation
 - SPOTemplateInstallBat : Console application that has no interface and is easier to automate

## SPOTemplateInstaller

### About this tool

First, you need to log in to your office 365 tenant.  
And you can search your site.  
Select one site to export or import a PnP Provisioning Template file.  
  
![Interface](https://cdn-ak.f.st-hatena.com/images/fotolife/m/micknabewata/20190217/20190217174310.png)
  
### Instlation

Get modules form [modules](https://github.com/MickNabewata/SPOTemplateInstaller/tree/master/SPOTemplateInstaller/modules) folder.  
To start this tool, run SPOTemplateInstaller.exe.

## SPOTemplateInstallBat

### About this tool

If you need to automate or more easier, use this tool.  

### Instlation

Get modules form [modules](https://github.com/MickNabewata/SPOTemplateInstaller/tree/master/SPOTemplateInstallBat/modules) folder.

### Usage

In the command line, run SPOTemplateInstallBat.exe with some parameters.  
  
    SET MODE="GET"
    SET SITE="https://contoso.sharepoint.com/sites/sample"
    SET ACCOUNT="user@contoso.onmicrosoft.com"
    SET PASSWORD="password"
    SET FOLDER="C:\work"
    SET FILE="sample.xml"
    
    SPOTemplateInstallBat.exe %MODE% %SITE% %ACCOUNT% %PASSWORD% %FOLDER% %FILE%

### Parameters

| position | explain | example |
----|---- | ----
| 1 | Running mode. should be GET or APPLY. | GET |
| 2 | Site url. | https://contoso.sharepoint.com/sites/sample |
| 3 | Account used to access the site. | user@contoso.onmicrosoft.com |
| 4 | Password of the account. | password |
| 5 | Folder to read or write xml files. | c:\work |
| 6 | File name of the xml file. | sample.xml |
