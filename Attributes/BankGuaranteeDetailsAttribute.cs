using System.ComponentModel.DataAnnotations;
using DotNETFunctions.Models;

namespace DotNETFunctions.Attributes
{
    public class BankGuaranteeDetailsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (FileUploadModel)validationContext.ObjectInstance;

            if (model.BankGuaranteeFile != null)
            {
                if (string.IsNullOrWhiteSpace(model.BankName))
                {
                    return new ValidationResult("Bank Name is required when uploading a Bank Guarantee File.");
                }
                if (model.GuaranteeExpiryDate == null)
                {
                    return new ValidationResult("Guarantee Expiry Date is required when uploading a Bank Guarantee File.");
                }
                if (string.IsNullOrWhiteSpace(model.GuaranteeAmount))
                {
                    return new ValidationResult("Guarantee Amount is required when uploading a Bank Guarantee File.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
