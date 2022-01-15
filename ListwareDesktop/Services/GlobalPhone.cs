using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ListwareDesktop.Framework;

namespace ListwareDesktop.Services
{
    class GlobalPhone : IWS
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
        #endregion        
        
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
                this.outputRecords = sh.sendRequest(this.endpoint, this.serviceOptions, inputRecords, typeof(GlobalPhoneRequest), typeof(GlobalPhoneRecord));
            }
        }

        public GlobalPhone() 
        {
            this.endpoint = @"http://globalphone.melissadata.net/v4/WEB/GlobalPhone/doGlobalPhone";
            this.maxRecsPerRequest = 100;
            this.needsAllRecords = false;

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.inputColumns = sh.returnProperties(typeof(GlobalPhoneRecord));
            }

            #region Service Settings
            this.serviceOptions = new Dictionary<string, string>();
            this.settingsList = new Dictionary<string, List<string>>();
            settingsList.Add("Options_VerifyPhone" , new List<string> {"Single","Express","Premium"});
            settingsList.Add("Options_CallerID" , new List<string> {"Single","False","True"});
            settingsList.Add("Options_TimeToWait" , new List<string> {"Manual"});
            settingsList.Add("Options_DefaultCallingCode", new List<string> { "Manual" });
            #endregion

            #region Output Columns
            //Set output columns
            this.outputColumns = new string[]
            {
                "PhoneNumber",
                "AdministrativeArea",
                "CountryAbbreviation",
                "CountryName",
                "Carrier",
                "CallerID",
                "DST",
                "InternationalPhoneNumber",
                "Language",
                "Latitude",
                "Longitude",
                "InternationalPrefix",
                "CountryDialingCode",
                "NationPrefix",
                "NationalDestinationCode",
                "SubscriberNumber",
                "UTC",
                "PostalCode",
                "Suggestions",
                "TimeZoneCode",
                "TimeZoneName"
            };
            #endregion
        }

        class GlobalPhoneRequest 
        {
            public string TransmissionReference { get; set; }
            public string CustomerID { get; set; }
            public string Options { get; set; }
            public GlobalPhoneRecord[] Records { get; set; }
        }

        class GlobalPhoneRecord 
        {
            public string RecordID { get; set; }
            public string PhoneNumber { get; set; }
            public string Country { get; set; }
            public string CountryOfOrigin { get; set; }
        }
    }
}
