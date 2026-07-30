FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Hotel.Reservation.Management/Hotel.Reservation.Management.API.csproj", "Hotel.Reservation.Management/"]
COPY ["Hotel.Reservation.Management.Infrastructure/Hotel.Reservation.Management.Infrastructure.csproj", "Hotel.Reservation.Management.Infrastructure/"]
COPY ["Hotel.Reservation.Management.Application/Hotel.Reservation.Management.Application.csproj", "Hotel.Reservation.Management.Application/"]
COPY ["Hotel.Reservation.Management.Domain/Hotel.Reservation.Management.Domain.csproj", "Hotel.Reservation.Management.Domain/"]

RUN dotnet restore "Hotel.Reservation.Management/Hotel.Reservation.Management.API.csproj"

COPY . .
WORKDIR "/src/Hotel.Reservation.Management"
RUN dotnet publish "Hotel.Reservation.Management.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Hotel.Reservation.Management.API.dll"]