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
    class BusinessCoder : IWS
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
            if (!this.serviceOptions.ContainsKey("ID"))
            {
                this.serviceOptions.Add("ID", this.userLicense);
            }
            else
            {
                this.serviceOptions["ID"] = this.userLicense;
            }

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.outputRecords = sh.sendRequest(this.endpoint, this.serviceOptions, inputRecords, typeof(BusinessCoderRequest), typeof(BusinessCoderRecord));
            }
        }

        public BusinessCoder()
        {
            this.endpoint = @"http://businesscoder.melissadata.net/WEB/BusinessCoder/doBusinessCoderUS";
            this.maxRecsPerRequest = 100;
            this.needsAllRecords = false;

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.inputColumns = sh.returnProperties(typeof(BusinessCoderRecord));
            }

            #region Service Settings
            this.serviceOptions = new Dictionary<string, string>();
            this.settingsList = new Dictionary<string, List<string>>();
            settingsList.Add("Cols", new List<string> {"Multiple","GrpAddressDetails","GrpBusinessCodes","GrpBusinessDescription","GrpGeoCode","GrpCensus","LocationType","Phone","EmployeesEstimate","SalesEstimate","StockTicker","WebAddress","Contacts"});
            settingsList.Add("Opt_ReturnDominantBusiness", new List<string> { "Single", "Yes", "No" });
            settingsList.Add("Opt_CentricHint", new List<string> { "Single", "None", "Name", "Address", "Phone" });
            settingsList.Add("Opt_MaxContacts", new List<string> { "Manual", "5" });
            settingsList.Add("Opt_SICNAICSConfidence", new List<string> { "Strict", "Loose" });
            #endregion

            #region Output Columns
            //Set output columns
            this.outputColumns = new string[]
            {
                "CompanyName",
                "AddressLine1",
                "Suite",
                "City",
                "State",
                "PostalCode",
                "MelissaEnterpriseKey",
                "LocationType",
                "Phone",
                "EmployeesEstimate",
                "SalesEstimate",
                "StockTicker",
                "WebAddress",
                "CountryCode",
                "CountryName",
                "DeliveryIndicator",
                "MelissaAddressKey",
                "MelissaAddressKeyBase",
                "EIN",
                "SICCode1",
                "SICCode2",
                "SICCode3",
                "NAICSCode1",
                "NAICSCode2",
                "NAICSCode3",
                "SICDescription1",
                "SICDescription2",
                "SICDescription3",
                "NAICSDescription1",
                "NAICSDescription2",
                "NAICSDescription3",
                "Latitude",
                "Longitude",
                "CountyName",
                "CountyFIPS",
                "CensusTract",
                "CenusBlock",
                "PlaceCode",
                "PlaceName",
                "TotalContacts",
                "TotalSuggestions",
                "Results"
            };
            #endregion
        }

        class BusinessCoderRequest
        {
            public string T { get; set; }
            public string Cols { get; set; }
            public string ID { get; set; }
            public string Opt { get; set; }
            public BusinessCoderRecord[] Records { get; set; }
        }

        class BusinessCoderRecord
        {
            public string Rec { get; set; }
            public string Comp { get; set; }
            public string Phone { get; set; }
            public string A1 { get; set; }
            public string A2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Postal { get; set; }
            public string Ctry { get; set; }
            public string MAK { get; set; }
            public string MEK { get; set; }
            public string Stock { get; set; }
            public string Web { get; set; }
        }

    }
}
