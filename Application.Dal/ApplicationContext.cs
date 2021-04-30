using System;
using System.Security.Cryptography.X509Certificates;
using Application.Dal.Domain.Files;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.News;
using Application.Dal.Domain.Permissions;
using Application.Dal.Domain.Settings;
using Application.Dal.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal
{
    public class ApplicationContext : DbContext
    {
         
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<FileBinary> FileBinary { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<PinNews> PinnedNews { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<UserRole> Roles { get; set; }
        public DbSet<PermissionRecord> Permissions { get; set; }
        public DbSet<User> UserInfo { get; set; }

        /// <summary>
        /// маппер USER-UserRole
        /// </summary>
        public DbSet<UserUserRoleMapping> UURM { get; set; }
        /// <summary>
        /// Маппер Permission-userRole
        /// </summary>
        public DbSet<PermissionRecordUserRoleMapping> PRURM { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Setting>().HasData(
                new Setting[]
                {
                    new Setting
                    {
                        Id= Guid.NewGuid().ToString(),
                        Name = "StoreFilesInDb",
                        Value = "false"
                    },
                    new Setting
                    {
                        Id= Guid.NewGuid().ToString(),
                        Name = "Application.Icon",
                        Value = "/images/layout_icons/header.png"
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
                    new Setting
                    {
                        Id= Guid.NewGuid().ToString(),
                        Name = "BirthdayPath"
                        ,Value = "http://localhost:50510/api/People/Birthdate?skip=0&take=10"
                    }, 
#if DEBUG
                    new Setting
                    {
                        Id=Guid.NewGuid().ToString(),
                        Name =  "Page.PageSize",
                        Value = 3.ToString()
                    },
#endif
#if RELEASE
new Setting
                    {
                        Id=Guid.NewGuid().ToString(),
                        Name =  "Page.PageSize",
                        Value = 10.ToString()
                    },
#endif

                    
                });

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole[] {
                    new UserRole
                    {
                        Name = "Администратор",
                        Active = true,
                        IsSystemRole = true,
                        SystemName = AppUserDefaults.AdministratorsRoleName,
                        Id ="1"
                    },
                    new UserRole
                    {
                        Name = "Модератор",
                        Active = true,
                        IsSystemRole = true,
                        SystemName = AppUserDefaults.ModeratorsRoleName,
                        Id ="2"
                    }, new UserRole
                    {
                        Name = "Сотрудник",
                        Active = true,
                        IsSystemRole = true,
                        SystemName = AppUserDefaults.RegisteredRoleName,
                        Id = "3"
                    }
                }
            );

            modelBuilder.Entity<PermissionRecord>().HasData(
                new PermissionRecord[]
                {
                    new PermissionRecord
                    {
                        Name = "Access admin area",
                        SystemName = "AccessAdminPanel",
                        Category = "Standart",
                        Id = "1"
                    },
                    new PermissionRecord
                    {
                        Name = "Admin area. Manage ACL",
                        SystemName = "ManageACL",
                        Category = "Configuration",
                        Id = "2"
                    },
                }
            );

            modelBuilder.Entity<PermissionRecordUserRoleMapping>().HasData(
                new PermissionRecordUserRoleMapping[]
                {
                    new PermissionRecordUserRoleMapping
                    {
                        Id = "1",
                        PermissionRecordId = "1",
                        UserRoleId = "1"
                    },
                    new PermissionRecordUserRoleMapping
                    {
                        Id = "2",
                        PermissionRecordId = "2",
                        UserRoleId = "1"
                    },
                }
            );

            modelBuilder.Entity<PinNews>().HasKey(pc => new {pc.CategoryId, pc.NewsItemId});
        }
    }
}
