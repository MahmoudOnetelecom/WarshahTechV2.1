using BL.Infrastructure;
using DL.MailModels;
using Microsoft.Extensions.Options;
using Model.ApiModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HELPER
{
    public interface ISMS
    {
        public string SendSMS(string MobileNum, string Message,int? warshahId, bool marketing);

        public string SmsFatoora(string Mobile, string LinkInvoic);
    }
    public class SMS:ISMS
    {

        private readonly MailSettings _mailSettings;
        private readonly IUnitOfWork _unitOfWork;


        public SMS(IOptions<MailSettings> mailSettings, IUnitOfWork unitOfWork)
        {
            _mailSettings = mailSettings.Value;
            _unitOfWork= unitOfWork;



        }

        public  string SendSMS(string MobileNum, string Message, int? warshahId,bool marketing)
        {
            try
            {
                var MessageCount = _unitOfWork.SMSCountRepository.GetMany(a => a.WarshahId == warshahId).FirstOrDefault();
                if (MessageCount != null&&marketing==true)
                {
                    if (MessageCount.MessageRemain>0)
                    {
                    
                        WebClient client1 = new WebClient();
                       //string baseurl1 = "http://mshastra.com/sendurlcomma.aspx?user=" + _mailSettings.ProfileId + "&pwd=" + _mailSettings.Pass + "&senderid=" + _mailSettings.SenderId + "&CountryCode=ALL&mobileno=" + MobileNum + "&msgtext=" + Message;
                        string baseurl1 = "http://mshastra.com/sendurlcomma.aspx?user=" + _mailSettings.ProfileId + "&pwd=" + _mailSettings.Pass + "&senderid=" + _mailSettings.SenderId + "&CountryCode=ALL&mobileno=" + MobileNum + "&msgtext=" + Message;

                        Stream data1 = client1.OpenRead(baseurl1);
                        StreamReader reader1 = new StreamReader(data1);
                        string s1 = reader1.ReadToEnd();
                        data1.Close();
                        reader1.Close();
                        return s1.ToString();
                    }
                    return "No Message Remaining";
                }
                if (marketing==false)
                {
                
                    WebClient client1 = new WebClient();
                    //string baseurl1 = "http://mshastra.com/sendurlcomma.aspx?user=" + _mailSettings.ProfileId + "&pwd=" + _mailSettings.Pass + "&senderid=" + _mailSettings.SenderId + "&CountryCode=ALL&mobileno=" + MobileNum + "&msgtext=" + Message;
                    string baseurl1 = "https://mshastra.com/sendurl.aspx?user=" + _mailSettings.ProfileId + "&pwd=" + _mailSettings.Pass + "&senderid=" + _mailSettings.SenderId + "&CountryCode=ALL&mobileno=" + MobileNum + "&msgtext=" + Message;

                    Stream data1 = client1.OpenRead(baseurl1);
                    StreamReader reader1 = new StreamReader(data1);
                    string s1 = reader1.ReadToEnd();
                    data1.Close();
                    reader1.Close();
                    return s1.ToString();
                }
                WebClient client = new WebClient();
            
                //string baseurl = "http://mshastra.com/sendurlcomma.aspx?user=" + _mailSettings.ProfileId + "&pwd=" + _mailSettings.Pass + "&senderid=" + _mailSettings.SenderId + "&CountryCode=ALL&mobileno=" + MobileNum + "&msgtext=" + Message;
                string baseurl = "https://mshastra.com/sendurl.aspx?user=" + _mailSettings.ProfileId + "&pwd=" + _mailSettings.Pass + "&senderid=" + _mailSettings.SenderId + "&CountryCode=ALL&mobileno=" + MobileNum + "&msgtext=" + Message;

                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return s.ToString();


            }
            catch (Exception ex)
            {

                return "";
            }
        

          
        }

        public string SmsFatoora(string Mobile, string LinkInvoic)
        {
            WebClient client1 = new WebClient();
            string baseurl1 = "http://mshastra.com/sendurlcomma.aspx?user=" + _mailSettings.ProfileId + "&pwd=" + _mailSettings.Pass + "&senderid=" + _mailSettings.SenderId + "&CountryCode=ALL&mobileno=" + Mobile + "&msgtext=" + LinkInvoic;
            Stream data1 = client1.OpenRead(baseurl1);
            StreamReader reader1 = new StreamReader(data1);
            string s1 = reader1.ReadToEnd();
            data1.Close();
            reader1.Close();
            return s1.ToString();

        }
    }
}
