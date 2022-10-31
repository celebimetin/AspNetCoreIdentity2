using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Yeni Şifre :")]
        [Required(ErrorMessage = "Şifre boş olamaz.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Yeni Şifre Tekrar :")]
        [Required(ErrorMessage = "Şifre tekrarı boş olamaz.")]
        [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}