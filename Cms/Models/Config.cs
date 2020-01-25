namespace Cms.Models
{


    public class Config
    {

      
        public string SmtpServerName {
            get {
                //webmail server name
                return (string)"";
            }
        }
        public int SmtpPort
        {
            get
            {
                //webmail server Port
                return (int)25;
            }
        }
        public string SmtpUserName
        {
            get
            {
                //webmail server Username
                return (string)"";
            }
        }
        public string SmtpPassword
        {
            get
            {
                //webmail server password
                return (string)"";
            }
        }
        public string SmtpFrom
        {
            get
            {
                //webmail server from adress
                return (string)"";
            }
        }


    }
}