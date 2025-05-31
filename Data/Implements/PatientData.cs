using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using System.Threading.Tasks;

namespace Data.Implements
{
    public class PatientData : BaseModelData<Patient>, IPatientData
    {
        public PatientData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var patient = await _context.Set<Patient>().FindAsync(id);
            if (patient == null)
                return false;
            patient.Status = status;
            _context.Entry(patient).Property(p => p.Status).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Patient patient)
        {
            var existingPatient = await _context.Patients.FindAsync(patient.Id);
            if (existingPatient == null) return false;
            _context.Patients.Update(existingPatient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}