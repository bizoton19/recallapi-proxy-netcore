FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

#copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

#Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "recallMicroservice.dll"]