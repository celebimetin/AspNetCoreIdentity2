﻿using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();

            if (password!.Contains(user.UserName!))
            {
                errors.Add(new() { Code = "PasswordContainsUserName", Description = "Şifre alanı kullanıcı adı içeremez." });
            }
            if (password!.StartsWith("1234"))
            {
                errors.Add(new() { Code = "PasswordContainsStartsWith", Description = "Şifre alanı ardışık sayı içeremez." });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}