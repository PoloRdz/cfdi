﻿using System;
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

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void sendMail(string subject, string[] recipients, Stream xml = null, string pdf = null)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("localhost");//
            //SmtpClient smtpClient = new SmtpClient("in.mailjet.com");//
            mail.From = new MailAddress("ics@tomza.com");//
            foreach(string recipient in recipients)
                mail.To.Add(recipient);
            mail.Subject = subject;
            mail.Body = "La factura electrónica de tu compra";
            if(xml != null)
                mail.Attachments.Add(new Attachment(xml, "cfdi.xml", "application/xml"));
            if (pdf != null)
                mail.Attachments.Add(new Attachment(pdf, "application/pdf"));
            smtpClient.Port = 25;
            //smtpClient.Credentials = new NetworkCredential("839ee32ede1fd662a560afa16925c14b", "faa1df3b77854cd13a2313c8a1184ace");
            try
            {
                logger.Info("Enviando correo a: " + mail.To);
                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                logger.Info("Error inesperado al enviar el correo");
                logger.Error(e);
            }
        }
    }
}
