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
    class GlobalAddress : IWS
    {
        #region Global Address Options (Settings)
        public Dictionary<string, string> serviceOptions { get; set; }          
        public Dictionary<string, List<string>> settingsList { get; set; }      
        public String[] inputColumns { get; set; }                              
        public String[] outputColumns { get; set; }                            
        public int maxRecsPerRequest { get; set; }                              
        public bool errorStatus { get; set; }                                   
        public string statusMessage { get; set; }                               
        public string userLicense { get; set; }                                
        public bool needsAllRecords { get; set; }                               
        public bool serviceFinishedProcessing { get; set; }                     
        public bool inputRecordsFinished { get; set; }                          
        public Record[] outputRecords { get; set; }                             
        public string endpoint { get; set; }
        private List<int> recordID;
        #endregion

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
                this.outputRecords = sh.sendRequest(this.endpoint, this.serviceOptions, inputRecords, typeof(GlobalAddressRequest), typeof(GlobalAddressRecord));
            }
        }

        public GlobalAddress()
        {
            this.endpoint = @"https://address.melissadata.net/v3/WEB/GlobalAddress/doGlobalAddress";
            this.maxRecsPerRequest = 100;
            this.needsAllRecords = false;

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.inputColumns = sh.returnProperties(typeof(GlobalAddressRecord));
            }

            #region settings
            this.serviceOptions = new Dictionary<string, string>();
            this.settingsList = new Dictionary<String, List<String>>();
            settingsList.Add("Options_DeliveryLines", new List<string> { "Single", "ON", "OFF" });
            settingsList.Add("Options_LineSeparator", new List<String> { "Single", "SEMICOLON", "PIPE", "CR", "LF", "CRLF", "TAB", "BR" });
            settingsList.Add("Options_OutputScript", new List<String> { "Single", "NOCHANGE", "LATIN", "NATIVE" });
            settingsList.Add("Options_OutputGeo", new List<String> { "Single", "ON", "OFF" });
            settingsList.Add("Options_CountryOfOrigin", new List<String> { "Manual" });
            #endregion

            #region output
            this.outputColumns = new String[]
            {
                "FormattedAddress", 
                "Organization",
                "AddressLine1", 
                "AddressLine2", 
                "AddressLine3", 
                "AddressLine4",
                "AddressLine5", 
                "AddressLine6", 
                "AddressLine7", 
                "AddressLine8",
                "SubPremises", 
                "DoubleDependentLocality",
                "DependentLocality", 
                "Locality",
                "SubAdministrativeArea", 
                "AdministrativeArea", 
                "PostalCode", 
                "AddressType",
                "AddressKey", 
                "SubNationalArea", 
                "CountryName", 
                "CountryISO3166_1_Alpha2",
                "CountryISO3166_1_Alpha3", 
                "CountryISO3166_1_Numeric", 
                "CountrySubdivisionCode", 
                "Thoroughfare", 
                "ThoroughfarePreDirection", 
                "ThoroughfareLeadingType",
                "ThoroughfareName",
                "ThoroughfareTrailingType", 
                "ThoroughfarePostDirection", 
                "DependentThoroughfare", 
                "DependentThoroughfarePreDirection", 
                "DependentThoroughfareLeadingType",
                "DependentThoroughfareName", 
                "DependentThoroughfareTrailingType", 
                "DependentThoroughfarePostDirection", 
                "Building",
                "PremisesType",
                "PremisesNumber", 
                "SubPremisesType", 
                "SubPremisesNumber", 
                "PostBox",
                "Latitude", 
                "Longitude"
            };
            #endregion
        }

        class GlobalAddressRequest 
        {
            public string TransmissionReference { get; set; }
            public string CustomerID { get; set; }
            public string Options { get; set; }
            public string Format { get; set; }
            public GlobalAddressRecord[] Records { get; set; }
        }

        class GlobalAddressRecord 
        {
            public string Organization { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string AddressLine5 { get; set; }
            public string AddressLine6 { get; set; }
            public string AddressLine7 { get; set; }
            public string AddressLine8 { get; set; }
            public string DoubleDependentLocality { get; set; }
            public string DependentLocality { get; set; }
            public string Locality { get; set; }
            public string SubAdministrativeArea { get; set; }
            public string AdministrativeArea { get; set; }
            public string PostalCode { get; set; }
            public string SubNationalArea { get; set; }
            public string Country { get; set; }
        }
    }
}
