using Microsoft.EntityFrameworkCore;

namespace D.Data;

public class DC(DbContextOptions<DC> options) : DbContext(options)
{
    public DbSet<SS> Sessions => Set<SS>();
}