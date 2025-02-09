using Microsoft.EntityFrameworkCore;

namespace api_part_project.Class
{
    public class AppDbContext : DbContext
    {
        public DbSet<Trader> Traders { get; set; }
        public DbSet<Trade> Trades { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string con = "server=mysqlstudenti.litv.sssvt.cz;database=4c2_zemantomas_db1;user=zemantomas;password=123456";
            optionsBuilder.UseMySQL(con);
        }
    }
}

