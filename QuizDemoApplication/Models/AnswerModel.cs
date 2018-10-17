using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizDemoApplication.Models
{
    public class AnswerModel
    {
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public Guid Token { get; set; }
        public List<ChoiceModel> ChoiceModels { get; set; }

        public string Answer { get; set; }
        public string Direction { get; set; }

        public List<int> UserSelectedId => ChoiceModels == null ? new List<int>() :
                     (List<int>)ChoiceModels.Where(x =>
                        x.IsChecked == "on" || "true".Equals(x.IsChecked, StringComparison.CurrentCultureIgnoreCase));
    }
}
