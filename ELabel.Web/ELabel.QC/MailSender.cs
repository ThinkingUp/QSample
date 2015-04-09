using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC
{
    public class MailSender
    {
        readonly SmtpClient _client;
        private bool success = false;
        public MailSender()
        {
            _client = new SmtpClient("smtp.gmail.com", 587);
        }
        

        public bool SendEmail(string body)
        {
            
            _client.Timeout = 10000;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
            _client.UseDefaultCredentials = false;
            _client.Credentials = new NetworkCredential("elabelhackathon2015@gmail.com", "test2015");
            _client.EnableSsl = true;
            MailMessage mm = new MailMessage("elabelhackathon2015@gmail.com", "elabelhackathon2015@gmail.com", "Alert Notification", body);
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


            try
            {
                Console.WriteLine("start to send email...");
                _client.Send(mm);
                success = true;
                Console.WriteLine("email was sent successfully!");
                
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
                success = false;

            }
            return success;
        }
            
            
    }
}
