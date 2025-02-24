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
}