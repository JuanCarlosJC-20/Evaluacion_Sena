using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.PatientDto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{ 
    /// <summary>
    /// Contiene la lógica de negocio de los métodos específicos para la entidad Patient
    /// Extiende BaseBusiness heredando la lógica de negocio de los métodos base 
    /// </summary>
    public class PatientBusiness : BaseBusiness<Patient, PatientDto>, IPatientBusiness
    {
        /// <summary>Proporciona acceso a los métodos de la capa de datos de pacientes</summary>
        private readonly IPatientData _patientData;

        /// <summary>
        /// Constructor de la clase PatientBusiness
        /// Inicializa una nueva instancia con las dependencias necesarias para operar con pacientes.
        /// </summary>
        public PatientBusiness(IPatientData patientData, IMapper mapper, ILogger<PatientBusiness> logger, IGenericIHelpers helpers)
            : base(patientData, mapper, logger, helpers)
        {
            _patientData = patientData;
        }

        /// <summary>
        /// Actualiza parcialmente un paciente en la base de datos
        /// </summary>
        public async Task<bool> UpdatePartialPatientAsync(UpdatePatientDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var patient = _mapper.Map<Patient>(dto);
            var result = await _patientData.UpdatePartial(patient); // esto ya retorna bool
            return result;
        }

        /// <summary>
        /// Desactiva un paciente en la base de datos
        /// </summary>
        public async Task<bool> DeleteLogicPatientAsync(DeleteLogicalPatientDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ArgumentException("El ID del paciente es inválido", nameof(dto.Id));

            var exists = await _patientData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException($"Paciente con ID {dto.Id} no encontrado");

            return await _patientData.ActiveAsync(dto.Id, dto.Status);
        }
    }
}