using ListwareDesktop.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListwareDesktop.Services
{
    internal class GlobalEmail : IWS
    {
        #region settings
        public string endpoint { get; set; }
        public string[] inputColumns { get; set; }
        public string[] outputColumns { get; set; }
        public int maxRecsPerRequest { get; set; }
        public Dictionary<string, string> serviceOptions { get; set; }
        public string userLicense { get; set; }
        public bool needsAllRecords { get; set; }
        public bool serviceFinishedProcessing { get; set; }
        public bool inputRecordsFinished { get; set; }
        public Record[] outputRecords { get; set; }
        public string statusMessage { get; set; }
        public bool errorStatus { get; set; }
        public Dictionary<string, List<string>> settingsList { get; set; }
        private List<int> recordID;
        #endregion settings

        //Send records to service and return output records
        public void sendToService(Record[] inputRecords)
        {
            //Add customer ID if it's not in there already
            if (!this.serviceOptions.ContainsKey("CustomerID"))
            {
                this.serviceOptions.Add("CustomerID", this.userLicense);
            }
            else
            {
                this.serviceOptions["CustomerID"] = this.userLicense;
            }

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.outputRecords = sh.sendRequest(this.endpoint, this.serviceOptions, inputRecords, typeof(GlobalEmailRequest), typeof(GlobalEmailRecord));
            }
        }

        public GlobalEmail()
        {
            this.endpoint = @"http://globalemail.melissadata.net/v3/WEB/GlobalEmail/doGlobalEmail";
            this.maxRecsPerRequest = 10;
            this.needsAllRecords = false;

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.inputColumns = sh.returnProperties(typeof(GlobalEmailRecord));
            }

            #region Service Settings
            this.serviceOptions = new Dictionary<string, string>();
            this.settingsList = new Dictionary<string, List<string>>();
            settingsList.Add("Options_VerifyMailBox", new List<string> { "Single", "Express", "Premium" });
            settingsList.Add("Options_DomainCorrection", new List<string> { "Single", "On", "Off" });
            settingsList.Add("Options_TimeToWait", new List<string> { "Manual", "5-45" });
            #endregion Service Settings

            #region Output Columns

            //Set output columns
            this.outputColumns = new string[]
            {
                "EmailAddress",
                "MailboxName",
                "DomainName",
                "TopLevelDomain",
                "TopLevelDomainName",
                "DateChecked"
            };

            #endregion Output Columns
        }

        class GlobalEmailRequest 
        {
            public string TransmissionReference { get; set; }
            public string CustomerID { get; set; }
            public string Options { get; set; }
            public string Format { get; set; }
            public GlobalEmailRecord[] Records { get; set; }
        }

        class GlobalEmailRecord 
        {
            public string RecordID { get; set; }
            public string Email { get; set; }
        }
    }
}