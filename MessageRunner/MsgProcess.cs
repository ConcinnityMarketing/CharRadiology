using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MessageRunner
{
    class MsgProcess
    {
        IDataService _dataService;
        public string Environ = ConfigurationManager.AppSettings["Environment"].ToString();
        public string Client = ConfigurationManager.AppSettings["Client"].ToString();
        public string chkGUID = "2932ff6d-0d0f-442d-b039-4aaace4fbaa3";

        public void PerformWork()
        {
            try
            {
                MessageData rd = new MessageData();
                rd.guid = chkGUID;
                rd.env = Environ;
                rd.client = "CRAD";

                ProcessMessages(rd);
            }
            catch (Exception ex)
            {
                string errdesc = ex.ToString();
                SendErrorEmail(errdesc, Environ);
            }
        }
        protected void ProcessMessages(MessageData md)
        {
            try
            {
                _dataService = new DataService();
                Object DataRequest = new Object();

                MessageProcessResultDtoWrapper dataResultDtoWrapper = new MessageProcessResultDtoWrapper();
                MessageData rd = new MessageData();
                rd.env = md.env;
                rd.guid = md.guid;
                rd.client = md.client;
                DataRequest = rd;

                string sResponse = _dataService.GetData(DataRequest, "MessageProcess");

                if (!string.IsNullOrWhiteSpace(sResponse))
                {
                    dataResultDtoWrapper = JsonConvert.DeserializeObject<MessageProcessResultDtoWrapper>(sResponse);
                    Console.WriteLine("Got DataDTO");
                }
                MessageReturn retResp = new MessageReturn();
                retResp = (MessageReturn)dataResultDtoWrapper?.MessageProcessResult;
                if (retResp != null)
                {
                    switch (retResp.code)
                    {
                        case GenericStatusCodes.Success:
                            break;
                        case GenericStatusCodes.Other:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void SendErrorEmail(string Text, string Environ)
        {
            string[] namesArray = ConfigurationManager.AppSettings["EmailAddressRecipients"].ToString().Split(',');
            List<string> namesList = new List<string>(namesArray.Length);
            namesList.AddRange(namesArray);
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAddressSender"].ToString());
            foreach (var item in namesList)
            {
                message.To.Add(item);
            }
            message.Subject = "Concinnity REST CRAD Message Sender Error - " + Environ;
            message.Body = "An error has occured in a Sender Process - Error: " + Text;
            SmtpClient client = new SmtpClient();
            client.Host = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
            client.Send(message);

        }


    }
}
