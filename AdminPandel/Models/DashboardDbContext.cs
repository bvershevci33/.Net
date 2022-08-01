using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Models
{
    public class DashboardDbContext :IdentityDbContext<ApplicationUser>
    {
        public DashboardDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set;}  
        public DbSet<Profesor> Profesors { get; set;}
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourses> StudentCourses { get; set; }
        public DbSet<PersonImg> PersonImgs { get; set; }
        public DbSet<SpResult> SpResults { get; set; }

    }
}
