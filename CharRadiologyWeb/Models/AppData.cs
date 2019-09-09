using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
//using GalaSoft.MvvmLight;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Configuration;


namespace CharRadiologyWeb
{
    public class AppData
  {
        public string Environment
        {
            get
            {
                string environ = GetSetting("api-environment");
                return environ;
            }
        }
        public string ApiAddress
        {
            get
            {
                string address = GetSetting("api-address");
                return address;
            }
        }
        public   string CouponFrequency
        {
            get
            {
                string couponfrequency = GetStaticSetting("api-couponfrequency");
                return couponfrequency;
            }
        }
        public  string CouponStart
        {
            get
            {
                string couponstart = GetStaticSetting("api-couponstart");
                return couponstart;
            }
        }
        public uint SGWTimer
        {
            get
            {
                uint timervalue = Convert.ToUInt32(GetSetting("api-sgwtimer"));
                return timervalue;
            }
        }
        public uint MarketingTimer
        {
            get
            {
                uint timervalue = Convert.ToUInt32(GetSetting("api-marketingtimer"));
                return timervalue;
            }
        }
        public AppData()
        {

        }
        private string GetSetting(string SettingType)
        {
            return ConfigurationManager.AppSettings[SettingType].ToString();
        }
        private static string GetStaticSetting(string SettingType)
        {
            return ConfigurationManager.AppSettings[SettingType].ToString();
        }

    }
}
