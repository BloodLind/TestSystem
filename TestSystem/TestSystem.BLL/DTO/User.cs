using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestSystem.BLL.DTO
{
    public class User
    {
        public int Id { get; set; } = 0;
        public string Firstname { get; set; } = "";
        public string Secondname { get; set; } = "";
        public string Fathername { get; set; } = "";
        public byte[] Photo { get; set; }
    }
}
