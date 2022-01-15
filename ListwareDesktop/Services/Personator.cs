using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ListwareDesktop.Framework;

namespace ListwareDesktop.Services
{
    class Personator : IWS
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
                this.outputRecords = sh.sendRequest(this.endpoint, this.serviceOptions, inputRecords, typeof(PersonatorRequest), typeof(PersonatorRecord));
            }
        }

        public Personator() 
        {
            this.endpoint = @"https://personator.melissadata.net/v3/WEB/ContactVerify/doContactVerify";
            this.maxRecsPerRequest = 100;
            this.needsAllRecords = false;

            using (ServiceHelper sh = new ServiceHelper())
            {
                this.inputColumns = sh.returnProperties(typeof(PersonatorRecord));
            }

            #region Service Settings
            this.serviceOptions = new Dictionary<string, string>();
            this.settingsList = new Dictionary<string, List<string>>();
            settingsList.Add("Actions", new List<string> { "Multiple", "Check", "Verify", "Append", "Move" });
            settingsList.Add("Columns", new List<string> {"Multiple", "GrpAll", "GrpNameDetails", "GrpParsedAddress", 
                "GrpAddressDetails", "GrpCensus", "GrpParsedEmail", "GrpParsedPhone", "GrpGeoCode", "GrpDemographicBasic", "GrpCensus2",
                "Plus4","PrivateMailBox","Suite","MoveDate","Occupation","OwnRent"});
            // Options Section
            settingsList.Add("Options_CentricHint" , new List<string> {"Single","Auto","Address","Phone","Email","Name","SSN"});
            settingsList.Add("Options_Append", new List<string> { "Single", "Blank", "CheckError", "Always"});
            settingsList.Add("Options_Diacritics", new List<string> { "Single", "Auto", "On", "Off" });
            settingsList.Add("Options_SSNCascade", new List<string> { "Single", "On", "Off" });
            settingsList.Add("Options_UsePreferredCity", new List<string> { "Single", "On", "Off" });
            settingsList.Add("Options_dvancedAddressCorrection", new List<string> { "Single", "On", "Off" });
            #endregion

            #region Output Columns
            //Set output columns
            this.outputColumns = new string[]
            {
                /*
                "GrpAddressDetails",
                "GrpCensus",
                "GrpCensus2",
                "GrpGeocode",
                "GrpDemographicBasic",
                "GrpIPAddress",
                "GrpNameDetails",
                "GrpParsedAddress",
                "GrpParsedEmail",
                "GrpParsedPhone",
                "MoveDate",
                "Occupation",
                "OwnRent",
                "PhoneCountryCode",
                "PhoneCountryName",
                "Plus4",
                "PrivateMailBox",
                "Suite"*/
                "AddressDeliveryInstallation",
                "AddressExtras",
                "AddressHouseNumber",
                "AddressKey",
                "AddressLine1",
                "AddressLine2",
                "AddressLockBox",
                "AddressPostDirection",
                "AddressPreDirection",
                "AddressPrivateMailboxName",
                "AddressPrivateMailboxRange",
                "AddressRouteService",
                "AddressStreetName",
                "AddressStreetSuffix",
                "AddressSuiteName",
                "AddressSuiteNumber",
                "AddressTypeCode",
                "AreaCode",
                "CBSACode",
                "CBSADivisionCode",
                "CBSADivisionLevel",
                "CBSADivisionTitle",
                "CBSALevel",
                "CBSATitle",
                "CarrierRoute",
                "CensusBlock",
                "CensusTract",
                "ChildrenAgeRange",
                "City",
                "CityAbbreviation",
                "CompanyName",
                "CongressionalDistrict",
                "CountryCode",
                "CountryName",
                "CountyFIPS",
                "CountyName",
                "CountySubdivisionCode",
                "CountySubdivisionName",
                "CountryName",
                "CreditCardUser",
                "DateOfBirth",
                "DateOfDeath",
                "DeliveryIndicator",
                "DeliveryPointCheckDigit",
                "DeliveryPointCode",
                "DemographicsGender",
                "DemographicsResults",
                "DistanceAddressToIP",
                "DomainName",
                "Education",
                "ElementarySchoolDistrictCode",
                "ElementarySchoolDistrictName",
                "EmailAddress",
                "EstimatedHomeValue",
                "EthnicCode",
                "EthnicGroup",
                "Gender",
                "Gender2",
                "HouseholdIncome ",
                "HouseholdSize",
                "IPAddress",
                "IPCity",
                "IPConnectionSpeed",
                "IPConnectionType",
                "IPContinent",
                "IPCountryAbbreviation",
                "IPCountryName",
                "IPDomainName",
                "IPISPName",
                "IPLatitude",
                "IPLongitude",
                "IPPostalCode",
                "IPProxyDescription",
                "IPProxyType",
                "IPRegion",
                "IPUTC",
                "Latitude",
                "LengthOfResidence ",
                "Longitude",
                "MailboxName",
                "MaritalStatus",
                "MelissaAddressKey",
                "MelissaAddressKeyBase",
                "MoveDate",
                "NameFirst",
                "NameFirst2",
                "NameFull",
                "NameLast",
                "NameLast2",
                "NameMiddle",
                "NameMiddle2",
                "NamePrefix",
                "NamePrefix2",
                "NameSuffix",
                "NameSuffix2",
                "NewAreaCode",
                "Occupation",
                "OwnRent",
                "PhoneCountryCode",
                "PhoneCountryName",
                "PhoneExtension",
                "PhoneNumber",
                "PhonePrefix",
                "PhoneSuffix",
                "PlaceCode",
                "PlaceName",
                "Plus4",
                "PoliticalParty",
                "PostalCode",
                "PresenceOfChildren",
                "PresenceOfSenior",
                "PrivateMailbox",
                "RecordExtras",
                "Salutation",
                "SecondarySchoolDistrictCode",
                "SecondarySchoolDistrictName",
                "State",
                "StateDistrictLower",
                "StateDistrictUpper",
                "StateName",
                "Suite",
                "TopLevelDomain",
                "TypesOfVehicles",
                "UTC",
                "UnifiedSchoolDistrictCode",
                "UnifiedSchoolDistrictName",
                "UrbanizationName "
            };
            #endregion
        }

        class PersonatorRequest
        {
            public string TransmissionReference { get; set; }
            public string Actions { get; set; }
            public string Columns { get; set; }
            public string CustomerID { get; set; }
            public string Options { get; set; }
            public PersonatorRecord[] Records { get; set; }
        }

        class PersonatorRecord
        {
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string BirthDay { get; set; }
            public string BirthMonth { get; set; }
            public string BirthYear { get; set; }
            public string City { get; set; }
            public string CompanyName { get; set; }
            public string Country { get; set; }
            public string EmailAddress { get; set; }
            public string FirstName { get; set; }
            public string FreeForm { get; set; }
            public string FullName { get; set; }
            public string IPAddress { get; set; }
            public string LastLine { get; set; }
            public string LastName { get; set; }
            public string MelissaAddressKey { get; set; }
            public string PhoneNumber { get; set; }
            public string PostalCode { get; set; }
            public string RecordID { get; set; }
            public string SocialSecurity { get; set; }
            public string State { get; set; }
        }

    }  
}
