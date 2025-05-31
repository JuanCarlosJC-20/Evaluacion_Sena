using Entity.Model;
using Data.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IPatientData : IBaseModelData<Patient>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Patient patient);
    }
}