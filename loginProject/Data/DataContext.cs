using loginProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace loginProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer("Server=Lenovo-ideapad\\SQLEXPRESS;Database=loginProject;Trusted_Connection=True;Encrypt=False");
        }
        public DbSet<Users> users => Set<Users>();

    }
}
