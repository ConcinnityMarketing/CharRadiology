using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NUnit.Framework;
using NHibernate;
using NHibernate.Context;
using System.Data;
using System.Data.Sql;
using CMParms;
using System.Configuration;
using CharRadiology.Core.Services;
using CharRadiology.Core.Models;
using CharRadiology.Core.Enums;
//using ConcinnityRefactor_CAI.Core.Models;
//using ConcinnityRefactor_CAI.Core.Services;
//using ConcinnityRefactor_CAI.Core.Web;
//using ConcinnityRefactor_CAI.Core.Enums;
//using ConcinnityRefactor_CAI.Core.Repositories;
//using ConcinnityRefactor_CAI.Core.Ioc;
using System.ComponentModel;
using System.Web.Util;
using System.Net.Mail;
using System.Data.SqlClient;

namespace CharRadiology
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
   // public MessageReturn MessageProcess(MessageData chkUser)

    public class RestService : IRestService
    {
        protected ISessionFactory SessionFactory { get; set; }
        private CharRadiology.Core.Services.IBusinessService businessService;
        protected string getConnectionString(string Environ, string Client, string Unit, string App)
        {
            Connections parm = new Connections();
            if (Environ == "")
                parm.Environment = ConfigurationManager.AppSettings["Environment"].ToString();
            else
                parm.Environment = Environ;
            if (Client == "")
                parm.Client = ConfigurationManager.AppSettings["Client"].ToString();
            else
                parm.Client = Client;
            if (Unit == "")
                parm.Unit = ConfigurationManager.AppSettings["Unit"].ToString();
            else
                parm.Unit = Unit;
            if (App == "")
                parm.Application = ConfigurationManager.AppSettings["Application"].ToString();
            else
                parm.Application = App;
            parm.GetSettings();
            return parm.ConnectionString;
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
            message.Subject = "Concinnity REST SC Monitor Webservice Error - " + Environ;
            message.Body = "An error has occured in a Webservice Process - Error: " + Text;
            SmtpClient client = new SmtpClient();
            client.Host = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
            client.Send(message);

        }
        protected void SendAdminEmail(string Text, List<Admin> admins)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAddressSender"].ToString());
            foreach (Admin ad in admins)
            {
                message.To.Add(new MailAddress(ad.EMAIL));
            }
            message.Subject = "Concinnity Recur Email Service Status";
            message.Body = "The Following is the Status of the Recur Email Process: " + "\r\n" + Text;
            SmtpClient client = new SmtpClient();
            client.Host = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
            client.Send(message);

        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public MessageReturn MessageProcessor(MessageData chkUser)
        {
            MessageReturn webreturn = new MessageReturn();
            //EmailReturn webreturn = new EmailReturn();
            BusinessModel bm = new BusinessModel();
            AdminReturn adreturn = new AdminReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //bm.ConfirmSite = ConfigurationManager.AppSettings["confirmSite"].ToString();
            bm.SMTPHost = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
            bm.retries = Convert.ToInt32(ConfigurationManager.AppSettings["Retries"].ToString());
            bm.WebID = "SPCK";
            if (chkUser.env == "DEVR")
            {
                bm.IContactID = ConfigurationManager.AppSettings["IContactSBID"].ToString();
                bm.IContactURI = ConfigurationManager.AppSettings["IContactSBURI"].ToString();
            }
            else
            {
                bm.IContactID = ConfigurationManager.AppSettings["IContactID"].ToString();
                bm.IContactURI = ConfigurationManager.AppSettings["IContactURI"].ToString();
            }
            bm.AOldListID = ConfigurationManager.AppSettings["IContactAOldList"].ToString();
            bm.ANewListID = ConfigurationManager.AppSettings["IContactANewList"].ToString();
            bm.BOldListID = ConfigurationManager.AppSettings["IContactBOldList"].ToString();
            bm.BNewListID = ConfigurationManager.AppSettings["IContactBNewList"].ToString();
            bm.AMessageID = ConfigurationManager.AppSettings["IContactAMessage"].ToString();
            bm.BMessageID = ConfigurationManager.AppSettings["IContactBMessage"].ToString();

            // IDbConnection cn = new SqlConnection();
            string errdesc = "";
            try
            {
                // cn.ConnectionString = getConnectionString(chkUser.env.ToUpper(), Client, Unit, App);
                //cn.Open();
                //IDbConnection conn = new SqlConnection(
                //  ISession session = sessions.OpenSession(conn);
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession());
                businessService = new SCMonitor.Core.Services.BusinessService(SessionFactory);
                webreturn = businessService.MessageProcess(chkUser, bm);
                //adreturn = businessService.GetAdmins(chkUser, bm);
                //string text = "\r\n" + "Totals: \r\n" + "Birthdays: " + webreturn.BirthDay_Count + "\r\n" + "Anniversaries: " + webreturn.Anniversary_Count;
                //SendAdminEmail(text, adreturn.adminList);
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = GenericStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.env);
            }
            finally
            {
                SessionFactory.Close();
            }
            webreturn.code = GenericStatusCodes.Success;
            return webreturn;
        }
        public List<State> StateList(GenericData Search)
        {
            List<State> customerList = new List<State>();
            string errdesc;
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(Search.ENV.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);

                customerList = businessService.StateList();
            }
            catch (Exception ex)
            {
                errdesc = ex.ToString();
                SendErrorEmail(errdesc, Search.ENV.ToUpper());
            }
            return customerList;
        }
        public PinEntryReturn GetUserPinEntry(PinEntryData chkUser)
        {
            PinEntryReturn webreturn = new PinEntryReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //string Environ = ConfigurationManager.AppSettings["Environment"].ToString();

            //IDbConnection cn = new SqlConnection();
            string errdesc = "";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(chkUser.env.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);
                if (businessService.GetGUID() == chkUser.guid)
                {
                    webreturn = businessService.GetUserPinEntryInfo(chkUser);
                }
                else
                {
                    errdesc = "Incorrect GUID in check valid response code";
                    SendErrorEmail(errdesc, chkUser.env.ToUpper());
                    webreturn.status = "N";
                    webreturn.code = PinEntryStatusCodes.Other;
                    webreturn.desc = "Error - " + errdesc;
                }
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = PinEntryStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.env);
            }
            finally
            {
                SessionFactory.Close();
            }

            return webreturn;
        }
        public PinEntryReturn GetCustomerInfo(PinEntryData chkUser)
        {
            PinEntryReturn webreturn = new PinEntryReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //string Environ = ConfigurationManager.AppSettings["Environment"].ToString();

            //IDbConnection cn = new SqlConnection();
            string errdesc = "";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(chkUser.env.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);
                if (businessService.GetGUID() == chkUser.guid)
                {
                    webreturn = businessService.GetCustomerInfo(chkUser);
                }
                else
                {
                    errdesc = "Incorrect GUID in get customer info";
                    SendErrorEmail(errdesc, chkUser.env.ToUpper());
                    webreturn.status = "N";
                    webreturn.code = PinEntryStatusCodes.Other;
                    webreturn.desc = "Error - " + errdesc;
                }
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = PinEntryStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.env);
            }
            finally
            {
                SessionFactory.Close();
            }

            return webreturn;
        }
        public ResponseCodeReturn GetResponseCode(PinEntryData chkUser)
        {
            ResponseCodeReturn webreturn = new ResponseCodeReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //string Environ = ConfigurationManager.AppSettings["Environment"].ToString();

            //IDbConnection cn = new SqlConnection();
            string errdesc = "";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(chkUser.env.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);
                if (businessService.GetGUID() == chkUser.guid)
                {
                    webreturn.response_code = businessService.GetResponseCode(chkUser.brand);
                    webreturn.status = "Success";
                    webreturn.code = GenericStatusCodes.Success;
                    webreturn.desc = "";
                }
                else
                {
                    errdesc = "Incorrect GUID in get customer info";
                    SendErrorEmail(errdesc, chkUser.env.ToUpper());
                    webreturn.status = "N";
                    webreturn.code = GenericStatusCodes.Other;
                    webreturn.desc = "Error - " + errdesc;
                }
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = GenericStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.env);
            }
            finally
            {
               SessionFactory.Close();
            }

            return webreturn;
        }
        public CommunicationReturn GetCommunication(TestingData chkUser)
        {
            CommunicationReturn webreturn = new CommunicationReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //string Environ = ConfigurationManager.AppSettings["Environment"].ToString();

            //IDbConnection cn = new SqlConnection();
            string errdesc = "";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(chkUser.env.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);
                if (businessService.GetGUID() == chkUser.guid)
                {
                    webreturn = businessService.GetCommunication(chkUser);
                }
                else
                {
                    errdesc = "Incorrect GUID in get Get Communication";
                    SendErrorEmail(errdesc, chkUser.env.ToUpper());
                    webreturn.status = "N";
                    webreturn.code = GenericStatusCodes.Other;
                    webreturn.desc = "Error - " + errdesc;
                }
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = GenericStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.env);
            }
            finally
            {
                SessionFactory.Close();
            }

            return webreturn;
        }
        public SurveyReturn SaveCustResponse(ProfileData chkUser)
        {
            SurveyReturn webreturn = new SurveyReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //string Environ = ConfigurationManager.AppSettings["Environment"].ToString();

            //IDbConnection cn = new SqlConnection();
            string errdesc = "";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(chkUser.ENV.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);
                if (businessService.GetGUID() == chkUser.GUID)
                {
                    webreturn = businessService.SaveCustResponse(chkUser);
                    webreturn.status = "success";
                    webreturn.code = SurveyStatusCodes.Success;
                    webreturn.desc = "";
                }
                else
                {
                    errdesc = "Incorrect GUID in save Cust Response";
                    SendErrorEmail(errdesc, chkUser.ENV.ToUpper());
                    webreturn.status = "N";
                    webreturn.code = SurveyStatusCodes.Other;
                    webreturn.desc = "Error - " + errdesc;
                }
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = SurveyStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.ENV);
            }
            finally
            {
                SessionFactory.Close();
            }

            return webreturn;
        }
        public MessageReturn UpdateTestingRecords(TestingData chkUser)
        {
            MessageReturn webreturn = new MessageReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //string Environ = ConfigurationManager.AppSettings["Environment"].ToString();

            //IDbConnection cn = new SqlConnection();
            string errdesc = "";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(chkUser.env.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);
                if (businessService.GetGUID() == chkUser.guid)
                {
                    webreturn = businessService.UpdateTestingRecords(chkUser);
                    webreturn.status = "success";
                    webreturn.code = GenericStatusCodes.Success;
                    webreturn.desc = "";
                }
                else
                {
                    errdesc = "Incorrect GUID in Update Testing Data";
                    SendErrorEmail(errdesc, chkUser.env.ToUpper());
                    webreturn.status = "N";
                    webreturn.code = GenericStatusCodes.Other;
                    webreturn.desc = "Error - " + errdesc;
                }
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = GenericStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.env);
            }
            finally
            {
                SessionFactory.Close();
            }

            return webreturn;
        }
        public SurveyReturn SaveCustQA(QAData chkUser)
        {
            SurveyReturn webreturn = new SurveyReturn();
            string Client = "SPC";
            string Unit = "CONS";
            string App = "WEB";
            //string Environ = ConfigurationManager.AppSettings["Environment"].ToString();

            //IDbConnection cn = new SqlConnection();
            string errdesc = "";
            IDbConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = getConnectionString(chkUser.ENV.ToUpper(), Client, Unit, App);
                cn.Open();
                DefaultKernelFactory.CreateKernel();
                SessionFactory = ServiceLocator.Get<ISessionFactory>();
                CurrentSessionContext.Bind(SessionFactory.OpenSession(cn));
                businessService = new BusinessService(SessionFactory);
                if (businessService.GetGUID() == chkUser.GUID)
                {
                    webreturn = businessService.SaveCustQA(chkUser);
                    webreturn.status = "success";
                    webreturn.code = SurveyStatusCodes.Success;
                    webreturn.desc = "";
                }
                else
                {
                    errdesc = "Incorrect GUID in save cust QA";
                    SendErrorEmail(errdesc, chkUser.ENV.ToUpper());
                    webreturn.status = "N";
                    webreturn.code = SurveyStatusCodes.Other;
                    webreturn.desc = "Error - " + errdesc;
                }
            }
            catch (Exception ex)
            {
                webreturn.status = "fail";
                webreturn.code = SurveyStatusCodes.Other;
                errdesc = ex.ToString();
                webreturn.desc = errdesc;
                SendErrorEmail(errdesc, chkUser.ENV);
            }
            finally
            {
                SessionFactory.Close();
            }

            return webreturn;
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
