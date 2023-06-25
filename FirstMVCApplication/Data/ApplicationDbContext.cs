using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirstMVCApplication.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FirstMVCApplication.Models.Employee> Employee { get; set; } = default!;
        public DbSet<FirstMVCApplication.Models.Address> Address { get; set; }
    }
