#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /
COPY ["./PaymentGateway.BankSimulator/PaymentGateway.BankSimulator.csproj", "PaymentGateway.BankSimulator/"]
RUN dotnet restore "PaymentGateway.BankSimulator/PaymentGateway.BankSimulator.csproj"
COPY . .
WORKDIR "/PaymentGateway.BankSimulator"
RUN dotnet build "PaymentGateway.BankSimulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentGateway.BankSimulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentGateway.BankSimulator.dll"]