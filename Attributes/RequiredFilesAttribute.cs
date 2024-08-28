using System.ComponentModel.DataAnnotations;
using DotNETFunctions.Models;

namespace DotNETFunctions.Attributes
{
    public class RequiredFilesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (FileUploadModel)validationContext.ObjectInstance;

            if (!model.IsVipUser)
            {
                if (model.FinancialFile == null || model.TechnicalFile == null || model.BankGuaranteeFile == null)
                {
                    return new ValidationResult("Non-VIP users must upload all three files: Financial, Technical, and Bank Guarantee.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
