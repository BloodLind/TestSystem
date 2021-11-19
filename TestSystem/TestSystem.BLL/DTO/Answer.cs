using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BLL.DTO
{
    public class Answer
    {
        public string Data { get; set; }
        public int Id { get; set; } = 0;
        public int? ThemeId { get; set; }
        public int? DisciplineId { get; set; }
        public override string ToString()
        {
            return Data;
        }
    }
}
