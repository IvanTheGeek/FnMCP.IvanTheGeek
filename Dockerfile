# Multi-stage build for Nexus MCP
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /src

# Copy project files
COPY src/FnMCP.Nexus/FnMCP.Nexus.fsproj ./src/FnMCP.Nexus/
RUN dotnet restore src/FnMCP.Nexus/FnMCP.Nexus.fsproj

# Copy source code
COPY . .

# Build and publish
WORKDIR /src/src/FnMCP.Nexus
RUN dotnet publish -c Release -o /app/publish --self-contained true -r linux-musl-x64 /p:PublishSingleFile=true

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine

# Install ICU libraries for globalization support
RUN apk add --no-cache icu-libs

# Set globalization to use ICU
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app

# Copy the pre-built binary from host (this will be overridden by the publish output in practice)
# But we're also copying from the build stage to ensure we have the latest
COPY --from=build /app/publish .

# Expose the MCP HTTP port
EXPOSE 18080

# Set ASP.NET Core URLs
ENV ASPNETCORE_URLS=http://+:18080

# Run the application
ENTRYPOINT ["./FnMCP.Nexus"]
