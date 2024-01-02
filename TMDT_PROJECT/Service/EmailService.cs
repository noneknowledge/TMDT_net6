using System.Net.Mail;
using System.Net;

namespace TMDT_PROJECT.Service
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _envi;

        public EmailService(IWebHostEnvironment envi)
        {
            _envi = envi;
        }
        public string Email
        {
            get { return "maildungchomucdichhoc@gmail.com"; }
        }
        public string password
        {
            get { return "guip jatn yaxg qdjd"; }
        }
        public void sendConfirmMail(string emailAddress, string subject, string url, string name)
        {
            try
            {
                string FilePath = _envi.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Email" +
                    Path.DirectorySeparatorChar.ToString() + "confirm_template.html";
                string body = "";
                using (StreamReader str = new StreamReader(FilePath))
                {
                    body = str.ReadToEnd();
                };

                string messageBody = body.Replace("{{{0}}}", subject);
                messageBody = messageBody.Replace("{{{1}}}", url);
                messageBody = messageBody.Replace("{{{2}}}", name);

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(Email);
                    mail.To.Add(emailAddress);
                    mail.Subject = subject;
                    mail.Body = messageBody;
                    mail.IsBodyHtml = true;


                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(Email, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }



            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
