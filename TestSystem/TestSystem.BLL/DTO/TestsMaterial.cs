using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BLL.DTO
{
    public class TestsMaterial
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public int? DisciplineId { get; set; }
        public int? LanguageId {get; set;}
        public int? InformaytionId { get; set; }
        public int? ThemeId { get; set; }
        public string Information { get; set; } = "";
    }
}
