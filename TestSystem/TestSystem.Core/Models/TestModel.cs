using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.Core.Models
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discipline { get; set; }
        public int DisciplineId { get; set; }
        public int ThemeId { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
        public int LanguageId { get; set; }
    }
}
