using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using EnvoyService.Core.Enums;
using System.Data;

namespace EnvoyService.Core.Models
{
    public class BusinessModel
    {
        public string WebID { get; set; }
        public string SMTPHost { get; set; }
        public string IContactID { get; set; }
        public string IContactURI { get; set; }
        public string ANewListID { get; set; }
        public string AOldListID { get; set; }
        public string BNewListID { get; set; }
        public string BOldListID { get; set; }
        public string EmailType { get; set; }
        public string BMessageID { get; set; }
        public string AMessageID { get; set; }
        public int retries { get; set; }
        public string ipaddress { get; set; }
        public string environ { get; set; }
        public string client { get; set; }
        public BusinessModel()
        {

        }
    }
    [DataContract]
    public class Notification
    {
        [DataMember]
        public string SubscriptionId { get; set; }
        [DataMember]
        public string AccountId { get; set; }
        [DataMember]
        public Text Text { get; set; }
    }
    public class Text
    {
        public string id { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
        public string State { get; set; }
        public string ContactId { get; set; }
        public string Inbound { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string FinalResult { get; set; }
        public string Message { get; set; }
        public TextRecord TextRecord { get; set; }
    }
    public class TextRecord
    {
        public string id { get; set; }
        public string Result { get; set; }
        public string FinishTime { get; set; }
        public string BilledAmount { get; set; }
        public string SwitchId { get; set; }
        public string CallerName { get; set; }
        public string Message { get; set; }
    }
    [DataContract]
    public class CallNotification
    {
        [DataMember]
        public string SubscriptionId { get; set; }
        [DataMember]
        public string AccountId { get; set; }
        [DataMember]
        public Call Call { get; set; }
    }
    public class Call
    {
        public string id { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
        public string State { get; set; }
        public string ContactId { get; set; }
        public string Inbound { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string FinalResult { get; set; }
        public CallRecord CallRecord { get; set; }
    }
    public class CallRecord
    {
        public string id { get; set; }
        public string Result { get; set; }
        public string FinishTime { get; set; }
        public string BilledAmount { get; set; }
        public string SwitchId { get; set; }
        public string CallerName { get; set; }
        public string AnswerTime { get; set; }
        public string Duration { get; set; }
        public string AgentCall { get; set; }
    }

    [DataContract]
    public class EmailData
    {
        [DataMember]
        public string env { get; set; }
    }
    [DataContract]
    public class TestingData
    {
        [DataMember]
        public string env { get; set; }
        [DataMember]
        public string guid { get; set; }
        [DataMember]
        public string indiv_id { get; set; }
    }
    [DataContract]
    public class ClientTestingData
    {
        [DataMember]
        public string env { get; set; }
        [DataMember]
        public string guid { get; set; }
        [DataMember]
        public string indiv_id { get; set; }
        [DataMember]
        public int test_number { get; set; }
    }
    [DataContract]
    public class MessageData
    {
        [DataMember]
        public string env { get; set; }

    }
    [DataContract]
    public class CommunicationReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public GenericStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public string indiv_id { get; set; }
        [DataMember]
        public DateTime test1_date { get; set; }
        [DataMember]
        public DateTime test2_date { get; set; }
    }
    [DataContract]
    public class EmailReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public MailStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public int Message_Count { get; set; }
        [DataMember]
        public int Anniversary_Count { get; set; }
    }
    [DataContract]
    public class RegisterReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public RegistrationStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public int indiv_id { get; set; }
        [DataMember]
        public int TankRecNum { get; set; }
        [DataMember]
        public string sessionid { get; set; }
    }
    [DataContract]
    public class MessageReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public GenericStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
    }
    [DataContract]
    public class ResponseCodeReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public GenericStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public string response_code { get; set; }
    }
    [DataContract]
    public class AdminReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public GenericStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public List<Admin> adminList { get; set; }
    }
    [DataContract]
    public class ProfileData
    {
        [DataMember]
        public string GUID { get; set; }
        [DataMember]
        public string ENV { get; set; }
        [DataMember]
        public string INDIV_ID { get; set; }
        [DataMember]
        public string FIRST_NAME { get; set; }
        [DataMember]
        public string MID_NAME { get; set; }
        [DataMember]
        public string LAST_NAME { get; set; }
        [DataMember]
        public string GENDER { get; set; }
        [DataMember]
        public string BIRTH_DATE { get; set; }
        [DataMember]
        public string ADDRESS1 { get; set; }
        [DataMember]
        public string ADDRESS2 { get; set; }
        [DataMember]
        public string CITY { get; set; }
        [DataMember]
        public string STATE { get; set; }
        [DataMember]
        public string ZIP { get; set; }
        [DataMember]
        public string ZIP4 { get; set; }
        [DataMember]
        public string STATUS { get; set; }
        [DataMember]
        public string USPS_STATUS { get; set; }
        [DataMember]
        public string USPS_OPT_CD { get; set; }
        [DataMember]
        public string AV_CODE { get; set; }
        [DataMember]
        public string PHONE { get; set; }
        [DataMember]
        public string EMAIL { get; set; }
        [DataMember]
        public string TEXT_MESSAGE { get; set; }
        [DataMember]
        public string EMAIL_STATUS { get; set; }
        [DataMember]
        public string PHONE_STATUS { get; set; }
        [DataMember]
        public string TEXT_MESSAGE_STATUS { get; set; }
        [DataMember]
        public string PHONE_OPT_CD { get; set; }
        [DataMember]
        public string EMAIL_OPT_CD { get; set; }
        [DataMember]
        public string TEXT_MESSAGE_OPT_CD { get; set; }
        [DataMember]
        public string SIGNATURE { get; set; }
        [DataMember]
        public string RESPONSE_CODE { get; set; }
        [DataMember]
        public string LAT { get; set; }
        [DataMember]
        public string LONG { get; set; }
        [DataMember]
        public string NAME_SUFX { get; set; }
        [DataMember]
        public string HOUSENUM { get; set; }
        [DataMember]
        public string COUNTRY { get; set; }
        [DataMember]
        public string DPBC { get; set; }
        [DataMember]
        public string COA_DATE { get; set; }
        [DataMember]
        public string FIPS { get; set; }
        [DataMember]
        public string EXTERNAL_REF_NUMBER { get; set; }
        [DataMember]
        public string COUNTY { get; set; }
        [DataMember]
        public string GEOCODE_LEVEL { get; set; }
        [DataMember]
        public string MATCH_KEY { get; set; }
        [DataMember]
        public string RESPONSE_TYPE { get; set; }
        [DataMember]
        public string MEDIA_CODE { get; set; }
        [DataMember]
        public string KEY_BATCH_ID { get; set; }
        [DataMember]
        public string USER_ID { get; set; }
        [DataMember]
        public string TankRecNum { get; set; }
        [DataMember]
        public string FIRST_RESPONSE_DATE { get; set; }
        [DataMember]
        public string FIRST_RESPONSE { get; set; }
        [DataMember]
        public string LAST_RESPONSE_DATE { get; set; }
        [DataMember]
        public string LAST_RESPONSE { get; set; }
        [DataMember]
        public string FULL_NAME { get; set; }
        [DataMember]
        public string CHECK_DIGIT { get; set; }
        [DataMember]
        public string CUST_PWD { get; set; }
        [DataMember]
        public string WEB_USER_ID { get; set; }
        [DataMember]
        public string WEB_SOURCE { get; set; }
        [DataMember]
        public string WEB_VERSION { get; set; }
        [DataMember]
        public string MGM_ID { get; set; }
        [DataMember]
        public string PROCEDURE_DATE { get; set; }
    }
    [DataContract]
    public class QAData
    {
        [DataMember]
        public string GUID { get; set; }
        [DataMember]
        public string ENV { get; set; }
        [DataMember]
        public string INDIV_ID { get; set; }
        [DataMember]
        public string RESPONSE_CODE { get; set; }
        [DataMember]
        public string RESPONSE_DATE { get; set; }
        [DataMember]
        public string EXTERNAL_REF_NUMBER { get; set; }
        [DataMember]
        public List<Question> q_and_a { get; set; }

    }
    [DataContract]
    public class State
    {
        [DataMember]
        public string state_abbr { get; set; }
        [DataMember]
        public string state_name { get; set; }
        public State()
        {
        }
        public State(string estate_abbr, string estate_name)
        {
            this.state_abbr = estate_abbr;
            this.state_name = estate_name;
        }
    }
    public class Question
    {
        [DataMember]
        public int question_code;
        [DataMember]
        public string question_desc;
        [DataMember]
        public int question_order;
        [DataMember]
        public string answer_table;
        [DataMember]
        public string multiple_answers;
        [DataMember]
        public List<Answer> answers;
        public Question()
        {
        }
        public Question(int equestion_code, string equestion_desc, int equestion_order, string eanswer_table, string emultiple_answers, List<Answer> eanswers)
        {
            this.question_code = equestion_code;
            this.question_desc = equestion_desc;
            this.question_order = equestion_order;
            this.answer_table = eanswer_table;
            this.multiple_answers = emultiple_answers;
            this.answers = eanswers;
        }
    }
    public class Answer
    {
        public int answer_code;
        public string answer_desc;
        public int answer_order;
        public Answer()
        {
        }
        public Answer(int eanswer_code, string eanswer_desc, int eanswer_order)
        {
            this.answer_code = eanswer_code;
            this.answer_desc = eanswer_desc;
            this.answer_order = eanswer_order;
        }
    }
    [DataContract]
    public class GenericData
    {
        [DataMember]
        public string ENV { get; set; }
        [DataMember]
        public string GUID { get; set; }
        [DataMember]
        public string SQL { get; set; }
        [DataMember]
        public string INDIV_ID { get; set; }
        [DataMember]
        public string RESPONSE_CODE { get; set; }
    }

    public class Admin
    {
        public string EMAIL { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public Admin()
        {

        }
        public Admin(string eEMAIL, string eFIRST_NAME, string eLAST_NAME)
        {
            this.EMAIL = eEMAIL;
            this.FIRST_NAME = eFIRST_NAME;
            this.LAST_NAME = eLAST_NAME;
        }
    }
    public class MessageFlow
    {
        public int mf_recnum { get; set; }
        public Int16 message_seq { get; set; }
        public string mfid { get; set; }
        public Int16 days_out { get; set; }
        public string channel { get; set; }
        public MessageFlow()
        {

        }
        public MessageFlow(int emf_recnum, Int16 emessage_seq, string emfid, Int16 edays_out, string echannel)
        {
            this.mf_recnum = emf_recnum;
            this.message_seq = emessage_seq;
            this.mfid = emfid;
            this.days_out = edays_out;
            this.channel = echannel;
        }
    }
    public class Communication
    {
        public int cc_recnum { get; set; }
        public int indiv_id { get; set; }
        public string channel { get; set; }
        public string mfid { get; set; }
        public string flow_id { get; set; }
        public DateTime rec_create_date { get; set; }
        public string cc_status { get; set; }
        public DateTime activate_date { get; set; }
        public DateTime actual_send_date { get; set; }
        public string delivery_flag { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

        public Communication()
        {

        }
        public Communication(int ecc_recnum, int eindiv_id, string echannel, string emfid, string eflow_id, DateTime erec_create_date, string ecc_status, DateTime eactivate_date, string eemail, string ephone)
        {
            this.cc_recnum = ecc_recnum;
            this.indiv_id = eindiv_id;
            this.channel = echannel;
            this.mfid = emfid;
            this.flow_id = eflow_id;
            this.rec_create_date = erec_create_date;
            this.cc_status = ecc_status;
            this.activate_date = eactivate_date;
            //this.actual_send_date = eactual_send_date;
            //this.delivery_flag = edelivery_flag;
            this.email = eemail;
            this.phone = ephone;
        }

    }
    public class SF_Client_Results
    {
        public int indiv_id { get; set; }
        public DateTime test1_date { get; set; }
        public DateTime test2_date { get; set; }
        public string test1_result { get; set; }
        public string test2_result { get; set; }
        public SF_Client_Results()
        {

        }
        public SF_Client_Results(int eindiv_id, string etest1_result, string etest2_result, DateTime etest1_date, DateTime etest2_date)
        {
            this.indiv_id = eindiv_id;
            this.test1_result = etest1_result;
            this.test2_result = etest2_result;
            this.test1_date = etest1_date;
            this.test2_date = etest2_date;
        }

    }
    public class Profile
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public DateTime birth_date { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
    public class PinEntryData
    {
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string env { get; set; }
        [DataMember]
        public string brand { get; set; }
        [DataMember]
        public string pin { get; set; }
        [DataMember]
        public string birth_year { get; set; }
        public string guid { get; set; }
    }
    [DataContract]
    public class PinEntryReturn
    {
        [DataMember]
        public string sessionid { get; set; }
        [DataMember]
        public int? indiv_id { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public PinEntryStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public Profile pinProfile { get; set; }
    }
    [DataContract]
    public class SurveyReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public SurveyStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public int indiv_id { get; set; }
        [DataMember]
        public int TankRecNum { get; set; }
    }

    public class AddrStndReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public GenericStatusCodes code { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public string ADDRESS1 { get; set; }
        [DataMember]
        public string ADDRESS2 { get; set; }
        [DataMember]
        public string CITY { get; set; }
        [DataMember]
        public string STATE { get; set; }
        [DataMember]
        public string ZIP { get; set; }
        [DataMember]
        public string ZIP4 { get; set; }
        [DataMember]
        public double LAT { get; set; }
        [DataMember]
        public double LONG { get; set; }
        [DataMember]
        public string AgeVerify { get; set; }
    }
    [DataContract]
    public class AgeVerifData
    {
        [DataMember]
        public string ENV { get; set; }
        [DataMember]
        public string GUID { get; set; }
        [DataMember]
        public string FIRST_NAME { get; set; }
        [DataMember]
        public string LAST_NAME { get; set; }
        [DataMember]
        public string ADDRESS1 { get; set; }
        [DataMember]
        public string ADDRESS2 { get; set; }
        [DataMember]
        public string CITY { get; set; }
        [DataMember]
        public string STATE { get; set; }
        [DataMember]
        public string ZIP { get; set; }
        [DataMember]
        public DateTime BIRTH_DATE { get; set; }
    }

    public class Account
    {
        public string AppId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string client_folder_id { get; set; }
        public string new_list_id { get; set; }
        public string old_list_id { get; set; }

        public Account()
        {

        }
        public Account(string eAppID, string eUsername, string ePassword, string eclient_folder_id, string enew_list_id, string eold_list_id)
        {
            this.AppId = eAppID;
            this.Username = eUsername;
            this.Password = ePassword;
            this.client_folder_id = eclient_folder_id;
            this.new_list_id = enew_list_id;
            this.old_list_id = eold_list_id;
        }
    }
    public class DBMessage
    {
        public int MESSAGE_ID { get; set; }
        public int MESSAGE_SEQ { get; set; }
        public string CHANNEL { get; set; }
        public string MFID { get; set; }
        public string MESSAGE_TEXT { get; set; }

        public DBMessage()
        {

        }
        public DBMessage(int eMESSAGE_ID, int eMESSAGE_SEQ)
        {
            this.MESSAGE_ID = eMESSAGE_ID;
            this.MESSAGE_SEQ = eMESSAGE_SEQ;
        }
        public DBMessage(string eMFID, int eMESSAGE_SEQ, int eMESSAGE_ID, string eCHANNEL, string eMESSAGE_TEXT)
        {
            this.MESSAGE_SEQ = eMESSAGE_SEQ;
            this.MESSAGE_ID = eMESSAGE_ID;
            this.CHANNEL = eCHANNEL;
            this.MFID = eMFID;
            this.MESSAGE_TEXT = eMESSAGE_TEXT;
        }

    }
    public class MessageModel
    {
        public int INDIV_ID { get; set; }
        public string MFID { get; set; }
        public string EMAIL { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string ADDRESS1 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string CHANNEL { get; set; }
        public DateTime? MESSAGE_DT { get; set; }
        public string STATUS { get; set; }
        public DateTime? UPDATE_DT { get; set; }
        public int MESSAGE_SEQ { get; set; }
        public int MD_RECNUM { get; set; }
        public string PHONE { get; set; }
        //public string OFFER_CODE { get; set; }
        //public string CHILD_NAME { get; set; }
        public MessageModel()
        {

        }
        public MessageModel(int eINDIV_ID, string eMFID, string eEMAIL, string eFIRST_NAME, string eLAST_NAME, string eADDRESS1, string eCITY, string eSTATE, string eZIP,
                                string eCHANNEL, DateTime? eMESSAGE_DT, string eSTATUS, DateTime? eUPDATE_DT, int eMESSAGE_SEQ, int eMD_RECNUM, string ePHONE)
        {
            this.INDIV_ID = eINDIV_ID;
            this.MFID = eMFID;
            this.EMAIL = eEMAIL;
            this.FIRST_NAME = eFIRST_NAME;
            this.LAST_NAME = eLAST_NAME;
            this.ADDRESS1 = eADDRESS1;
            this.CITY = eCITY;
            this.STATE = eSTATE;
            this.ZIP = eZIP;
            this.CHANNEL = eCHANNEL;
            this.MESSAGE_DT = eMESSAGE_DT;
            this.CHANNEL = eCHANNEL;
            this.UPDATE_DT = eUPDATE_DT;
            this.MESSAGE_SEQ = eMESSAGE_SEQ;
            this.MD_RECNUM = eMD_RECNUM;
            this.PHONE = ePHONE;
            //this.OFFER_CODE = eOFFER_CODE;
            //this.CHILD_NAME = eCHILD_NAME;
        }
    }
    public class MessageDetail
    {
        public int INDIV_ID { get; set; }
        public Int16 MESSAGE_SEQ { get; set; }
        public string MFID { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string CHANNEL { get; set; }
        public DateTime MESSAGE_DT { get; set; }

    }
    public class ContactTest
    {
        public string contactId { get; set; }
        public string email { get; set; }
        public string prefix { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string suffix { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string phone { get; set; }
        public string business { get; set; }
        public string status { get; set; }
        public string indiv_id { get; set; }
        public string late { get; set; }
        public string overdue { get; set; }
    }
    [Serializable]
    public class Contact
    {
        #region "Private Fields"

        private string _email;
        private string _prefix;
        private string _firstName;
        private string _lastName;
        private string _suffix;
        private string _street;
        private string _city;
        private string _state;
        private string _postalCode;
        private string _phone;
        private string _business;
        private string _status;
        private string _indiv_id;
        private string _test1_date;
        private string _test2_date;

        #endregion

        #region "Public Properties"

        public string email
        {
            get { return (_email == null ? string.Empty : _email); }
            set { _email = (value != null ? value.Trim() : value); }
        }
        public string prefix
        {
            get { return (_prefix == null ? string.Empty : _prefix); }
            set { _prefix = (value != null ? value.Trim() : value); }
        }
        public string firstName
        {
            get { return (_firstName == null ? string.Empty : _firstName); }
            set { _firstName = (value != null ? value.Trim() : value); }
        }
        public string lastName
        {
            get { return (_lastName == null ? string.Empty : _lastName); }
            set { _lastName = (value != null ? value.Trim() : value); }
        }
        public string suffix
        {
            get { return (_suffix == null ? string.Empty : _suffix); }
            set { _suffix = (value != null ? value.Trim() : value); }
        }
        public string street
        {
            get { return (_street == null ? string.Empty : _street); }
            set { _street = (value != null ? value.Trim() : value); }
        }
        public string city
        {
            get { return (_city == null ? string.Empty : _city); }
            set { _city = (value != null ? value.Trim() : value); }
        }
        public string state
        {
            get { return (_state == null ? string.Empty : _state); }
            set { _state = (value != null ? value.Trim() : value); }
        }
        public string postalCode
        {
            get { return (_postalCode == null ? string.Empty : _postalCode); }
            set { _postalCode = (value != null ? value.Trim() : value); }
        }
        public string phone
        {
            get { return (_phone == null ? string.Empty : _phone); }
            set { _phone = (value != null ? value.Trim() : value); }
        }
        public string business
        {
            get { return (_business == null ? string.Empty : _business); }
            set { _business = (value != null ? value.Trim() : value); }
        }
        public string status
        {
            get { return (_status == null ? string.Empty : _status); }
            set { _status = (value != null ? value.Trim() : value); }
        }
        public string indiv_id
        {
            get { return (_indiv_id == null ? string.Empty : _indiv_id); }
            set { _indiv_id = (value != null ? value.Trim() : value); }
        }
        public string test1_date
        {
            get { return (_test1_date == null ? string.Empty : _test1_date); }
            set { _test1_date = (value != null ? value.Trim() : value); }
        }
        public string test2_date
        {
            get { return (_test2_date == null ? string.Empty : _test2_date); }
            set { _test2_date = (value != null ? value.Trim() : value); }
        }

        #endregion
        public Contact()
        {

        }
        public Contact(string eemail,
         string eprefix,
         string efirstName,
         string elastName,
         string esuffix,
         string estreet,
         string ecity,
         string estate,
         string epostalCode,
         string ephone,
         string ebusiness,
         string estatus,
         string eindiv_id,
         string etest1_date,
         string etest2_date)
        {
            this.email = eemail;
            this.prefix = eprefix;
            this.firstName = efirstName;
            this.lastName = elastName;
            this.suffix = esuffix;
            this.street = estreet;
            this.city = ecity;
            this.state = estate;
            this.postalCode = epostalCode;
            this.phone = ephone;
            this.business = ebusiness;
            this.status = estatus;
            this.indiv_id = eindiv_id;
            this.test1_date = etest1_date;
            this.test2_date = etest2_date;
        }
    }
    [Serializable]
    public class ContactWithId
    {
        #region "Private Fields"

        private int _contactId;
        private string _email;
        private string _prefix;
        private string _firstName;
        private string _lastName;
        private string _suffix;
        private string _street;
        private string _city;
        private string _state;
        private string _postalCode;
        private string _phone;
        private string _business;
        private string _status;
        private string _indiv_id;
        private string _late;
        private string _overdue;

        #endregion

        #region "Public Properties"

        public int contactId
        {
            get { return _contactId; }
            set { _contactId = value; }
        }
        public string email
        {
            get { return (_email == null ? string.Empty : _email); }
            set { _email = (value != null ? value.Trim() : value); }
        }
        public string prefix
        {
            get { return (_prefix == null ? string.Empty : _prefix); }
            set { _prefix = (value != null ? value.Trim() : value); }
        }
        public string firstName
        {
            get { return (_firstName == null ? string.Empty : _firstName); }
            set { _firstName = (value != null ? value.Trim() : value); }
        }
        public string lastName
        {
            get { return (_lastName == null ? string.Empty : _lastName); }
            set { _lastName = (value != null ? value.Trim() : value); }
        }
        public string suffix
        {
            get { return (_suffix == null ? string.Empty : _suffix); }
            set { _suffix = (value != null ? value.Trim() : value); }
        }
        public string street
        {
            get { return (_street == null ? string.Empty : _street); }
            set { _street = (value != null ? value.Trim() : value); }
        }
        public string city
        {
            get { return (_city == null ? string.Empty : _city); }
            set { _city = (value != null ? value.Trim() : value); }
        }
        public string state
        {
            get { return (_state == null ? string.Empty : _state); }
            set { _state = (value != null ? value.Trim() : value); }
        }
        public string postalCode
        {
            get { return (_postalCode == null ? string.Empty : _postalCode); }
            set { _postalCode = (value != null ? value.Trim() : value); }
        }
        public string phone
        {
            get { return (_phone == null ? string.Empty : _phone); }
            set { _phone = (value != null ? value.Trim() : value); }
        }
        public string business
        {
            get { return (_business == null ? string.Empty : _business); }
            set { _business = (value != null ? value.Trim() : value); }
        }
        public string status
        {
            get { return (_status == null ? string.Empty : _status); }
            set { _status = (value != null ? value.Trim() : value); }
        }
        public string indiv_id
        {
            get { return (_indiv_id == null ? string.Empty : _indiv_id); }
            set { _indiv_id = (value != null ? value.Trim() : value); }
        }
        public string late
        {
            get { return (_late == null ? string.Empty : _late); }
            set { _late = (value != null ? value.Trim() : value); }
        }

        #endregion
    }
    [Serializable]
    public class messages
    {
        public string messageId { get; set; }
        public string campaignId { get; set; }
        public string subject { get; set; }
        public string messageType { get; set; }
        public string messageName { get; set; }
        public string htmlBody { get; set; }
        public string textBody { get; set; }
        public string createDate { get; set; }
        public string clientFolderId { get; set; }
        public string clientId { get; set; }
    }
    [Serializable]
    public class NewMessage
    {
        public string campaignId { get; set; }
        public string subject { get; set; }
        public string messageType { get; set; }
        public string messageName { get; set; }
        public string htmlBody { get; set; }
        public string textBody { get; set; }
    }
    [Serializable]
    public class message
    {
        public string campaignId { get; set; }
        public string subject { get; set; }
        public string messageType { get; set; }
        public string messageName { get; set; }
        public string htmlBody { get; set; }
        public string textBody { get; set; }
    }
    [Serializable]
    public class Subscription
    {
        public string contactId { get; set; }
        public string listId { get; set; }
        public string status { get; set; }
    }
    [Serializable]
    public class NewList
    {
        public string listId { get; set; }
        public string status { get; set; }
    }

    [Serializable]
    public class Send
    {
        public string messageId { get; set; }
        public string includeListIds { get; set; }
    }
    [Serializable]
    public class ContactU
    {
        public string contactId { get; set; }
        public string email { get; set; }
        public string prefix { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string suffix { get; set; }
        public string street { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string phone { get; set; }
        public string business { get; set; }
        public string status { get; set; }
        public string late { get; set; }
        public string indiv_id { get; set; }
    }
}


