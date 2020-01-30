using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;

namespace CharRunner
{
    class MsgProcess
    {
        public void PerformWork()
        {
            try
            {
                DateTime dt = DateTime.Now;
                MessageSvc.RestServiceClient client = new MessageSvc.RestServiceClient();
                MessageSvc.MessageData msgdata = new MessageSvc.MessageData();
                MessageSvc.MessageReturn msgret = new MessageSvc.MessageReturn();
                msgdata.env = ConfigurationManager.AppSettings["Environment"];
               // Console.WriteLine("The Messages Process was started at {0}", dt.ToString());
                msgret = client.MessageProcessor(msgdata);
            }
            catch (Exception ex)
            {
                string errdesc = ex.ToString();
                SendErrorEmail(errdesc, "PROD");
            }
        }

        protected void SendErrorEmail(string Text, string Environ)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAddressSender"].ToString());
            message.To.Add(new MailAddress(ConfigurationManager.AppSettings["EmailAddressRecipients"].ToString()));
            message.Subject = "Concinnity REST CRAD Messages Webservice Error - " + Environ;
            message.Body = "An error has occured in a CRAD Messages Webservice Process - Error: " + Text;
            SmtpClient client = new SmtpClient();
            client.Host = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
            client.Send(message);

        }

    }
}
