using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirstMVCApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : IdentityDbContext<Employee>
{
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FirstMVCApplication.Models.Employee> Employee { get; set; } = default!;
        public DbSet<FirstMVCApplication.Models.Address> Address { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(
             new IdentityRole { Name = "Admin", NormalizedName  = "ADMIN" },
             new IdentityRole { Name = "User",  NormalizedName  = "USER" },
             new IdentityRole { Name = "hr", NormalizedName = "HR" },
             new IdentityRole { Name = "it", NormalizedName = "IT" }
        );
    }
}
