using Business.Interfaces;
using Entity.Dtos.PatientDto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using Web.Controllers.Implements;

namespace Web.Controllers.Medical
{
    /// <summary>
    /// Controlador para la gestión de pacientes del sistema médico.
    /// Hereda funcionalidad CRUD básica del GenericController y añade operaciones específicas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PatientController : GenericController<PatientDto, Patient>
    {
        private readonly IPatientBusiness _patientBusiness;

        /// <summary>
        /// Constructor del controlador de pacientes
        /// </summary>
        /// <param name="patientBusiness">Servicio de lógica de negocio para pacientes</param>
        /// <param name="logger">Servicio de logging</param>
        public PatientController(IPatientBusiness patientBusiness, ILogger<GenericController<PatientDto, Patient>> logger)
            : base(patientBusiness, logger)
        {
            _patientBusiness = patientBusiness ?? throw new ArgumentNullException(nameof(patientBusiness));
        }

        /// <summary>
        /// Obtiene el ID de la entidad para el método CreatedAtAction
        /// </summary>
        /// <param name="dto">DTO del paciente</param>
        /// <returns>ID del paciente</returns>
        protected override int GetEntityId(PatientDto dto)
        {
            return dto.Id;
        }

        /// <summary>
        /// Actualiza parcialmente los datos de un paciente
        /// </summary>
        /// <param name="updateDto">DTO con los datos a actualizar del paciente</param>
        /// <returns>Resultado de la operación de actualización parcial</returns>
        /// <response code="200">Paciente actualizado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Paciente no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("update-partial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartial([FromBody] UpdatePatientDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _patientBusiness.UpdatePartialPatientAsync(updateDto);
                if (!result)
                    return NotFound($"Paciente con ID {updateDto.Id} no encontrado");

                return Ok(new { Success = true, Message = "Paciente actualizado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar paciente: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Paciente no encontrado: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente el paciente: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Realiza un borrado lógico del paciente (lo marca como inactivo)
        /// </summary>
        /// <param name="deleteDto">DTO con el ID del paciente y el estado a establecer</param>
        /// <returns>Resultado de la operación de borrado lógico</returns>
        /// <response code="200">Paciente desactivado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Paciente no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("delete-logic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLogic([FromBody] DeleteLogicalPatientDto deleteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _patientBusiness.DeleteLogicPatientAsync(deleteDto);
                if (!result)
                    return NotFound($"Paciente con ID {deleteDto.Id} no encontrado");

                return Ok(new { Success = true, Message = "Paciente desactivado exitosamente" });
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Error de validación: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Paciente no encontrado: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al desactivar el paciente: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}