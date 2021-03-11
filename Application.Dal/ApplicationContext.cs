using System;
using Application.Dal.Domain.Files;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.News;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileBinary> FileBinary { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
