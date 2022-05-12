# JanuszPOL Bet API
# Conventions
* NEVER merge to `main` branch!!!! Create new one from `main`, do your changes, push, create pull request, let know other devs, after review merg to `main` with squash
* table names in singular form, eg. account table name is `Account`
* tables primary key consist of table name and `Id` with `long` type , eg. Account table has PK with name: `AccountId` 
* all string columns has length constraint (resonable legth)
* services do not use DbContext, all data requests are done in repositories
# Installation
1. Install VS Studio 2022
2. Install Entity Framework Core Tools: `dotnet tool install --global dotnet-ef`
3. TODO: run migrations
4. TODO