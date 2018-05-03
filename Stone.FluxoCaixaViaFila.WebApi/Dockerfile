FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Stone.FluxoCaixaViaFila.sln ./
COPY Stone.FluxoCaixaViaFila.WebApi/Stone.FluxoCaixaViaFila.WebApi.csproj Stone.FluxoCaixaViaFila.WebApi/
COPY Stone.FluxoCaixaViaFila.Domain/Stone.FluxoCaixaViaFila.Domain.csproj Stone.FluxoCaixaViaFila.Domain/
COPY Stone.FluxoCaixaViaFila.Infra.MQ/Stone.FluxoCaixaViaFila.Infra.MQ.csproj Stone.FluxoCaixaViaFila.Infra.MQ/
COPY Stone.FluxoCaixaViaFila.Common/Stone.FluxoCaixaViaFila.Common.csproj Stone.FluxoCaixaViaFila.Common/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Stone.FluxoCaixaViaFila.WebApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Stone.FluxoCaixaViaFila.WebApi.dll"]
