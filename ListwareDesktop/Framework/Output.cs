using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ListwareDesktop.Framework
{
    public class Output
    {
        private StreamWriter streamWriter;
        private string[] headerFieldNames;
        private string filePath { get; set; }
        private string delimiter;
        private string qualifier;
        private int linesWritten;

        //Constructor that sets all the settings for the writer
        internal Output(string filePath, string delimiter, string qualifier)
        {
            this.linesWritten = 0;
            this.filePath = filePath;
            this.delimiter = delimiter;
            this.qualifier = qualifier;
            this.streamWriter = new StreamWriter(filePath);
            streamWriter.AutoFlush = true;
        }

        //Writes records with qualifier + delimiter
        internal void sendRecords(Record[] recordsToBeWritten) 
        {
            this.checkIfEmpty(recordsToBeWritten[0]);
            foreach (Record tempRecord in recordsToBeWritten) 
            {
                string tempString = "";
                foreach (string currentHeader in headerFieldNames) 
                {
                    if ((qualifier == null) && (tempRecord.fieldAndData[currentHeader].Contains(delimiter)))
                    {
                        tempString += "\"" + tempRecord.fieldAndData[currentHeader].Trim() + "\"" + delimiter;
                    }
                    else
                    {
                        tempString += qualifier + tempRecord.fieldAndData[currentHeader].Trim() + qualifier + delimiter;
                    }
                }
                streamWriter.WriteLine(tempString.TrimEnd(new char[]{delimiter[0]}));
                linesWritten++;
            }
        }

        internal int numberOfLinesWritten() 
        {
            return this.linesWritten;
        }

        //Writes header line
        internal void writeHeaders(Record sampleRecord) 
        {
            headerFieldNames = sampleRecord.fieldAndData.Keys.ToArray();

            string tempString = "";

            foreach (string header in headerFieldNames) 
            {
                tempString += qualifier + header + qualifier + delimiter;
            }

            streamWriter.WriteLine(tempString.TrimEnd(new char[]{delimiter[0]}));
        }

        internal void checkIfEmpty(Record sampleRecord) 
        {
            if (new FileInfo(filePath).Length == 0) 
            {
                this.writeHeaders(sampleRecord);
            }
        }

        //Close writer
        internal void closeWriter() 
        {
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }
}
