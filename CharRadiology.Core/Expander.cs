using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CharRadiology.Core.Models;
using CharRadiology.Core.Enums;
using Dapper;
using NHibernate;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Xml;
namespace CharRadiology.Core
{
    public class Expander
    {

        protected static ISessionFactory sessionFactory;
        protected static List<Communication> comList = new List<Communication>();
        protected static List<MessageFlow> mfList = new List<MessageFlow>();
        public Expander()
        {

        }
        public Expander(ISessionFactory sessionFactory)
        {
            Expander.sessionFactory = sessionFactory;
        }

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
        protected static void ExecuteSQL(string sql)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            var parameters = new DynamicParameters();
            parameters = null;
            currentSession.Connection.Execute(sql, parameters, commandTimeout: 10 * 60, commandType: CommandType.Text);
        }
        public bool ExpandCommunication()
        {
            bool retbool = false;
            MessageDetail mdetail = new MessageDetail();
            SurveyReturn smd = new SurveyReturn();
            try
            {
                comList = CommunicationList();
                mfList = MessageFlowList();
                string sql = "";
                DateTime dtToday = DateTime.Today;
                dtToday = dtToday.AddDays(-1);
                foreach (Communication com in comList)
                {
                    if (!(com.test1_date == Convert.ToDateTime("1/1/1900")) && com.test1_date > dtToday)
                    {
                        sql = "update CUSTOMER_COMMUNICATION set STATUS = 'I' where INDIV_ID = " + com.indiv_id;
                        ExecuteSQL(sql);

                        foreach (MessageFlow mf in mfList)
                        {
                            if (mf.test_number == 1)
                            {
                                mdetail.INDIV_ID = com.indiv_id;
                                mdetail.MESSAGE_SEQ = mf.message_seq;
                                mdetail.MFID = mf.mfid;
                                mdetail.CHANNEL = mf.channel;
                                mdetail.TEST_NUMBER = mf.test_number;
                                if (mf.message_seq == 0)
                                    mdetail.MESSAGE_DT = com.procedure_date;
                                else
                                {
                                    mdetail.MESSAGE_DT = com.test1_date.AddDays(mf.days_out);
                                }
                                //mdetail.MESSAGE_DT = com.procedure_date.AddDays(mf.days_out);
                                mdetail.EMAIL = com.email;
                                mdetail.PHONE = com.phone;
                                mdetail.PROCEDURE_DATE = com.procedure_date;
                                smd = SaveMessageDetail(mdetail);
                            }
                        }
                    }
                    if (!(com.test2_date == Convert.ToDateTime("1/1/1900")) && com.test2_date > dtToday && com.test1_date > dtToday)
                    {
                        sql = "update CUSTOMER_COMMUNICATION set STATUS = 'I' where INDIV_ID = " + com.indiv_id;
                        ExecuteSQL(sql);

                        foreach (MessageFlow mf in mfList)
                        {
                            if (mf.test_number == 2)
                            {
                                mdetail.INDIV_ID = com.indiv_id;
                                mdetail.MESSAGE_SEQ = mf.message_seq;
                                mdetail.MFID = mf.mfid;
                                mdetail.CHANNEL = mf.channel;
                                mdetail.TEST_NUMBER = mf.test_number;
                                if (mf.message_seq == 0)
                                    mdetail.MESSAGE_DT = com.procedure_date;
                                else
                                {
                                    mdetail.MESSAGE_DT = com.test2_date.AddDays(mf.days_out);
                                }
                                //mdetail.MESSAGE_DT = com.procedure_date.AddDays(mf.days_out);
                                mdetail.EMAIL = com.email;
                                mdetail.PHONE = com.phone;
                                mdetail.PROCEDURE_DATE = com.procedure_date;
                                smd = SaveMessageDetail(mdetail);
                            }
                        }
                    }
                }
                sql = "update CUSTOMER_COMMUNICATION set STATUS = 'P' where STATUS = 'I'";
                ExecuteSQL(sql);

                retbool = true;
            }
            catch (Exception ex)
            {
                throw new Exception("ExpandCommunication Exception: " + ex.ToString());
            }
            return retbool;
        }
        protected SurveyReturn SaveMessageDetail(MessageDetail chkUser)
        {
            SurveyReturn retuser = new SurveyReturn();
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                var parameters = new DynamicParameters();
                parameters.Add("@INDIV_ID",chkUser.INDIV_ID);
                parameters.Add("@MESSAGE_SEQ", chkUser.MESSAGE_SEQ);
                parameters.Add("@TEST_NUMBER", chkUser.TEST_NUMBER);
                parameters.Add("@MFID", chkUser.MFID);
                parameters.Add("@CHANNEL", chkUser.CHANNEL);
                parameters.Add("@MESSAGE_DT", chkUser.MESSAGE_DT);
                parameters.Add("@EMAIL", chkUser.EMAIL);
                parameters.Add("@PHONE", chkUser.PHONE);
                parameters.Add("@PROCEDURE_DATE", chkUser.PROCEDURE_DATE);
                currentSession.Connection.Execute("usp_savecustmsgdetail", parameters, commandType: CommandType.StoredProcedure);

                retuser.code = SurveyStatusCodes.Success;
                retuser.status = "Success";
                retuser.desc = "";

            }
            catch (Exception ex)
            {
                throw new Exception("SaveMessageDetail Exception: " + ex.ToString());
            }

            return retuser;
        }
        protected List<Communication> CommunicationList()
        {
            var currentSession = sessionFactory.GetCurrentSession();
            IEnumerable results = currentSession.Connection.Query(@"SELECT INDIV_ID, EMAIL, PHONE, ACTIVATE_DT, STATUS, LAST_UPDATE_DT, isnull(convert(varchar, PROCEDURE_DATE, 101),'1/1/1900') PROCEDURE_DATE, isnull(convert(varchar, TEST1_DATE, 101),'1/1/1900') TEST1_DATE, isnull(convert(varchar, TEST2_DATE, 101),'1/1/1900') TEST2_DATE
                                                                     FROM CUSTOMER_COMMUNICATION
                                                                    WHERE (STATUS IS NULL OR STATUS = 'I')");
            List<Communication> comList = new List<Communication>();
            foreach (dynamic row in results)
            {
                comList.Add(new Communication(row.INDIV_ID, row.EMAIL, row.PHONE, row.ACTIVATE_DT, row.STATUS, Convert.ToDateTime(row.PROCEDURE_DATE), Convert.ToDateTime(row.TEST1_DATE), Convert.ToDateTime(row.TEST2_DATE)));
            }
            return comList;
        }
        protected List<MessageFlow> MessageFlowList()
        {
            var currentSession = sessionFactory.GetCurrentSession();
            IEnumerable results = currentSession.Connection.Query(@"SELECT MF_RECNUM, MESSAGE_SEQ, TEST_NUMBER, MFID, DAYS_OUT, CHANNEL, UPDATE_DT
                                                                    FROM REF_MESSAGE_FLOW");
            List<MessageFlow> comList = new List<MessageFlow>();
            foreach (dynamic row in results)
            {
                comList.Add(new MessageFlow(row.MF_RECNUM, row.MESSAGE_SEQ, row.TEST_NUMBER, row.MFID, row.DAYS_OUT, row.CHANNEL));
            }
            return comList;
        }
    }
}
