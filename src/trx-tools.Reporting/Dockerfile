# Setup base image
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS base

LABEL maintainer="BrammyS <https://github.com/BrammyS>"
LABEL org.label-schema.name="trx-tools.Reporting"
LABEL org.label-schema.description="A simple tool to generate reports from a TRX files."
LABEL org.label-schema.url="https://github.com/BrammyS/trx-tools"

# Setup workspace
WORKDIR /src
COPY /src .
WORKDIR /src/trx-tools.Reporting

# Restore dependencies
RUN dotnet restore "trx-tools.Reporting.csproj" --runtime linux-musl-x64

# Publish the application
RUN dotnet publish "trx-tools.Reporting.csproj" \
    -c Release -o /app/publish \
    --no-restore \
    --runtime linux-musl-x64

# Setup final runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-alpine AS final

# Create a user to run the application
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app

# upgrade musl to remove potential vulnerability
RUN apk upgrade musl

USER dotnetuser
WORKDIR /app
COPY --from=base /app/publish .

RUN chmod +x trx-tools.Reporting
ENTRYPOINT ["/app/trx-tools.Reporting"]