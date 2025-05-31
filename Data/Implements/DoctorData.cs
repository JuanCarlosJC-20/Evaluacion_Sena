using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using System.Threading.Tasks;

namespace Data.Implements
{ 
    public class DoctorData : BaseModelData<Doctor>, IDoctorData
    {
        public DoctorData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var doctor = await _context.Set<Doctor>().FindAsync(id);
            if (doctor == null)
                return false;
            doctor.Status = status;
            _context.Entry(doctor).Property(d => d.Status).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Doctor doctor)
        {
            var existingDoctor = await _context.Doctors.FindAsync(doctor.Id);
            if (existingDoctor == null) return false;
            _context.Doctors.Update(existingDoctor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}