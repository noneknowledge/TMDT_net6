namespace TMDT_PROJECT.Service
{
    public interface IEmailService
    {
        public void sendConfirmMail(string emailAddress, string subject, string url, string name);
    }
}
