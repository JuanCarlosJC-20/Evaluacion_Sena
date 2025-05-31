using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.DoctorDto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;
using Utilities.Interfaces;
using ValidationException = Utilities.Exceptions.ValidationException;

namespace Business.Implements
{
    /// <summary>
    /// Contiene la lógica de negocio de los métodos específicos para la entidad Doctor
    /// Extiende BaseBusiness heredando la lógica de negocio de los métodos base 
    /// </summary>
    public class DoctorBusiness : BaseBusiness<Doctor, DoctorDto>, IDoctorBusiness
    {
        /// <summary>Proporciona acceso a los métodos de la capa de datos de doctores</summary>
        private readonly IDoctorData _doctorData;

        /// <summary>
        /// Constructor de la clase DoctorBusiness
        /// Inicializa una nueva instancia con las dependencias necesarias para operar con doctores.
        /// </summary>
        public DoctorBusiness(IDoctorData doctorData, IMapper mapper, ILogger<DoctorBusiness> logger, IGenericIHelpers helpers)
            : base(doctorData, mapper, logger, helpers)
        {
            _doctorData = doctorData;
        }

        /// <summary>
        /// Actualiza parcialmente un doctor en la base de datos
        /// </summary>
        public async Task<bool> UpdatePartialDoctorAsync(UpdateDoctorDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var doctor = _mapper.Map<Doctor>(dto);
            var result = await _doctorData.UpdatePartial(doctor); // esto ya retorna bool
            return result;
        }

        /// <summary>
        /// Desactiva un doctor en la base de datos
        /// </summary>
        public async Task<bool> DeleteLogicDoctorAsync(DeleteLogicalDoctorDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del doctor es inválido");

            var exists = await _doctorData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("doctor", dto.Id);

            return await _doctorData.ActiveAsync(dto.Id, dto.Status);
        }
    }
}