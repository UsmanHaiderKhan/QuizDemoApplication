namespace QuizDemoClasses.QuizManagement
{
    public class TestQuestion
    {

        public int Id { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public bool IsActive { get; set; }
        public Question Question { get; set; }
        public Test Test { get; set; }
        public QuestionDuration QuestionDuration { get; set; }
        public TestPaper TestPaper { get; set; }




    }
}
