using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BLL.DTO
{
    public class Question
    {
        public string QuestionData { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public Answer CorrectAnswer { get; set; } = null;
        public int Id { get; set; } = 0;
        public int? DisciplineId { get; set; }
        public int? ThemeId { get; set; }

        public override string ToString()
        {
            return QuestionData;
        }
    }
}
