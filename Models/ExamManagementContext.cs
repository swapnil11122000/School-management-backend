using Microsoft.EntityFrameworkCore;

namespace TestbackendProject.Models
{
    public class ExamManagementContext : DbContext
    {
        public ExamManagementContext(DbContextOptions<ExamManagementContext> options)
           : base(options) // Pass the options to the base DbContext class
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<SeatingPlan> SeatingPlans { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // StudentSubject Many-to-Many Configuration
            modelBuilder.Entity<StudentSubject>()
                .HasKey(ss => new { ss.StudentID, ss.SubjectID });

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentID);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectID);

            // SeatingPlan Constraints
            modelBuilder.Entity<SeatingPlan>()
                .HasKey(sp => sp.SeatingID); // Explicitly set SeatingID as the primary key

            modelBuilder.Entity<SeatingPlan>()
                .HasOne(sp => sp.Classroom)
                .WithMany(c => c.SeatingPlans)
                .HasForeignKey(sp => sp.ClassroomID);

            modelBuilder.Entity<SeatingPlan>()
                .HasOne(sp => sp.Student)
                .WithOne(s => s.SeatingPlan)
                .HasForeignKey<SeatingPlan>(sp => sp.StudentID);

            // Score Configuration
            modelBuilder.Entity<Score>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Explicitly configure the SubjectID foreign key
            modelBuilder.Entity<Score>()
     .HasOne(s => s.Subject)
     .WithMany(sub => sub.Scores) // Add navigation to Scores in Subject
     .HasForeignKey(s => s.SubjectID)
     .OnDelete(DeleteBehavior.Restrict);


            // Explicitly configure decimal precision for ScoreValue
            modelBuilder.Entity<Score>()
                .Property(s => s.ScoreValue)
                .HasColumnType("decimal(18,2)");
        }
    }
}
