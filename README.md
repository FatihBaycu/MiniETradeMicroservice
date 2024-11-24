
# MiniETrade Microservice

  

This repository contains the **MiniETrade Microservice** project, which includes multiple microservices for managing a simple e-commerce system. The services interact with each other through Dockerized environments and use various databases like PostgreSQL, MSSQL, and MongoDB.

  

## Table of Contents

1. [Project Overview](#project-overview)

2. [Technologies Used](#technologies-used)

3. [Prerequisites](#prerequisites)

4. [Database Setup](#database-setup)

	- [PostgreSQL Setup](#postgresql-setup)

	- [MSSQL Setup](#mssql-setup)

	- [MongoDB Setup](#mongodb-setup)

5. [Microservices Setup](#microservices-setup)

	- [Network Creation](#network-creation)

	- [Products API](#products-api)

	- [Carts API](#carts-api)

	- [Orders API](#orders-api)

7. [Running the Project](#running-the-project)

8. [Notes](#notes)

## Project Overview

  

MiniETrade is a modular e-commerce project built using microservices architecture. The services include:

-  **Gateway API**: Acts as a reverse proxy to route requests to appropriate services.

-  **Products API**: Manages product-related operations.

-  **Carts API**: Handles shopping cart operations.

-  **Orders API**: Processes orders.

-  **Users Database**: Manages user-related data.

  

## Technologies Used

  

-  **C# .NET Core** for microservices

-  **Docker** for containerization

-  **PostgreSQL, MSSQL, and MongoDB** for database storage

-  **ASP.NET Core** for RESTful APIs

  

## Prerequisites

  

- Docker Desktop installed on your system.

- PostgreSQL and MongoDB clients (optional, for manual database interactions).

- MSSQL tools like Azure Data Studio (optional). 

## Database Setup

  

### PostgreSQL Setup

1. Run PostgreSQL container for **shopping carts** and **users**:

``docker run --name postgres -e POSTGRES_PASSWORD=1  -e  POSTGRES_DB=shoppingcartsdb  -d  -p  5432:5432  postgres ``  

### MSSQL Setup

1. Run PMSSQL container for **products**:
``docker run -d -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Udemy#123" -e "MSSQL_PID=Evaluation" -p 1433:1433 --name sqlpreview --hostname sqlpreview mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04``
 
### MongoDB Setup

1. Run MongoDB container for **orders**:

``docker pull mongo``
``docker run -d --name mongodb --network my-network -p 27017:27017 -v mongodbdata:/data/db mongo``

## Microservices Setup

### Network Creation
1. Create a Docker network for the microservices: **Network**:
``docker network create my-network`` 
 

### Building and Running Microservices

1.  **Gateway API**:    
``docker build -t gateway .``
``docker run -d --name gateway-api --network my-network -p 5000:7979 gateway``
2.  **Products API**:    
 ``docker build -t products-api . ``
``docker run -d --name products-api --network my-network -p 5001:8080 products-api``
    
3.  **Carts API**:    
``docker build -t carts-api .``
``docker run -d --name carts-api --network my-network -p 5002:8082 carts-api``
    
4.  **Orders API**:   
``docker build -t orders .``
``docker run -d --name orders --network my-network -p 5003:8081 orders``

## Running the Project

1.  Ensure all services are running:
    
    -   Gateway API: `http://localhost:5000:7979`
    -   Products API: `http://localhost:5001:8080`
    -   Carts API: `http://localhost:5002:8082`
    -   Orders API: `http://localhost:5003:8082`
    -   MongoDB: `http://localhost:27017:27017`
    -   PostgreSQL: `http://localhost:5432:5432` (Shopping Carts), `http://localhost:5432:5432` (Users)
    -   MSSQL: `http://localhost:1433:1433`
2.  Use tools like Postman to interact with the APIs and test functionality. [Postman Collection here.](https://github.com/FatihBaycu/MiniETradeMicroservice/blob/main/External/MiniETradeMicroservice.postman_collection.json)

## Notes

-   I did this project with Taner Saydam's [Microservice Architecture Learn (Beginner level)](https://www.udemy.com/course/microservice-architecture-ogrenelim/) course on Udemy.
