# NameCheck

NameCheck is simple web API providing features to check for a **name availability on WHOIS and social networks**.
It helps choosing a name for your new project. A name which is available and ready to be used.

There are other better, more complete, more polished tools existing on the web (eg: [namechk.com](http://namechk.com/))

However for a contiguous project ([Project Igniter](https://github.com/PulsarBlow/projectigniter)) i needed this kind of features as a web api.
I didn't find any free one matching my needs, so here comes this project.

The API is hosted at https://namecheck.azurewebsites.net


## Configuration settings

This project is hosted on Azure Websites, so it uses the [CloudConfigurationManager Class](http://msdn.microsoft.com/en-us/library/microsoft.windowsazure.cloudconfigurationmanager.aspx) to load settings from the hosting environment configuration.

When running the site localy (development), upon the first build a default appSettings.config file is generated in the Configuration directory, if and only if doesn't exist yet.
This file is ignored by git (as per .gitignore configuration), so you can safely edit it with your config values.

#### Settings explained


``TwitterConsumerKey``
``TwitterConsumerSecret``
``TwitterAccessToken``
``TwitterAccessTokenSecret``
There are the standard Twitter API authentication values. Used to query Twitter for existing users.


``AuthorizationSecret``
This is a secret you define to block unwanted access to the Monitoring and Batch checks of this app.

``StorageConnectionString``
Azure Storage connection string. This is where we log checked names. Should be ``UseDevelopmentStorage=true`` while running this app localy.

``GandiApiKey``
Your [GANDI](http://gandi.net) API key. Used to query domains availabilities.

``CorsAllowOrigins``
``CorsAllowMethods``
``CorsAllowHeaders``
These are the [Cross-origin resource sharing](http://en.wikipedia.org/wiki/Cross-origin_resource_sharing) configuration exposed by the API endpoint.

``CacheDurationMin``
To release pressure on Gandi API and Twitter API rate limits we cache name checks for a given duration.
