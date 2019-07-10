using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageRunner
{
    public class MessageReturn
    {
        public string status { get; set; }
        public GenericStatusCodes code { get; set; }
        public string desc { get; set; }
    }
    public class MessageData
    {
        public string guid { get; set; }
        public string env { get; set; }
        public string client { get; set; }
    }
    public class MessageProcessResultDtoWrapper
    {
        public MessageReturn MessageProcessResult
        {
            get;
            set;
        }
    }
    public class MessageProcessListResultDtoWrapper
    {
        public CampaignBuildReturn MessageProcessListResult
        {
            get;
            set;
        }
    }

    public class CampaignBuildReturn
    {
        public string status { get; set; }
        public GenericStatusCodes code { get; set; }
        public string desc { get; set; }
        public List<Campaign_Build> lstCampaignBuild { get; set; }
    }

    public class Campaign_Build
    {
        public string VAR_VALUE { get; set; }
        public string VAR_DESCRIPTION { get; set; }
    }
    public class CheckExistData
    {
        public string guid { get; set; }
        public string env { get; set; }
        public string client { get; set; }
        public string sql { get; set; }
        public string ReturnType { get; set; }
    }

    public enum GenericStatusCodes
    {
        Success = 1,
        Fail = 2,
        Other = 3
    }

}
