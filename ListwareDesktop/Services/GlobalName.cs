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
    class GlobalName : IWS
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
                this.outputRecords = sh.sendRequest(this.endpoint, this.serviceOptions, inputRecords, typeof(GlobalNameRequest), typeof(GlobalNameRecord));
            }
        }

        public GlobalName() 
        {
            this.endpoint = @"https://globalname.melissadata.net/V3/WEB/GlobalName/doGlobalName";
            this.maxRecsPerRequest = 100;
            this.needsAllRecords = false;

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.inputColumns = sh.returnProperties(typeof(GlobalNameRecord));
            }

            #region Service Settings
            this.serviceOptions = new Dictionary<string, string>();
            this.settingsList = new Dictionary<string, List<string>>();
            settingsList.Add("Options_CorrectFirstName", new List<string> { "Single", "OFF", "ON" });
            settingsList.Add("Options_NameHint", new List<string> { "Single", "DefinitelyFull", "VeryLikelyFull", "ProbablyFull", "Varying", "ProbablyInverse", "VeryLikelyInverse", "DefinitelyInverse", "MixedFirstName", "MixedLastName" });
            settingsList.Add("Options_GenderPopulation", new List<string> { "Single", "Male", "Mixed", "Female" });
            settingsList.Add("Options_GenderAggression", new List<string> { "Single", "Aggressive", "Neutral", "Conservative" });
            settingsList.Add("Options_MiddleNameLogic", new List<string> { "Single", "ParseLogic", "HyphenatedLast", "MiddleName" });
            #endregion

            #region Output Columns
            //Set output columns
            this.outputColumns = new string[] {
                "Company",
                "NamePrefix",
                "NameFirst",
                "NameMiddle",
                "NameLast",
                "NameSuffix",
                "Gender",
                "NamePrefix2",
                "NameFirst2",
                "NameMiddle2",
                "NameLast2",
                "NameSuffix2",
                "Gender2"
            };
            #endregion
        }

        class GlobalNameRequest 
        {
            public string TransmissionReference { get; set; }
            public string CustomerID { get; set; }
            public string Options { get; set; }
            public string Format { get; set; }
            public GlobalNameRecord[] Records { get; set; }
        }

        class GlobalNameRecord 
        {
            public string RecordID { get; set; }
            public string Company { get; set; }
            public string FullName { get; set; }
        }
    }
}
