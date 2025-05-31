using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
    }
}
