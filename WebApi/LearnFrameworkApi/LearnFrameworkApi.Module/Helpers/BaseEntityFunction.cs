using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Datas.Entities;
using LearnFrameworkApi.Module.Datas;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers
{
    public static class BaseEntityFunction
    {
        public static void SetUpdateAuditData(this AppDbContext context, EntityEntry entry)
        {
            if (entry.Entity is BaseEntity currentObject)
            {
                currentObject.UpdatedAt = DateTime.Now;
                currentObject.UpdatedBy = context.currentUserResolver.CurrentUsername;
            }
            else if (entry.Entity is AppUser applicationUser)
            {
                applicationUser.UpdatedAt = DateTime.Now;
                applicationUser.UpdatedBy = context.currentUserResolver.CurrentUsername;
            }
            else if (entry.Entity is AppRole appRole)
            {
                appRole.UpdatedAt = DateTime.Now;
                appRole.UpdatedBy = context.currentUserResolver.CurrentUsername;
            }
        }

        public static void SetCreateAuditData(this AppDbContext context, EntityEntry entry)
        {
            if (entry.Entity is BaseEntity currentObject)
            {
                if (currentObject.Id == Guid.Empty)
                    currentObject.Id = Guid.NewGuid();

                if (currentObject.CreatedAt == DateTime.MinValue)
                    currentObject.CreatedAt = DateTime.Now;

                if (string.IsNullOrEmpty(currentObject.CreatedBy))
                    currentObject.CreatedBy = context.currentUserResolver?.CurrentUsername ?? "";
            }
            else if (entry.Entity is AppUser applicationUser)
            {
                if (applicationUser.Id == Guid.Empty)
                    applicationUser.Id = Guid.NewGuid();

                if (applicationUser.CreatedAt == DateTime.MinValue)
                    applicationUser.CreatedAt = DateTime.Now;

                if (string.IsNullOrEmpty(applicationUser.CreatedBy))
                    applicationUser.CreatedBy = context.currentUserResolver.CurrentUsername;
            }
            else if (entry.Entity is AppRole appRole)
            {
                if (appRole.Id == Guid.Empty)
                    appRole.Id = Guid.NewGuid();

                if (appRole.CreatedAt == DateTime.MinValue)
                    appRole.CreatedAt = DateTime.Now;

                if (string.IsNullOrEmpty(appRole.CreatedBy))
                    appRole.CreatedBy = context.currentUserResolver.CurrentUsername;
            }
        }
    }
}
