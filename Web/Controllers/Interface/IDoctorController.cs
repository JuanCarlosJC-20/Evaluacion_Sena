using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.PatientDto;
using Entity.Dtos.DoctorDto;
using Entity.Dtos.AppointmentDto;
using Entity.Model;
using Web.Controllers.Interface;

namespace Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz específica para el controlador de pacientes.
    /// Hereda operaciones CRUD básicas e incluye métodos específicos para la gestión de pacientes.
    /// </summary>
    public interface IDoctorController : IGenericController<DoctorDto, Doctor>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de un doctor
        /// </summary>
        /// <param name="updateDto">DTO con los datos a actualizar del doctor</param>
        /// <returns>Resultado de la operación de actualización parcial</returns>
        Task<IActionResult> UpdatePartial(UpdateDoctorDto updateDto);

        /// <summary>
        /// Realiza un borrado lógico del doctor (lo marca como inactivo)
        /// </summary>
        /// <param name="deleteDto">DTO con el ID del doctor y el estado a establecer</param>
        /// <returns>Resultado de la operación de borrado lógico</returns>
        Task<IActionResult> DeleteLogic(DeleteLogicalDoctorDto deleteDto);
    }

}