FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

WORKDIR /src
COPY . ./

RUN dotnet restore
RUN dotnet build -c Release --no-restore

FROM base as test

RUN dotnet test -c Release --no-build --no-restore --verbosity normal


FROM base as publish

RUN dotnet publish -c Release -o /out --no-restore ./JanuszPOL.JanuszPOLBets.API/JanuszPOL.JanuszPOLBets.API.csproj


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=publish /out .

EXPOSE 80
ENTRYPOINT ["dotnet", "JanuszPOL.JanuszPOLBets.API.dll"]
