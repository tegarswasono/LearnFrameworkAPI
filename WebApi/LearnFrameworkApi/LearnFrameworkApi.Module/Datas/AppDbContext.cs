using LearnFrameworkApi.Module.Datas.Entities.Configuration;
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
        //public readonly ICurrentUserResolver currentUserResolver;
        //public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserResolver currentUserResolver) : base(options)
        //{
        //    this.currentUserResolver = currentUserResolver;
        //}

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


    }
}
