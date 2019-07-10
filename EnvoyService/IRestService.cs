using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using EnvoyService.Core.Models;
using System.Data;
using System.Data.Sql;

namespace EnvoyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRestService
    {

        // TODO: Add your service operations here
        // Miscellaneous Processes


        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
        //[WebInvoke(Method = "POST", UriTemplate = "MessageProcess", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //string MessageProcess(string chkUser);

        [WebInvoke(Method = "POST", UriTemplate = "MessageProcess", ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        MessageReturn MessageProcess(MessageData chkUser);

        [WebInvoke(Method = "POST", UriTemplate = "StateList", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<State> StateList(GenericData Search);

        //[WebGet(UriTemplate = "GetUserPinEntry", ResponseFormat = WebMessageFormat.Json)]
        [WebInvoke(Method = "POST", UriTemplate = "GetUserPinEntry", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        PinEntryReturn GetUserPinEntry(PinEntryData chkUser);

        //[WebGet(UriTemplate = "GetCustomerInfo", ResponseFormat = WebMessageFormat.Json)]
        [WebInvoke(Method = "POST", UriTemplate = "GetCustomerInfo", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        PinEntryReturn GetCustomerInfo(PinEntryData chkUser);

        [WebInvoke(Method = "POST", UriTemplate = "SaveCustQA", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        SurveyReturn SaveCustQA(QAData chkUser);

        [WebInvoke(Method = "POST", UriTemplate = "SaveCustResponse", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        SurveyReturn SaveCustResponse(ProfileData chkUser);

        [WebInvoke(Method = "POST", UriTemplate = "UpdateTestingRecords", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        MessageReturn UpdateTestingRecords(TestingData chkUser);

        //[WebGet(UriTemplate = "GetResponseCode", ResponseFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //ResponseCodeReturn GetResponseCode(PinEntryData chkUser);

        //[WebGet(UriTemplate = "GetCommunication", ResponseFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //CommunicationReturn GetCommunication(TestingData chkUser);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

}

