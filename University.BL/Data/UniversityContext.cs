using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;

namespace University.BL.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext() : base("DefaultConnection")
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Enrollment> OfficeAssignment { get; set; }
        public DbSet<Enrollment> Instructor { get; set; }
        public DbSet<Enrollment> Department { get; set; }
        public DbSet<Enrollment> CourseInstructor { get; set; }
    }
}
