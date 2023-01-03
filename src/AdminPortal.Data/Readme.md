# To Execute a migration action

# -s -> the Startup project target (which contains the connection string for the DB)

dotnet ef migrations add <MigrationName> -s ../AdminPortal.Api/AdminPortal.Api.csproj

# To apply the migration into DB

dotnet ef database update -s ../AdminPortal.Api/AdminPortal.Api.csproj

# To remove the migration

dotnet ef migrations remove -s ../AdminPortal.Api/AdminPortal.Api.csproj