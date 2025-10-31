using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence
{
	public class AppDbContext : IdentityDbContext
	{
		public DbSet<Date> Dates { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<List> Lists { get; set; }
		public DbSet<ItemDate> ItemDates { get; set; }
		public AppDbContext(DbContextOptions options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Date>(e =>
			{
				e.HasKey(k => k.Id).HasName("DateId");
				e.Property(p=>p.date).HasColumnType("Date").IsRequired();
			});
			modelBuilder.Entity<Item>(e => {
				e.HasKey(k => k.Id).HasName("ItemId");
				e.Property(p => p.Name).HasMaxLength(50);
				e.Property(p => p.Description).HasMaxLength(150);
				e.HasOne(p => p.User).WithMany(p => p.Items);

			});
			modelBuilder.Entity<List>(e => {
				e.HasKey(k => k.Id).HasName("ListId");
				e.HasOne(p => p.User).WithMany(p => p.Lists);
				e.Property(p => p.Name).HasMaxLength(50);
				e.Property(p => p.Description).HasMaxLength(150);

			});
			modelBuilder.Entity<ItemDate>(e => {
				e.HasKey(e => e.Id); // مفتاح مستقل
				e.HasOne(e => e.Date).WithMany(e=>e.ItemDates).HasForeignKey(e=>e.DateId);
				e.HasOne(e => e.Item).WithMany(e => e.ItemDates).HasForeignKey(e => e.ItemId);
				
			});

		}


	}
}
