using System.ComponentModel.DataAnnotations;

namespace DotNETFunctions.Models
{
    public class FileUpload
    {
        [Display(Name = "Financial File")]
        public IFormFile? FinancialFile { get; set; }

        [Display(Name = "Technical File")]
        public IFormFile? TechnicalFile { get; set; }

        [Display(Name = "Bank Guarantee File")]
        public IFormFile? BankGuaranteeFile { get; set; }

        [Display(Name = "VIP User")]
        public bool IsVipUser { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "Bank Name is required when uploading a Bank Guarantee File")]
        public string? BankName { get; set; }

        [Display(Name = "Guarantee Expiry Date")]
        [Required(ErrorMessage = "Guarantee Expiry Date is required when uploading a Bank Guarantee File")]
        [DataType(DataType.Date)]
        public DateTime? GuaranteeExpiryDate { get; set; }

        [Display(Name = "Guarantee Amount")]
        [Required(ErrorMessage = "Guarantee Amount is required when uploading a Bank Guarantee File")]
        [Range(0, double.MaxValue, ErrorMessage = "Guarantee Amount must be a positive number")]
        public decimal? GuaranteeAmount { get; set; }
    }
}
