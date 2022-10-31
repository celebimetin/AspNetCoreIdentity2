namespace AspNetCoreIdentityApp.Web.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailLink(string resetPasswordEmailLink, string toEmail);
    }
}