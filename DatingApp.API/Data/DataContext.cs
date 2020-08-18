using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }                               // Calling parents contructor that has options

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}