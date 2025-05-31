using Data.Implements;
using Entity.Dtos.AppointmentDto;
using Entity.Model;

namespace Business.Interfaces
{
    /// <summary>
    /// Define los métodos de negocio específicos para la gestión de citas médicas.
    /// Hereda operaciones CRUD genéricas de <see cref="IBaseBusiness{Appointment, AppointmentDto}"/>.
    /// </summary>
    public interface IAppointmentBusiness : IBaseBusiness<Appointment, AppointmentDto>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de una cita médica.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos actualizados de la cita, como fecha, motivo o paciente.</param>
        /// <returns>True si la actualización fue exitosa; de lo contrario false</returns>
        Task<bool> UpdatePartialAppointmentAsync(UpdateAppointmentDto dto);

        /// <summary>
        /// Realiza un borrado lógico de la cita, marcándola como inactiva en lugar de eliminarla físicamente.
        /// </summary>
        /// <param name="dto">DTO que contiene el ID de la cita a desactivar.</param>
        /// <returns>True si el borrado lógico fue exitoso; de lo contrario false</returns>
        Task<bool> DeleteLogicAppointmentAsync(DeleteLogicalAppointmentDto dto);
    }
}