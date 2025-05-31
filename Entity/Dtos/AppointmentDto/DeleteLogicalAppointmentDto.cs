using Entity.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.AppointmentDto
{
    public class DeleteLogicalAppointmentDto : BaseDto
    {
        public DeleteLogicalAppointmentDto()
        {
            Status = false;
        }
    }
}