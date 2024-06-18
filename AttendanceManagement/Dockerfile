FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AttendanceManagement/AttendanceManagement.csproj", "AttendanceManagement/"]
RUN dotnet restore "AttendanceManagement/AttendanceManagement.csproj"
COPY . .
WORKDIR "/src/AttendanceManagement"
RUN dotnet build "AttendanceManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AttendanceManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AttendanceManagement.dll"]
