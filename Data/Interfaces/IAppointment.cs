using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    /// <summary>
    /// Interfaz específica para operaciones de datos de citas médicas.
    /// Hereda las operaciones CRUD básicas de IBaseModelData y añade métodos específicos para citas.
    /// </summary>
    public interface IAppointmentData : IBaseModelData<Appointment>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Appointment appointment);
    }
}
