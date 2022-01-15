using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ListwareDesktop.Framework
{
    public class Input
    {
        private StreamReader streamReader;
        internal string[] headerFieldNames { get; set; }
        private string filePath { get; set; }
        private string delimiter;
        private string qualifier;

        //Constructor, set all options for the stream reader
        internal Input(string filePath, string delimiter, string qualifier) 
        {
            this.filePath = filePath;
            this.delimiter = delimiter;
            this.qualifier = qualifier;
            this.streamReader = new StreamReader(filePath);
            this.headerFieldNames = getFields(streamReader.ReadLine());
        }

        //Get the fields from a full line, use a regex to split if there's a specific type of qualifier
        private string[] getFields(string inputText) 
        {
            List<string> tokens = new List<string>();

            if (qualifier == null)
            {
                return inputText.Split(delimiter[0]).Select(s => s.Trim()).ToArray();
            }
            else 
            {
                string pattern = string.Format(@"{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))",Regex.Escape(delimiter),Regex.Escape(qualifier));

                string[] split = Regex.Split(inputText, pattern);

                return split.Select(s => s.Trim(qualifier[0],' ')).ToArray();
            }
        }

        //Use this method for the file preview pane
        internal Record[] getRecordsForPreview() 
        {
            List<Record> recordList = new List<Record>();

            for (int i = 0; i < 100; i++) 
            {
                if (!streamReader.EndOfStream) 
                {
                    recordList.Add(new Record(headerFieldNames, getFields(streamReader.ReadLine())));
                }
            }

            return recordList.ToArray();
        }

        //Get lines using the reader, split them with getFields()
        internal Record[] getRecords(int amountOfRecords) 
        {
            List<Record> returnRecordList = new List<Record>();

            for (int i = 0; i < amountOfRecords; i++) 
            {
                if (!streamReader.EndOfStream) 
                {
                    string[] fields = getFields(streamReader.ReadLine());
                    returnRecordList.Add(new Record(headerFieldNames, fields));
                }
            }

            return returnRecordList.ToArray();
        }

        //To make sure we end when we're supposed to
        internal bool checkForEnd() 
        {
            return streamReader.EndOfStream;
        }

        //To dispose of the reader
        internal void closeReader()
        {
            streamReader.Close();
            streamReader.Dispose();
        }

    }
}
