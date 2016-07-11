# SociolRoom Analytics
Sociol Room is a webiste displaing in a wall the cognitive analysis result of pictures posted in Twitter and Instagram with a specific hashtag.

#Before starting
The website is designed to run on Microsoft Azure.
The steps to run the app are:

##Azure provisionning
* Open an Azure account
* Create cognitive service for Vision, Emotion, Face https://www.microsoft.com/cognitive-services/en-us/apis
* Creat an Bing Translator app https://www.microsoft.com/en-us/translator/getstarted.aspx
* Create a Document DB database
* Create an Azure service bus
* Create a storage
* Create an app service and a web site
* Enable Always On https://azure.microsoft.com/en-us/documentation/articles/web-sites-configure/
* Create a Twitter app https://dev.twitter.com/

##Configuration
* Clone the repository
* Create on the root folder a .config with 2 files secretAppSettings.config and secretConnectionStrings.config

secretAppSettings.config
* Replace with your keys
<?xml version="1.0" encoding="utf-8"?>
<appSettings>
  <add key="Microsoft.ServiceBus.ConnectionString" value="Endpoint=sb://[ENDPOINT];SharedAccessKeyName=[KeyName];SharedAccessKey=[Key]"/>
  <add key="DocumentDBUrl" value="" />
  <add key="DocumentDBPrimaryKey" value="" />
  <add key="Cognitive:VisionAPIKey" value="" />
  <add key="Cognitive:EmotionAPIKey" value="" />
  <add key="Cognitive:FaceAPIKey" value="" />
  <add key="Bing:TranslatorClientID" value="" />
  <add key="Bing:TranslatorClientSecret" value="" />
  <add key="Twitter:ConsumerKey" value="" />
  <add key="Twitter:ConsumerSecret" value="" />
  <add key="Twitter:AccessToken" value="" />
  <add key="Twitter:AccessTokenSecret" value="" />
</appSettings>

secretConnectionStrings.config
<?xml version="1.0" encoding="utf-8"?>
<connectionStrings>
  <add name="AzureWebJobsDashboard" connectionString="DefaultEndpointsProtocol=https;AccountName=[Account];AccountKey=[Key]" />
  <add name="AzureWebJobsStorage" connectionString="DefaultEndpointsProtocol=https;AccountName=[Account]AccountKey=[Key]" />
  <add name="AzureWebJobsServiceBus" connectionString="Endpoint=sb://[ENDPOINT];SharedAccessKeyName=[KeyName];SharedAccessKey=[Key]" />
</connectionStrings>

Enjoy !
