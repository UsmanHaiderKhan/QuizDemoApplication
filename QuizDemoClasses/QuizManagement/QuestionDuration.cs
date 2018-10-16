using System;

namespace QuizDemoClasses.QuizManagement
{
    public class QuestionDuration
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public int TestQuestionId { get; set; }
        public DateTime RequesTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public DateTime AnswerTime { get; set; }




    }
}
