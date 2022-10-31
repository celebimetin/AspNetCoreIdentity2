﻿using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel() { }
        public SignUpViewModel(string userName, string email, string phone, string password, string passwordConfirm)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
            PasswordConfirm = passwordConfirm;
        }

        [Display(Name = "Kullanıcı Adı :")]
        [Required(ErrorMessage = "Kullanıcı Adı boş olamaz.")]
        public string UserName { get; set; }

        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email boş olamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        public string Email { get; set; }

        [Display(Name = "Telefon :")]
        [Required(ErrorMessage = "Telefon boş olamaz.")]
        public string Phone { get; set; }

        [Display(Name = "Şifre :")]
        [Required(ErrorMessage = "Şifre boş olamaz.")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar :")]
        [Required(ErrorMessage = "Şifre tekrar boş olamaz.")]
        [Compare(nameof(Password), ErrorMessage ="Şifre aynı değildir.")]
        public string PasswordConfirm { get; set; }
    }
}