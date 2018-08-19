using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace SLib.Network.Email
{
    public class EmailSender
    {
        readonly EmailConfig _config;


        public EmailSender(EmailConfig config)
        {
            _config = config;
        }       


        public void SendMail(string to, string from, string subject, string body, params string[] filenames)
        {
            SendMail( new[] {to}, from, subject, body, filenames );
        }


        public void SendMail(string[] to, string from, string subject, string body, params string[] filenames)
        {
            var attachments = new Attachment[0];

            try
            {
                attachments = GetAttachmentsFromFilenames(filenames);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }

            SendMail( to, from, subject, body, attachments );
        }


        public void SendMail(string to, string from, string subject, string body, Attachment[] attachments)
        {
            SendMail( new[] {to}, from, subject, body, attachments );
        }


        public void SendMail(string[] to, string from, string subject, string body, Attachment[] attachments)
        {
            try
            {
                var smtp = new SmtpClient(_config.SmtpHost, _config.SmtpPort);
                MailMessage email = CreateNewBasicMessage(from, subject, body, attachments, false);
                AddRecipients(email, to);

                smtp.Send(email);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }
        }


        public void SendHtmlMail(string to, string from, string subject, string body, params string[] filenames)
        {
            SendHtmlMail( new[] {to}, from, subject, body, filenames );
        }


        public void SendHtmlMail(string[] to, string from, string subject, string body, params string[] filenames)
        {
            var attachments = new Attachment[0];

            try
            {
                attachments = GetAttachmentsFromFilenames(filenames);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }

            SendHtmlMail( to, from, subject, body, attachments );
        }


        public void SendHtmlMail(string to, string from, string subject, string body, Attachment[] attachments)
        {
            SendHtmlMail( new[] {to}, from, subject, body, attachments );
        }


        public void SendHtmlMail(string[] to, string from, string subject, string body, Attachment[] attachments)
        {
            try
            {
                var smtp = new SmtpClient(_config.SmtpHost, _config.SmtpPort);
                MailMessage email = CreateNewBasicMessage(from, subject, body, attachments, true);
                AddRecipients(email, to);

                smtp.Send(email);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }
        }


        static Attachment[] GetAttachmentsFromFilenames(IEnumerable<string> filenames)
        {
            var attachments = new List<Attachment>();
            if (filenames != null)
            {
                foreach (string filename in filenames)
                {
                    Stream stream = File.OpenRead(filename);
                    attachments.Add(new Attachment(stream, filename));
                }
            }
            return attachments.ToArray();
        }


        static void AddRecipients(MailMessage email, string[] recipients)
        {
            foreach (string toAddress in recipients)
            {
                email.To.Add(toAddress);   
            }            
        }


        static MailMessage CreateNewBasicMessage(string from, string subject, string body, Attachment[] attachments, bool isHtmlEmail)
        {
            var email = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
            };

            if (attachments.Length > 0)
            {
                foreach (Attachment attachment in attachments)
                {
                    email.Attachments.Add(attachment);
                }
            }

            if (isHtmlEmail)
            {
                email.IsBodyHtml = true;
            }
            return email;
        }
    }
}
