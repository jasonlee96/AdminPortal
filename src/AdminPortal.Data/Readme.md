# To Execute a migration action

# -s -> the Startup project target (which contains the connection string for the DB)

dotnet ef migration add <MigrationName> -s ../AdminPortal.Api/AdminPortal.Api.csproj

# To apply the migration into DB

dotnet ef database update