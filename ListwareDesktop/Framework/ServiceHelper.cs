using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Reflection;
using Newtonsoft.Json;

namespace ListwareDesktop.Framework
{
    internal class ServiceHelper : IDisposable
    {
        private int[] recordID { get; set; }
        bool disposed = false;

        public Record[] sendRequest(string endpoint, Dictionary<string, string> serviceOptions, Record[] inputRecords, Type requestType, Type recordType) 
        {
            this.adjustDictionary(ref serviceOptions);
            JObject requestJObject = this.createInputJObject(serviceOptions, inputRecords, requestType, recordType);
            JObject responseJObject = this.sendJSONPOSTRequest(endpoint, requestJObject);
            return this.returnRecords(responseJObject);
        }

        private void adjustDictionary(ref Dictionary<string, string> serviceOptions) 
        {
            List<string[]> tempStrList = new List<string[]>();
            List<string> keysToRemove = new List<string>();
            foreach (KeyValuePair<string, string> serviceOption in serviceOptions) 
            {
                if (serviceOption.Key.Contains('_')) 
                {
                    tempStrList.Add(new [] {(serviceOption.Key.Split('_'))[0], serviceOption.Key.Split('_')[1] + ":" + serviceOption.Value});
                    keysToRemove.Add(serviceOption.Key);
                }
            }

            foreach (string keyToRemove in keysToRemove) 
            {
                serviceOptions.Remove(keyToRemove);
            }

            Dictionary<string, string> adjustedOptions = new Dictionary<string, string>();
            foreach (string[] optionAndValue in tempStrList) 
            {
                if (!adjustedOptions.ContainsKey(optionAndValue[0]))
                {
                    adjustedOptions.Add(optionAndValue[0], optionAndValue[1]);
                }
                else 
                {
                    adjustedOptions[optionAndValue[0]] += ";" + optionAndValue[1];
                }
            }

            foreach (KeyValuePair<string, string> adjustedOption in adjustedOptions) 
            {
                if (!serviceOptions.ContainsKey(adjustedOption.Key))
                {
                    serviceOptions.Add(adjustedOption.Key, adjustedOption.Value);
                }
                else 
                {
                    serviceOptions[adjustedOption.Key] = adjustedOption.Value;
                }
            }
        }

        private void retryOnException(int times, TimeSpan delay, Action operation) 
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    operation();
                    break;
                }
                catch (Exception ex)
                {
                    if (attempts == times)
                    {
                        throw;
                    }

                    Task.Delay(delay).Wait();
                }
            } while (true);
        }

        //Method that sends JObject as a JSON POST request to the endpoint given
        internal JObject sendJSONPOSTRequest(string endpoint, JObject jsonInput) 
        {
            JObject jsonOutput;
            int attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    HttpWebRequest httpWebRequest;

                    httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    //Open StreamWriter to send request
                    using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsonInput);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Create a web response object to get response
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    //Get response with StreamReader
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();
                        jsonOutput = JObject.Parse(result);
                    }
                    break;
                }
                catch (Exception ex) 
                {
                    if (attempts == 10)
                    {
                        throw;
                    }
                }
            } while (true);

            return jsonOutput;
        }

        //Method that creates a JObject based on request structure, field names, and input records
        private JObject createInputJObject(Dictionary<string, string> serviceOptions, Record[] inputRecords, Type requestType, Type recordType) 
        {
            this.recordID = new int[inputRecords.Length];
            this.recordID = inputRecords.Select(s => s.recordID).ToArray();

            object request = Activator.CreateInstance(requestType);
            foreach (KeyValuePair<string, string> serviceOption in serviceOptions) 
            {
                this.setRequestFields(serviceOption.Key, serviceOption.Value, requestType, ref request);
            }

            this.setRequestFields("TransmissionReference", MainForm._lwdtSrc, requestType, ref request);

            Array records = Array.CreateInstance(recordType, inputRecords.Length);
            foreach (Record inputRecord in inputRecords)
            {
                object record = Activator.CreateInstance(recordType);
                foreach (KeyValuePair<string, string> inputFieldAndData in inputRecord.fieldAndData)
                {
                    this.setRequestFields(inputFieldAndData.Key, inputFieldAndData.Value, recordType, ref record);
                }
                records.SetValue(record, Array.IndexOf(inputRecords, inputRecord));
            }

            this.setRequestFields("Records", records, requestType, ref request);
            return JObject.FromObject(request);
        }

        //Method that changes JObject output from service to array of Record
        private Record[] returnRecords(JObject serviceResponseJObject) 
        {
            List<Record> tempList = new List<Record>();
            JToken outputRecords = serviceResponseJObject["Records"];
            int recordCounter = 0;
            foreach (JToken tempToken in outputRecords.Children())
            {
                Record tempRecord = new Record();

                foreach (JToken innerTempToken in tempToken.Children())
                {
                    var property = innerTempToken as JProperty;
                    tempRecord.addField("MD_" + property.Name.ToString(), property.Value.ToString());
                }
                tempRecord.recordID = this.recordID[recordCounter];
                recordCounter++;
                tempList.Add(tempRecord);
            }

            //Return an array of output records
            Record[] processedRecords = tempList.ToArray();
            return processedRecords;
        }

        //Method that sets a field name of an 
        private void setRequestFields(string fieldName, object fieldValue, Type inputObjectType, ref object inputObject) 
        {
            if (inputObjectType.GetProperty(fieldName) != null)
            {
                inputObjectType.GetProperty(fieldName).SetValue(inputObject, fieldValue);
            }
        }

        public string[] returnProperties(Type recordType) 
        {
            return (recordType.GetProperties()).Select(o => o.Name).ToArray();
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (disposed) 
            {
                return;
            }
            if (disposing) 
            {
                //Free unmanaged resources, however since we have none, do nothing
                this.recordID = null;
                disposed = true;
            }
        }
    }
}
