using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MVCProjectdbContext : IdentityDbContext<AppUser>
    {

        // To Apply Dependency Injection DI. let CLR Creating the object
        public MVCProjectdbContext(DbContextOptions<MVCProjectdbContext> options) : base(options) { }
        
        

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server = DESKTOP-0PFJ581//MSSQLSERVER2; database = MVCProject;; trusted_connection = true;");
        }*/


        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
