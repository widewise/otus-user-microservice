FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Otus.Microservice.User.csproj", "Otus.Microservice.User/"]
RUN dotnet restore "Otus.Microservice.User/Otus.Microservice.User.csproj"
WORKDIR "/src/Otus.Microservice.User"
COPY . .
RUN dotnet build "Otus.Microservice.User.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Otus.Microservice.User.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Otus.Microservice.User.dll"]
