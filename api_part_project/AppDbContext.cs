using Microsoft.EntityFrameworkCore;

namespace api_part_project
{
    public class AppDbContext : DbContext
    {
        public DbSet<Trader> Traders { get; set; }
        public DbSet<Trade> Trades { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string con = "server=mysqlstudenti.litv.sssvt.cz+database=[jméno databáze];username=zemantomas;password=123456";
            optionsBuilder.UseMySQL(con);
        }
    }
}

