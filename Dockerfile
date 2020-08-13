FROM microsoft/dotnet:latest
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "ProductManagementApi.dll"]