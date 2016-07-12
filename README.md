# SociolRoom Analytics
Sociol Room is a webiste displaying in a wall the cognitive analysis results of pictures posted in Twitter and Instagram with a specific hashtag.
You can have a demo: http://sociolroom-analytics.azurewebsites.net

#Before starting
The website is designed to run on Microsoft Azure.
The steps to run the app are:

##Azure provisionning
* Open an Azure account
* Create cognitive service for Vision, Emotion, Face https://www.microsoft.com/cognitive-services/en-us/apis
* Create an Bing Translator app https://www.microsoft.com/en-us/translator/getstarted.aspx
* Create a Document DB database
* Create an Azure service bus
* Create a storage
* Create an app service and a web site
* Enable Always On https://azure.microsoft.com/en-us/documentation/articles/web-sites-configure/
* Create a Twitter app https://dev.twitter.com/

##Configuration
* Clone the repository
* Create a folder named .config on the root folder with 2 files: secretAppSettings.config and secretConnectionStrings.config
* Replace with your keys
* 
secretAppSettings.config
<pre>
&lt;?xml version="1.0" encoding="utf-8"?&gt;
&lt;appSettings&gt;
  &lt;add key="Microsoft.ServiceBus.ConnectionString" value="Endpoint=sb://[ENDPOINT];SharedAccessKeyName=[KeyName];SharedAccessKey=[Key]"/&gt;
  &lt;add key="DocumentDBUrl" value="" /&gt;
  &lt;add key="DocumentDBPrimaryKey" value="" /&gt;
  &lt;add key="Cognitive:VisionAPIKey" value="" /&gt;
  &lt;add key="Cognitive:EmotionAPIKey" value="" /&gt;
  &lt;add key="Cognitive:FaceAPIKey" value="" /&gt;
  &lt;add key="Bing:TranslatorClientID" value="" /&gt;
  &lt;add key="Bing:TranslatorClientSecret" value="" /&gt;
  &lt;add key="Twitter:ConsumerKey" value="" /&gt;
  &lt;add key="Twitter:ConsumerSecret" value="" /&gt;
  &lt;add key="Twitter:AccessToken" value="" /&gt;
  &lt;add key="Twitter:AccessTokenSecret" value="" /&gt;
&lt;/appSettings&gt;
</pre>
secretConnectionStrings.config
<pre>
&lt;?xml version="1.0" encoding="utf-8"?&gt;
&lt;connectionStrings&gt;
  &lt;add name="AzureWebJobsDashboard" connectionString="DefaultEndpointsProtocol=https;AccountName=[Account];AccountKey=[Key]" /&gt;
  &lt;add name="AzureWebJobsStorage" connectionString="DefaultEndpointsProtocol=https;AccountName=[Account]AccountKey=[Key]" /&gt;
  &lt;add name="AzureWebJobsServiceBus" connectionString="Endpoint=sb://[ENDPOINT];SharedAccessKeyName=[KeyName];SharedAccessKey=[Key]" /&gt;
&lt;/connectionStrings&gt;
</pre>
Enjoy !
