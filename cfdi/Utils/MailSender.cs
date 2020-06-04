using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace cfdi.Utils
{
    public class MailSender
    {
        public static void sendMail(string subject, string[] recipients, string[] files = null)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("localhost");//
            mail.From = new MailAddress("noreplay@gastomza.com");//
            foreach(string recipient in recipients)
                mail.To.Add(recipient);
            mail.Subject = subject;
            mail.Body = "La factura electrónica de tu compra";
            if(files != null)
            {
                foreach (string file in files)
                    mail.Attachments.Add(new Attachment(file));
            }
            smtpClient.Port = 25;
            //smtpClient.Credentials = new NetworkCredential("noreplay@gastomza.com", "Amonos123");
            try
            {
                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                //log
            }
        }
    }
}
