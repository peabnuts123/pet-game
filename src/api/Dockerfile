# Builder container
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /app

# Copy project manifests
COPY PetGame.sln PetGame.sln
COPY PetGame.Common/PetGame.Common.csproj PetGame.Common/PetGame.Common.csproj
COPY PetGame.Web/PetGame.Web.csproj PetGame.Web/PetGame.Web.csproj
COPY PetGame.Business/PetGame.Business.csproj PetGame.Business/PetGame.Business.csproj
COPY PetGame.Data/PetGame.Data.csproj PetGame.Data/PetGame.Data.csproj
COPY PetGame.Config/PetGame.Config.csproj PetGame.Config/PetGame.Config.csproj

COPY PetGame.Common.Test/PetGame.Common.Test.csproj PetGame.Common.Test/PetGame.Common.Test.csproj
COPY PetGame.Business.Test/PetGame.Business.Test.csproj PetGame.Business.Test/PetGame.Business.Test.csproj

RUN dotnet --version

RUN dotnet restore

COPY . .

RUN dotnet publish --configuration Release

# Runtime container
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime

WORKDIR /app

RUN adduser -D petgame
USER petgame

COPY --from=build /app/PetGame.Web/bin/Release/netcoreapp3.1/* ./

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ENVIRONMENT_ID=docker

ENTRYPOINT ["dotnet", "PetGame.Web.dll"]

