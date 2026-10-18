# FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
# WORKDIR /src

# COPY *.csproj .
# RUN dotnet restore

# COPY . .
# RUN dotnet publish -c release -o /app

# FROM mcr.microsoft.com/dotnet/aspnet:10.0
# WORKDIR /app
# COPY --from=build /app/publish .

# ENTRYPOINT [ "dotnet", "HBookingWeb.dll" ]


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["HBooking.slnx", "./"]
COPY ["HBook.Application/HBooking.Application.csproj", "HBook.Application/"]
COPY ["HBook.Domain/HBooking.Domain.csproj", "HBook.Domain/"]
COPY ["HBook.Infrastructure/HBooking.Infrastructure.csproj", "HBook.Infrastructure/"]
COPY ["HBookWeb/HBookingWeb.csproj", "HBookWeb/"]

RUN dotnet restore "HBooking.slnx"

COPY . .
RUN dotnet publish "HBookWeb/HBookingWeb.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HBookingWeb.dll"]