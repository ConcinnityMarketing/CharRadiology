using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
//using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CharRadiologyWeb
{
    public partial class index : System.Web.UI.Page
    {
        public const string CheckGUID = "2932ff6d-0d0f-442d-b039-4aaace4fbaa3";
        public const string Client = "CHAR";
        public const string Brand = "CHAR";
        DataService _dataService = new DataService();
        Messages msg = new Messages();
        //protected App App = new App();
        public static string Environ = ConfigurationManager.AppSettings["Environment"].ToString();
        public string SMTPHost = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
        public class Messages
        {
            public string header { get; set; }
            public string message { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                }
                catch (Exception ex)
                {
                    string errdesc = ex.ToString();
                    Session["ERRORMESSAGE"] = errdesc;
                    errdesc = ex.ToString();
                    //SendErrorEmail(errdesc, Environ);
                    Response.Redirect("ErrorPage.aspx", true);
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SignUpData Customer = new SignUpData();
                _dataService = new DataService();
                //string fixVal = txtMobilePhone.Text;
                //fixVal = fixVal.Replace("(", "");
                //fixVal = fixVal.Replace(")", "");
                //fixVal = fixVal.Replace(" ", "");
                //fixVal = fixVal.Replace("-", "");
                string strResponse = Session["RESPONSE_CODE"] == null ? Brand.ToUpper() + "WEB19" : Session["RESPONSE_CODE"].ToString();
                if (ValidateInput())
                {
                    // If questions are there
                    Answer tans = new Answer();
                    Question ques = new Question();
                    List<Question> lq = new List<Question>();
                    List<Answer> la = new List<Answer>();
                    tans.answer_desc = CheckBox1.Checked ? "Yes" : "No";
                    tans.answer_code = CheckBox1.Checked ? "Yes" : "No";
                    la.Add(tans);
                    ques.answers = la;
                    //ques.question_codeSpecified = true;
                    ques.question_code = "800";
                    ques.question_desc = "I would like to receive information from Charlotte Radiology about Breast Services. This may include a welcome email and regular newsletters.";
                    lq.Add(ques);

                    //tans = new Answer();
                    //ques = new Question();
                    //la = new List<Answer>();
                    //tans.answer_desc = CheckBox2.Checked ? "Yes" : "No";
                    //tans.answer_code = CheckBox2.Checked ? "Yes" : "No";
                    //la.Add(tans);
                    //ques.answers = la;
                    //ques.question_code = "801";
                    //ques.question_desc = "I would like to receive information from Charlotte Radiology about Vein Services. This may include a welcome email and regular newsletters.";
                    //lq.Add(ques);
                    Customer = new SignUpData
                    {
                        //brand = user.Brand,
                        ENV = Environ,
                        GUID = CheckGUID,
                        CLIENT = Client,
                        //INDIV_ID = Session["INDIV_ID"] == null ? 0 : (int)Session["INDIV_ID"],
                        FIRST_NAME = txtFirstName.Text,
                        LAST_NAME = txtLastName.Text,
                        //ADDRESS1 = txtAddress1.Text,
                        //ADDRESS2 = txtAddress2?.Text,
                        //GENDER = cboCCGender.SelectedValue,
                        //BIRTH_DATE = radCCDOB.SelectedDate.Value.ToString(),
                        //CITY = txtCity.Text,
                        //STATE = ddState.SelectedValue,
                        //ZIP = txtZip.Text,
                        STATUS = "0",
                        EMAIL_STATUS = "0",
                        USPS_STATUS = "0",
                        PHONE_STATUS = "0",
                        TEXT_MESSAGE_STATUS = "0",
                        EMAIL_OPT_CD = "I",
                        USPS_OPT_CD = "I",
                        PHONE_OPT_CD = "I",
                        TEXT_MESSAGE_OPT_CD = "I",
                        //PHONE = fixVal,
                        EMAIL = txtEmail.Text,
                        //TEXT_MESSAGE = fixVal,
                        RESPONSE_CODE = strResponse,
                        SURVEYS = lq
                    };
                    Object DataRequest = new Object();
                    RegisterResultDtoWrapper dataResultDtoWrapper = new RegisterResultDtoWrapper();

                    DataRequest = Customer;
                    string sResponse = _dataService.GetData(DataRequest, "Register");

                    if (!string.IsNullOrWhiteSpace(sResponse))
                    {
                        dataResultDtoWrapper = JsonConvert.DeserializeObject<RegisterResultDtoWrapper>(sResponse);
                        Debug.WriteLine("Got DataDTO");
                    }

                    RegisterReturn retResp = new RegisterReturn();
                    retResp = (RegisterReturn)dataResultDtoWrapper?.RegisterResult;

                    switch (retResp.code)
                    {
                        case RegistrationStatusCodes.Other:
                            btnSubmit.Enabled = true;
                            btnSubmit.CssClass = "btn btn-kool";
                            // msg.message = "Unknown error. Please contact customer service.";
                            // msg.header = "UNKNOWN ERROR";
                            break;
                        case RegistrationStatusCodes.Success:
                            //txtMsg.Visible = true;
                            //txtMsg.Text = "Record saved successfully";
                            msg.message = "Record created successfully";
                            msg.header = "Success!";
                            //RadNotification1.Title = msg.header;
                            //RadNotification1.Text = msg.message;
                            //RadNotification1.Show();

                            //Session["EnvoyUser"] = App.User;
                            ClearFields();
                            Response.Redirect("Thankyou.aspx", false);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    msg.message = "You have not entered all required information.  Please ensure all required information is entered and try again.";
                    msg.header = "Alert";
                    RadNotification1.Title = msg.header;
                    RadNotification1.Text = msg.message;
                    RadNotification1.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                string errdesc = ex.ToString();
                Session["ERRORMESSAGE"] = errdesc;
                errdesc = ex.ToString();
                //SendErrorEmail(errdesc, Environ);
                Response.Redirect("ErrorPage.aspx", true);
            }

        }
        protected bool ValidateInput()
        {
            bool sentBool = true;
            int intValue;

            //txtMsg.Visible = false;
            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                txtFirstName.BorderColor = Color.Red;
                sentBool = false;
            }
            else
                txtFirstName.BorderColor = Color.Black;
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                txtLastName.BorderColor = Color.Red;
                sentBool = false;
            }
            else
                txtLastName.BorderColor = Color.Black;
            //if (string.IsNullOrEmpty(txtAddress1.Text))
            //{
            //    txtAddress1.BorderColor = Color.Red;
            //    sentBool = false;
            //}
            //else
            //    txtAddress1.BorderColor = Color.Black;
            //if (string.IsNullOrEmpty(txtCity.Text))
            //{
            //    txtCity.BorderColor = Color.Red;
            //    sentBool = false;
            //}
            //else
            //    txtCity.BorderColor = Color.Black;
            ////RadComboBoxItem item = cboCCState.FindItemByValue(customer.STATE);
            ////item.Selected = true;
            //if (string.IsNullOrEmpty(ddState.SelectedValue))
            //{
            //    ddState.ForeColor = Color.Red;
            //    sentBool = false;
            //}
            //else
            //    ddState.ForeColor = Color.Black;
            //if (string.IsNullOrEmpty(txtZip.Text))
            //{
            //    txtZip.BorderColor = Color.Red;
            //    sentBool = false;
            //}
            //else
            //    txtZip.BorderColor = Color.Black;
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.BorderColor = Color.Red;
                sentBool = false;
            }
            else
                if (!IsValidEmailAddress(txtEmail.Text))
                {
                    txtEmail.BorderColor = Color.Red;
                    sentBool = false;
                }
                else
                    txtEmail.BorderColor = Color.Black;
            
            return sentBool;
        }
        void ClearFields()
        {
            txtEmail.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
        }
        public bool IsValidEmailAddress(string strEmail)
        {
            // Allows common email address that can start with a alphanumeric char and contain word, dash and period characters
            // followed by a domain name meeting the same criteria followed by a alpha suffix between 2 and 9 character lone
            //Dim regExPattern As String = "^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
            string regExPattern = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            //Dim regExPattern As String = "^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+\.)+[A-Z]{2,4}$"
            //Dim regExPattern As String = "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
            return MatchString(strEmail, regExPattern);
        }
        public bool MatchString(string str, string regexstr)
        {
            str = str.Trim();
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(regexstr);

            return pattern.IsMatch(str);
        }
        private string GetPage()
        {
            return System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        }

        protected string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                    IP4Address = IPA.ToString();

            }

            if (IP4Address != String.Empty)
                return IP4Address;


            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                    IP4Address = IPA.ToString();

            }

            return IP4Address;
        }

        protected void SendErrorEmail(string Text, string Environ)
        {
            string[] namesArray = ConfigurationManager.AppSettings["EmailAddressRecipients"].ToString().Split(',');
            List<string> namesList = new List<string>(namesArray.Length);
            namesList.AddRange(namesArray);
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAddressSender"].ToString());
            foreach (var item in namesList)
            {
                message.To.Add(item);
            }
            message.Subject = "Charlotte Radiology Web Error - " + Environ;
            message.Body = "An error has occurred in Register.aspx - Error: " + Text;
            SmtpClient client = new SmtpClient();
            client.Host = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
            client.Send(message);


        }

    }
}