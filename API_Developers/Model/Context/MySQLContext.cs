using Microsoft.EntityFrameworkCore;

namespace API_Developers.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext() { }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
        public DbSet<Developer> Developers { get; set; }

    }
}
