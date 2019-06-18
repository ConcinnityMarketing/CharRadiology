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
            RegisterReturn smd = new RegisterReturn();
            try
            {
                comList = CommunicationList();
                string sql = "";
                DateTime dtToday = DateTime.Today;
                //dtToday = dtToday.AddDays(-1);
                foreach (Communication com in comList)
                {
                        sql = "update CUSTOMER_COMMUNICATION set COMM_STATUS = 'I' where CC_ID = " + com.cc_id;
                        ExecuteSQL(sql);
                    mfList = MessageFlowList(com.mfid);

                    foreach (MessageFlow mf in mfList)
                        {
                                mdetail.INDIV_ID = com.indiv_id;
                                mdetail.MESSAGE_SEQ = mf.message_seq;
                                mdetail.MFID = mf.mfid;
                                mdetail.CHANNEL = mf.channel;
                                mdetail.MFID = mf.mfid;
                                mdetail.MESSAGE_DT = com.activate_dt.AddDays(mf.days_out);
                                mdetail.EMAIL = com.email;
                                mdetail.PHONE = com.phone;
                                smd = SaveMessageDetail(mdetail);
                        }
                }
                sql = "update CUSTOMER_COMMUNICATION set COMM_STATUS = 'P' where COMM_STATUS = 'I'";
                ExecuteSQL(sql);

                retbool = true;
            }
            catch (Exception ex)
            {
                throw new Exception("ExpandCommunication Exception: " + ex.ToString());
            }
            return retbool;
        }
        protected RegisterReturn SaveMessageDetail(MessageDetail chkUser)
        {
            RegisterReturn retuser = new RegisterReturn();
            try
            {
                var currentSession = sessionFactory.GetCurrentSession();
                var parameters = new DynamicParameters();
                parameters.Add("@INDIV_ID",chkUser.INDIV_ID);
                parameters.Add("@MESSAGE_SEQ", chkUser.MESSAGE_SEQ);
                parameters.Add("@MFID", chkUser.MFID);
                parameters.Add("@CHANNEL", chkUser.CHANNEL);
                parameters.Add("@MESSAGE_DT", chkUser.MESSAGE_DT);
                parameters.Add("@EMAIL", chkUser.EMAIL);
                parameters.Add("@PHONE", chkUser.PHONE);
                currentSession.Connection.Execute("usp_savecustmsgdetail", parameters, commandType: CommandType.StoredProcedure);

                retuser.code = RegistrationStatusCodes.Success;
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
            IEnumerable results = currentSession.Connection.Query(@"SELECT CC_ID, INDIV_ID, EMAIL, TEXT_MESSAGE, ACTIVATE_DT, COMM_STATUS, MFID
                                                                     FROM CUSTOMER_COMMUNICATION
                                                                    WHERE (COMM_STATUS IS NULL OR COMM_STATUS = 'I')");
            List<Communication> comList = new List<Communication>();
            foreach (dynamic row in results)
            {
                comList.Add(new Communication(row.CC_ID, row.INDIV_ID, row.MFID, row.EMAIL, row.TEXT_MESSAGE, row.ACTIVATE_DT, row.COMM_STATUS));
            }
            return comList;
        }
        protected List<MessageFlow> MessageFlowList(string strMFID)
        {
            var currentSession = sessionFactory.GetCurrentSession();
            IEnumerable results = currentSession.Connection.Query(@"SELECT MF_RECNUM, MESSAGE_SEQ, MFID, DAYS_OUT, CHANNEL, UPDATE_DT
                                                                    FROM REF_MESSAGE_FLOW where MFID = @MFID", new { MFID = strMFID });
            List<MessageFlow> comList = new List<MessageFlow>();
            foreach (dynamic row in results)
            {
                comList.Add(new MessageFlow(row.MF_RECNUM, row.MESSAGE_SEQ, row.MFID, row.DAYS_OUT, row.CHANNEL));
            }
            return comList;
        }
    }
}
