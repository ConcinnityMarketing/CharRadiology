using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Net;
using System.Net.Http;
namespace CharRadiologyWeb
{
    public class DataService : IDataService
    {

        public AppData ad = new AppData();
        protected class Messages
        {
            public string header { get; set; }
            public string message { get; set; }
        }

        protected static Messages msg
        {
            get;
            set;
        }
        public string Environment
        {
            get
            {
                string environ = ad.Environment;
                return environ;

            }
        }

        public string GetData(Object DataRequest, string DataMethod)
        {
            RestClient client = new RestClient();
            string sResponse = null;
            string parms = "";

            string DataResult = "";
            DataResultDtoWrapper dataResultDtoWrapper = new DataResultDtoWrapper();

            try
            {
                DataRequestDtoWrapper dataRequestDtoWrapper = new DataRequestDtoWrapper();
                dataRequestDtoWrapper.chkUser = DataRequest;

                using (var httpClient = CreateClient())
                {
                    var postJson = JsonConvert.SerializeObject(dataRequestDtoWrapper);
                    var content = new StringContent(postJson, Encoding.UTF8, "application/json");

                    client.EndPoint = httpClient.BaseAddress.ToString() + "/" + DataMethod;

                    client.Method = RestClient.HttpVerb.POST;
                    client.ContentType = "application/json";
                    client.PostData = postJson;
                    sResponse = client.MakeRequest(parms);
                }
            }
            catch (Exception ex)
            {
                // todo: implement pop-up
                //msg.message = "Unable to connect with the server. Check your internet connection and try again.";
                //msg.header = "Connection Error";
                throw ex;
            }
            return sResponse;
        }

        public HttpClient CreateClient()
        {
            string address = ad.ApiAddress;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(address)
            };
            return httpClient;
        }

    }
}
