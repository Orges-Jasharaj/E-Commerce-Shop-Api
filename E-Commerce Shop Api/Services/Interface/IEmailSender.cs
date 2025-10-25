namespace E_Commerce_Shop_Api.Services.Interface
{
    public interface IEmailSender
    {
        Task SendEmail(string to, string subject, string body);
        Task SendEmailWithTemplateAsync(string to, string subject, string firstName, string lastName);



    }
}
