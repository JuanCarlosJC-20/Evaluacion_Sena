using Utilities.Interfaces;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Helpers
{
    public class GenericHelpers : IGenericIHelpers
    {
        private readonly IDatetimeHelper _datetimeHelper;
        private readonly IValidationHelper _validationHelper;

        public GenericHelpers(
            IDatetimeHelper datetimeHelper,
            IValidationHelper validationHelper)
        {
            _datetimeHelper = datetimeHelper;
            _validationHelper = validationHelper;
        }

        public DateTime GetCurrentUtcDateTime() => _datetimeHelper.GetCurrentUtcDateTime();
        public DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId) => _datetimeHelper.ConvertToLocalTime(utcDateTime, timeZoneId);
        public DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId) => _datetimeHelper.ConvertToUtc(localDateTime, timeZoneId);
        public string FormatDateTime(DateTime dateTime, string format = null) => _datetimeHelper.FormatDateTime(dateTime, format);
        public int CalculateAge(DateTime birthDate) => _datetimeHelper.CalculateAge(birthDate);
        public bool IsWeekend(DateTime date) => _datetimeHelper.IsWeekend(date);
        public bool IsBusinessHour(DateTime dateTime, int startHour = 9, int endHour = 17) => _datetimeHelper.IsBusinessHour(dateTime, startHour, endHour);


        public bool IsValidPhoneNumber(string phoneNumber) => _validationHelper.IsValidPhoneNumber(phoneNumber);
        public bool IsStrongPassword(string password) => _validationHelper.IsStrongPassword(password);
        public bool IsValidUrl(string url) => _validationHelper.IsValidUrl(url);
        public bool IsValidIp(string ipAddress) => _validationHelper.IsValidIp(ipAddress);
        public bool IsValidCreditCard(string cardNumber) => _validationHelper.IsValidCreditCard(cardNumber);
        public bool IsValidIdentityNumber(string identityNumber) => _validationHelper.IsValidIdentityNumber(identityNumber);
        public Task<ValidationResult> Validate<T>(T dto) => _validationHelper.Validate(dto);

    }
}