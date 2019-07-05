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
                        sql = "update CUSTOMER_COMMUNICATIONS set CC_STATUS = 'INPROCESS' where CC_RECNUM = " + com.cc_recnum;
                        ExecuteSQL(sql);
                    mfList = MessageFlowList(com.mfid);

                    foreach (MessageFlow mf in mfList)
                        {
                                mdetail.INDIV_ID = com.indiv_id;
                                mdetail.MESSAGE_SEQ = mf.message_seq;
                                mdetail.MFID = mf.mfid;
                                mdetail.CHANNEL = mf.channel;
                                mdetail.MFID = mf.mfid;
                                mdetail.MESSAGE_DT = com.activate_date.AddDays(mf.days_out);
                                mdetail.EMAIL = com.email;
                                mdetail.PHONE = com.phone;
                                smd = SaveMessageDetail(mdetail);
                        }
                }
                //sql = "update CUSTOMER_COMMUNICATIONS set CC_STATUS = 'COMPLETE' where COMM_STATUS = 'INPROCESS'";
                //ExecuteSQL(sql);

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
            DateTime dtToday = DateTime.Today;
            IEnumerable results = currentSession.Connection.Query(@"SELECT  [CC_RECNUM] ,CC.INDIV_ID ,[CHANNEL] ,[MFID] ,[FLOW_ID] ,[REC_CREATE_DATE] ,[CC_STATUS] ,[ACTIVATE_DATE], EMAIL, PHONE
                                                                     FROM CUSTOMER_COMMUNICATIONS CC INNER JOIN CUSTOMER_PROFILE CP ON CP.INDIV_ID = CC.INDIV_ID
                                                                    WHERE ( CC_STATUS = 'ACTIVE' AND ACTIVATE_DATE = '" +  dtToday + "')");
            List<Communication> comList = new List<Communication>();
            foreach (dynamic row in results)
            {
                comList.Add(new Communication(row.CC_RECNUM, row.INDIV_ID, row.CHANNEL, row.MFID, row.FLOW_ID, row.REC_CREATE_DATE, row.CC_STATUS, row.ACTIVATE_DATE, row.EMAIL, row.PHONE));
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
