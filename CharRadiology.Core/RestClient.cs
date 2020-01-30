using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharRadiology.Core.Enums;
using CharRadiology.Core.Models;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;
namespace CharRadiology.Core
{
    public class RestClient
    {
        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string API_Version { get; set; }
        public string PostData { get; set; }

        public RestClient()
        {
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
        }
        public RestClient(string endpoint)
        {
            EndPoint = endpoint;
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
        }
        public RestClient(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            //ContentType = "application/json";
            ContentType = "text/xml";
            PostData = "";
        }

        public RestClient(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
        }


        public string MakeRequest(Account acc)
        {
            return MakeRequest("", acc);
        }
        public DataSet MakeRequestDS(Account acc)
        {
            return MakeRequestDS("", acc);
        }

        public string MakeRequest(string parameters, Account acc)
        {
            try
            {

                var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);
                request.Accept = ContentType;
                request.Method = Method.ToString();
                //request.ContentLength = 0;
                request.ContentType = ContentType;
                request.Headers.Add("API-Version", "2.2");
                request.Headers.Add("API-AppId", acc.AppId);
                request.Headers.Add("API-Username", acc.Username);
                request.Headers.Add("API-Password", acc.Password);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (!string.IsNullOrEmpty(PostData) && (Method == HttpVerb.POST || Method == HttpVerb.PUT))
                {
                    var encoding = new UTF8Encoding();
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                        throw new ApplicationException(message);
                    }

                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                    }

                    return responseValue;
                }
            }
            catch (Exception ex)
            {

                var message = ex.ToString();
                throw new ApplicationException(message);
            }
        }
        public DataSet MakeRequestDS(string parameters, Account acc)
        {
            try
            {

                var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);
                DataSet ds = new DataSet();
                request.Accept = ContentType;
                request.Method = Method.ToString();
                //request.ContentLength = 0;
                request.ContentType = ContentType;
                request.Headers.Add("API-Version", "2.2");
                request.Headers.Add("API-AppId", acc.AppId);
                request.Headers.Add("API-Username", acc.Username);
                request.Headers.Add("API-Password", acc.Password);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (!string.IsNullOrEmpty(PostData) && (Method == HttpVerb.POST || Method == HttpVerb.PUT))
                {
                    var encoding = new UTF8Encoding();
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                        throw new ApplicationException(message);
                    }

                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(responseValue);
                    using (MemoryStream stream = new MemoryStream(buffer))
                    {
                        XmlReader reader = XmlReader.Create(stream);
                        ds.ReadXml(reader);
                    }
                    return ds;
                }
            }
            catch (Exception ex)
            {

                var message = ex.ToString();
                throw new ApplicationException(message);
            }

        }

    } // class
}
