using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<ProfilePhoto> ProfilePhotos { get; set; }

		public DbSet<Address> Addresses { get; set; }

		public DbSet <Role> Roles { get; set; }

		public DbSet<UserRole> UserRoles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            // Priority Fluent API > Conventions

            // Fluent API configuration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
