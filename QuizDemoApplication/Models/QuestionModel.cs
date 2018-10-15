using System.Collections.Generic;

namespace QuizDemoApplication.Models
{
    public class QuestionModel
    {
        public int TotalQuestionInSet { get; set; }
        public int QuestionNumber { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public int Point { get; set; }
        public List<OptionsModel> Options { get; set; }
    }
}
