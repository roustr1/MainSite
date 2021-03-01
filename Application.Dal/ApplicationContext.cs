using System;
using Application.Dal.Domain.Menu;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem[]
                {
                    new MenuItem { 
                        Id=Guid.NewGuid().ToString(),
                        Name = "Home",
                        ActionName = "Index",
                        lastChangeAuthor = "Auto_Created",
                        IsActive = true,
                        LastChangeDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        ToolTip = "Домашняя страница",
                        URL = "home/index"
                    },
                    new MenuItem {
                    Id=Guid.NewGuid().ToString(),
                    Name = "Home1",
                    ActionName = "Index1",
                    lastChangeAuthor = "Auto_Created",
                    IsActive = true,
                    LastChangeDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    ToolTip = "Вторая страница",
                    URL = "home/index"
                    }
                });
        }
    }
}
