using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListwareDesktop.Framework
{
    //Basic record class. 
    //Each class will be made for a record, and it will hold the
    //header column name as well as the column data for 1 record.
    public class Record
    {
        internal Dictionary<string, string> fieldAndData { get; set; }
        internal int recordID { get; set; }

        internal Record(Dictionary<string, string> inputDictionary) 
        {
            this.fieldAndData = new Dictionary<string, string>(inputDictionary);
        }

        //Constructor
        internal Record(string[] fieldNames, string[] data)
        {
            //Allocate new dictionary
            this.fieldAndData = new Dictionary<string, string>();

            //Zip up list of corresponding header names and data into a single dictionary for the record
            fieldAndData = fieldNames.Zip(data, (f, d) => new { f, d }).ToDictionary(x => x.f, x => x.d);
        }

        //Constructor to create a new record from the input record
        //There were some issues with records being passed by reference rather than value
        internal Record(Record inputRecord) 
        {
            this.fieldAndData = new Dictionary<string, string>(inputRecord.fieldAndData);

            this.recordID = inputRecord.recordID;
        }

        //This is to add a singular field to the record
        internal void addField(string fieldName, string fieldValue)
        {
            if (!fieldAndData.ContainsKey(fieldName))
            {
                fieldAndData.Add(fieldName, fieldValue);
            }
            else
            {
                fieldAndData[fieldName] = fieldValue;
            }
        }

        //This method is used to combine pass through fields with output fields.
        //Ensure that they have different field names
        internal void combineRecord(Record tempRecord) 
        {
            foreach (KeyValuePair<string, string> innerKVP in tempRecord.fieldAndData) 
            {
                this.addField(innerKVP.Key.ToString(), innerKVP.Value.ToString());
            }
        }

        //Initializing a new record
        internal Record()
        {
            this.fieldAndData = new Dictionary<string, string>();
        }
    }
}
