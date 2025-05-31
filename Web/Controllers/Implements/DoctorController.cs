using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Dtos.DoctorDto;
using Entity.Model;
using Web.Controllers.Implements;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de doctores del sistema médico.
    /// Hereda funcionalidad CRUD básica del GenericController y añade operaciones específicas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DoctorController : GenericController<DoctorDto, Doctor>
    {
        private readonly IDoctorBusiness _doctorBusiness;

        /// <summary>
        /// Constructor del controlador de doctores
        /// </summary>
        /// <param name="doctorBusiness">Servicio de lógica de negocio para doctores</param>
        /// <param name="logger">Servicio de logging</param>
        public DoctorController(IDoctorBusiness doctorBusiness, ILogger<GenericController<DoctorDto, Doctor>> logger)
            : base(doctorBusiness, logger)
        {
            _doctorBusiness = doctorBusiness ?? throw new ArgumentNullException(nameof(doctorBusiness));
        }

        /// <summary>
        /// Obtiene el ID de la entidad para el método CreatedAtAction
        /// </summary>
        /// <param name="dto">DTO del doctor</param>
        /// <returns>ID del doctor</returns>
        protected override int GetEntityId(DoctorDto dto)
        {
            return dto.Id;
        }

        /// <summary>
        /// Actualiza parcialmente los datos de un doctor
        /// </summary>
        /// <param name="updateDto">DTO con los datos a actualizar del doctor</param>
        /// <returns>Resultado de la operación de actualización parcial</returns>
        /// <response code="200">Doctor actualizado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Doctor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("update-partial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartial([FromBody] UpdateDoctorDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _doctorBusiness.UpdatePartialDoctorAsync(updateDto);
                if (!result)
                    return NotFound($"Doctor con ID {updateDto.Id} no encontrado");

                return Ok(new { Success = true, Message = "Doctor actualizado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar doctor: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Doctor no encontrado: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente el doctor: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Realiza un borrado lógico del doctor (lo marca como inactivo)
        /// </summary>
        /// <param name="deleteDto">DTO con el ID del doctor y el estado a establecer</param>
        /// <returns>Resultado de la operación de borrado lógico</returns>
        /// <response code="200">Doctor desactivado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Doctor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("delete-logic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLogic([FromBody] DeleteLogicalDoctorDto deleteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _doctorBusiness.DeleteLogicDoctorAsync(deleteDto);
                if (!result)
                    return NotFound($"Doctor con ID {deleteDto.Id} no encontrado");

                return Ok(new { Success = true, Message = "Doctor desactivado exitosamente" });
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Error de validación: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Doctor no encontrado: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al desactivar el doctor: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}