# Library Management API

[DotNet]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[DotNet-url]: https://learn.microsoft.com/en-us/dotnet/?WT.mc_id=dotnet-35129-website
[Postgres]: https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white
[Postgres-url]: https://www.postgresql.org/docs/
[React]: https://img.shields.io/badge/react-%2320232a.svg?style=for-the-badge&logo=react&logoColor=%2361DAFB
[React-url]: https://reactjs.org/

 [![Dotnet]][DotNet-url]  [![Postgres]][Postgres-url] [![React]][React-url]

## Table of Contents
- [Description](#description)
  - [About application](#about-application)
  - [Built with](#built-with)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Run the project](#run-the-project)
- [Using the App](#using-the-app)

## Description
### About application
This application is sample online sports betting application. It has following functionalities:
  - generating sports offer for the day
  - paying in tickets
  - adding top offer
  - wallet transactions


### Built with

- **Framework** - [ASP.NET](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-7.0)
- **Database** - [PostgreSQL](https://www.postgresql.org/docs/) 
- **ORM** - [EntityFrameworkCore](https://github.com/dotnet/efcore)
- **Frontend** - [React](https://reactjs.org/)

## Getting Started

### Prerequisites
- install [dotnet SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- install [PostgreSQL](https://www.postgresql.org/download/) 
### Run the project

1. Clone GitHub repository:
   ```
   git clone https://github.com/vicemilin/betting-appliaction.git
   ```

#### Start backend application

1. open ```VM.BettingApplication``` folder

2. create `sport` schema in your PG database

3. run ```dotnet restore``` to fetch Nuget packages 

4. run ```dotnet build``` to build the solution 

5. run migrations using: 
   ``` 
   dotnet run --project VM.BettingApplication.Migrations/VM.BettingApplication.Migrations.csproj d="Host=localhost;Port=5432;Password=postgres;Username=postgres;Database=postgres"
   ```

6. run API using
   ``` 
    dotnet run --project VM.BettingApplication.API/VM.BettingApplication.API.csproj   
   ```
    

#### Start frontend application

1. Open ```app``` folder

2. Install all packages:
   ```
   npm install --legacy-peer-deps
   ```
  - ```--legacy-peer-deps``` is used because ``` @mui/styles``` has not updated dependency list for ```react v18```

3. Start the project
    ```
    npm run start
    ```
​
## Using the App

API description can be found in Swagger UI page after backend application is started by opening
http://localhost:5242/swagger

You can add funds to user wallet by calling 
`POST api/Wallet/AddTransaction` endpoint and providing following body:
```
{
  "transactionId": "<random uuid>",
  "amount": <amount you want to deposit>,
  "type": 1 (1 is credit trasaction type and 0 is debit transaction type)
}
```

You can add an event to top offer by calling 
`POST api/Offer/AddToTopOffer/<id of event you want to add to top offer>`.

Application settings are contained in `VM.BettingApplication.API/appsettings.json` 
```
  "ApplicationSettings": {
    "DatabaseConnectionString": "Host=localhost;Port=5432;Password=postgres;Username=postgres;Database=postgres",
    "NumberOfEventsPerSportPerDay": 5,
    "TopOfferOddsBoostPercentage": 5,
    "MinPayinAmount": 0.5,
    "MaxEventsFromTopOffer": 1,
    "MinimumOddsWithTopOffer": 1.10,
    "MinimumEventsWithTopOffer": 5
  }
```
Here you can edit database connection string and betting settings (betting settings should probably be moved to a database table at some point).