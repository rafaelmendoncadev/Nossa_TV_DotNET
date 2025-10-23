This folder will contain EF Core migrations. Migrations are created with `dotnet ef migrations add Initial` and applied with `dotnet ef database update`.

If you run the app, Program.cs will call `context.Database.MigrateAsync()` automatically to apply pending migrations.
