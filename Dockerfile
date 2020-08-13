# Build runtime image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
ENTRYPOINT ["dotnet", "ProductManagementApi.dll"]