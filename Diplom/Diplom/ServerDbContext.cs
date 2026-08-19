using Diplom;
using Microsoft.EntityFrameworkCore;

public class ServerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Logbook> Logbooks { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Homework> Homeworks { get; set; }
    public DbSet<Mark> Marks { get; set; }
    public DbSet<Balance> Balances { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<PaidLesson> PaidLessons { get; set; }

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Day)
            .WithMany(d => d.Lessons)
            .HasForeignKey(l => l.DayId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Subject)
            .WithMany(s => s.Lessons)
            .HasForeignKey(l => l.SubjectId);
    

        modelBuilder.Entity<Logbook>()
            .HasOne(l => l.Subject)
            .WithMany(s => s.Logbooks)
            .HasForeignKey(l => l.SubjectId);

        modelBuilder.Entity<Logbook>()
            .HasOne(l => l.Group)
            .WithMany(g => g.Logbooks)
            .HasForeignKey(l => l.GroupId);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Events)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Lesson)
            .WithMany(l => l.Events)
            .HasForeignKey(e => e.LessonId);

        modelBuilder.Entity<Event>()
            .Property(e => e.Presence)
            .HasMaxLength(1);

        modelBuilder.Entity<Mark>()
            .HasOne(m => m.Student)
            .WithMany(s => s.Marks)
            .HasForeignKey(m => m.StudentId);

        modelBuilder.Entity<Mark>()
            .HasOne(m => m.Lesson)
            .WithMany(l => l.Marks)
            .HasForeignKey(m => m.LessonId);

        modelBuilder.Entity<Mark>()
            .HasOne(m => m.Logbook)
            .WithMany(l => l.Marks)
            .HasForeignKey(m => m.LogbookId);

        modelBuilder.Entity<Mark>()
            .HasIndex(m => new { m.StudentId, m.LessonId, m.LogbookId })
            .IsUnique();

        modelBuilder.Entity<Profile>()
            .HasOne(p => p.Student)
            .WithOne()
            .HasForeignKey<Profile>(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Student)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.StudentId);

        modelBuilder.Entity<PaidLesson>()
            .HasOne(p => p.Student)
            .WithMany(s => s.PaidLessons)
            .HasForeignKey(p => p.StudentId);

        modelBuilder.Entity<PaidLesson>()
            .HasOne(p => p.Lesson)
            .WithMany()
            .HasForeignKey(p => p.LessonId);

        modelBuilder.Entity<PaidLesson>()
            .HasOne(p => p.Parent)
            .WithMany()
            .HasForeignKey(p => p.ParentId);
    }
}
