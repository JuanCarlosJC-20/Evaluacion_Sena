using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Dtos.AppointmentDto;
using Entity.Model;
using Web.Controllers.Implements;
using Utilities.Exceptions;

namespace Web.Controllers.Medical
{
    /// <summary>
    /// Controlador para la gestión de citas médicas del sistema.
    /// Hereda funcionalidad CRUD básica del GenericController y añade operaciones específicas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AppointmentController : GenericController<AppointmentDto, Appointment>
    {
        private readonly IAppointmentBusiness _appointmentBusiness;

        /// <summary>
        /// Constructor del controlador de citas médicas
        /// </summary>
        /// <param name="appointmentBusiness">Servicio de lógica de negocio para citas médicas</param>
        /// <param name="logger">Servicio de logging</param>
        public AppointmentController(IAppointmentBusiness appointmentBusiness, ILogger<GenericController<AppointmentDto, Appointment>> logger)
            : base(appointmentBusiness, logger)
        {
            _appointmentBusiness = appointmentBusiness ?? throw new ArgumentNullException(nameof(appointmentBusiness));
        }

        /// <summary>
        /// Obtiene el ID de la entidad para el método CreatedAtAction
        /// </summary>
        /// <param name="dto">DTO de la cita médica</param>
        /// <returns>ID de la cita médica</returns>
        protected override int GetEntityId(AppointmentDto dto)
        {
            return dto.Id;
        }

        /// <summary>
        /// Actualiza parcialmente los datos de una cita médica
        /// </summary>
        /// <param name="updateDto">DTO con los datos a actualizar de la cita</param>
        /// <returns>Resultado de la operación de actualización parcial</returns>
        /// <response code="200">Cita médica actualizada exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Cita médica no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("update-partial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartial([FromBody] UpdateAppointmentDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _appointmentBusiness.UpdatePartialAppointmentAsync(updateDto);
                if (!result)
                    return NotFound($"Cita médica con ID {updateDto.Id} no encontrada");

                return Ok(new { Success = true, Message = "Cita médica actualizada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar cita médica: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Cita médica no encontrada: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente la cita médica: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Realiza un borrado lógico de la cita médica (la marca como inactiva)
        /// </summary>
        /// <param name="deleteDto">DTO con el ID de la cita y el estado a establecer</param>
        /// <returns>Resultado de la operación de borrado lógico</returns>
        /// <response code="200">Cita médica desactivada exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Cita médica no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("delete-logic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLogic([FromBody] DeleteLogicalAppointmentDto deleteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _appointmentBusiness.DeleteLogicAppointmentAsync(deleteDto);
                if (!result)
                    return NotFound($"Cita médica con ID {deleteDto.Id} no encontrada");

                return Ok(new { Success = true, Message = "Cita médica desactivada exitosamente" });
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Error de validación: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Cita médica no encontrada: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al desactivar la cita médica: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}