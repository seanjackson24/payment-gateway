#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /
COPY ["./PaymentGateway/PaymentGateway.csproj", "PaymentGateway/"]
COPY ["./PaymentGateway.Domain/PaymentGateway.Domain.csproj", "PaymentGateway.Domain/"]
COPY ["./PaymentGateway.Common/PaymentGateway.Common.csproj", "PaymentGateway.Common/"]
RUN dotnet restore "PaymentGateway/PaymentGateway.csproj"
COPY . .
WORKDIR "/PaymentGateway"
RUN dotnet build "PaymentGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentGateway.dll"]