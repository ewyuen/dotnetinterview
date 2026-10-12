using EFCoreDataAccess.Data;

namespace EFCoreDataAccess.Services;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _context;

    public DatabaseInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        // Ensure database is created
        await _context.Database.EnsureCreatedAsync();

        // Apply any pending migrations (if we had them)
        // await _context.Database.MigrateAsync();
    }
}