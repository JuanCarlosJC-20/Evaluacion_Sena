using Entity.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.AppointmentDto
{
    public class AppointmentDto : BaseDto
    {
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
