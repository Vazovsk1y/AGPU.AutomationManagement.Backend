FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN groupadd -r appgroup &&  \
    useradd -r -g appgroup -d /home/appuser -m appuser

WORKDIR /app
EXPOSE 5555

RUN mkdir -p /app/data-protection-keys &&  \
    chown -R appuser:appgroup /app/data-protection-keys

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AGPU.AutomationManagement.WebApi/AGPU.AutomationManagement.WebApi.csproj", "src/AGPU.AutomationManagement.WebApi/"]
COPY ["src/AGPU.AutomationManagement.Application/AGPU.AutomationManagement.Application.csproj", "src/AGPU.AutomationManagement.Application/"]
COPY ["src/AGPU.AutomationManagement.DAL.PostgreSQL/AGPU.AutomationManagement.DAL.PostgreSQL.csproj", "src/AGPU.AutomationManagement.DAL.PostgreSQL/"]
COPY ["src/AGPU.AutomationManagement.Domain/AGPU.AutomationManagement.Domain.csproj", "src/AGPU.AutomationManagement.Domain/"]
RUN dotnet restore "src/AGPU.AutomationManagement.WebApi/AGPU.AutomationManagement.WebApi.csproj"
COPY . .
WORKDIR "/src/src/AGPU.AutomationManagement.WebApi"
RUN dotnet build "AGPU.AutomationManagement.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AGPU.AutomationManagement.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN chown -R appuser:appgroup /app
USER appuser

ENTRYPOINT ["dotnet", "AGPU.AutomationManagement.WebApi.dll"]
