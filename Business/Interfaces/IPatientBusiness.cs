using Entity.Dtos.PatientDto;
using Entity.Model;

namespace Business.Interfaces
{
    /// <summary>
    /// Define los métodos de negocio específicos para la gestión de pacientes.
    /// Hereda operaciones CRUD genéricas de <see cref="IBaseBusiness{Patient, PatientDto}"/>.
    /// </summary>
    public interface IPatientBusiness : IBaseBusiness<Patient, PatientDto>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de un paciente.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos actualizados del paciente, como nombre, email o teléfono.</param>
        /// <returns>True si la actualización fue exitosa; de lo contrario false</returns>
        Task<bool> UpdatePartialPatientAsync(UpdatePatientDto dto);

        /// <summary>
        /// Realiza un borrado lógico del paciente, marcándolo como inactivo en lugar de eliminarlo físicamente.
        /// </summary>
        /// <param name="dto">DTO que contiene el ID del paciente a desactivar.</param>
        /// <returns>True si el borrado lógico fue exitoso; de lo contrario false</returns>
        Task<bool> DeleteLogicPatientAsync(DeleteLogicalPatientDto dto);
    }
}