using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Datas.Entities.Master;
using LearnFrameworkApi.Module.Helpers;
using LearnFrameworkApi.Module.Services.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public readonly ICurrentUserResolver currentUserResolver;
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserResolver currentUserResolver) : base(options)
        {
            this.currentUserResolver = currentUserResolver;
        }

        //Configuration
        public DbSet<SmtpSetting> SmtpSettings { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }


        //Master
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }


        public override int SaveChanges()
        {
            ChangeTracker.Entries().Where(p => p.State == EntityState.Added)
                .ToList()
                .ForEach(entry =>
                {
                    this.SetCreateAuditData(entry);
                });

            ChangeTracker.Entries().Where(p => p.State == EntityState.Modified)
                .ToList()
                .ForEach(entry =>
                {
                    this.SetUpdateAuditData(entry);
                });
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.Entries().Where(p => p.State == EntityState.Added)
                .ToList()
                .ForEach(entry =>
                {
                    this.SetCreateAuditData(entry);
                });

            ChangeTracker.Entries().Where(p => p.State == EntityState.Modified)
                .ToList()
                .ForEach(entry =>
                {
                    this.SetUpdateAuditData(entry);
                });
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
