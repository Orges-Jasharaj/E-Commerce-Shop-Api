namespace E_Commerce_Shop_Api.Dtos.System
{
    public class MailerSendSettings
    {
        public const string SectionName = "MailerSend";
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
