using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace cfdi.Utils
{
    public class MailSender
    {
        public static void sendMail(string subject, string[] recipients, Stream xml = null, Stream pdf = null)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("localhost");//
            mail.From = new MailAddress("noreplay@gastomza.com");//
            foreach(string recipient in recipients)
                mail.To.Add(recipient);
            mail.Subject = subject;
            mail.Body = "La factura electrónica de tu compra";
            if(xml != null)
                mail.Attachments.Add(new Attachment(xml, "cfdi.xml", "application/xml"));
            if (pdf != null)
                mail.Attachments.Add(new Attachment(pdf, "Recibo.pdf", "application/pdf"));
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
