using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.AppointmentDto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;
using Utilities.Interfaces;
using ValidationException = Utilities.Exceptions.ValidationException;

namespace Business.Implements
{
    /// <summary>
    /// Contiene la lógica de negocio de los métodos específicos para la entidad Appointment
    /// Extiende BaseBusiness heredando la lógica de negocio de los métodos base 
    /// </summary>
    public class AppointmentBusiness : BaseBusiness<Appointment, AppointmentDto>, IAppointmentBusiness
    {
        /// <summary>Proporciona acceso a los métodos de la capa de datos de citas médicas</summary>
        private readonly IAppointmentData _appointmentData;

        /// <summary>
        /// Constructor de la clase AppointmentBusiness
        /// Inicializa una nueva instancia con las dependencias necesarias para operar con citas médicas.
        /// </summary>
        public AppointmentBusiness(IAppointmentData appointmentData, IMapper mapper, ILogger<AppointmentBusiness> logger, IGenericIHelpers helpers)
            : base(appointmentData, mapper, logger, helpers)
        {
            _appointmentData = appointmentData;
        }

        /// <summary>
        /// Actualiza parcialmente una cita médica en la base de datos
        /// </summary>
        public async Task<bool> UpdatePartialAppointmentAsync(UpdateAppointmentDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var appointment = _mapper.Map<Appointment>(dto);
            var result = await _appointmentData.UpdatePartial(appointment); // esto ya retorna bool
            return result;
        }

        /// <summary>
        /// Desactiva una cita médica en la base de datos
        /// </summary>
        public async Task<bool> DeleteLogicAppointmentAsync(DeleteLogicalAppointmentDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID de la cita es inválido");

            var exists = await _appointmentData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("cita médica", dto.Id);

            return await _appointmentData.ActiveAsync(dto.Id, dto.Status);
        }
    }
}