#URL Shortener
This repository contains a basic implementation of the URL shortening service.
Using this service users can make long links shorter and simpler. Also, it is possible to view the list of shortened URLs with basic information about them. 

This repository consists of two parts: API and UI projects.

##### You can find this project is live here: https://urlshortener.netlify.app/

###Concept
The concept is simple. API consists of two separate parts: a controller that handles unique keys generation (these keys are used to match the original URLs) and a controller that handles short URLs creation/resolving/querying.

The main goal of the unique keys generation controller is to create enough unique keys for matching them with original URLs and maintain a data retention policy - delete used keys and obsolete shortened URLs. This API could be triggered with something like Azure WebJobs on some basis.

The other controller is responsible for creating/querying/resolving shortened URLs.
During the creation or resolving of shortened URLs data is put into the cache for some period of time for quicker access and resolution in the future. Also, a small portion of unique keys is always present in cache, that also helps to keep perfomance higher.

Updating information (redirects count and last redirect date) of shortened URLs implemented using message broker. This keeps process of URLs resolving as quick as possible, because we just need to redirect to URL we took from cache, after this messages consumer is responsible for cache and DB update.

###Demo
Please see the gif below to get a basic understanding of this app.
![Demo](https://media.giphy.com/media/1NVudWnCLNq8Ko3Ymu/giphy.gif)

###Technologies used
API is built using ASP.NET Core 5.0, with Redis as a cache provider, Cosmos DB as a database, and RabbitMQ as a message broker.
UI is built using React and MUI React UI library.
Some parts of the application are deployed to Azure and some others to the different free services.

###How to run locally
To run this application locally you should have running instances of Redis Cache and RabbitMQ. Connection to CosmosDB is mocked with an in-memory database, so there is no need to have the running CosmosDB on your device.

After preparing the needed services you should update appsettings.Development.json with appropriate connection strings and values. After this, you can run UI with the command **npm start** and API as a regular C# project.

As a result, you will have UI live on http://localhost:3000 and API live on https://localhost:5001. To get a Swagger description visit https://localhost:5001/swagger/index.html.

###Areas to improve
This project contains a lot of possible points of improvement. Some of the essential points are: covering code with tests, adding security tokens to establish secure communication between API and UI, adding more detailed exceptions and errors handling, and adding more documentation. Also, good ideas would be to run performance testing and containerize this solution.
