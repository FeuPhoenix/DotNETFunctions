using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DotNETFunctions.Attributes;

namespace DotNETFunctions.Models
{
    [RequiredFiles]
    [BankGuaranteeDetails]
    public class FileUploadModel
    {
        [Display(Name = "Financial File (PDF, max 10MB)")]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf" })]
        public IFormFile? FinancialFile { get; set; }

        [Display(Name = "Technical File (PDF, max 10MB)")]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf" })]
        public IFormFile? TechnicalFile { get; set; }

        [Display(Name = "Bank Guarantee File (PDF, max 5MB)")]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf" })]
        public IFormFile? BankGuaranteeFile { get; set; }

        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [Display(Name = "Guarantee Expiry Date")]
        [DataType(DataType.Date)]
        [FutureDate(1, ErrorMessage = "Guarantee Expiry Date must be at least 1 month in the future.")]
        public DateTime? GuaranteeExpiryDate { get; set; }

        [Display(Name = "Guarantee Amount")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d{1,3}(,\d{3})*(\.\d+)?$", ErrorMessage = "Invalid format for Guarantee Amount")]
        public string? GuaranteeAmount { get; set; }

        [Display(Name = "VIP User")]
        public bool IsVipUser { get; set; }
    }
}
