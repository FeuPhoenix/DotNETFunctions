using System;
using System.ComponentModel.DataAnnotations;

namespace DotNETFunctions.Attributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        private readonly int _monthsInFuture;

        public FutureDateAttribute(int monthsInFuture)
        {
            _monthsInFuture = monthsInFuture;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.Now.AddMonths(_monthsInFuture))
                {
                    return new ValidationResult($"Date must be at least {_monthsInFuture} month(s) in the future.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
