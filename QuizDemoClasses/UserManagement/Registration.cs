using QuizDemoClasses.QuizManagement;
using System;

namespace QuizDemoClasses.UserManagement
{
    public class Registration
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TestId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpireTime { get; set; }
        public Student Student { get; set; }
        public TestPaper TestPaper { get; set; }
        public Test Test { get; set; }
        public QuestionDuration QuestionDuration { get; set; }

    }
}
