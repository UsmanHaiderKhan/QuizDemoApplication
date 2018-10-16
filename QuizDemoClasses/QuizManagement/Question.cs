using System.Collections.Generic;

namespace QuizDemoClasses.QuizManagement
{
    public class Question
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string QuestionType { get; set; }
        public string Questino1 { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; }
        public QuestionCategory QuestionCategory { get; set; }

        public List<Choice> Choices { get; set; }

        public Question()
        {
            Choices = new List<Choice>();
        }

    }


}
