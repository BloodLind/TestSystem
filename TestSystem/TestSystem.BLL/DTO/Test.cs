using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BLL.DTO
{
    public class Test
    {
        public string Name { get; set; }
        public int? LanguageId { get; set; }
        public int? DisciplineId { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public int? ThemeId { get; set; }
        public int Id { get; set; } = 0;
    }
}
