namespace TMDT_PROJECT.Models
{
    public interface IEmailService
    {
        public void sendConfirmMail(string emailAddress, string subject, string url, string name);
    }
}
