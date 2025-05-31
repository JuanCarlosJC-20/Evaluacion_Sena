using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.Base;

namespace Entity.Dtos.AppointmentDto
{
    public class UpdateAppointmentDto : BaseDto
    {
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
