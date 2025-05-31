using Entity.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.PatientDto
{
    public class DeleteLogicalPatientDto : BaseDto
    {
        public DeleteLogicalPatientDto()
        {
            Status = false;
        }
    }
}
