# JanuszPOL Bet API
# Conventions
* NEVER merge to `main` branch!!!! Create new one from `main`, do your changes, push, create pull request, let know other devs, after review merg to `main` with squash
* table names in singular form, eg. account table name is `Account`
* tables primary key are named `Id` with `long` type , eg. Account table has PK with name: `Id` 
* all string columns has length constraint (resonable legth)
* services do not use DbContext, all data requests are done in repositories
# Installation
1. Install VS Studio 2022
2. Install Entity Framework Core Tools: `dotnet tool install --global dotnet-ef`
3. TODO: run migrations
4. TODO

# Migrations
### Add migration example
```
 C:\...\JanuszPOL.JanuszPOLBets.Data> dotnet ef migrations add AddTeams --startup-project="../JanuszPOL.JanuszPOLBets.API/JanuszPOL.JanuszPOLBets.API.csproj"
 ```

# Production deploy

create .env file and fill contents with actual production data
```
ConnectionStrings__DefaultConnection="Server=db;Database=Bets;User=sa;Password=dbpassword1;"
JWT__ValidAudience="https://januszpol.eu"
JWT__ValidIssuer="https://api.januszpol.eu"
JWT__Secret="JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"
SA_PASSWORD="dbpassword1"
API_IMAGE="januszpol-bets-api:0.0.1"
FRONT_IMAGE="januszpol-bets-frontend:0.0.2"
ACME_EMAIL="jedrek@fijalkowscy.com"
FRONTEND_DOMAIN="januszpol.eu"
API_DOMAIN="api.januszpol.eu"
```
