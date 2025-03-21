using KubbAdminAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KubbAdminAPI;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    
    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<Participation> Participations { get; set; }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));
        
        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime
            ((BaseModel)entity.Entity).Updated = now;
        }
    }


}