using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email boş olamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}