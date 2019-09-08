using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using EnvoyService.Core.Models;
using EnvoyService.Core.Enums;
using Dapper;
using NHibernate;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Xml;
using System.Security;
using CMParms;
using System.Net;
using cmextwebsvc.BusinessLayer;
using System.Web;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Timers;
using CallfireApiClient;
using CallfireApiClient.Api.Account.Model;
using CallfireApiClient.Api.CallsTexts.Model;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using CallFire_csharp_sdk.API;
using CallFire_csharp_sdk.Common.Resource;
using CallFire_csharp_sdk.Common.Result;
using CallFire_csharp_sdk.Common.DataManagement;

namespace EnvoyService.Core
{
    public abstract class Messenger
    {
        protected static Int16 CheckStatus { get; set; }
        protected static string sendId { get; set; }
        protected static string sendStatus { get; set; }
        protected static Account moveAcc { get; set; }
        protected static RestClient moveRest { get; set; }
        protected static int processGroupId { get; set; }
        protected static System.Timers.Timer aTimer;
        protected static System.Timers.Timer bTimer;
        protected static int MessageTotal { get; set; }

        protected static ISessionFactory sessionFactory;
        protected static void CreateProcessGroupHistoryEntry(int processGroupId, string message, int procNum, int procStep)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            var parameters = new DynamicParameters();
            parameters.Add("@procID", processGroupId);
            parameters.Add("@procDesc", message);
            parameters.Add("@procNum", procNum);
            parameters.Add("@procStep", procStep);
            currentSession.Connection.Execute("usp_UpdatePGHist", parameters, commandType: CommandType.StoredProcedure);
        }
        protected static int CreateProcessGroup(string procType, string procStat, string procNote, int parentID, int jobinstanceID)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            var parameters = new DynamicParameters();
            parameters.Add("@procType", procType);
            parameters.Add("@procStat", procStat);
            parameters.Add("@procNote", procNote);
            parameters.Add("@parentID", parentID);
            parameters.Add("@jobInstanceID", jobinstanceID);
            parameters.Add("@thisPG_ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            currentSession.Connection.Execute("usp_rl_CreatePG", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@thisPG_ID");
        }
        protected void EndProcessGroup(int processGroupId, string note, string procStat)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            var parameters = new DynamicParameters();
            parameters.Add("@procStat", procStat);
            parameters.Add("@procNote", note);
            parameters.Add("@thisPG_ID", processGroupId);
            currentSession.Connection.Execute("usp_EndPG", parameters, commandType: CommandType.StoredProcedure);
        }

        protected static void ExecuteSQL(string sql)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            var parameters = new DynamicParameters();
            parameters = null;
            currentSession.Connection.Execute(sql, parameters, commandTimeout: 10 * 60, commandType: CommandType.Text);
        }

        protected BooleanSwitch myBooleanSwitch = new BooleanSwitch("myBooleanSwitch",
    "Controls Global Tracing");
        protected TraceSwitch myTraceSwitch = new TraceSwitch("myTraceSwitch",
            "Controls Trace Levels");
        public virtual MessageReturn GetMessages(MessageData chkUser, BusinessModel bm)
        {
            MessageReturn retdata = new MessageReturn();
            return retdata;
        }
        public virtual MessageReturn ProcessMessages(MessageData chkUser, BusinessModel bm)
        {
            MessageReturn retdata = new MessageReturn();
            return retdata;
        }
        protected static List<MessageModel> CustMessageList(string RecurType)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            string sql = "";
            List<MessageModel> emailList = new List<MessageModel>();
            try
            {
                IEnumerable results = currentSession.Connection.Query(@"SELECT e.INDIV_ID, e.EMAIL, e.MESSAGE_DT, e.CHANNEL, e.STATUS, e.UPDATE_DT, p.FIRST_NAME, p.LAST_NAME, p.ADDRESS1, p.CITY, p.STATE, p.ZIP, e.MESSAGE_SEQ, e.MD_RECNUM, e.PHONE, e.MFID
                                                                    FROM CUSTOMER_MESSAGE_DETAIL e inner join CUSTOMER_PROFILE p on p.INDIV_ID = e.INDIV_ID
                                                                    inner join CUSTOMER_COMMUNICATIONS c on c.INDIV_ID = e.INDIV_ID and c.MFID = e.MFID
                                                                    WHERE (e.STATUS IS NULL) AND e.CHANNEL = @type", new { @type = RecurType });

                foreach (dynamic row in results)
                {
                    emailList.Add(new MessageModel(row.INDIV_ID, row.MFID, row.EMAIL, row.FIRST_NAME, row.LAST_NAME, row.ADDRESS1, row.CITY,
                                                       row.STATE, row.ZIP, row.CHANNEL, row.MESSAGE_DT, row.STATUS, row.UPDATE_DT, row.MESSAGE_SEQ, row.MD_RECNUM, row.PHONE, 0));
                    // Update Contact On Database to Interim status

                    //sql = "update RECUR_EMAIL set RECUR_STATUS = 'I' where indiv_id = " + row.INDIV_ID + " AND RECUR_TYPE = '" + row.RECUR_TYPE + "'";
                    // ExecuteSQL(sql);
                }

            }
            catch (Exception ex)
            {

                throw new Exception("CustMessageList Exception: " + ex.ToString());
            }

            return emailList;

        }
        protected static List<DBMessage> MessageList(string Environment, string RecurType)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            IEnumerable results = currentSession.Connection.Query(@"SELECT MFID, MESSAGE_ID, MESSAGE_SEQ, MESSAGE_TEXT, CHANNEL FROM REF_MESSAGES
                                                                    WHERE ENVIRONMENT = @env AND CHANNEL = @type", new { @env = Environment, @type = RecurType });

            List<DBMessage> msgList = new List<DBMessage>();
            foreach (dynamic row in results)
            {
                msgList.Add(new DBMessage(row.MFID, row.MESSAGE_SEQ, row.MESSAGE_ID, row.CHANNEL,  row.MESSAGE_TEXT));
            }

            return msgList;
        }
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
        protected bool IsEmpty(DataSet dataSet)
        {
            foreach (DataTable Table in dataSet.Tables)
            {
                return false;
            }
            return true;
        }

    }
    public class MailMessenger : Messenger
    {
        protected static List<Account> accList = new List<Account>();
        protected static List<MessageModel> emailList = new List<MessageModel>();
        protected static List<DBMessage> msgList = new List<DBMessage>();

        public MailMessenger()
        {

        }
        public MailMessenger(ISessionFactory sessionFactory)
        {
            Messenger.sessionFactory = sessionFactory;
        }
        public override MessageReturn GetMessages(MessageData chkUser, BusinessModel bm)
        {
            MessageReturn retdata = new MessageReturn();
            return retdata;
        }
        public override MessageReturn ProcessMessages(MessageData chkUser, BusinessModel bm)
        {
            MessageReturn retdata = new MessageReturn();
            // chkUser.env = "QA";
            EmailReturn user = new EmailReturn();
            // string strURISufx = "/c/";
            string IContactID = bm.IContactID;
            string retValue = "";
            string strURI = bm.IContactURI;
            //string strURI = bm.IContactURI + strURISufx;
            DataSet ds = new DataSet();
            DataTable dt;
            string EmailFrom = "";
            string EmailSubject = "";
            string EmailMessage = "";
            string EmailBody = "";
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                ConsumerClass CustomerObject = new ConsumerClass();
                CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
                processGroupId = CreateProcessGroup("RE", "R", "Starting Messenger Process", 0, 0);
                CreateProcessGroupHistoryEntry(processGroupId, "Start Messenger Process", 0, 1000);
                ds = CustomerObject.GetProcessEmailElements(bm.client);
                dt = ds.Tables[0];
                if (!IsEmpty(ds))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        EmailFrom = row["ProcessEmailFrom"].ToString();
                        EmailSubject = row["ProcessEmailSubject"].ToString();
                        EmailMessage = row["ProcessEmailMessage"].ToString();
                        //if (string.IsNullOrEmpty(chkUser.message))
                        //{
                        //    EmailBody = chkUser.rec_name + "," + "\r\n" + "\r\n" + EmailMessage + "\r\n";
                        //}
                        //else
                        //{
                        //    EmailBody = chkUser.rec_name + "," + "\r\n" + "\r\n" + chkUser.message + "\r\n";
                        //}

                    }
                }
                var client = new RestClient();
                client.EndPoint = strURI;
                client.Method = HttpVerb.GET;
                string accountID = "";
                string clientFolderID = "";
                Account acc = new Account();
                accList = AccountList(chkUser.env);
                foreach (Account row in accList)
                {
                    acc.AppId = row.AppId;
                    acc.Username = row.Username;
                    acc.Password = row.Password;
                    acc.client_folder_id = row.client_folder_id;
                    acc.new_list_id = row.new_list_id;
                    acc.old_list_id = row.old_list_id;
                }
                ds = client.MakeRequestDS(acc);
                foreach (DataRow ReturnRow in ds.Tables[2].Rows)
                {
                    accountID = ReturnRow["accountId"].ToString();
                }
                string requestURI = strURI + accountID + "/c/";
                //Leave next section commented unless starting new client
                client.EndPoint = requestURI;
                ds = client.MakeRequestDS(acc);
                //ds.Tables[0].TableName = "accounts";
                foreach (DataRow ReturnRow in ds.Tables[2].Rows)
                {
                    string strClientFolder = ReturnRow["clientFolderId"].ToString();
                    if (strClientFolder == acc.client_folder_id)
                        clientFolderID = strClientFolder;
                }
                //Leave previous section commented unless starting new client
                //clientFolderID = "16096";
                if (string.IsNullOrEmpty(clientFolderID))
                {
                    throw new Exception("ProcessMessages Exception: clientfolderid not found: id = " + acc.client_folder_id);
                }
                requestURI += clientFolderID + "/";


                client.EndPoint = requestURI;
                retdata = ProcessMailMessages(chkUser, bm, client, acc);
                bool rtnAnniversary = false;
                //if (rtnBirthday)
                //{
                //    client.EndPoint = requestURI;
                //    rtnAnniversary = AnniversaryProcess(chkUser, bm, client, acc);
                //}
                //user.Anniversary_Count = AnniversaryTotal;
                //string sql = "update CUSTOMER_COMMUNICATIONS set CC_STATUS = 'COMPLETE' where CC_STATUS = 'INPROCESS'";
                //ExecuteSQL(sql);
                user.Message_Count = MessageTotal;
                user.status = "success";
                user.code = MailStatusCodes.Success;
                user.desc = retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("ProcessMessages Exception: " + ex.ToString());
            }
            EndProcessGroup(processGroupId, "Messenger Process Successful", "C");
            CreateProcessGroupHistoryEntry(processGroupId, "Messenger Process Completed", 0, 1100);
            return retdata;
        }

        protected static MessageReturn ProcessMailMessages(MessageData chkUser, BusinessModel bm, RestClient client, Account acc)
        {
            MessageReturn retdata = new MessageReturn();
            // Birthday Email Processing
            // Build XML to save Contact first
            // Get List of email IDs
            CreateProcessGroupHistoryEntry(processGroupId, "Begin MailMessages Process", 0, 1010);
            CheckStatus = 0;
            DateTime dt = DateTime.Now;
            //Console.WriteLine("Start MailMessages Process at {0}", dt.ToString());
            DateTime MessageDT = DateTime.Today;
            string Channel = "EM";
            Contact c;
            string requestURI = client.EndPoint;
            bool retSub = false;
            bool blContacts = false;

            List<Contact> contactList = new List<Contact>();
            try
            {
                DBMessage msg = new DBMessage();
                msgList = MessageList(chkUser.env, Channel);
                DateTime dtToday = DateTime.Today;
                DateTime? dateOrNull;
                DateTime newSelectedDate = DateTime.Today;
                TimeSpan ts;
                foreach (DBMessage mg in msgList)
                {
                    //    if (mg.SEQ == sgw)
                    //    {
                    //        MessageID = mg.MESSAGE_ID.ToString();
                    //        break;
                    //    }

                    emailList = CustMessageList(Channel);
                    foreach (MessageModel em in emailList)
                    {
                        c = new Contact();
                        c.email = em.EMAIL;
                        c.status = "normal";
                        c.firstName = em.FIRST_NAME;
                        c.lastName = em.LAST_NAME;
                        c.prefix = "";
                        c.state = em.STATE;
                        c.street = em.ADDRESS1;
                        c.postalCode = em.ZIP;
                        c.phone = "";
                        c.city = em.CITY;
                        c.indiv_id = em.INDIV_ID.ToString();
                      //  c.offer_code = em.OFFER_CODE;
                      //  c.child_name = em.CHILD_NAME;
                        //dateOrNull = em.PROCEDURE_DATE;
                        //if (dateOrNull != null)
                        //{
                        //    newSelectedDate = dateOrNull.Value;
                        //}
                        //ts = dtToday - newSelectedDate;
                        ////c.late = ts.TotalDays.ToString();
                        //c.test1_date = em.TEST1_DATE.ToString("MMMM dd, yyyy");
                        //c.test2_date = em.TEST2_DATE.ToString("MMMM dd, yyyy");
                        var checkDate = new DateTime(MessageDT.Year, em.MESSAGE_DT.Value.Month, em.MESSAGE_DT.Value.Day);
                        //Changed 5/9/2016 added check for not null email
                        if (checkDate == MessageDT && em.MFID == mg.MFID && em.MESSAGE_SEQ == mg.MESSAGE_SEQ && !(string.IsNullOrEmpty(em.EMAIL)))
                        {
                            string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'I' where MD_RECNUM = " + em.MD_RECNUM;
                            ExecuteSQL(sql);
                            client.EndPoint = requestURI;
                            retSub = Subscribe(client, acc, c);
                            MessageTotal++;
                            blContacts = true;
                            contactList.Add(c);
                        }
                    }
                    CreateProcessGroupHistoryEntry(processGroupId, "Get MailMessages List", MessageTotal, 1010);
                    if (blContacts)
                    {

                        // Send Message to List
                        //int sgw = GetSGW();
                        string MessageID = mg.MESSAGE_ID.ToString();
                        client.EndPoint = requestURI;
                        // Create a timer with a 60 second interval.
                        bTimer = new System.Timers.Timer(60000);
                        CreateProcessGroupHistoryEntry(processGroupId, "Emailing To List", MessageTotal, 1020);
                        string retSend = EmailToList(client, acc, MessageID);
                        // Get SendID to check if sent
                        var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var dict = jsonSerializer.Deserialize<Dictionary<string, dynamic>>(retSend);
                        sendId = dict["sends"][0]["sendId"];
                        sendStatus = dict["sends"][0]["status"];
                        moveAcc = acc;
                        client.EndPoint = requestURI;
                        moveRest = client;
                        moveRest.EndPoint = client.EndPoint + "sends/" + sendId;
                        // don't move on until the status is released
                        bTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        bTimer.Interval = 30000;
                        bTimer.Enabled = true;
                        Console.Write("Checking Send Status ");
                        while (!(sendStatus == "released"))
                        {
                            if (CheckStatus >= bm.retries)
                                throw new Exception("MailMessages Process Exception: Exceeded Check Email Status Retry Count of: " + bm.retries.ToString());
                        }
                        bTimer.Stop();
                        // move contacts to list of old email sends
                        client.EndPoint = requestURI;
                        List<MessageModel> relist = new List<MessageModel>();
                        MessageModel re;
                        foreach (Contact item in contactList)
                        {
                            re = new MessageModel();
                            re.EMAIL = item.email;
                            relist.Add(re);
                        }
                        CreateProcessGroupHistoryEntry(processGroupId, "Copying List To Completed", 0, 1030);
                        string retList = MoveToList(client, acc, relist);
                        CampaignReturn cr = new CampaignReturn();
                        // Cleanup Contacts On Database that were sent the email and set to processed
                        foreach (MessageModel em in emailList)
                        {
                            var checkDate = new DateTime(MessageDT.Year, em.MESSAGE_DT.Value.Month, em.MESSAGE_DT.Value.Day);
                            //Changed 5/9/2016 added check for not null email
                            if (checkDate == MessageDT && em.MFID == mg.MFID && em.MESSAGE_SEQ == mg.MESSAGE_SEQ && !(string.IsNullOrEmpty(em.EMAIL)))
                            {
                                string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'P', ACTUAL_DT = GETDATE() where MD_RECNUM = " + em.MD_RECNUM;
                                ExecuteSQL(sql);
                                // Store Campaign History Record
                                em.PG_ID = processGroupId;
                                cr = CampaignHistoryProcess(em);
                            }

                        }
                        //        foreach (Contact co in contactList)
                        //{
                        //    string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'P' where CHANNEL = '" + Channel + "' and STATUS = 'I' and email = '" + co.email + "'";
                        //}
                    }
                    blContacts = false;
                }
                retdata.code = GenericStatusCodes.Success;
            }
            catch (Exception ex)
            {
                throw new Exception("MailMessages Exception: " + ex.ToString());
            }
            return retdata;
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                string sResponse = null;
                //Uri uri = new Uri(moveRest.EndPoint + "sends/" + sendId);
                moveRest.Method = HttpVerb.GET;
                moveRest.ContentType = "text/xml";
                //moveRest.EndPoint = uri.ToString();
                sResponse = "";
                sResponse = moveRest.MakeRequest(moveAcc);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(sResponse.ToString());
                XmlNodeList xnList = xdoc.SelectNodes("/response/send");
                foreach (XmlNode xn in xnList)
                {
                    // sendStatus = xn.InnerText;
                    sendStatus = xn["status"].InnerText;
                }
                CheckStatus++;
                Console.Write(".");

                //if (sendStatus == "pending")
                //{
                //    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                //}

            }
            catch (Exception ex)
            {
                throw new Exception("OnTimedEvent Exception: " + ex.ToString());
            }
        }
        protected static bool Subscribe(RestClient client, Account acc, Contact contact)
        {
            bool success = false;
            try
            {
                string InputEndPoint = client.EndPoint;
                // check to see if contact exists
                // Get Contact ID
                client.EndPoint = InputEndPoint;
                var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                Uri uri = new Uri(InputEndPoint + "contacts/");
                string json = jsonSerializer.Serialize(contact);
                string sResponse = null;
                string data = "";
                client.PostData = "";
                string contactId = GetContactId(client, contact.email, acc);
                if (string.IsNullOrEmpty(contactId))
                {
                    // Add to contacts

                    client.EndPoint = uri.ToString();
                    json = "[" + json + "]";
                    data = json;
                    sResponse = "";
                    client.Method = HttpVerb.POST;
                    client.ContentType = "application/json";
                    client.PostData = json;
                    sResponse = client.MakeRequest(acc);

                    // Get Contact ID
                    client.EndPoint = InputEndPoint;
                    client.PostData = "";
                    contactId = GetContactId(client, contact.email, acc);
                }
                // Update contact
                client.EndPoint = InputEndPoint;
                int intContactId = Convert.ToInt32(contactId);
                bool updatesuccess = UpdateContact(client, intContactId, acc, contact);
                // Subscribe to List
                Subscription sub = new Subscription();
                sub.contactId = contactId;
                sub.listId = acc.new_list_id;
                sub.status = "normal";

                uri = new Uri(InputEndPoint + "subscriptions/");
                client.EndPoint = uri.ToString();
                jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                json = jsonSerializer.Serialize(sub);
                json = "[" + json + "]";
                data = json;
                sResponse = "";
                client.Method = HttpVerb.POST;
                client.ContentType = "application/json";
                client.PostData = json;
                sResponse = client.MakeRequest(acc);

                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Subscribe Exception: " + ex.ToString());
            }
            return success;

        }
        protected static string EmailToList(RestClient client, Account acc, string Message)
        {
            // Anneiversary Message - 2107614
            // Birthday Message - 2107613
            bool success = false;
            string sResponse = null;
            messages draftmsg = new messages();
            message msg = new message();
            string InputEndPoint = client.EndPoint;
            try
            {
                // Get Old Message
                draftmsg = GetMessage(client, Message, acc);
                // Create New Message
                msg.campaignId = draftmsg.campaignId;
                msg.subject = draftmsg.subject;
                msg.messageType = draftmsg.messageType;
                msg.messageName = draftmsg.messageName;
                msg.htmlBody = draftmsg.htmlBody;
                msg.textBody = draftmsg.textBody;
                Uri uri = new Uri(InputEndPoint + "messages/");
                client.EndPoint = uri.ToString();
                var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var stringwriter = new System.IO.StringWriter();
                string json = jsonSerializer.Serialize(msg);
                json = "[" + json + "]";
                string data = json;
                sResponse = "";
                client.Method = HttpVerb.POST;
                client.ContentType = "application/json";
                client.PostData = json;
                sResponse = client.MakeRequest(acc);
                messages retmsg = new messages();
                var dict = jsonSerializer.Deserialize<Dictionary<string, dynamic>>(sResponse);
                retmsg.messageId = dict["messages"][0]["messageId"];

                // Send to List
                Send sub = new Send();
                sub.messageId = retmsg.messageId;
                sub.includeListIds = acc.new_list_id;

                uri = new Uri(InputEndPoint + "sends/");
                client.EndPoint = uri.ToString();
                jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                json = jsonSerializer.Serialize(sub);
                json = "[" + json + "]";
                data = json;
                sResponse = "";
                client.Method = HttpVerb.POST;
                client.ContentType = "application/json";
                client.PostData = json;
                sResponse = client.MakeRequest(acc);
                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception("EmailToList Exception: " + ex.ToString());
            }
            return sResponse;

        }
        protected static string MoveToList(RestClient client, Account acc, List<MessageModel> emailList)
        {
            bool success = false;
            string sResponse = "";
            string SubscriptionID = "";
            string NewSubID = "";
            string contactId = "";
            //Contact con = new Contact();
            Uri uri;
            try
            {
                string InputEndPoint = client.EndPoint;
                // Move to List
                NewList sub = new NewList();
                sub.listId = acc.old_list_id;
                sub.status = "normal";
                foreach (MessageModel em in emailList)
                {
                    client.EndPoint = InputEndPoint;
                    contactId = GetContactId(client, em.EMAIL, acc);
                    if (!(string.IsNullOrEmpty(contactId)))
                    {
                        client.EndPoint = InputEndPoint;
                        NewSubID = acc.new_list_id + "_" + contactId;
                        bool IsSubscribed = GetSubscription(client, NewSubID, acc);
                        if (IsSubscribed)
                        {
                            SubscriptionID = acc.new_list_id + "_" + contactId;
                            client.EndPoint = InputEndPoint;
                            bool retSub = TryMove(client, sub, acc, SubscriptionID);
                        }
                    }
                    else
                    {
                        string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'U' where STATUS = 'I' and email = '" + em.EMAIL + "'";
                        ExecuteSQL(sql);

                    }

                }
                success = true;
            }
            catch (Exception ex)
            {
                //success = false;
                throw new Exception("MoveToList Exception: " + ex.ToString());
            }
            return sResponse;


        }
        protected static bool TryMove(RestClient client, NewList sub, Account acc, string SubscriptionID)
        {
            string sResponse = "";
            bool retBool = false;
            try
            {
                Uri uri = new Uri(client.EndPoint + "subscriptions/" + SubscriptionID);
                client.EndPoint = uri.ToString();
                var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string json = jsonSerializer.Serialize(sub);
                //json = "[" + json + "]";
                string data = json;
                client.Method = HttpVerb.PUT;
                client.ContentType = "application/json";
                client.PostData = json;
                sResponse = client.MakeRequest(acc);
                retBool = true;
            }
            catch (Exception ex)
            {

                return retBool;
            }
            return retBool;
        }
        protected static string GetContactId(RestClient client, string email, Account acc)
        {
            string contactId = "";
            try
            {
                string sResponse = null;
                Uri uri = new Uri(client.EndPoint + "contacts?status=total&email=" + email + "");
                client.Method = HttpVerb.GET;
                client.ContentType = "text/xml";
                client.EndPoint = uri.ToString();
                sResponse = "";
                sResponse = client.MakeRequest(acc);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(sResponse.ToString());
                XmlNodeList xnList = xdoc.SelectNodes("/response/contacts/contact/contactId");
                foreach (XmlNode xn in xnList)
                {
                    contactId = xn.InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetContactId Exception: " + ex.ToString());
            }
            return contactId;
        }
        protected static Contact GetContact(RestClient client, string contactid, Account acc)
        {
            Contact con = new Contact();
            string ret = "";
            try
            {
                string sResponse = null;
                Uri uri = new Uri(client.EndPoint + "contacts/" + contactid + "");
                client.Method = HttpVerb.GET;
                client.ContentType = "text/xml";
                client.EndPoint = uri.ToString();
                sResponse = "";
                sResponse = client.MakeRequest(acc);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(sResponse.ToString());
                XmlNodeList xnList = xdoc.SelectNodes("/response/contact");
                foreach (XmlNode xn in xnList)
                {
                    ret = xn.InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetContactId Exception: " + ex.ToString());
            }
            return con;
        }
        protected static bool GetSubscription(RestClient client, string subscriptionid, Account acc)
        {
            bool ret = false;
            string status = "";
            try
            {
                string sResponse = null;
                Uri uri = new Uri(client.EndPoint + "subscriptions/" + subscriptionid + "");
                client.Method = HttpVerb.GET;
                client.ContentType = "text/xml";
                client.EndPoint = uri.ToString();
                sResponse = "";
                sResponse = client.MakeRequest(acc);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(sResponse.ToString());
                XmlNodeList xnList = xdoc.SelectNodes("/response/subscription");
                foreach (XmlNode xn in xnList)
                {
                    //ret = xn.InnerText;
                    status = xn["status"].InnerText;
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                return ret;
                //   throw new Exception("GetSubscription Exception: " + ex.ToString());
            }
            return ret;
        }
        protected static messages GetMessage(RestClient client, string messageID, Account acc)
        {
            messages msg = new messages();

            string contactId = "";
            try
            {
                string sResponse = null;
                Uri uri = new Uri(client.EndPoint + "messages/" + messageID + "");
                client.Method = HttpVerb.GET;
                client.ContentType = "text/xml";
                client.EndPoint = uri.ToString();
                sResponse = "";
                sResponse = client.MakeRequest(acc);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(sResponse.ToString());
                XmlNodeList xnList = xdoc.SelectNodes("/response/message");
                foreach (XmlNode xn in xnList)
                {
                    msg.clientFolderId = xn["clientFolderId"].InnerText;
                    msg.campaignId = xn["campaignId"].InnerText;
                    msg.clientId = xn["clientId"].InnerText;
                    msg.createDate = xn["createDate"].InnerText;
                    msg.htmlBody = xn["htmlBody"].InnerText;
                    msg.messageId = xn["messageId"].InnerText;
                    msg.messageName = xn["messageName"].InnerText;
                    msg.messageType = xn["messageType"].InnerText;
                    msg.subject = xn["subject"].InnerText;
                    msg.textBody = xn["textBody"].InnerText;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMessage Exception: " + ex.ToString());
            }
            return msg;
        }
        protected static bool UpdateContact(RestClient client, int contactid, Account acc, Contact con)
        {
            bool retBool = false;
            try
            {
                var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();


                //testing
                // Contact getcon = new Contact();
                //getcon = GetContact(client, contactid, acc);
                // Update Contact
                ContactWithId c = new ContactWithId();
                //sub.contactId = Convert.ToInt32(contactid);
                //sub.late = Convert.ToInt32(con.late);
                //sub.indiv_id = con.indiv_id;
                //sub.street2 = "Test Street 2";
                c.contactId = contactid;
                c.email = con.email;
                c.status = "normal";
                c.firstName = con.firstName;
                c.lastName = con.lastName;
                c.prefix = "";
                c.state = con.state;
                c.street = con.street;
                c.postalCode = con.postalCode;
                c.phone = "";
                c.city = con.city;
                c.indiv_id = con.indiv_id;
               // c.offer_code = con.offer_code;
               // c.child_name = con.child_name;
                //string json = jsonSerializer.Serialize(c);
                var s = new JavaScriptSerializer();
                string jsonClient = s.Serialize(c);
                //WebOperationContext.Current.OutgoingResponse.ContentType =
                //    "application/json; charset=utf-8";
                //Stream str =  new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(c);
                //JToken t1 = JToken.Parse("{}");
                string str = JToken.Parse(json).ToString();

                string data = "";
                client.PostData = "";
                string sResponse = null;
                Uri uri = new Uri(client.EndPoint + "contacts/" + contactid + "");
                //Uri uri = new Uri(client.EndPoint + contactid + "");
                client.Method = HttpVerb.POST;
                client.ContentType = "application/json";
                client.EndPoint = uri.ToString();
                //json = "[" + json + "]";
                data = json;
                sResponse = "";
                client.PostData = json;
                sResponse = client.MakeRequest(acc);
                retBool = true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateContact Exception: " + ex.ToString());
            }
            return retBool;
        }
        //public HttpResponseMessage ReturnPureJson(object responseModel)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    string Json = Newtonsoft.Json.JsonConvert.SerializeObject(c);

        //    string jsonClient = Json.Encode(responseModel);
        //    byte[] resultBytes = Encoding.UTF8.GetBytes(jsonClient);
        //    response.Content = new StreamContent(new MemoryStream(resultBytes));
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

        //    return response;
        //}
        protected List<Account> AccountList(string Environment)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            IEnumerable results = currentSession.Connection.Query(@"SELECT APP_ID, USERNAME, PASSWORD, CLIENT_FOLDER_ID, NEW_LIST_ID, OLD_LIST_ID FROM REF_ICONTACT_ACCT
                                                                    WHERE ENVIRONMENT = @env", new { @env = Environment });

            List<Account> accList = new List<Account>();
            foreach (dynamic row in results)
            {
                accList.Add(new Account(row.APP_ID, row.USERNAME, row.PASSWORD, row.CLIENT_FOLDER_ID, row.NEW_LIST_ID, row.OLD_LIST_ID));
            }

            return accList;

        }
        public static List<CustomerSearch> GetCustomerDetails(string strIndivID)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            List<CustomerSearch> customerSearchList = new List<CustomerSearch>();
            try
            {
                IEnumerable results = currentSession.Connection.Query(@" exec usp_get_customer_info @IndivID", new { @IndivID = strIndivID });
                foreach (dynamic row in results)
                {
                    customerSearchList.Add(new CustomerSearch(row.INDIV_ID, row.MRN, row.NAME_PREFIX, row.FIRST_NAME, row.MID_NAME, row.LAST_NAME, row.NAME_SUFX, row.GENDER, row.BIRTH_DATE,
                                                              row.ADDRESS1, row.ADDRESS2, row.CITY, row.STATE, row.ZIP, row.ZIP4, row.STATUS, row.USPS_STATUS, row.USPS_OPT_CD, row.SMS_NUMBER,
                                                              row.PHONE, row.EMAIL, row.SMS_STATUS, row.EMAIL_STATUS, row.PHONE_STATUS, row.INSURANCE_PROVIDER, row.PHONE_OPT_CD, row.EMAIL_OPT_CD,
                                                              row.TEXT_MESSAGE_OPT_CD, row.EXAM_SCHEDULE, row.FIRST_RESPONSE_DATE, row.MAM_PATIENT_TYPE, row.CALLBACK_STATUS, row.RETURN_DATE, row.FULL_NAME,
                                                              row.CITY_STATE_ZIP, row.RECORD_CREATE_DATE, row.RETURN_EXAM_TYPE, row.LAST_TRANS_DATE, row.LAST_UPDATE_DATE, row.EXTERNAL_REF_NUMBER,
                                                              row.LOAD_PG_ID, row.CM_CREATE_DATE, row.YOB, row.EFirst_Name, row.ELast_Name, row.EAddress1, row.EBirth_Date, row.MOB));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetCustomerDetails Exception: " + ex.ToString());
            }
            return customerSearchList;
        }
        protected static CampaignReturn CampaignHistoryProcess(MessageModel mm)
        {
            CampaignReturn retUser = new CampaignReturn();
            CampaignHistoryCustomerReturn chr = new CampaignHistoryCustomerReturn();
            
            try
            {
                chr = GetCustomerCampaignInfo(mm);
                retUser = SaveCampaignHistory(chr.campaignHistoryData);
            }
            catch (Exception ex)
            {
                throw new Exception("CampaignHistoryProcess Exception: " + ex.ToString());
            }
            return retUser;
        }
        protected static CampaignHistoryCustomerReturn GetCustomerCampaignInfo(MessageModel chkUser)
        {
            CampaignHistoryCustomerReturn user = new CampaignHistoryCustomerReturn();
            CampaignHistoryData cd = new CampaignHistoryData();
            Profile pro = new Profile();
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                List<CustomerSearch> searchlist = new List<CustomerSearch>();
                searchlist = GetCustomerDetails(chkUser.INDIV_ID.ToString());
                //user.CustomerSearchList = searchlist;
                foreach (CustomerSearch row in searchlist)
                {
                    cd.FIRST_NAME = row.FIRST_NAME;
                    cd.INDIV_ID = row.INDIV_ID;
                    cd.MFID = chkUser.MFID;
                    cd.CLIENT = "CRAD";
                    cd.NAME_PREFIX = row.NAME_PREFIX;
                    cd.FIRST_NAME = row.FIRST_NAME;
                    cd.MID_NAME = row.MID_NAME;
                    cd.LAST_NAME = row.LAST_NAME;
                    cd.NAME_SUFX = row.NAME_SUFX;
                    cd.GENDER = row.GENDER;
                    cd.BIRTH_DATE = row.BIRTH_DATE;
                    cd.ADDRESS1 = row.ADDRESS1;
                    cd.ADDRESS2 = row.ADDRESS2;
                    cd.CITY = row.CITY;
                    cd.STATE = row.STATE;
                    cd.ZIP = row.ZIP;
                    cd.ZIP4 = row.ZIP4;
                    cd.CITYSTATEZIP = row.CITY_STATE_ZIP;
                    cd.FULLNAME = row.FULL_NAME;
                    cd.STATUS = row.STATUS;
                    cd.USPS_STATUS = row.USPS_STATUS;
                    cd.USPS_OPT_CD = row.USPS_OPT_CD;
                    cd.PHONE = row.PHONE;
                    cd.EMAIL = row.EMAIL;
                    cd.TEXT_MESSAGE = row.SMS_NUMBER;
                    cd.EMAIL_STATUS = row.EMAIL_STATUS;
                    cd.PHONE_STATUS = row.PHONE_STATUS;
                    cd.TEXT_MESSAGE_STATUS = row.SMS_STATUS;
                    cd.PHONE_OPT_CD = row.PHONE_OPT_CD;
                    cd.EMAIL_OPT_CD = row.EMAIL_OPT_CD;
                    cd.TEXT_MESSAGE_OPT_CD = row.SMS_OPT_CD;
                    cd.CHANNEL = chkUser.CHANNEL;
                    cd.BLAST_DATE = DateTime.Today;
                    cd.EXTRACT_DATE = DateTime.Today;
                    cd.EXTERNAL_REF_NUM = row.EXTERNAL_REF_NUMBER;
                    cd.PG_ID = chkUser.PG_ID;
                }
                user.campaignHistoryData = cd;
            }
            catch (Exception ex)
            {
                throw new Exception("GetCustomerCampaignInfo Exception: " + ex.ToString());
            }
            return user;
        }

        protected static CampaignReturn SaveCampaignHistory(CampaignHistoryData chkUser)
        {
            CampaignReturn retuser = new CampaignReturn();
            //ConsumerClass CustomerObject = new ConsumerClass();
            //AddrStndReturn retaddr = new AddrStndReturn();
            //AgeVerifData avfdata = new AgeVerifData();
            //PinEntryData pindata = new PinEntryData();
            //CampaignHistoryCustomerReturn pinreturn = new CampaignHistoryCustomerReturn();
            ProfileData profile = new ProfileData();

            try
            {
                var GUID = "963b4e28-15a2-46aa-bbc7-3dcbc44c62b4";

                var currentSession = sessionFactory.GetCurrentSession();
                //CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
                //pindata.pin = chkUser.INDIV_ID.ToString();
                //pinreturn = GetCustomerInfo(pindata);
                //string strResponseCode = "SPWEB16RES";
                string strChannel = "TXT";
               // string strResponseCode = GetResponseCode(strChannel);
                //if (pinreturn.code == PinEntryStatusCodes.Success)
                //{
                //    chkUser.FIRST_NAME = pinreturn.pinProfile.first_name;
                //    chkUser.LAST_NAME = pinreturn.pinProfile.last_name;
                //    chkUser.ADDRESS1 = pinreturn.pinProfile.address1;
                //    chkUser.ADDRESS2 = pinreturn.pinProfile.address2;
                //    chkUser.CITY = pinreturn.pinProfile.city;
                //    chkUser.STATE = pinreturn.pinProfile.state;
                //    chkUser.ZIP = pinreturn.pinProfile.zip;
                //    chkUser.BIRTH_DATE = pinreturn.pinProfile.birth_date;
                //    chkUser.EMAIL = pinreturn.pinProfile.email;
                //    chkUser.PHONE = pinreturn.pinProfile.phone;

                //}


                //chkUser.INDIV_ID = CustomerObject.GetNextIndivId(GUID);
                //avfdata.ADDRESS1 = chkUser.ADDRESS1;
                //avfdata.ADDRESS2 = chkUser.ADDRESS2;
                //avfdata.CITY = chkUser.CITY;
                //avfdata.STATE = chkUser.STATE;
                //avfdata.ZIP = chkUser.ZIP;
                //retaddr = AddressStandardize(avfdata);

                var parameters = new DynamicParameters();
                parameters.Add("@MFID", chkUser.MFID);
                parameters.Add("@SEED", chkUser.SEED);
                parameters.Add("@CLIENT", chkUser.CLIENT);
                parameters.Add("@PG_ID", chkUser.PG_ID);
                parameters.Add("@EXTRACT_DATE", chkUser.EXTRACT_DATE);
                parameters.Add("@CHECK_DIGIT", chkUser.CHECK_DIGIT);
                parameters.Add("@PIN", chkUser.PIN);
                parameters.Add("@EXTERNAL_REF_NUM", chkUser.EXTERNAL_REF_NUM);
                parameters.Add("@NAME_PREFIX", chkUser.NAME_PREFIX);
                parameters.Add("@FIRST_NAME", chkUser.FIRST_NAME);
                parameters.Add("@LAST_NAME", chkUser.LAST_NAME);
                parameters.Add("@MID_NAME", chkUser.MID_NAME);
                parameters.Add("@NAME_SUFX", chkUser.NAME_SUFX);
                parameters.Add("@FULLNAME", chkUser.FULLNAME);
                parameters.Add("@GENDER", chkUser.GENDER);
                parameters.Add("@BIRTH_DATE ", chkUser.BIRTH_DATE);
                parameters.Add("@ADDRESS1 ", chkUser.ADDRESS1);
                parameters.Add("@ADDRESS2 ", chkUser.ADDRESS2);
                parameters.Add("@CITY", chkUser.CITY);
                parameters.Add("@STATE", chkUser.STATE);
                parameters.Add("@ZIP", chkUser.ZIP);
                parameters.Add("@ZIP4", chkUser.ZIP4);
                parameters.Add("@CITYSTATEZIP", chkUser.CITYSTATEZIP);
                parameters.Add("@EMAIL_OPT_CODE", chkUser.EMAIL_OPT_CD);
                parameters.Add("@USPS_OPT_CODE", chkUser.USPS_OPT_CD);
                parameters.Add("@TEXT_MSG_OPT_CD", chkUser.TEXT_MESSAGE_OPT_CD);
                parameters.Add("@PHONE", chkUser.PHONE);
                parameters.Add("@EMAIL", chkUser.EMAIL);
                parameters.Add("@SIGNATURE", chkUser.SIGNATURE);
                parameters.Add("@HHKEY", chkUser.HHKEY);
                parameters.Add("@STATUS", chkUser.STATUS);
                parameters.Add("@EMAIL_STATUS", chkUser.EMAIL_STATUS);
                parameters.Add("@USPS_STATUS", chkUser.USPS_STATUS);
                parameters.Add("@UNDELIV_FLG", chkUser.UNDELIV_FLG);
                parameters.Add("@SIGNATURE", chkUser.SIGNATURE);
                parameters.Add("@UNDELIV_REASON_CD", chkUser.UNDELIV_REASON_CD);
                parameters.Add("@CHANNEL", chkUser.CHANNEL);
                parameters.Add("@EMAIL_CPGN_ID", chkUser.EMAIL_CPGN_ID);
                parameters.Add("@BLAST_DATE", chkUser.BLAST_DATE);
                parameters.Add("@EMAIL_OPEN", chkUser.EMAIL_OPEN);
                parameters.Add("@EMAIL_BOUNCE", chkUser.EMAIL_BOUNCE);
                parameters.Add("@EMAIL_CLICK", chkUser.EMAIL_CLICK);
                parameters.Add("@EMAIL_UNSUB", chkUser.EMAIL_UNSUB);
                parameters.Add("@EMAIL_COMP", chkUser.EMAIL_COMP);
                parameters.Add("@EMAIL_LAST_TRANS", chkUser.EMAIL_LAST_TRANS);
                parameters.Add("@INDIV_ID", Convert.ToInt32(chkUser.INDIV_ID));
                currentSession.Connection.Execute("usp_save_campaign_history", parameters, commandType: CommandType.StoredProcedure);
                //retuser.indiv_id = chkUser.INDIV_ID;
                retuser.code = GenericStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("SaveCampaignHistoryException: " + ex.ToString());
            }

            return retuser;

        }

    }
    //public class TextMessenger : Messenger
    //{
    //    protected static List<Account> accList = new List<Account>();
    //    protected static List<MessageModel> emailList = new List<MessageModel>();
    //    protected static List<DBMessage> msgList = new List<DBMessage>();

    //    public TextMessenger()
    //    {

    //    }
    //    public TextMessenger(ISessionFactory sessionFactory)
    //    {
    //        Messenger.sessionFactory = sessionFactory;
    //    }
    //    public override MessageReturn GetMessages(MessageData chkUser, BusinessModel bm)
    //    {
    //        MessageReturn retdata = new MessageReturn();
    //        return retdata;
    //    }
    //    public override MessageReturn ProcessMessages(MessageData chkUser, BusinessModel bm)
    //    {
    //        MessageReturn retdata = new MessageReturn();
    //        //CallfireClient Client = new CallfireClient("a59672830bd6", "7959b638d85859f5");
    //        var client = new RestSharp.RestClient("https://www.callfire.com/api/1.1/rest/");
    //        client.Authenticator = new HttpBasicAuthenticator("a59672830bd6", "7959b638d85859f5");
    //        var request = new RestRequest("text", Method.POST);
    //        EmailReturn user = new EmailReturn();
    //        DataSet ds = new DataSet();
    //        DataTable dt;
    //        string EmailFrom = "";
    //        string EmailSubject = "";
    //        string EmailMessage = "";
    //        string EmailBody = "";
    //        string retValue = "";
    //        try
    //        {
    //            var currentSession = sessionFactory.GetCurrentSession();
    //            ConsumerClass CustomerObject = new ConsumerClass();
    //            CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
    //            processGroupId = CreateProcessGroup("RE", "R", "Starting Text Messenger Process", 0, 0);
    //            CreateProcessGroupHistoryEntry(processGroupId, "Start Messenger Process", 0, 1000);
    //            ds = CustomerObject.GetProcessEmailElements(bm.WebID);
    //            dt = ds.Tables[0];
    //            if (!IsEmpty(ds))
    //            {
    //                foreach (DataRow row in dt.Rows)
    //                {
    //                    EmailFrom = row["ProcessEmailFrom"].ToString();
    //                    EmailSubject = row["ProcessEmailSubject"].ToString();
    //                    EmailMessage = row["ProcessEmailMessage"].ToString();
    //                    //if (string.IsNullOrEmpty(chkUser.message))
    //                    //{
    //                    //    EmailBody = chkUser.rec_name + "," + "\r\n" + "\r\n" + EmailMessage + "\r\n";
    //                    //}
    //                    //else
    //                    //{
    //                    //    EmailBody = chkUser.rec_name + "," + "\r\n" + "\r\n" + chkUser.message + "\r\n";
    //                    //}

    //                }
    //                //if (rtnBirthday)
    //                //{
    //                //    client.EndPoint = requestURI;
    //                //    rtnAnniversary = AnniversaryProcess(chkUser, bm, client, acc);
    //                //}
    //                //user.Anniversary_Count = AnniversaryTotal;
    //                user.Message_Count = MessageTotal;
    //                user.status = "success";
    //                user.code = GenericStatusCodes.Success;
    //                user.desc = retValue;
    //            }
    //            CheckStatus = 0;
    //            //Console.WriteLine("Start MailMessages Process at {0}", dt.ToString());
    //            DateTime MessageDT = DateTime.Today;
    //            string Channel = "SMS";
    //            Contact c;
    //            TextRecipient tr;
    //            var recipients = new List<TextRecipient>();
    //            //string requestURI = client.EndPoint;
    //            bool retSub = false;
    //            bool blContacts = false;

    //            List<Contact> contactList = new List<Contact>();

    //            DBMessage msg = new DBMessage();
    //            msgList = MessageList(chkUser.env, Channel);
    //            DateTime dtToday = DateTime.Today;
    //            DateTime? dateOrNull;
    //            DateTime newSelectedDate = DateTime.Today;
    //            TimeSpan ts;
    //            foreach (DBMessage mg in msgList)
    //            {
    //                //    if (mg.SEQ == sgw)
    //                //    {
    //                //        MessageID = mg.MESSAGE_ID.ToString();
    //                //        break;
    //                //    }
    //                recipients = new List<TextRecipient>();
    //                emailList = CustMessageList(Channel);
    //                int msgCount = 0;
    //                string strRecipients = "";
    //                StringBuilder sb = new StringBuilder();
    //                foreach (MessageModel em in emailList)
    //                {
    //                    c = new Contact();
    //                    c.email = em.EMAIL;
    //                    c.status = "normal";
    //                    c.firstName = em.FIRST_NAME;
    //                    c.lastName = em.LAST_NAME;
    //                    c.prefix = "";
    //                    c.state = em.STATE;
    //                    c.street = em.ADDRESS1;
    //                    c.postalCode = em.ZIP;
    //                    c.phone = "";
    //                    c.city = em.CITY;
    //                    c.indiv_id = em.INDIV_ID.ToString();
    //                    dateOrNull = em.PROCEDURE_DATE;
    //                    if (dateOrNull != null)
    //                    {
    //                        newSelectedDate = dateOrNull.Value;
    //                    }
    //                    ts = dtToday - newSelectedDate;
    //                    //c.late = ts.TotalDays.ToString();
    //                    c.test1_date = em.TEST1_DATE.ToString("MMMM dd, yyyy");
    //                    c.test2_date = em.TEST2_DATE.ToString("MMMM dd, yyyy");
    //                    var checkDate = new DateTime(MessageDT.Year, em.MESSAGE_DT.Value.Month, em.MESSAGE_DT.Value.Day);
    //                    if (checkDate == MessageDT && em.TEST_NUMBER == mg.TEST_NUMBER && em.MESSAGE_SEQ == mg.MESSAGE_SEQ)
    //                    {
    //                        string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'I' where MD_RECNUM = " + em.MD_RECNUM;
    //                        ExecuteSQL(sql);
    //                        //client.EndPoint = requestURI;
    //                        //retSub = Subscribe(client, acc, c, bm.BNewListID);
    //                        //tr = new TextRecipient { Message = mg.MESSAGE_TEXT, PhoneNumber = em.PHONE };
    //                        //recipients.Add(tr);
    //                        msgCount++;
    //                        if (msgCount == 1)
    //                        {
    //                            sb.Append(em.PHONE);
    //                        }
    //                        else
    //                        {
    //                            sb.Append(", ");
    //                            sb.Append(em.PHONE);
    //                        }
    //                        MessageTotal++;
    //                        blContacts = true;
    //                        contactList.Add(c);
    //                    }
    //                }
    //                // send texts
    //                if (msgCount > 0)
    //                {
    //                    //IList<Text> texts = Client.TextsApi.Send(recipients);
    //                    request.AddParameter("Type", "TEXT");
    //                    request.AddParameter("To", sb.ToString());
    //                    request.AddParameter("Message", mg.MESSAGE_TEXT);
    //                    var response = client.Execute(request);
    //                    string content = response.Content;
    //                }
    //                CreateProcessGroupHistoryEntry(processGroupId, "Get TextMessages List", MessageTotal, 1010);
    //                    List<MessageModel> relist = new List<MessageModel>();
    //                    MessageModel re;
    //                    foreach (Contact item in contactList)
    //                    {
    //                        re = new MessageModel();
    //                        re.EMAIL = item.email;
    //                        relist.Add(re);
    //                    }
    //                    CreateProcessGroupHistoryEntry(processGroupId, "Copying List To Completed", 0, 1030);
    //                    //string retList = MoveToList(client, acc, bm.BNewListID, bm.BOldListID, relist);
    //                    // Cleanup Contacts On Database that were sent the email and set to processed
    //                    foreach (MessageModel em in emailList)
    //                    {
    //                        var checkDate = new DateTime(MessageDT.Year, em.MESSAGE_DT.Value.Month, em.MESSAGE_DT.Value.Day);
    //                        if (checkDate == MessageDT && em.TEST_NUMBER == mg.TEST_NUMBER && em.MESSAGE_SEQ == mg.MESSAGE_SEQ)
    //                        {
    //                            string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'P', ACTUAL_DT = GETDATE() where MD_RECNUM = " + em.MD_RECNUM;
    //                            ExecuteSQL(sql);
    //                        }

    //                    }
    //                    //        foreach (Contact co in contactList)
    //                    //{
    //                    //    string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'P' where CHANNEL = '" + Channel + "' and STATUS = 'I' and email = '" + co.email + "'";
    //                    //}
    //                }
    //                blContacts = false;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("ProcessMessages Exception: " + ex.ToString());
    //        }
    //        EndProcessGroup(processGroupId, "Messenger Process Successful", "C");
    //        CreateProcessGroupHistoryEntry(processGroupId, "Messenger Process Completed", 0, 1100);
    //        return retdata;
    //        return retdata;
    //    }
    //    protected static MessageReturn ProcessTextMessages(MessageData chkUser, BusinessModel bm, RestClient client, Account acc)
    //    {
    //        MessageReturn retdata = new MessageReturn();
    //        return retdata;
    //    }
    //}
    //public class IVRMessenger : Messenger
    //{
    //    protected static List<Account> accList = new List<Account>();
    //    protected static List<MessageModel> emailList = new List<MessageModel>();
    //    protected static List<DBMessage> msgList = new List<DBMessage>();

    //    public IVRMessenger()
    //    {

    //    }
    //    public IVRMessenger(ISessionFactory sessionFactory)
    //    {
    //        Messenger.sessionFactory = sessionFactory;
    //    }
    //    public override MessageReturn GetMessages(MessageData chkUser, BusinessModel bm)
    //    {
    //        MessageReturn retdata = new MessageReturn();
    //        return retdata;
    //    }
    //    public override MessageReturn ProcessMessages(MessageData chkUser, BusinessModel bm)
    //    {
    //        MessageReturn retdata = new MessageReturn();
    //        //CallfireClient Client = new CallfireClient("a59672830bd6", "7959b638d85859f5");
    //        //var client = new RestSharp.RestClient("https://www.callfire.com/api/1.1/rest/");
    //        //client.Authenticator = new HttpBasicAuthenticator("a59672830bd6", "7959b638d85859f5");
    //        //var request = new RestRequest("text", Method.POST);
    //        EmailReturn user = new EmailReturn();
    //        var client = new CallFire_csharp_sdk.API.CallfireClient("a59672830bd6", "7959b638d85859f5", CallfireClients.Rest);
    //        var callClient = client.Call;
    //        var cfBroadcastConfig = new CfIvrBroadcastConfig();
    //        cfBroadcastConfig.FromNumber = "13367939605";

    //        DataSet ds = new DataSet();
    //        DataTable dt;
    //        string EmailFrom = "";
    //        string EmailSubject = "";
    //        string EmailMessage = "";
    //        string EmailBody = "";
    //        string retValue = "";
    //        try
    //        {
    //            var currentSession = sessionFactory.GetCurrentSession();
    //            ConsumerClass CustomerObject = new ConsumerClass();
    //            CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
    //            processGroupId = CreateProcessGroup("RE", "R", "Starting IVR Messenger Process", 0, 0);
    //            CreateProcessGroupHistoryEntry(processGroupId, "Start Messenger Process", 0, 1000);
    //            ds = CustomerObject.GetProcessEmailElements(bm.WebID);
    //            dt = ds.Tables[0];
    //            if (!IsEmpty(ds))
    //            {
    //                foreach (DataRow row in dt.Rows)
    //                {
    //                    EmailFrom = row["ProcessEmailFrom"].ToString();
    //                    EmailSubject = row["ProcessEmailSubject"].ToString();
    //                    EmailMessage = row["ProcessEmailMessage"].ToString();
    //                    //if (string.IsNullOrEmpty(chkUser.message))
    //                    //{
    //                    //    EmailBody = chkUser.rec_name + "," + "\r\n" + "\r\n" + EmailMessage + "\r\n";
    //                    //}
    //                    //else
    //                    //{
    //                    //    EmailBody = chkUser.rec_name + "," + "\r\n" + "\r\n" + chkUser.message + "\r\n";
    //                    //}

    //                }
    //                //if (rtnBirthday)
    //                //{
    //                //    client.EndPoint = requestURI;
    //                //    rtnAnniversary = AnniversaryProcess(chkUser, bm, client, acc);
    //                //}
    //                //user.Anniversary_Count = AnniversaryTotal;
    //                user.Message_Count = MessageTotal;
    //                user.status = "success";
    //                user.code = GenericStatusCodes.Success;
    //                user.desc = retValue;
    //            }
    //            CheckStatus = 0;
    //            //Console.WriteLine("Start MailMessages Process at {0}", dt.ToString());
    //            DateTime MessageDT = DateTime.Today;
    //            string Channel = "IVR";
    //            Contact c;
    //            TextRecipient tr;
    //            var recipients = new List<TextRecipient>();
    //            //string requestURI = client.EndPoint;
    //            bool retSub = false;
    //            bool blContacts = false;

    //            List<Contact> contactList = new List<Contact>();

    //            DBMessage msg = new DBMessage();
    //            msgList = MessageList(chkUser.env, Channel);
    //            DateTime dtToday = DateTime.Today;
    //            DateTime? dateOrNull;
    //            DateTime newSelectedDate = DateTime.Today;
    //            TimeSpan ts;
    //            foreach (DBMessage mg in msgList)
    //            {
    //                //    if (mg.SEQ == sgw)
    //                //    {
    //                //        MessageID = mg.MESSAGE_ID.ToString();
    //                //        break;
    //                //    }
    //                recipients = new List<TextRecipient>();
    //                emailList = CustMessageList(Channel);
    //                int msgCount = 0;
    //                string strRecipients = "";
    //                StringBuilder sb = new StringBuilder();
    //                foreach (MessageModel em in emailList)
    //                {
    //                    c = new Contact();
    //                    c.email = em.EMAIL;
    //                    c.status = "normal";
    //                    c.firstName = em.FIRST_NAME;
    //                    c.lastName = em.LAST_NAME;
    //                    c.prefix = "";
    //                    c.state = em.STATE;
    //                    c.street = em.ADDRESS1;
    //                    c.postalCode = em.ZIP;
    //                    c.phone = "";
    //                    c.city = em.CITY;
    //                    c.indiv_id = em.INDIV_ID.ToString();
    //                    dateOrNull = em.PROCEDURE_DATE;
    //                    if (dateOrNull != null)
    //                    {
    //                        newSelectedDate = dateOrNull.Value;
    //                    }
    //                    ts = dtToday - newSelectedDate;
    //                    //c.late = ts.TotalDays.ToString();
    //                    c.test1_date = em.TEST1_DATE.ToString("MMMM dd, yyyy");
    //                    c.test2_date = em.TEST2_DATE.ToString("MMMM dd, yyyy");
    //                    var checkDate = new DateTime(MessageDT.Year, em.MESSAGE_DT.Value.Month, em.MESSAGE_DT.Value.Day);
    //                    if (checkDate == MessageDT && em.TEST_NUMBER == mg.TEST_NUMBER && em.MESSAGE_SEQ == mg.MESSAGE_SEQ)
    //                    {
    //                        string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'I' where MD_RECNUM = " + em.MD_RECNUM;
    //                        ExecuteSQL(sql);
    //                        //client.EndPoint = requestURI;
    //                        //retSub = Subscribe(client, acc, c, bm.BNewListID);
    //                        //tr = new TextRecipient { Message = mg.MESSAGE_TEXT, PhoneNumber = em.PHONE };
    //                        //recipients.Add(tr);
    //                        msgCount++;
    //                        if (msgCount == 1)
    //                        {
    //                            sb.Append(em.PHONE);
    //                        }
    //                        else
    //                        {
    //                            sb.Append(", ");
    //                            sb.Append(em.PHONE);
    //                        }
    //                        MessageTotal++;
    //                        blContacts = true;
    //                        contactList.Add(c);
    //                    }
    //                }
    //                // send texts
    //                if (msgCount > 0)
    //                {
    //                    //IList<Text> texts = Client.TextsApi.Send(recipients);

    //                    //request.AddParameter("Type", "TEXT");
    //                    //request.AddParameter("To", sb.ToString());
    //                    //request.AddParameter("Message", mg.MESSAGE_TEXT);
    //                    //var response = client.Execute(request);
    //                    //string content = response.Content;

    //                    cfBroadcastConfig.DialplanXml = mg.MESSAGE_TEXT;
    //                    var toNumber = new CfToNumber();
    //                    toNumber.Value = sb.ToString();
    //                    var sendCall = new CfSendCall();
    //                    sendCall.Type = CfBroadcastType.Ivr;
    //                    sendCall.ToNumber = new[] { toNumber };
    //                    sendCall.Item = cfBroadcastConfig;
    //                    var id = callClient.SendCall(sendCall);

    //                }
    //                CreateProcessGroupHistoryEntry(processGroupId, "Get IVRMessages List", MessageTotal, 1010);
    //                List<MessageModel> relist = new List<MessageModel>();
    //                MessageModel re;
    //                foreach (Contact item in contactList)
    //                {
    //                    re = new MessageModel();
    //                    re.EMAIL = item.email;
    //                    relist.Add(re);
    //                }
    //                CreateProcessGroupHistoryEntry(processGroupId, "Copying List To Completed", 0, 1030);
    //                //string retList = MoveToList(client, acc, bm.BNewListID, bm.BOldListID, relist);
    //                // Cleanup Contacts On Database that were sent the email and set to processed
    //                foreach (MessageModel em in emailList)
    //                {
    //                    var checkDate = new DateTime(MessageDT.Year, em.MESSAGE_DT.Value.Month, em.MESSAGE_DT.Value.Day);
    //                    if (checkDate == MessageDT && em.TEST_NUMBER == mg.TEST_NUMBER && em.MESSAGE_SEQ == mg.MESSAGE_SEQ)
    //                    {
    //                        string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'P', ACTUAL_DT = GETDATE() where MD_RECNUM = " + em.MD_RECNUM;
    //                        ExecuteSQL(sql);
    //                    }

    //                }
    //                //        foreach (Contact co in contactList)
    //                //{
    //                //    string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'P' where CHANNEL = '" + Channel + "' and STATUS = 'I' and email = '" + co.email + "'";
    //                //}
    //            }
    //            blContacts = false;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("ProcessMessages Exception: " + ex.ToString());
    //        }
    //        EndProcessGroup(processGroupId, "Messenger Process Successful", "C");
    //        CreateProcessGroupHistoryEntry(processGroupId, "Messenger Process Completed", 0, 1100);
    //        return retdata;
    //        return retdata;
    //    }
    //    protected static MessageReturn ProcessTextMessages(MessageData chkUser, BusinessModel bm, RestClient client, Account acc)
    //    {
    //        MessageReturn retdata = new MessageReturn();
    //        return retdata;
    //    }
    //}
}
