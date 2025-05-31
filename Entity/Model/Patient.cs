using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public int DNI { get; set; }
    }
}
