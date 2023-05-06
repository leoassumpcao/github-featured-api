#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GitHubFeatured.API/GitHubFeatured.API.csproj", "src/GitHubFeatured.API/"]
COPY ["src/GithubFeatured.Application/GithubFeatured.Application.csproj", "src/GithubFeatured.Application/"]
COPY ["src/GitHubFeatured.Domain/GitHubFeatured.Domain.csproj", "src/GitHubFeatured.Domain/"]
COPY ["src/GithubFeatured.Infra/GithubFeatured.Infra.csproj", "src/GithubFeatured.Infra/"]
RUN dotnet restore "src/GitHubFeatured.API/GitHubFeatured.API.csproj"
COPY . .
WORKDIR "/src/src/GitHubFeatured.API"
RUN dotnet build "GitHubFeatured.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GitHubFeatured.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GitHubFeatured.API.dll"]