using System.Collections.Generic;

namespace QuizDemoClasses.QuizManagement
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public double DurationInMint { get; set; }

        public List<TestQuestion> TestQuestions { get; set; }

        public Test()
        {
            TestQuestions = new List<TestQuestion>();
        }


    }

}
