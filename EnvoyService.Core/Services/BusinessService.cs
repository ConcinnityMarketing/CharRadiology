using System;
using System.Diagnostics;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;
using NHibernate;
using System.Text;
using System.Data.SqlClient;
using System.Dynamic;
using System.Xml;
using System.Security;
using EnvoyService.Core.Models;
using EnvoyService.Core.Enums;
using CMParms;
using System.Net;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Timers;
using cmextwebsvc.BusinessLayer;
using System.Globalization;
using Microsoft.VisualBasic;
using MailBee;
using MailBee.Mime;
using MailBee.SmtpMail;
using MailBee.Pop3Mail;
using MailBee.DnsMX;
using Cobisi.EmailVerify;
using cmextwebsvc.PRODAVLayer;
using CryptSharp;

namespace EnvoyService.Core.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly ISessionFactory sessionFactory;
        public BusinessService(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
        private string CheckGUID = "2932ff6d-0d0f-442d-b039-4aaace4fbaa3";
        public string GetGUID()
        {
            return CheckGUID;
        }
        protected int GetIndivIDbyPhone(string Phone)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            return currentSession.Connection.Query<int>("select indiv_id from customer_profile where phone = '" + Phone + "'").FirstOrDefault();
        }
        protected void ExecuteSQL(string sql)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            var parameters = new DynamicParameters();
            parameters = null;
            currentSession.Connection.Execute(sql, parameters, commandTimeout: 10 * 60, commandType: CommandType.Text);
        }

        public AdminReturn GetAdmins(MessageData chkUser, BusinessModel bm)
        {
            AdminReturn user = new AdminReturn();
            List<Admin> adminlist = new List<Admin>();
            try
            {
                adminlist = AdminList();
                user.status = "success";
                user.adminList = adminlist;
                user.code = GenericStatusCodes.Success;
                user.desc = "Got Admin List";
            }
            catch (Exception ex)
            {
                throw new Exception("GetAdmins Exception: " + ex.ToString());
            }
            return user;
        }
        public MessageReturn MessageProcess(MessageData chkUser, BusinessModel bm)
        {
            // chkUser.env = "QA";
            bool blExpand = false;
            MessageReturn user = new MessageReturn();
            CRADExpander exp = new CRADExpander(sessionFactory);
            MailMessenger mm = new MailMessenger(sessionFactory);
            //TextMessenger tm = new TextMessenger(sessionFactory);
            //IVRMessenger im = new IVRMessenger(sessionFactory);
            try
            {
                //user = LogClientResults(bm);
                blExpand = exp.ExpandCommunication(); //uncomment after testing
                user = mm.ProcessMessages(chkUser, bm);
                //user = tm.ProcessMessages(chkUser, bm);
                //user = im.ProcessMessages(chkUser, bm);
            }
            catch (Exception ex)
            {
                throw new Exception("MessageProcess Exception: " + ex.ToString());
            }
            // string strURISufx = "/c/";
            return user;
        }
        protected bool IsEmpty(DataSet dataSet)
        {
            foreach (DataTable Table in dataSet.Tables)
            {
                return false;
            }
            return true;
        }

        protected List<Admin> AdminList()
        {
            var currentSession = sessionFactory.GetCurrentSession();
            IEnumerable results = currentSession.Connection.Query(@"SELECT EMAIL, FIRST_NAME, LAST_NAME FROM RECUR_EMAIL_ADMIN
                                                                    WHERE STATUS = 'A'");

            List<Admin> accList = new List<Admin>();
            foreach (dynamic row in results)
            {
                accList.Add(new Admin(row.EMAIL, row.FIRST_NAME, row.LAST_NAME));
            }

            return accList;


        }
        protected MatchKey CreateMatchKey(ProfileData chkUser)
        {
            MatchKey retKey = new MatchKey();
            //ConsumerClass CustomerObject = new ConsumerClass();
            Profile user = new Profile();
            try
            {
                //user = GetCustomerInfo(Convert.ToInt32(chkUser.INDIV_ID));
                //user.indiv_id = Convert.ToInt32(chkUser.INDIV_ID);
                string zip4 = "";
                retKey.namekey = string.Empty;
                retKey.hhkey = string.Empty;
                chkUser.LAST_NAME = string.IsNullOrEmpty(chkUser.LAST_NAME) ? "|" : chkUser.LAST_NAME;
                if (chkUser.LAST_NAME.Length < 3)
                {
                    for (int i = 1; i <= 3 - chkUser.LAST_NAME.Length; i++)
                    {
                        chkUser.LAST_NAME += "|";
                    }
                }
                chkUser.FIRST_NAME = string.IsNullOrEmpty(chkUser.FIRST_NAME) ? "|" : chkUser.FIRST_NAME;
                if (chkUser.FIRST_NAME.Length < 3)
                {
                    for (int i = 1; i <= 3 - chkUser.FIRST_NAME.Length; i++)
                    {
                        chkUser.FIRST_NAME += "|";
                    }
                }
                retKey.namekey = chkUser.ZIP + Convert.ToDateTime(chkUser.BIRTH_DATE).ToString("yyyyMMdd") + chkUser.LAST_NAME.Substring(0, 3).ToUpper() + chkUser.FIRST_NAME.Substring(0, 2).ToUpper();
                string strAddress1 = string.IsNullOrEmpty(chkUser.ADDRESS1) ? "|" : chkUser.ADDRESS1;
                string strAddress2 = string.IsNullOrEmpty(chkUser.ADDRESS2) ? "|" : chkUser.ADDRESS2;
                string strZip4 = string.IsNullOrEmpty(chkUser.ZIP4) ? "0000" : zip4;
                string strTempHouseNum = strAddress1.IndexOf(" ") <= 0 ? "|" : strAddress1.Substring(0, strAddress1.IndexOf(" "));
                string strHouseNum = string.IsNullOrEmpty(strTempHouseNum) ? "|" : strTempHouseNum;
                string strHHKey = user.zip + "-" + strZip4 + "-" + strHouseNum + "-" + strAddress2;

                retKey.hhkey = strHHKey.Length > 60 ? strHHKey.Substring(0, 60) : strHHKey;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateMatchKey Exception: " + ex.ToString());
            }
            return retKey;
        }
        protected int CheckDupCustomer(ProfileData chkUser)
        {
            int MatchID = 0;
            var currentSession = sessionFactory.GetCurrentSession();
            try
            {
                MatchKey matchkey = new MatchKey();
                matchkey = CreateMatchKey(chkUser);
                string fixVal = matchkey.namekey;
                fixVal = fixVal.Replace("'", "''");
                var sb = new StringBuilder();
                //if (String.IsNullOrEmpty(chkUser.INDIV_ID))
                //    chkUser.INDIV_ID = "0";
                sb.AppendFormat("SELECT TOP (1) INDIV_ID FROM MATCH_CUSTOMER WHERE MATCHKEY = '");
                //if (!String.IsNullOrEmpty(chkUser.INDIV_ID))
                if (string.IsNullOrEmpty(chkUser.INDIV_ID))
                    sb.Append(fixVal + "' AND INDIV_ID <> " + chkUser.INDIV_ID);
                else
                    sb.Append(fixVal + "'");
                MatchID = currentSession.Connection.Query<int>(sb.ToString()).FirstOrDefault();
                if (MatchID == 0)
                {
                    sb = new StringBuilder();
                    sb.AppendFormat("SELECT TOP (1) INDIV_ID FROM CUSTOMER_PROFILE WHERE EMAIL = '");
                    sb.Append(chkUser.EMAIL.Trim() + "' AND INDIV_ID <> " + chkUser.INDIV_ID + " ORDER BY LAST_UPDATE_DATE DESC");
                    MatchID = currentSession.Connection.Query<int>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckDupCustomer Exception: " + ex.ToString());
            }
            return MatchID;
        }

        public RegisterReturn Register(SignUpData chkUser)
        {
            RegisterReturn retuser = new RegisterReturn();
            ConsumerClass CustomerObject = new ConsumerClass();
            AddrStndReturn retaddr = new AddrStndReturn();
            AgeVerifData avfdata = new AgeVerifData();
            try
            {
                var GUID = "963b4e28-15a2-46aa-bbc7-3dcbc44c62b4";

                var currentSession = sessionFactory.GetCurrentSession();
                //CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
                //chkUser.INDIV_ID = CustomerObject.GetNextIndivId(GUID);

                avfdata.ADDRESS1 = chkUser.ADDRESS1;
                avfdata.ADDRESS2 = chkUser.ADDRESS2;
                avfdata.CITY = chkUser.CITY;
                avfdata.STATE = chkUser.STATE;
                avfdata.ZIP = chkUser.ZIP;
                retaddr = AddressStandardize(avfdata);

                chkUser.ADDRESS1 = retaddr.ADDRESS1;
                chkUser.ADDRESS2 = retaddr.ADDRESS2;
                chkUser.CITY = retaddr.CITY;
                chkUser.STATE = retaddr.STATE;
                chkUser.ZIP = retaddr.ZIP;
                chkUser.ZIP4 = retaddr.ZIP4;

                DateTime dateCheck;
                string strDOB;

                chkUser.BIRTH_DATE = string.IsNullOrEmpty(chkUser.BIRTH_DATE) || !(DateTime.TryParse(chkUser.BIRTH_DATE, out dateCheck)) ? DateTime.Today.ToString() : chkUser.BIRTH_DATE;

                //int intIndivID = CheckDupCustomer(chkUser);
                //if (intIndivID == 0)
                //{
                //  retuser = SaveCustProfile(chkUser);
                retuser = SaveTankCustProfile(chkUser);
                if (chkUser.SURVEYS != null)
                {
                    QAData qd = new QAData();
                    qd.q_and_a = chkUser.SURVEYS;
                    qd.TANKRECNUM = retuser.TankRecNum;
                    qd.RESPONSE_CODE = chkUser.RESPONSE_CODE;
                    qd.RESPONSE_DATE = DateTime.Now.ToString();
                    retuser = SaveCustQA(qd);
                }

                    //chkUser.INDIV_ID = retuser.indiv_id.ToString();
                    //}
                    //else
                    //{
                    //    chkUser.INDIV_ID = intIndivID;
                    //    retuser = UpdateCustProfile(chkUser);
                    //}
                    //retuser = SaveCustResponse(chkUser);

                    retuser.code = RegistrationStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("RegisterException: " + ex.ToString());
            }

            return retuser;

        }
        protected RegisterReturn SaveTankCustProfile(SignUpData chkUser)
        {
            RegisterReturn retuser = new RegisterReturn();
            try
            {
                var GUID = "963b4e28-15a2-46aa-bbc7-3dcbc44c62b4";

                var currentSession = sessionFactory.GetCurrentSession();
                var strEmpty = string.Empty;
                //CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
                //chkUser.INDIV_ID = CustomerObject.GetNextIndivId(GUID);
                //avfdata.ADDRESS1 = chkUser.ADDRESS1;
                //avfdata.ADDRESS2 = chkUser.ADDRESS2;
                //avfdata.CITY = chkUser.CITY;
                //avfdata.STATE = chkUser.STATE;
                //avfdata.ZIP = chkUser.ZIP;
                //retaddr = AddressStandardize(avfdata);

                var parameters = new DynamicParameters();
                parameters.Add("@NAME_PREFIX", chkUser.NAME_PREFIX);
                parameters.Add("@FIRST_NAME", chkUser.FIRST_NAME);
                parameters.Add("@LAST_NAME", chkUser.LAST_NAME);
                parameters.Add("@MID_NAME", chkUser.MID_NAME);
                parameters.Add("@NAME_SUFX", chkUser.NAME_SUFX);
                parameters.Add("@GENDER", chkUser.GENDER);
                parameters.Add("@BIRTH_DATE ", Convert.ToDateTime(chkUser.BIRTH_DATE));
                parameters.Add("@ADDRESS1 ", chkUser.ADDRESS1);
                parameters.Add("@ADDRESS2 ", chkUser.ADDRESS2);
                parameters.Add("@CITY", chkUser.CITY);
                parameters.Add("@STATE", chkUser.STATE);
                parameters.Add("@ZIP", chkUser.ZIP);
                parameters.Add("@ZIP4", chkUser.ZIP4);
                parameters.Add("@EMAIL", chkUser.EMAIL);
                parameters.Add("@PHONE", chkUser.PHONE);
                parameters.Add("@SMS_NUMBER", chkUser.TEXT_MESSAGE);
                parameters.Add("@STATUS", "0");
                parameters.Add("@EMAIL_STATUS", "");
                parameters.Add("@USPS_STATUS", "");
                parameters.Add("@PHONE_STATUS", "");
                parameters.Add("@SMS_STATUS", "");
                parameters.Add("@EMAIL_OPT_CD", chkUser.EMAIL_OPT_CD);
                parameters.Add("@USPS_OPT_CD", chkUser.USPS_OPT_CD);
                parameters.Add("@PHONE_OPT_CD", chkUser.PHONE_OPT_CD);
                parameters.Add("@SMS_OPT_CD", chkUser.TEXT_MESSAGE_OPT_CD);
                parameters.Add("@RESPONSE_CODE", chkUser.RESPONSE_CODE);
                parameters.Add("@TankRecnum", dbType: DbType.Int32, direction: ParameterDirection.Output);
                currentSession.Connection.Execute("usp_save_tank_cust_registration", parameters, commandType: CommandType.StoredProcedure);
                retuser.TankRecNum = parameters.Get<int>("@TankRecnum");

               // retuser.indiv_id = Convert.ToInt32(chkUser.INDIV_ID);
                retuser.code = RegistrationStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("SaveTankCustProfile Exception: " + ex.ToString());
            }

            return retuser;

        }
        protected RegisterReturn SaveCustQA(QAData chkUser)
        {
            RegisterReturn retuser = new RegisterReturn();
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                // Store Q & A data
                if (chkUser.q_and_a != null)
                {
                    foreach (Question sv in chkUser.q_and_a)
                    {
                        if (sv.answers != null)
                        {
                            foreach (Answer ans in sv.answers)
                            {
                                // if (ans.answer_code != 0)
                                if (!(ans == null))
                                {
                                    var parameters = new DynamicParameters();
                                    parameters.Add("@TankRecnum", Convert.ToInt32(chkUser.TANKRECNUM));
                                    parameters.Add("@Question", sv.question_code);
                                    parameters.Add("@Answer", ans.answer_desc);
                                    currentSession.Connection.Execute("[sp_save_tank_cust_qa", parameters, commandType: CommandType.StoredProcedure);

                                }
                            }
                        }
                    }
                }
                retuser.code = RegistrationStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("SaveCRTankQA Exception: " + ex.ToString());
            }

            return retuser;

        }

        public PinEntryReturn GetUserPinEntryInfo(PinEntryData chkUser)
        {
            PinEntryReturn user = new PinEntryReturn();
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                ConsumerClass CustomerObject = new ConsumerClass();
                DataSet ds;
                DataTable dt;
                CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
                if (chkUser.pin == null)
                    chkUser.pin = "00";
                if (chkUser.pin.Length < 2)
                    chkUser.pin = "00";
                else
                    chkUser.pin = chkUser.pin.Substring(0, chkUser.pin.Length - 1);
                int intIndivID = Convert.ToInt32(chkUser.pin);
                int IDZip = CustomerObject.CheckIndivIdYOB(intIndivID, chkUser.birth_year);
                if (IDZip > 1)
                {
                    intIndivID = IDZip;
                    IDZip = 0;
                }
                if (IDZip == 0)
                {
                    ds = CustomerObject.GetCustomerNameAddress(intIndivID);
                    if (!IsEmpty(ds))
                    {
                        dt = ds.Tables[0];
                        Profile pro = new Profile();
                        foreach (DataRow row in dt.Rows)
                        {
                            pro.first_name = row["FIRST_NAME"].ToString();
                            pro.last_name = row["LAST_NAME"].ToString();
                            pro.address1 = row["ADDRESS1"].ToString();
                            pro.address2 = row["ADDRESS2"].ToString();
                            pro.city = row["CITY"].ToString();
                            pro.state = row["STATE"].ToString();
                            pro.zip = row["ZIP"].ToString();
                            pro.phone = row["PHONE"].ToString();
                            pro.birth_date = (DateTime)row["BIRTH_DATE"];
                            pro.email = row["EMAIL"].ToString();
                            user.indiv_id = (int)row["INDIV_ID"];
                        }
                        user.pinProfile = pro;
                        user.status = "success";
                        user.code = PinEntryStatusCodes.Success;
                        user.desc = "";
                    }
                    else
                    {
                        user.status = "fail";
                        user.code = PinEntryStatusCodes.Other;
                        user.desc = "Other Error";
                    }
                }
                else
                {
                    user.status = "fail";
                    user.code = PinEntryStatusCodes.InvalidPin;
                    user.desc = "Invalid Pin/Year Combination";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserInfo Exception: " + ex.ToString());
            }
            return user;
        }
        public PinEntryReturn GetCustomerInfo(PinEntryData chkUser)
        {
            PinEntryReturn user = new PinEntryReturn();
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                ConsumerClass CustomerObject = new ConsumerClass();
                DataSet ds;
                DataTable dt;
                CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
                int intIndivID = Convert.ToInt32(chkUser.pin);
                ds = CustomerObject.GetCustomerNameAddress(intIndivID);
                if (!IsEmpty(ds))
                {
                    dt = ds.Tables[0];
                    Profile pro = new Profile();
                    foreach (DataRow row in dt.Rows)
                    {
                        pro.first_name = row["FIRST_NAME"].ToString();
                        pro.last_name = row["LAST_NAME"].ToString();
                        pro.address1 = row["ADDRESS1"].ToString();
                        pro.address2 = row["ADDRESS2"].ToString();
                        pro.city = row["CITY"].ToString();
                        pro.state = row["STATE"].ToString();
                        pro.zip = row["ZIP"].ToString();
                        pro.phone = row["PHONE"].ToString();
                        pro.birth_date = (DateTime)row["BIRTH_DATE"];
                        pro.email = row["EMAIL"].ToString();
                        user.indiv_id = (int)row["INDIV_ID"];
                    }
                    user.pinProfile = pro;
                    user.status = "success";
                    user.code = PinEntryStatusCodes.Success;
                    user.desc = "";
                }
                else
                {
                    user.status = "fail";
                    user.code = PinEntryStatusCodes.Other;
                    user.desc = "Other Error";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserInfo Exception: " + ex.ToString());
            }
            return user;
        }
        //public MessageReturn Notify(Notification TextNotification, BusinessModel bm)
        //{
        //    MessageReturn webreturn = new MessageReturn();
        //    SurveyReturn svyreturn = new SurveyReturn();
        //    ProfileData profile = new ProfileData();
        //    QAData qa = new QAData();
        //    PinEntryData pindata = new PinEntryData();
        //    PinEntryReturn pinreturn = new PinEntryReturn();
        //    StringBuilder sb = new StringBuilder();
        //    TestingData td = new TestingData();
        //    CommunicationReturn cr = new CommunicationReturn();

        //    try
        //    {
        //        webreturn = SaveTextNotificationCallback(TextNotification);
        //        //string strPhone = TextNotification.Text.FromNumber.Substring(1, TextNotification.Text.FromNumber.Length - 1);

        //        sb.Append(TextNotification.Text.FromNumber);
        //        sb.Remove(0, 1);
        //        pindata.env = bm.environ;
        //        pindata.guid = CheckGUID;
        //        pindata.ip = bm.ipaddress;
        //        pindata.pin = GetIndivIDbyPhone(sb.ToString()).ToString();

        //        pinreturn = GetCustomerInfo(pindata);
        //        //string strResponseCode = "SPWEB16RES";
        //        string strChannel = "TXT";
        //        string strResponseCode = GetResponseCode(strChannel);
        //        if (pinreturn.code == PinEntryStatusCodes.Success)
        //        {
        //            profile.FIRST_NAME = pinreturn.pinProfile.first_name;
        //            profile.LAST_NAME = pinreturn.pinProfile.last_name;
        //            profile.ADDRESS1 = pinreturn.pinProfile.address1;
        //            profile.ADDRESS2 = pinreturn.pinProfile.address2;
        //            profile.CITY = pinreturn.pinProfile.city;
        //            profile.STATE = pinreturn.pinProfile.state;
        //            profile.ZIP = pinreturn.pinProfile.zip;
        //            profile.BIRTH_DATE = pinreturn.pinProfile.birth_date.ToString();
        //            profile.EMAIL = pinreturn.pinProfile.email;
        //            profile.PHONE = pinreturn.pinProfile.phone;

        //        }

        //        profile.EMAIL_OPT_CD = "I";
        //        profile.ENV = bm.environ;
        //        profile.USER_ID = "SCMonitorWeb";
        //        profile.USPS_OPT_CD = "I";
        //        profile.RESPONSE_TYPE = "S";
        //        profile.RESPONSE_CODE = strResponseCode;
        //        profile.WEB_USER_ID = strResponseCode;
        //        profile.WEB_SOURCE = "MultiWebSource";
        //        profile.WEB_VERSION = "1.0";
        //        DateTime dateValue;
        //        if (profile.COA_DATE == null || !(DateTime.TryParse(profile.COA_DATE.ToString(), out dateValue)) || Convert.ToDateTime(profile.COA_DATE) < Convert.ToDateTime("01/01/1900"))
        //        {
        //            profile.COA_DATE = "01/01/1900";
        //        }
        //        if (!string.IsNullOrEmpty(pindata.pin))
        //        {
        //            profile.INDIV_ID = pindata.pin;
        //        }
        //        profile.GUID = CheckGUID;

        //        //
        //        //uncomment following lines after test
        //        //
        //        //qa = BuildCheckedItems();
        //        //Question[] questions = new Question[1];
        //        List<Question> questions = new List<Question>();
        //        //questions = qa.q_and_a;
        //        // add the Textbox answers
        //        Answer tans = new Answer();
        //        //Answer[] answers = new Answer[1];
        //        Question ques = new Question();
        //        List<Answer> answers = new List<Answer>();
        //        tans.answer_desc = TextNotification.Text.TextRecord.Message.ToUpper();
        //        answers.Add(tans);
        //        //Array.Resize(ref questions, questions.Length + 1);
        //        //ques.question_code = 100;
        //        DateTime dtToday = DateTime.Today;
        //        DateTime dtTest2 = Convert.ToDateTime("01/01/1900");

        //        td.env = bm.environ;
        //        td.indiv_id = pindata.pin;

        //        cr = GetCommunication(td);
        //        //strTestNumber = dtToday < cr.test2_date ? "1" : "2";
        //        int TestNumber = (cr.test2_date == dtTest2) || (dtToday < cr.test2_date) ? 1 : 2;

        //        ques.question_code = TestNumber;

        //        ques.answers = answers;
        //        //questions[0] = ques;
        //        questions.Add(ques);
        //        qa.q_and_a = questions;
        //        svyreturn = SaveCustResponse(profile);
        //        if (svyreturn.code == SurveyStatusCodes.Success)
        //        {
        //            if (svyreturn.code == SurveyStatusCodes.Success)
        //            {
        //                qa.EXTERNAL_REF_NUMBER = svyreturn.TankRecNum.ToString();
        //                qa.ENV = bm.environ;
        //                qa.GUID = CheckGUID;
        //                qa.INDIV_ID = pindata.pin;
        //                qa.RESPONSE_CODE = strResponseCode;
        //                qa.RESPONSE_DATE = DateTime.Today.ToString();
        //                if (qa.RESPONSE_DATE == null || !(DateTime.TryParse(qa.RESPONSE_DATE.ToString(), out dateValue)) || Convert.ToDateTime(qa.RESPONSE_DATE) < Convert.ToDateTime("01/01/1900"))
        //                {
        //                    qa.RESPONSE_DATE = DateTime.Today.ToString(); ;
        //                }
        //                svyreturn = SaveCustQA(qa);
        //            }
        //        }
        //        if (!(TextNotification.Text.TextRecord.Message.ToUpper() == "DNT"))
        //        {
        //            td.env = bm.environ;
        //            td.indiv_id = pindata.pin;
        //            webreturn = UpdateTestingRecords(td);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Notify: " + ex.ToString());
        //    }

        //    return webreturn;

        //}
        //public MessageReturn CallNotify(CallNotification CallNotification, BusinessModel bm)
        //{
        //    MessageReturn webreturn = new MessageReturn();
        //    SurveyReturn svyreturn = new SurveyReturn();
        //    ProfileData profile = new ProfileData();
        //    QAData qa = new QAData();
        //    PinEntryData pindata = new PinEntryData();
        //    PinEntryReturn pinreturn = new PinEntryReturn();
        //    StringBuilder sb = new StringBuilder();
        //    TestingData td = new TestingData();
        //    CommunicationReturn cr = new CommunicationReturn();

        //    var client = new CallFire_csharp_sdk.API.CallfireClient("a59672830bd6", "7959b638d85859f5", CallfireClients.Rest);
        //    var callClient = client.Call;

        //    var actionQuery = new CfActionQuery();
        //    actionQuery.Inbound = false;

        //    try
        //    {
        //        CallNotification.Call.CallRecord.Result = "NONE";

        //        actionQuery.ToNumber = CallNotification.Call.ToNumber;
        //        var cfCallQueryResult = callClient.QueryCalls(actionQuery);

        //        if (cfCallQueryResult.Calls != null)
        //        {
        //            var cfCall = cfCallQueryResult.Calls[0];
        //            if (cfCall.CallRecord[0].QuestionResponse != null)
        //            {
        //                switch (cfCall.CallRecord[0].QuestionResponse[0].Response)
        //                {
        //                    case "1":
        //                        CallNotification.Call.CallRecord.Result = "NEG";
        //                        break;
        //                    case "2":
        //                        CallNotification.Call.CallRecord.Result = "POS";
        //                        break;
        //                    default:
        //                        CallNotification.Call.CallRecord.Result = "DNT";
        //                        break;
        //                }

        //            }
        //        }
        //        webreturn = SaveCallNotificationCallback(CallNotification);
        //        //string strPhone = TextNotification.Text.FromNumber.Substring(1, TextNotification.Text.FromNumber.Length - 1);

        //        sb.Append(CallNotification.Call.ToNumber);
        //        sb.Remove(0, 1);
        //        pindata.env = bm.environ;
        //        pindata.guid = CheckGUID;
        //        pindata.ip = bm.ipaddress;
        //        pindata.pin = GetIndivIDbyPhone(sb.ToString()).ToString();

        //        pinreturn = GetCustomerInfo(pindata);
        //        //string strResponseCode = "SPWEB16RES";
        //        string strChannel = "IVR";
        //        string strResponseCode = GetResponseCode(strChannel);

        //        if (pinreturn.code == PinEntryStatusCodes.Success)
        //        {
        //            profile.FIRST_NAME = pinreturn.pinProfile.first_name;
        //            profile.LAST_NAME = pinreturn.pinProfile.last_name;
        //            profile.ADDRESS1 = pinreturn.pinProfile.address1;
        //            profile.ADDRESS2 = pinreturn.pinProfile.address2;
        //            profile.CITY = pinreturn.pinProfile.city;
        //            profile.STATE = pinreturn.pinProfile.state;
        //            profile.ZIP = pinreturn.pinProfile.zip;
        //            profile.BIRTH_DATE = pinreturn.pinProfile.birth_date.ToString();
        //            profile.EMAIL = pinreturn.pinProfile.email;
        //            profile.PHONE = pinreturn.pinProfile.phone;

        //        }

        //        profile.EMAIL_OPT_CD = "I";
        //        profile.ENV = bm.environ;
        //        profile.USER_ID = "SCMonitorWeb";
        //        profile.USPS_OPT_CD = "I";
        //        profile.RESPONSE_TYPE = "S";
        //        profile.RESPONSE_CODE = strResponseCode;
        //        profile.WEB_USER_ID = strResponseCode;
        //        profile.WEB_SOURCE = "MultiWebSource";
        //        profile.WEB_VERSION = "1.0";
        //        DateTime dateValue;
        //        if (profile.COA_DATE == null || !(DateTime.TryParse(profile.COA_DATE.ToString(), out dateValue)) || Convert.ToDateTime(profile.COA_DATE) < Convert.ToDateTime("01/01/1900"))
        //        {
        //            profile.COA_DATE = "01/01/1900";
        //        }
        //        if (!string.IsNullOrEmpty(pindata.pin))
        //        {
        //            profile.INDIV_ID = pindata.pin;
        //        }
        //        profile.GUID = CheckGUID;

        //        //
        //        //uncomment following lines after test
        //        //
        //        //qa = BuildCheckedItems();
        //        //Question[] questions = new Question[1];
        //        List<Question> questions = new List<Question>();
        //        //questions = qa.q_and_a;
        //        // add the Textbox answers
        //        Answer tans = new Answer();
        //        //Answer[] answers = new Answer[1];
        //        Question ques = new Question();
        //        List<Answer> answers = new List<Answer>();
        //        tans.answer_desc = CallNotification.Call.CallRecord.Result.ToUpper();
        //        answers.Add(tans);
        //        //Array.Resize(ref questions, questions.Length + 1);
        //        //ques.question_code = 100;
        //        DateTime dtToday = DateTime.Today;
        //        DateTime dtTest2 = Convert.ToDateTime("01/01/1900");

        //        td.env = bm.environ;
        //        td.indiv_id = pindata.pin;

        //        cr = GetCommunication(td);
        //        //strTestNumber = dtToday < cr.test2_date ? "1" : "2";
        //        int TestNumber = (cr.test2_date == dtTest2) || (dtToday < cr.test2_date) ? 1 : 2;

        //        ques.question_code = TestNumber;
        //        ques.answers = answers;
        //        //questions[0] = ques;
        //        questions.Add(ques);
        //        qa.q_and_a = questions;
        //        svyreturn = SaveCustResponse(profile);
        //        if (svyreturn.code == SurveyStatusCodes.Success)
        //        {
        //            if (svyreturn.code == SurveyStatusCodes.Success)
        //            {
        //                qa.EXTERNAL_REF_NUMBER = svyreturn.TankRecNum.ToString();
        //                qa.ENV = bm.environ;
        //                qa.GUID = CheckGUID;
        //                qa.INDIV_ID = pindata.pin;
        //                qa.RESPONSE_CODE = strResponseCode;
        //                qa.RESPONSE_DATE = DateTime.Today.ToString();
        //                if (qa.RESPONSE_DATE == null || !(DateTime.TryParse(qa.RESPONSE_DATE.ToString(), out dateValue)) || Convert.ToDateTime(qa.RESPONSE_DATE) < Convert.ToDateTime("01/01/1900"))
        //                {
        //                    qa.RESPONSE_DATE = DateTime.Today.ToString(); ;
        //                }
        //                svyreturn = SaveCustQA(qa);
        //            }
        //        }
        //        if (!(CallNotification.Call.CallRecord.Result.ToUpper() == "DNT" || CallNotification.Call.CallRecord.Result.ToUpper() == "NONE"))
        //        {
        //            td.env = bm.environ;
        //            td.indiv_id = pindata.pin;
        //            webreturn = UpdateTestingRecords(td);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Notify: " + ex.ToString());
        //    }

        //    return webreturn;

        //}
        public MessageReturn SaveTextNotificationCallback(Notification chkUser)
        {
            MessageReturn retuser = new MessageReturn();
            try
            {

                var currentSession = sessionFactory.GetCurrentSession();
                var parameters = new DynamicParameters();
                parameters.Add("@SubscriptionId", chkUser.SubscriptionId);
                parameters.Add("@AccountId", chkUser.AccountId);
                parameters.Add("@Text_id", chkUser.Text.id);
                parameters.Add("@FromNumber", chkUser.Text.FromNumber);
                parameters.Add("@ToNumber", chkUser.Text.ToNumber);
                parameters.Add("@State", chkUser.Text.State);
                parameters.Add("@ContactId", chkUser.Text.ContactId);
                parameters.Add("@Inbound", chkUser.Text.Inbound);
                parameters.Add("@Created ", Convert.ToDateTime(chkUser.Text.Created));
                parameters.Add("@Modified ", Convert.ToDateTime(chkUser.Text.Modified));
                parameters.Add("@FinalResult ", chkUser.Text.FinalResult);
                parameters.Add("@Message", chkUser.Text.Message);
                parameters.Add("@TextRecord_id", chkUser.Text.TextRecord.id);
                parameters.Add("@Result", chkUser.Text.TextRecord.Result);
                parameters.Add("@FinishTime", Convert.ToDateTime(chkUser.Text.TextRecord.FinishTime));
                parameters.Add("@BilledAmount", chkUser.Text.TextRecord.BilledAmount);
                parameters.Add("@SwitchId", chkUser.Text.TextRecord.SwitchId);
                parameters.Add("@CallerName", chkUser.Text.TextRecord.CallerName);
                parameters.Add("@TextRecord_Message", chkUser.Text.TextRecord.Message);
                parameters.Add("@AnswerTime", DateTime.Now);
                parameters.Add("@Duration", 0);
                parameters.Add("@AgentCall", "");
                currentSession.Connection.Execute("usp_savenotification_callback", parameters, commandType: CommandType.StoredProcedure);
                retuser.code = GenericStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("SaveNotificationCallbackException: " + ex.ToString());
            }

            return retuser;

        }
        public MessageReturn SaveCallNotificationCallback(CallNotification chkUser)
        {
            MessageReturn retuser = new MessageReturn();
            try
            {

                var currentSession = sessionFactory.GetCurrentSession();
                var parameters = new DynamicParameters();
                parameters.Add("@SubscriptionId", chkUser.SubscriptionId);
                parameters.Add("@AccountId", chkUser.AccountId);
                parameters.Add("@Text_id", chkUser.Call.id);
                parameters.Add("@FromNumber", chkUser.Call.FromNumber);
                parameters.Add("@ToNumber", chkUser.Call.ToNumber);
                parameters.Add("@State", chkUser.Call.State);
                parameters.Add("@ContactId", chkUser.Call.ContactId);
                parameters.Add("@Inbound", chkUser.Call.Inbound);
                parameters.Add("@Created ", Convert.ToDateTime(chkUser.Call.Created));
                parameters.Add("@Modified ", Convert.ToDateTime(chkUser.Call.Modified));
                parameters.Add("@FinalResult ", chkUser.Call.FinalResult);
                parameters.Add("@Message", chkUser.Call.CallRecord.Result);
                parameters.Add("@TextRecord_id", chkUser.Call.CallRecord.id);
                parameters.Add("@Result", chkUser.Call.CallRecord.Result);
                parameters.Add("@FinishTime", Convert.ToDateTime(chkUser.Call.CallRecord.FinishTime));
                parameters.Add("@BilledAmount", chkUser.Call.CallRecord.BilledAmount);
                parameters.Add("@SwitchId", chkUser.Call.CallRecord.SwitchId);
                parameters.Add("@CallerName", chkUser.Call.CallRecord.CallerName);
                parameters.Add("@TextRecord_Message", chkUser.Call.CallRecord.Result);
                parameters.Add("@AnswerTime", Convert.ToDateTime(chkUser.Call.CallRecord.AnswerTime));
                parameters.Add("@Duration", Convert.ToInt32(chkUser.Call.CallRecord.Duration));
                parameters.Add("@AgentCall", chkUser.Call.CallRecord.AgentCall);
                currentSession.Connection.Execute("usp_savenotification_callback", parameters, commandType: CommandType.StoredProcedure);
                retuser.code = GenericStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("SaveNotificationCallbackException: " + ex.ToString());
            }

            return retuser;

        }
        public MessageReturn UpdateTestingRecords(TestingData chkUser)
        {
            MessageReturn retuser = new MessageReturn();
            CommunicationReturn cr = new CommunicationReturn();
            try
            {
                string strTestNumber = "";
                DateTime dtToday = DateTime.Today;
                DateTime dtTest2 = Convert.ToDateTime("01/01/1900");
                cr = GetCommunication(chkUser);
                //strTestNumber = dtToday < cr.test2_date ? "1" : "2";
                strTestNumber = (cr.test2_date == dtTest2) || (dtToday < cr.test2_date) ? "1" : "2";

                string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'N' where status is null and indiv_id = " + chkUser.indiv_id + " and test_number = " + strTestNumber;
                ExecuteSQL(sql);

                retuser.code = GenericStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateTestingRecordsException: " + ex.ToString());
            }

            return retuser;
        }
        public CommunicationReturn GetCommunication(TestingData chkUser)
        {
            CommunicationReturn retuser = new CommunicationReturn();
            try
            {
                int intIndivID = 0;
                var currentSession = sessionFactory.GetCurrentSession();
                IEnumerable results = currentSession.Connection.Query(@"select indiv_id,isnull( TEST1_DATE,'1/1/1900') test1_date, isnull(TEST2_DATE,'1/1/1900') test2_date from customer_communication
                                                                        where indiv_id = @indiv_id", new { @indiv_id = chkUser.indiv_id });
                foreach (dynamic row in results)
                {

                    intIndivID = row.indiv_id;
                    retuser.test1_date = row.test1_date;
                    retuser.test2_date = row.test2_date;
                }
                retuser.indiv_id = intIndivID.ToString();
                retuser.code = GenericStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";
            }
            catch (Exception ex)
            {
                throw new Exception("GetCommunicationException: " + ex.ToString());
            }

            return retuser;
        }
        //protected MessageReturn LogClientResults(BusinessModel bm)
        //{
        //    MessageReturn webreturn = new MessageReturn();
        //    ProfileData profile = new ProfileData();
        //    PinEntryData pindata = new PinEntryData();
        //    PinEntryReturn pinreturn = new PinEntryReturn();
        //    StringBuilder sb = new StringBuilder();
        //    ClientTestingData td = new ClientTestingData();
        //    List<SF_Client_Results> resultList = new List<SF_Client_Results>();
        //    string strResult = "";
        //    try
        //    {
        //        pindata.env = bm.environ;
        //        pindata.guid = CheckGUID;
        //        pindata.ip = bm.ipaddress;
        //        resultList = ClientResultList();
        //        foreach (SF_Client_Results res in resultList)
        //        {
        //            // Update the SALESFORCE_TEST_RESULTS_FROM_CLIENT Table
        //            string sql = "update SALESFORCE_TEST_RESULTS_FROM_CLIENT set STATUS = 'I' where STATUS IS NULL AND INDIV_ID = " + res.indiv_id;
        //            ExecuteSQL(sql);

        //            pindata.pin = res.indiv_id.ToString();
        //            pinreturn = GetCustomerInfo(pindata);
        //            string strResponseCode = "SPSF16RESULT";
        //            if (pinreturn.code == PinEntryStatusCodes.Success)
        //            {
        //                profile.FIRST_NAME = pinreturn.pinProfile.first_name;
        //                profile.LAST_NAME = pinreturn.pinProfile.last_name;
        //                profile.ADDRESS1 = pinreturn.pinProfile.address1;
        //                profile.ADDRESS2 = pinreturn.pinProfile.address2;
        //                profile.CITY = pinreturn.pinProfile.city;
        //                profile.STATE = pinreturn.pinProfile.state;
        //                profile.ZIP = pinreturn.pinProfile.zip;
        //                profile.BIRTH_DATE = pinreturn.pinProfile.birth_date.ToString();
        //                profile.EMAIL = pinreturn.pinProfile.email;
        //                profile.PHONE = pinreturn.pinProfile.phone;

        //            }

        //            profile.EMAIL_OPT_CD = "I";
        //            profile.ENV = bm.environ;
        //            profile.USER_ID = "SCMonitorWeb";
        //            profile.USPS_OPT_CD = "I";
        //            profile.RESPONSE_TYPE = "S";
        //            profile.RESPONSE_CODE = strResponseCode;
        //            profile.WEB_USER_ID = strResponseCode;
        //            profile.WEB_SOURCE = "MultiWebSource";
        //            profile.WEB_VERSION = "1.0";
        //            DateTime dateValue;
        //            if (profile.COA_DATE == null || !(DateTime.TryParse(profile.COA_DATE.ToString(), out dateValue)) || Convert.ToDateTime(profile.COA_DATE) < Convert.ToDateTime("01/01/1900"))
        //            {
        //                profile.COA_DATE = "01/01/1900";
        //            }
        //            if (!string.IsNullOrEmpty(pindata.pin))
        //            {
        //                profile.INDIV_ID = pindata.pin;
        //            }
        //            profile.GUID = CheckGUID;

        //            DateTime dtToday = DateTime.Today;
        //            DateTime dtTest = Convert.ToDateTime("01/01/1900");
        //            //test1 result
        //            if (!(string.IsNullOrEmpty(res.test1_result)))
        //            {
        //                SurveyReturn svyreturn = new SurveyReturn();
        //                List<Question> questions = new List<Question>();
        //                QAData qa = new QAData();
        //                List<Answer> answers = new List<Answer>();
        //                Answer tans = new Answer();
        //                Question ques = new Question();
        //                strResult = res.test1_result.Trim().ToUpper();
        //                tans.answer_desc = strResult == "NEGATIVE" ? "NEG" : "POS";

        //                answers.Add(tans);

        //                ques.question_code = 1;

        //                ques.answers = answers;
        //                //questions[0] = ques;
        //                questions.Add(ques);
        //                qa.q_and_a = questions;
        //                svyreturn = SaveCustResponse(profile);
        //                if (svyreturn.code == SurveyStatusCodes.Success)
        //                {
        //                    if (svyreturn.code == SurveyStatusCodes.Success)
        //                    {
        //                        qa.EXTERNAL_REF_NUMBER = svyreturn.TankRecNum.ToString();
        //                        qa.ENV = bm.environ;
        //                        qa.GUID = CheckGUID;
        //                        qa.INDIV_ID = pindata.pin;
        //                        qa.RESPONSE_CODE = strResponseCode;
        //                        qa.RESPONSE_DATE = res.test1_date.ToString();
        //                        if (qa.RESPONSE_DATE == null || !(DateTime.TryParse(qa.RESPONSE_DATE.ToString(), out dateValue)) || Convert.ToDateTime(qa.RESPONSE_DATE) <= Convert.ToDateTime("01/01/1900"))
        //                        {
        //                            qa.RESPONSE_DATE = DateTime.Today.ToString(); ;
        //                        }
        //                        svyreturn = SaveCustQA(qa);
        //                    }
        //                }
        //                td.env = bm.environ;
        //                td.indiv_id = pindata.pin;
        //                td.test_number = 1;
        //                webreturn = UpdateClientTestingRecords(td);
        //            }
        //            //test2 results
        //            if (!(string.IsNullOrEmpty(res.test2_result)))
        //            {
        //                SurveyReturn svyreturn = new SurveyReturn();
        //                List<Question> questions = new List<Question>();
        //                QAData qa = new QAData();
        //                List<Answer> answers = new List<Answer>();
        //                Answer tans = new Answer();
        //                Question ques = new Question();
        //                strResult = res.test2_result.Trim().ToUpper();
        //                tans.answer_desc = strResult == "NEGATIVE" ? "NEG" : "POS";
        //                answers.Add(tans);

        //                ques.question_code = 2;

        //                ques.answers = answers;
        //                //questions[0] = ques;
        //                questions.Add(ques);
        //                qa.q_and_a = questions;
        //                svyreturn = SaveCustResponse(profile);
        //                if (svyreturn.code == SurveyStatusCodes.Success)
        //                {
        //                    if (svyreturn.code == SurveyStatusCodes.Success)
        //                    {
        //                        qa.EXTERNAL_REF_NUMBER = svyreturn.TankRecNum.ToString();
        //                        qa.ENV = bm.environ;
        //                        qa.GUID = CheckGUID;
        //                        qa.INDIV_ID = pindata.pin;
        //                        qa.RESPONSE_CODE = strResponseCode;
        //                        qa.RESPONSE_DATE = res.test2_date.ToString();
        //                        if (qa.RESPONSE_DATE == null || !(DateTime.TryParse(qa.RESPONSE_DATE.ToString(), out dateValue)) || Convert.ToDateTime(qa.RESPONSE_DATE) <= Convert.ToDateTime("01/01/1900"))
        //                        {
        //                            qa.RESPONSE_DATE = DateTime.Today.ToString(); ;
        //                        }
        //                        svyreturn = SaveCustQA(qa);
        //                    }
        //                }
        //                td.env = bm.environ;
        //                td.indiv_id = pindata.pin;
        //                td.test_number = 2;
        //                webreturn = UpdateClientTestingRecords(td);
        //            }
        //            // Update the SALESFORCE_TEST_RESULTS_FROM_CLIENT Table
        //            sql = "update SALESFORCE_TEST_RESULTS_FROM_CLIENT set STATUS = 'P' where STATUS = 'I' AND INDIV_ID = " + res.indiv_id;
        //            ExecuteSQL(sql);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("LogClientResults: " + ex.ToString());
        //    }

        //    return webreturn;

        //}
        protected List<SF_Client_Results> ClientResultList()
        {
            var currentSession = sessionFactory.GetCurrentSession();
            string sql = "";
            IEnumerable results = currentSession.Connection.Query(@"SELECT SF_TEST1_RESULT, SF_TEST2_RESULT, isnull( SF_TEST1_RPT_DATE,'1/1/1900') SF_TEST1_RPT_DATE, isnull( SF_TEST2_RPT_DATE,'1/1/1900') SF_TEST2_RPT_DATE, INDIV_ID ,STATUS FROM SALESFORCE_TEST_RESULTS_FROM_CLIENT
                                                                    WHERE (STATUS IS NULL)");
            List<SF_Client_Results> resultList = new List<SF_Client_Results>();

            foreach (dynamic row in results)
            {
                resultList.Add(new SF_Client_Results(row.INDIV_ID, row.SF_TEST1_RESULT, row.SF_TEST2_RESULT, row.SF_TEST1_RPT_DATE, row.SF_TEST2_RPT_DATE));
            }

            return resultList;

        }
        public MessageReturn UpdateClientTestingRecords(ClientTestingData chkUser)
        {
            MessageReturn retuser = new MessageReturn();
            try
            {
                string strTestNumber = "";
                DateTime dtToday = DateTime.Today;
                DateTime dtTest2 = Convert.ToDateTime("01/01/1900");
                //strTestNumber = dtToday < cr.test2_date ? "1" : "2";
                strTestNumber = chkUser.test_number.ToString();

                string sql = "update CUSTOMER_MESSAGE_DETAIL set STATUS = 'N' where status is null and indiv_id = " + chkUser.indiv_id + " and test_number = " + strTestNumber;
                ExecuteSQL(sql);

                retuser.code = GenericStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateTestingRecordsException: " + ex.ToString());
            }

            return retuser;
        }

        public SurveyReturn SaveCustResponse(ProfileData chkUser)
        {
            SurveyReturn retuser = new SurveyReturn();
            ConsumerClass CustomerObject = new ConsumerClass();
            AddrStndReturn retaddr = new AddrStndReturn();
            AgeVerifData avfdata = new AgeVerifData();
            try
            {
                var GUID = "963b4e28-15a2-46aa-bbc7-3dcbc44c62b4";

                var currentSession = sessionFactory.GetCurrentSession();
                //CustomerObject.ConnectionString = currentSession.Connection.ConnectionString;
                //chkUser.INDIV_ID = CustomerObject.GetNextIndivId(GUID);
                avfdata.ADDRESS1 = chkUser.ADDRESS1;
                avfdata.ADDRESS2 = chkUser.ADDRESS2;
                avfdata.CITY = chkUser.CITY;
                avfdata.STATE = chkUser.STATE;
                avfdata.ZIP = chkUser.ZIP;
                retaddr = AddressStandardize(avfdata);

                var parameters = new DynamicParameters();
                parameters.Add("@USERID", chkUser.USER_ID);
                parameters.Add("@WEB_USER_ID", chkUser.WEB_USER_ID);
                parameters.Add("@WEB_SOURCE", chkUser.WEB_SOURCE);
                parameters.Add("@WEB_VERSION", chkUser.WEB_VERSION);
                parameters.Add("@FIRST_NAME", chkUser.FIRST_NAME);
                parameters.Add("@LAST_NAME", chkUser.LAST_NAME);
                parameters.Add("@MID_NAME", chkUser.MID_NAME);
                parameters.Add("@GENDER", chkUser.GENDER);
                parameters.Add("@BIRTH_DATE ", chkUser.BIRTH_DATE);
                parameters.Add("@ADDRESS1 ", retaddr.ADDRESS1);
                parameters.Add("@ADDRESS2 ", retaddr.ADDRESS2);
                parameters.Add("@CITY", retaddr.CITY);
                parameters.Add("@STATE", retaddr.STATE);
                parameters.Add("@ZIP", retaddr.ZIP);
                parameters.Add("@ZIP4", retaddr.ZIP4);
                parameters.Add("@EMAIL_OPT_CODE", chkUser.EMAIL_OPT_CD);
                parameters.Add("@USPS_OPT_CODE", chkUser.USPS_OPT_CD);
                parameters.Add("@TEXT_MSG_OPT_CD", chkUser.TEXT_MESSAGE_OPT_CD);
                parameters.Add("@PHONE", chkUser.PHONE);
                parameters.Add("@EMAIL", chkUser.EMAIL);
                parameters.Add("@AV_CODE", chkUser.AV_CODE);
                parameters.Add("@RESPONSE_CODE", chkUser.RESPONSE_CODE);
                parameters.Add("@STATUS", chkUser.STATUS);
                parameters.Add("@EMAIL_STATUS", chkUser.EMAIL_STATUS);
                parameters.Add("@USPS_STATUS", chkUser.USPS_STATUS);
                parameters.Add("@CUST_PWD", chkUser.CUST_PWD);
                parameters.Add("@SIGNATURE", chkUser.SIGNATURE);
                parameters.Add("@MGM_ID", chkUser.MGM_ID);
                parameters.Add("@LAT", retaddr.LAT);
                parameters.Add("@LONG", retaddr.LONG);
                parameters.Add("@PROCEDURE_DATE", chkUser.PROCEDURE_DATE);
                parameters.Add("@WebRecnumProf", dbType: DbType.Int32, direction: ParameterDirection.Output);
                if (!(string.IsNullOrEmpty(chkUser.INDIV_ID) || chkUser.INDIV_ID == "0"))
                {
                    parameters.Add("@INDIV_ID", Convert.ToInt32(chkUser.INDIV_ID));
                    currentSession.Connection.Execute("usp_savecustresponse_withid", parameters, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    parameters.Add("@INDIV_ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    currentSession.Connection.Execute("usp_savecustresponse", parameters, commandType: CommandType.StoredProcedure);
                }
                retuser.TankRecNum = parameters.Get<int>("@WebRecnumProf");
                retuser.indiv_id = (string.IsNullOrEmpty(chkUser.INDIV_ID) || chkUser.INDIV_ID == "0") ? parameters.Get<int>("@INDIV_ID") : Convert.ToInt32(chkUser.INDIV_ID);
                retuser.code = SurveyStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("SaveCustResponseException: " + ex.ToString());
            }

            return retuser;

        }
        public string GetResponseCode(string Channel)
        {
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                return currentSession.Connection.Query<string>("SELECT TOP (1) RESPONSE_CODE FROM RESPONSES WHERE CHANNEL = '" + Channel + "' ORDER BY START_DATE DESC").FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("GetResponseCode Exception: " + ex.ToString());
            }
        }

        public List<State> StateList()
        {
            var currentSession = sessionFactory.GetCurrentSession();

            IEnumerable results = currentSession.Connection.Query(@"SELECT STATE AS STATE_ABBR, STATE_LONG AS STATE_NAME
			                                                      FROM REF_STATES WHERE CONT_US = 'Y' ORDER BY STATE_LONG ASC");
            List<State> codeList = new List<State>();

            foreach (dynamic row in results)
            {
                //if (string.IsNullOrEmpty(row.STATE_ABBR))
                //codeList.Add(new State("Select State", row.STATE_NAME));
                // else
                codeList.Add(new State(row.STATE_ABBR, row.STATE_NAME));
            }

            return codeList;

        }
        public AddrStndReturn AddressStandardize(AgeVerifData chkUser)
        {
            AddrStndReturn retuser = new AddrStndReturn();
            AddrService.AddrStndReturn retaddr = new AddrService.AddrStndReturn();
            AddrService.AddrStndData addr = new AddrService.AddrStndData();
            AddrService.RestService addrclient = new AddrService.RestService();
            string GeoGUID = "963b4e28-15a2-46aa-bbc7-3dcbc44c62b4";
            //string stdAddress1 = "";
            //string stdAddress2 = "";
            //string stdCity = "";
            //string stdState = "";
            //string stdZip = "";
            //string stdZip4 = "";
            string strEmpty = String.Empty;
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                // 1. First we need to Address standardize the input address
                addr.ADDRESS1 = chkUser.ADDRESS1;
                addr.ADDRESS2 = chkUser.ADDRESS2;
                addr.CITY = chkUser.CITY;
                addr.STATE = chkUser.STATE;
                addr.ZIP = chkUser.ZIP;
                addr.guid = GeoGUID;
                retuser.LAT = 0;
                retuser.LONG = 0;
                retaddr = addrclient.GetStndAddress(addr);
                //if (retaddr.code == AddrService.AddressStatusCodes.Success)
                //{
                retuser.ADDRESS1 = string.IsNullOrEmpty(retaddr.ADDRESS1) ? chkUser.ADDRESS1 : retaddr.ADDRESS1;
                retuser.ADDRESS2 = string.IsNullOrEmpty(retaddr.ADDRESS2) ? chkUser.ADDRESS2 : retaddr.ADDRESS2;
                retuser.CITY = string.IsNullOrEmpty(retaddr.CITY) ? chkUser.CITY : retaddr.CITY;
                retuser.STATE = string.IsNullOrEmpty(retaddr.STATE) ? chkUser.STATE : retaddr.STATE;
                retuser.ZIP = string.IsNullOrEmpty(retaddr.ZIP) ? chkUser.ZIP : retaddr.ZIP;
                retuser.ZIP4 = string.IsNullOrEmpty(retaddr.ZIP4) ? "0000" : retaddr.ZIP4;
                retuser.LAT = retaddr.LAT;
                retuser.LONG = retaddr.LONG;
                //}
                retuser.code = GenericStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";
            }
            catch (Exception ex)
            {
                throw new Exception("AddressStandardize Exception: " + ex.ToString());
            }
            return retuser;
        }

    }
}

