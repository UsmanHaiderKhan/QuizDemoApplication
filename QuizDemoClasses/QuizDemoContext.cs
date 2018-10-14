using QuizDemoClasses.QuizManagement;
using QuizDemoClasses.UserManagement;
using System.Data.Entity;

namespace QuizDemoClasses
{
    public class QuizDemoContext : DbContext
    {
        public QuizDemoContext() : base("name=constr")
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }

        public DbSet<TestPaper> TestPapers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<QuestionDuration> QuestionDurations { get; set; }

    }
}
