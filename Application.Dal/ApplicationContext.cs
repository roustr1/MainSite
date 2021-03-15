using System;
using Application.Dal.Domain.Files;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.News;
using Application.Dal.Domain.Settings;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileBinary> FileBinary { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Setting>().HasData(
                new Setting[]
                {
                    new Setting
                    {
                        Id= Guid.NewGuid().ToString(),
                        Name = "Application.Icon",
                        Value = "/content/layout_icons/header.png"
                    },
                    new Setting
                    {
                        Id= Guid.NewGuid().ToString(),
                        Name = "Application.Name"
                        ,Value = ""
                    }
                    ,new Setting
                    {
                        Id= Guid.NewGuid().ToString(),
                        Name = "Application.Copy"
                        ,Value = ""
                    },
#if DEBUG
                    new Setting
                    {
                        Id=Guid.NewGuid().ToString(),
                        Name =  "Page.PageSize",
                        Value = 3.ToString()
                    }
#endif
#if RELEASE
new Setting
                    {
                        Id=Guid.NewGuid().ToString(),
                        Name =  "Page.PageSize",
                        Value = 10.ToString()
                    }
#endif
                });
        }
    }
}
