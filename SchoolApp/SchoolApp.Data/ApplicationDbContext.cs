using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;

namespace SchoolApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext()
    {

    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Section> Sections { get; set; }
    public virtual DbSet<Class> Classes { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Remark> Remarks { get; set; }
    public virtual DbSet<Absence> Absences { get; set; }
    public virtual DbSet<Grade> Grades { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<SubjectTeacher> SubjectsTeachers { get; set; }
    public virtual DbSet<SubjectStudent> SubjectsStudents { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}