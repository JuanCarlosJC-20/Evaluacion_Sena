using Entity.Dtos.DoctorDto;
using Entity.Model;

namespace Business.Interfaces
{
    /// <summary>
    /// Define los métodos de negocio específicos para la gestión de doctores.
    /// Hereda operaciones CRUD genéricas de <see cref="IBaseBusiness{Doctor, DoctorDto}"/>.
    /// </summary>
    public interface IDoctorBusiness : IBaseBusiness<Doctor, DoctorDto>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de un doctor.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos actualizados del doctor, como nombre o especialidad.</param>
        /// <returns>True si la actualización fue exitosa; de lo contrario false</returns>
        Task<bool> UpdatePartialDoctorAsync(UpdateDoctorDto dto);

        /// <summary>
        /// Realiza un borrado lógico del doctor, marcándolo como inactivo en lugar de eliminarlo físicamente.
        /// </summary>
        /// <param name="dto">DTO que contiene el ID del doctor a desactivar.</param>
        /// <returns>True si el borrado lógico fue exitoso; de lo contrario false</returns>
        Task<bool> DeleteLogicDoctorAsync(DeleteLogicalDoctorDto dto);
    }
}