# Truestory_WebAPI
This API is built on .Net 8 with the following implementation:
MediatR archiecture
cache mechanism
UnitTesting
Logging
HTTPClient
Swagger
Policy
Pagination

# Decision Process
The current architecture approach is called compnent based. Most solutions in the project ought to have their library eg Business logic, third party integration, internal integration etc for re-usability purposes
Using Mediator design for the API is due to the application of SOLID principle and scalability.
Pagination feature was added to GetListObjectsQuery because this is capable to slowing down the entire server if a large dataset is returned at once.
The use of cache is to improve the speed performance as the data can get really large as time goes on.
Policy was introduced to fix network issues, since we are dealing with external API.
Serilog is used to ensure every request and response data are traceable in case of crash or fatal error.
Swagger is the default documentation tool used for this project.

# Running
Running this app is very easy if you are using visual studio 2022 or any other microsoft supported IDEs. 

# Testing
This API comes with swagger, a documentation tool and testing of API endpoints. Every endpoints have a well documentation of what to expect from the request and how the response will be.



# Written Task
The following are the ways I will approach such a project:

## Business requirement
It is always very much important to understand the business requirement that brought about the codebase and logic. Understanding the business perspective will prepare you to know the why, how, when of the codebase.

## Access the codebase
I will go through the documentation of the codebase to understand the poor coding structure, logic flow, outdated libraries etc
I will go through the codebase (Audit) to review the bottleneck, architecture, coding practices etc

## Priority
I will focus on modules/features that are very important to the business
Deal with small compinents in this critical modules to avoid code break

## Test
For every changes there must be a test to prevent regression

## Introduce modern framework and design principles
We have SOLID princples, MVC to decouple the entire system and improve performance 
We have .Net 8 that is highly efficient and can be introduce gradually

## Agile, DevOps and CI/CD
I will introduce agile methodogies for documentation, DevOps and CI/CD for easy of deployment, version control etc





