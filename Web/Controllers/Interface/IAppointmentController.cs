using Entity.Dtos.AppointmentDto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Interface
{
    public interface IAppointmentController : IGenericController<AppointmentDto, Appointment>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de una cita médica
        /// </summary>
        /// <param name="updateDto">DTO con los datos a actualizar de la cita</param>
        /// <returns>Resultado de la operación de actualización parcial</returns>
        Task<IActionResult> UpdatePartial(UpdateAppointmentDto updateDto);

        /// <summary>
        /// Realiza un borrado lógico de la cita médica (la marca como inactiva)
        /// </summary>
        /// <param name="deleteDto">DTO con el ID de la cita y el estado a establecer</param>
        /// <returns>Resultado de la operación de borrado lógico</returns>
        Task<IActionResult> DeleteLogic(DeleteLogicalAppointmentDto deleteDto);
    }
}
