using IHK.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using IHK.DB.SeedBuilder;

namespace IHK.DB
{
    public class DataContext : DbContext
    {
        private IConfigurationRoot _config;

        public DataContext(IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            _config = config;
        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (_config != null)
            {
                optionsBuilder.UseSqlServer(_config["ConnectionStrings:DefaultConnection"]);
            }
        }

        public DbSet<Mieter> Mieters { get; set; }
        //public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RoleToUser> RoleToUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LayoutTheme> LayoutThemes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleToUser>().HasKey(t => new { t.UserId, t.RoleId });
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.Role).WithMany(r => r.RoleToUsers).HasForeignKey(rtu => rtu.RoleId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.User).WithMany(r => r.RoleToUsers).HasForeignKey(rtu => rtu.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LayoutTheme>().ToTable("LayoutTheme").HasKey(lt => lt.Id);
            modelBuilder.Entity<User>().ToTable("User").HasOne(u => u.LayoutTheme);
            modelBuilder.Entity<Mieter>().ToTable("Mieter");

        }
    }
}
