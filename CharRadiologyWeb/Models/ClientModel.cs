using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharRadiologyWeb
{
    public class GetUserPinEntryResultDtoWrapper
    {
        public PinEntryReturn GetUserPinEntryResult
        {
            get;
            set;
        }
    }

    public class RegisterResultDtoWrapper
    {
        public RegisterReturn RegisterResult
        {
            get;
            set;
        }
    }
    public class CheckValidResponseCodeResultDtoWrapper
    {
        public CheckCodeReturn CheckValidResponseCodeResult
        {
            get;
            set;
        }
    }
    public class PinEntryData
    {
        public string ip { get; set; }
        public string env { get; set; }
        public string brand { get; set; }
        public string pin { get; set; }
        public string guid { get; set; }
    }
    public class PinEntryReturn
    {
        public string sessionid { get; set; }
        public int? indiv_id { get; set; }
        public string status { get; set; }
        public PinEntryStatusCodes code { get; set; }
        public string desc { get; set; }
        public Profile pinProfile { get; set; }
    }

    public class RegisterReturn
    {
        public string status { get; set; }
        public RegistrationStatusCodes code { get; set; }
        public string desc { get; set; }
        public int indiv_id { get; set; }
        public int TankRecNum { get; set; }
    }
    public class CheckCodeReturn
    {
        public string status { get; set; }
        public GenericStatusCodes code { get; set; }
        public string desc { get; set; }
    }

    public class GenericData
    {
        public string ENV { get; set; }
        public string GUID { get; set; }
        public string SQL { get; set; }
        public string INDIV_ID { get; set; }
        public string RESPONSE_CODE { get; set; }
    }

    public class Profile
    {
        public int indiv_id { get; set; }
        public string complete_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string zip4 { get; set; }
        public string email { get; set; }
        public string text { get; set; }
        public string vin { get; set; }
        public string vehicle_miles { get; set; }
        public string thank_you { get; set; }

    }

    public class ProfileData
    {
        public string GUID { get; set; }
        public string ENV { get; set; }
        public string CLIENT { get; set; }
        public int INDIV_ID { get; set; }
        public string REP_NAME { get; set; }
        public string NAME_PREFIX { get; set; }
        public string COMPLETE_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string MID_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string NAME_SUFX { get; set; }
        public string GENDER { get; set; }
        public string BIRTH_DATE { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string ZIP4 { get; set; }
        public string STATUS { get; set; }
        public string USPS_STATUS { get; set; }
        public string USPS_OPT_CD { get; set; }
        public string AV_CODE { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string TEXT_MESSAGE { get; set; }
        public string EMAIL_STATUS { get; set; }
        public string PHONE_STATUS { get; set; }
        public string TEXT_MESSAGE_STATUS { get; set; }
        public string PHONE_OPT_CD { get; set; }
        public string EMAIL_OPT_CD { get; set; }
        public string TEXT_MESSAGE_OPT_CD { get; set; }
        public string SIGNATURE { get; set; }
        public string RESPONSE_CODE { get; set; }
        public string LAT { get; set; }
        public string LONG { get; set; }
        public string HOUSENUM { get; set; }
        public string COUNTRY { get; set; }
        public string DPBC { get; set; }
        public string COA_DATE { get; set; }
        public string FIPS { get; set; }
        public string EXTERNAL_REF_NUMBER { get; set; }
        public string COUNTY { get; set; }
        public string GEOCODE_LEVEL { get; set; }
        public string MATCH_KEY { get; set; }
        public string RESPONSE_TYPE { get; set; }
        public string RESPONSE_NOTE { get; set; }
        public string MEDIA_CODE { get; set; }
        public string KEY_BATCH_ID { get; set; }
        public string USER_ID { get; set; }
        public string WebRecnumProf { get; set; }
        public string FIRST_RESPONSE_DATE { get; set; }
        public string FIRST_RESPONSE { get; set; }
        public string LAST_RESPONSE_DATE { get; set; }
        public string LAST_RESPONSE { get; set; }
        public string VIN { get; set; }
        public string VEHICLE_MILES { get; set; }
    }

    public class User
    {
        public string userid { get; set; }
        public string userstatus { get; set; }
        public string userlevel { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string client { get; set; }

    }
    public class SessionData
    {
        public string sessionid { get; set; }
        public string ip { get; set; }
        public string page_id { get; set; }
        public string guid { get; set; }
        public string env { get; set; }
        public string client { get; set; }
    }
    public class Navigation
    {
        public string survey { get; set; }
        public string survey_text { get; set; }
        public string survey_type { get; set; }
        public string survey_status { get; set; }
        public string last_page { get; set; }
    }

    public class LoginUserResultDtoWrapper
    {
        public LoginReturn LoginUserResult
        {
            get;
            set;
        }
    }
    public class ForgotPWResultDtoWrapper
    {
        public LoginReturn ForgotPWResult
        {
            get;
            set;
        }
    }
    public class GetUserSecResultDtoWrapper
    {
        public LoginReturn GetUserSecResult
        {
            get;
            set;
        }
    }
    public class ResetPWResultDtoWrapper
    {
        public LoginReturn ResetPWResult
        {
            get;
            set;
        }
    }

    public class SessionLogonResultDtoWrapper
    {
        public LoginReturn SessionLogonResult
        {
            get;
            set;
        }
    }

    public class LogonUser
    {
        public string EMAIL_ID { get; set; }
        public int ENVOY_ID { get; set; }
        public string PASSWORD { get; set; }
        public string REMOTEIP { get; set; }
        public string ENV { get; set; }
        public string CLIENT { get; set; }
        public string GUID { get; set; }
    }
    public class LoginReturn
    {
        public string status { get; set; }
        public LoginStatusCodes code { get; set; }
        public string desc { get; set; }
        public bool validsession { get; set; }
        public string sessionid { get; set; }
        public User user { get; set; }
    }
    public class ForgotPWData
    {
        public string EMAIL_ID { get; set; }
        public int ENVOY_ID { get; set; }
        public string REMOTEIP { get; set; }
        public string ENV { get; set; }
        public string CLIENT { get; set; }
        public string GUID { get; set; }
        public string RESET_URI { get; set; }

    }

    public class CheckExistData
    {
        public string guid { get; set; }
        public string env { get; set; }
        public string client { get; set; }
        public string sql { get; set; }
        public string ReturnType { get; set; }
    }
    public class CheckExistResultDtoWrapper
    {
        public CheckExistReturn CheckExistsResult
        {
            get;
            set;
        }
    }
    public class CheckExistReturn
    {
        public string status { get; set; }
        public GenericStatusCodes code { get; set; }
        public string desc { get; set; }
        public string ReturnValue { get; set; }
    }


}