using HackYeah_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HackYeah_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public DbSet<Rcb> Rcbs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rcb>().HasData(

            );
        }
    }
}