using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace Common
{
    public class SmtpEmailService : IDisposable
    {

        public void SendEmail(string toAddress, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new Exception("Password cannot be empty!");
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                throw new Exception("Username cannot be empty!");
            }

            if (string.IsNullOrWhiteSpace(Host))
            {
                throw new Exception("Host cannot be empty!");
            }

            var from = new MailAddress(Username);
            var to = new MailAddress(toAddress);

            try
            {

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(Username);
                    mail.To.Add(toAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(Host, 587))
                    {
                        smtp.Credentials = new NetworkCredential(Username, Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

            }
            catch (Exception ex)
            {
                string errMsg = $"{MethodBase.GetCurrentMethod().Name}: {ex.ToString()}";
                Debug.WriteLine(errMsg);
            }
        }

        public SmtpEmailService(string host, string username, string password)
        {
            Password = password;
            Username = username;
            Host = host;
        }

        public void Dispose()
        {
            Password = null;
            Username = null;
            Host = null;
        }

        private string Password { get; set; }
        private string Username { get; set; }
        private string Host { get; set; }

    }

}
