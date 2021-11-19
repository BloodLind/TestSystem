using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BLL.DTO
{
    public class Language
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
