using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListwareDesktop.Framework
{
    public interface IWS
    {
        /*
         * Those who are creating a web service class must do so in the following way
         * All of the following methods must be implemented, with the class for the service extending this interface
         * 
         *  settingsList - A dictionary of all top level elements and values. Declare and set this in the constructor. For example,
         *          Smartmover would use columns, JobID, NumberOfMonthsRequested,ProcessingType,ListOwnerFreqProcessing
         *          There are three types of options integrated into settingsList: Manual, Single, and Multiple
         *          Manual is a manual entry option. For example, NumberOfMonthsRequested requires the user to put in a value between 6 and 48.
         *              Therefore, the entry for this option would look like the following:
         *              Key: NumberOfMonthsRequested
         *              Value: Manual,6-48
         *              With manual entry options, the second value of the key is what the display box will show to the user. This value will not be taken into the WS
         *          Single is for options where users can only select a single option. For example, ProcessingType requires a user to pick either Residence, Business, Individual, etc.
         *              Therefore, the entry for the option would look like the following:
         *              Key: ProcessingType
         *              Value: Single, Standard, Individual, IndividualAndBusiness, Business, Residential
         *          Multiple is for options where users can select multiple options simultaneously. For example, Columns, the user can pick grpStandardized,grpOriginal, or even individual columns like so:
         *              Key: Columns
         *              Value: Multiple, DPVFootNotes, MoveReturnCode, Plus4, PrivateMailBox, Suite, GrpParsed, GrpName, GrpOriginal, GrpStandardized
         *              
         *  serviceOptions - This is a dictionary of all of the options, as well as the values corresponding to the options that the user has chosen. This will be passed back 
         *          into the class after the user has selected all of their options. There is a possibility where this can be blank if a user has not selected anything at all.
         *          Declare this in the constructor but DO NOT set any values.
         *          
         *  inputColumns - A list of all of the input columns for the service. Declare and set this in the constructor.
         *
         *  outputColumns - A list of all of the possible output columns for the service. Declare and set this in the constructor.
         *
         *  maxRecsPerRequest - This controls how many records will be passed to the service at a time by the GUI. Declare and set this in the constructor.
         *
         *  sendToService - This is the method that the GUI passes the records into. This is the "black box". This method is where you will do your processing however you wish.
         *
         *  outputRecords - This is where you will place the output records when you are finished processing.
         *  
         *  errorStatus - A boolean that will be false by default. Set to true when there is an error and the GUI will stop processing.
         *  
         *  statusMessage - A statusMessage that will be passed to the GUI. When there is an issue, set this to some message for the user and set errorStatus to true. If there is no error
         *          but you want to send a message to the user regardless (like a warning or processing %), set this with errorStatus set to false.
         *          
         *  userLicense - The user's license will be set here by the GUI for the class.
         * 
         *  needsAllRecords - Set this to true if the class needs all records before processing (such as the MatchUp Webservice)
         *  
         *  serviceFinishedProcessing - If needsAllRecords is set to true, set this bool to true when you are done processing. Otherwise, you can leave it alone.
         *  
         *  inputRecordsFinished - If needs all records is set to true, then the GUI will set this bool to true when it has finished passing in all of the records.
         */

        string endpoint { get; set; }

        //Dictionary of all options and the values the user has set for them
        Dictionary<string, string> serviceOptions { get; set; }

        //Dictionary of all options and their possible values
        Dictionary<string, List<string>> settingsList { get; set; }

        //All possible input columns DECLARE AND SET IN CONSTRUCTOR
        string[] inputColumns { get; set; }

        //All possible output columns DECLARE AND SET IN CONSTRUCTOR
        string[] outputColumns { get; set; }

        //Maximum records per request, typically 100 or 1 DECLARE AND SET IN CONSTRUCTOR
        int maxRecsPerRequest { get; set; }

        //Send input records to service and set the output records
        void sendToService(Record[] inputRecords);

        //Set this to the outputRecords that are to be returned to the GUI
        Record[] outputRecords { get; set; }

        //This is set to true if there is an error
        bool errorStatus { get; set; }

        //Any possible status messages to pass to the GUI, including error status messages
        string statusMessage { get; set; }

        //This is set so the service can access the license more easily
        string userLicense { get; set; }

        //This is to differentiate the types of services
        //For services such as Personator, SmartMover that can receive x request records then return y response records select false
        //For services such as MUWS where you need all records before returning select true
        bool needsAllRecords { get; set; }

        //This boolean is set by the WS class
        //Lets the GUI know when the class is ready to output records
        bool serviceFinishedProcessing { get; set; }

        //This boolean is set by the GUI
        //Lets the WS class know when the GUI is finished sending in records
        bool inputRecordsFinished { get; set; }
    }
}
