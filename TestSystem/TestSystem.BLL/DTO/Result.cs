using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BLL.DTO
{
    public class Result
    {
        public int Id { get; set; } = 0;
        public int TestId { get; set; }
        public int CorrectPercent { get; set; }
        public int Points { get; set; }
        public int? UserId { get; set; } 
    }
}
