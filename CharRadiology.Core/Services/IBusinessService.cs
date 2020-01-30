using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using CharRadiology.Core.Models;

namespace CharRadiology.Core.Services
{
    public interface IBusinessService
    {
        MessageReturn MessageProcess(MessageData chkUser, BusinessModel bm);
        AdminReturn GetAdmins(MessageData chkUser, BusinessModel bm);
        List<State> StateList();
        PinEntryReturn GetUserPinEntryInfo(PinEntryData chkUser);
        PinEntryReturn GetCustomerInfo(PinEntryData chkUser);
        string GetGUID();
        SurveyReturn SaveCustResponse(ProfileData chkUser);
        SurveyReturn SaveCustQA(QAData chkUser);
        MessageReturn Notify(Notification TextNotification, BusinessModel bm);
        MessageReturn UpdateTestingRecords(TestingData chkUser);
        MessageReturn CallNotify(CallNotification CallNotification, BusinessModel bm);
        string GetResponseCode(string Channel);
        CommunicationReturn GetCommunication(TestingData chkUser);
    }
}
