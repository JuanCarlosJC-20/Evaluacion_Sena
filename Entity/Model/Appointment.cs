using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Appointment : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }   

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
