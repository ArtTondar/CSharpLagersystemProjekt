# Steps to a successful migration:

## Prerequisites 
- The solution should build
- Models exist
- Connection string is configured
- Add Entity Framework Core (the default .NET ORM) via the NuGet Package Manager.
- Check versions match with .net core.
- run ` dotnet tool install --global dotnet-ef --version x.x` in powershell. 
- Check versions of dotnet tool and nuget package match: `dotnet-ef --version`


## Steps for the first time run

- Add YourName-DbContext class that inherits EntityFramework.DbContext
- Add YourName-DbContext to services: `builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));`
- Important: add a DbSet for each models that should be mirrored in the database. This represents the table and allows for basic CRUD operations through LINQ.

 
 
## Steps for initial and subsequent model changes

- Open powershell, navigate to the base of the project that contains the dbcontext and name the change: `dotnet ef migrations add MigrationName `. This generates migration files that describe the database schema changes.
- Apply the stored changes: `dotnet ef database update `
