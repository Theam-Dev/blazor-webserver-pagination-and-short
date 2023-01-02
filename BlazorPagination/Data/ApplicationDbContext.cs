using BlazorPagination.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlazorPagination.Data
{
    
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
    }
}
