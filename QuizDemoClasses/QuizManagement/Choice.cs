namespace QuizDemoClasses.QuizManagement
{
    public class Choice
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Lable { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; }



    }
}
