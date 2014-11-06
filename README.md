# NameCheck

NameCheck is simple web API providing features to check for a **name availability on WHOIS and social networks**.  
It helps choosing a name for your new project. A name which is available and ready to be used.  

There are other better, more complete, more polished tools existing on the web (eg: [namechk.com](http://namechk.com/)).

However for a contiguous project ([Project Igniter](https://github.com/PulsarBlow/projectigniter)) i needed this kind of features as a web api.  
I didn't find any free one matching my needs, so here comes this project.

The API is hosted at https://namecheck.azurewebsites.net


## Configuration settings

This project is hosted on Azure Websites, so it uses the [CloudConfigurationManager Class](http://msdn.microsoft.com/en-us/library/microsoft.windowsazure.cloudconfigurationmanager.aspx) to load settings from the hosting environment configuration.

When running the site localy (development), upon the first build a default appSettings.config file is generated in the Configuration directory, if and only if doesn't exist yet.
This file is ignored by git (as per .gitignore configuration), so you can safely edit it with your config values.

#### Settings explained

``GoogleAppId``  
``GoogleAppSecret``  
MVC Authentication is plugged on Google OAuth2 via Owin. You need to create a new app in the google developper console, active Google Plus API, fill the consent page and create a new set of API credentials.

``AuthorizedEmails``  
After being authenticated with Google OAuth2, users have to be authorized. Based on this setting and the email provided by the Google OAuth2 authentication, authorized users can access advanced features of the web site (Batch mode, monitoring).
It's a coma separated list. You can use wildcard to provide authorization for an entire domain (eg: *@example.com)

``AuthorizedApiToken``  
Name check batch mode via API requires a token base authentication. Define this token in this setting.

``TwitterConsumerKey``  
``TwitterConsumerSecret``  
``TwitterAccessToken``  
``TwitterAccessTokenSecret``  
There are the standard Twitter API authentication values. Used to query Twitter for existing users.

``GandiApiKey``  
Your [GANDI](http://gandi.net) API key. Used to query domains availabilities.

``StorageConnectionString``  
Azure Storage connection string. This is where we log checked names. Should be ``UseDevelopmentStorage=true`` while running this app localy.

``CorsAllowOrigins``  
``CorsAllowMethods``  
``CorsAllowHeaders``  
These are the [Cross-origin resource sharing](http://en.wikipedia.org/wiki/Cross-origin_resource_sharing) configuration exposed by the API endpoint.

``CacheDurationMin``  
To release pressure on Gandi API and Twitter API rate limits we cache name checks for a given duration.
