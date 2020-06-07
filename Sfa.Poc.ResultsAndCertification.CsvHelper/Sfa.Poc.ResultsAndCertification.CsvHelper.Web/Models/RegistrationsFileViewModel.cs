using Microsoft.AspNetCore.Http;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models
{
    public class RegistrationsFileViewModel
    {
        [Required(ErrorMessage = "Please select registration file.")]
        [DataType(DataType.Upload)]
        [MaxFileSizeInMb(5, ErrorMessage = "Maximum allowed file size is 5mb")]
        [AllowedExtensions(".csv", ErrorMessage = "File extension is not valid.")] // Comma-separated extensions.
        [MaxRecordCount(10000, ErrorMessage = "Max limit on number of records are exceeded.")] // TODO: Config/param
        public IFormFile RegistrationFile { get; set; }
    }
}
