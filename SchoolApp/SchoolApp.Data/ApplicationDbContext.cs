using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;

namespace SchoolApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Student>()
               .HasOne(s => s.Class)
               .WithMany(c => c.Students)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SubjectStudent>()
               .HasOne(ss => ss.Student)
               .WithMany(s => s.SubjectStudents)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SubjectStudent>()
               .HasOne(ss => ss.Subject)
               .WithMany(s => s.SubjectStudents)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SubjectTeacher>()
               .HasOne(st => st.Teacher)
               .WithMany(t => t.SubjectTeachers)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SubjectTeacher>()
               .HasOne(st => st.Subject)
               .WithMany(s => s.SubjectTeachers)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Absence>()
               .Property(a => a.IsExcused)
               .HasDefaultValue(false);

        base.OnModelCreating(builder);
    }
}