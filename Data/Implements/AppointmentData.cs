using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using System.Threading.Tasks;

namespace Data.Implements
{
    public class AppointmentData : BaseModelData<Appointment>, IAppointmentData
    {
        public AppointmentData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var appointment = await _context.Set<Appointment>().FindAsync(id);
            if (appointment == null)
                return false;
            appointment.Status = status;
            _context.Entry(appointment).Property(a => a.Status).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Appointment appointment)
        {
            var existingAppointment = await _context.Appointments.FindAsync(appointment.Id);
            if (existingAppointment == null) return false;
            _context.Appointments.Update(existingAppointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}