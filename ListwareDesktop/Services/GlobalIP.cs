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
    class GlobalIP : IWS
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
        public bool errorStatus { get; set; }
        public string statusMessage { get; set; }
        public Dictionary<string, List<string>> settingsList { get; set; }
        private List<int> recordID;
        #endregion

        //Send records to service and return output records
        public void sendToService(Record[] inputRecords)
        {            
            // Add CustomerID to ServiceOptions
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
                this.outputRecords = sh.sendRequest(this.endpoint, this.serviceOptions, inputRecords, typeof(GlobalIPRequest), typeof(GlobalIPRecord));
            }
        }

        public GlobalIP() 
        {
            this.endpoint = @"https://globalip.melissadata.net/v4/web/iplocation/doiplocation";
            this.maxRecsPerRequest = 100;
            this.needsAllRecords = false;

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.inputColumns = sh.returnProperties(typeof(GlobalIPRecord));
            }

            #region Service Settings
            this.serviceOptions = new Dictionary<string, string>();
            this.settingsList = new Dictionary<string, List<string>>();
            #endregion

            #region Output Columns
            //Set output columns
            this.outputColumns = new string[] {
                "IPAddress",
                "Latitude",
                "Longitude",
                "PostalCode",
                "Region",
                "ISPName",
                "DomainName",
                "City",
                "CountryName",
                "CountryAbbreviation",
                "ConnectionSpeed",
                "ConnectionType",
                "UTC",
                "Continent",
                "ProxyType",
                "ProxyDescription",
                //"Result"
            };
            #endregion
        }

        class GlobalIPRequest 
        {
            public string TransmissionReference { get; set; }
            public string CustomerID { get; set; }
            public GlobalIPRecord[] Records { get; set; }
        }

        class GlobalIPRecord 
        {
            public string RecordID { get; set; }
            public string IPAddress { get; set; }
        }
    }
}
