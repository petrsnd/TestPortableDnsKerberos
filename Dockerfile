FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /TestPortableDnsKerberos

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /TestPortableDnsKerberos
COPY --from=build-env /TestPortableDnsKerberos/out .
ENTRYPOINT ["TestPortableDnsKerberos"]
