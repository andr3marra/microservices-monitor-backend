# Microservice Monitor Backend [![.NET](https://github.com/andr3marra/microservices-monitor-backend/actions/workflows/dotnet.yml/badge.svg)](https://github.com/andr3marra/microservices-monitor-backend/actions/workflows/dotnet.yml)
Microservice Monitor Backend is a .NET RESTful API used for monitor services via health check. It can poll services healthchecks to store the status, and it can link a service dependency, listed in the healthcheck itself, to other service.

It can be used with Microservice Monitor Frontend, a React Js web server used to display all information. See [andr3marra/microservices-monitor-frontend](https://github.com/andr3marra/microservices-monitor-frontend) for more information.
---
## How it works
It's basically a service healthcheck information agregator.
1. Its a API where you can make POST requests to register a service that supports healthchecks.
2. In the background the service(microservices-monitor) will poll the healthcheck endpoint to obtain data.
3. The services registered information in the microservice-monitor can be accesed using a GET request.
## Getting Started
This API suports OpenAPI Specification via Swagger. It has CRUD endpoints for managing the services being monitored.
## Config
The listening port can be set in `properties/launchSettings.json`.