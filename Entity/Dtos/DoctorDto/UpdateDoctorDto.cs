using Entity.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.DoctorDto
{
    public class UpdateDoctorDto : BaseDto
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
    }
}
