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
    public interface IPatientController : IGenericController<PatientDto, Patient>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de un paciente
        /// </summary>
        /// <param name="updateDto">DTO con los datos a actualizar del paciente</param>
        /// <returns>Resultado de la operación de actualización parcial</returns>
        Task<IActionResult> UpdatePartial(UpdatePatientDto updateDto);

        /// <summary>
        /// Realiza un borrado lógico del paciente (lo marca como inactivo)
        /// </summary>
        /// <param name="deleteDto">DTO con el ID del paciente y el estado a establecer</param>
        /// <returns>Resultado de la operación de borrado lógico</returns>
        Task<IActionResult> DeleteLogic(DeleteLogicalPatientDto deleteDto);
    }
}