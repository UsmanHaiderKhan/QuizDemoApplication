namespace QuizDemoClasses.QuizManagement
{
    public class TestPaper
    {
        public int Id { get; set; }
        public int RegistrartionId { get; set; }
        public int TestQuestionId { get; set; }
        public int ChoiceId { get; set; }
        public string Answer { get; set; }
        public string MarkScored { get; set; }

        public Choice Choice { get; set; }



    }
}
