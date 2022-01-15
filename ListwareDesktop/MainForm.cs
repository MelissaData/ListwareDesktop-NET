using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Net;
using System.Xml;
using System.Web;
using System.Windows.Forms.DataVisualization.Charting;
using ListwareDesktop.Framework;
using ListwareDesktop.Windows;
using ListwareDesktop.Services;
using ListwareDesktop.Reporting;

namespace ListwareDesktop
{
    public partial class MainForm : Form
    {
        #region Global Vars
        //Some static fields that are used between the service, the GUI, and the different windows
        private static readonly string _version = "2.1";
        private static readonly int _defaultThreads = 5;
        internal static readonly string _lwdtSrc = "mdSrc:{product:ListwareDesktop;version:" + _version + "}";
        private static string creditAmt = "";

        //Service Options
        private static ConcurrentQueue<Record[]> inputQueue;
        private static ConcurrentQueue<Record[]> outputQueue;
        private static IWS serviceObject;
        private static string userLicense;
        internal static Type serviceType;
        internal static Dictionary<string, string> inputAliases;
        internal static string[] selectedOutputs;
        internal static Dictionary<string, string> serviceOptions = new Dictionary<string, string>();

        //To make sure the input and output files are set before running
        private string inputFilePath;
        private string outputFilePath;
        private bool doNotShowOverwritePrompt = new bool();
        private bool overwriteFile = new bool();
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(true);
        #endregion

        //Constructor for the MainForm
        internal MainForm()
        {
            InitializeComponent();
            this.Text += " " + _version;

            //Generate all available classes from the "Services" folder at runtime
            this.generateClassesForComboBox();
            this.numberOfThreadsMenuStripTextBox.Text = _defaultThreads.ToString();
            //Pull up saved license if any
            if (!String.IsNullOrEmpty(Properties.Settings.Default.customerID))
            {
                //If there is a saved license, place it into the license textbox
                //Set it to be shown with password characters
                //Enable the groupboxes on the form
                userLicense = Properties.Settings.Default.customerID;
                setLicenseTextBox.Text = userLicense;
                this.setLicenseTextBox.PasswordChar = '\u2022';
                this.enableInputProgressGroupBoxes();

                //Get credit count for license in another thread
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    this.getCreditCount();
                }).Start();
            }
            else 
            {
                //If no license is saved in the config, focus on the license text box and disable everything else on the form
                this.setLicenseTextBox.Select();
                this.disableGroupBoxes();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Meta Options
        //Make a list of the classes under the "Services" folder
        internal void generateClassesForComboBox() 
        {
            Type[] serviceList = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "ListwareDesktop.Services", StringComparison.Ordinal)).ToArray();
            serviceSelectComboBox.Items.AddRange(serviceList.Select(t => t.Name).Where(o => !o.Contains("Request") && !o.Contains("Record")).ToArray());
        }

        //Set the service when the selection in the service select combo box is changed
        //When a service is selected from the combobox, userService is set to a new instance of the selected class
        //The title of the form is changed
        private void serviceSelectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            serviceOptions = new Dictionary<string, string>();
            serviceType = Type.GetType("ListwareDesktop.Services." + serviceSelectComboBox.SelectedItem.ToString());
            this.Text = "Listware Desktop " + _version + " : " + serviceType.Name;
            serviceProgressCheckBox.Checked = true;
            this.Update();
        }

        //Set the license when leaving the text box
        private void setLicenseTextBox_Leave(object sender, EventArgs e)
        {
            {
                if (!String.IsNullOrEmpty(setLicenseTextBox.Text))
                {
                    //Set license when leaving textbox when not null or empty
                    //Save the license, set password characters, enable the form, then get the credits on the license in another thread
                    MainForm.userLicense = setLicenseTextBox.Text;
                    Properties.Settings.Default.customerID = setLicenseTextBox.Text;
                    Properties.Settings.Default.Save();
                    this.setLicenseTextBox.PasswordChar = '\u2022';
                    this.enableInputProgressGroupBoxes();
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        this.getCreditCount();
                    }).Start();

                    //If the license is entered after everything else is set, enable the run button
                    if (serviceProgressCheckBox.Checked && inputFileProgressCheckBox.Checked && outputFileProgressCheckBox.Checked) 
                    {
                        this.runButton.Enabled = true;
                    }
                }
                else
                {
                    //If the license is null or empty then set the saved license as blank, disable everything
                    Properties.Settings.Default.customerID = setLicenseTextBox.Text;
                    Properties.Settings.Default.Save();
                    MainForm.userLicense = setLicenseTextBox.Text;
                    creditsLabel.Visible = false;
                    this.disableGroupBoxes();
                    this.runButton.Enabled = false;
                }
            }
        }

        //Set the license when pressing enter on license text box
        private void setLicenseTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) 
            {
                //If enter is pressed while we are in the text box
                if (!String.IsNullOrEmpty(setLicenseTextBox.Text))
                {
                    //Set license when leaving textbox when not null or empty
                    //Save the license, set password characters, enable the form, then get the credits on the license in another thread
                    MainForm.userLicense = setLicenseTextBox.Text;
                    Properties.Settings.Default.customerID = setLicenseTextBox.Text;
                    Properties.Settings.Default.Save();
                    this.setLicenseTextBox.PasswordChar = '\u2022';
                    this.enableInputProgressGroupBoxes();
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        this.getCreditCount();
                    }).Start();

                    //If the license is entered after everything else is set, enable the run button
                    if (serviceProgressCheckBox.Checked && inputFileProgressCheckBox.Checked && outputFileProgressCheckBox.Checked)
                    {
                        this.runButton.Enabled = true;
                    }
                }
                else
                {
                    //If the license is null or empty then set the saved license as blank, disable everything
                    Properties.Settings.Default.customerID = setLicenseTextBox.Text;
                    Properties.Settings.Default.Save();
                    MainForm.userLicense = setLicenseTextBox.Text;
                    creditsLabel.Visible = false;
                    this.disableGroupBoxes();
                    this.runButton.Enabled = false;
                }
            }
        }

        //When we enter the license textbox, change the password character to null to display the license
        private void setLicenseTextBox_Enter(object sender, EventArgs e)
        {
            this.setLicenseTextBox.PasswordChar = '\0';
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Main Buttons
        //Select Input File in GUI
        private void inputFileSelectButton_Click(object sender, EventArgs e)
        {
            string defaultLocation;

            //Check textbox if it is empty, if it's not then check if the path is valid then open that directory
            //Otherwise open the current directory
            if ((String.IsNullOrEmpty(inputFileTextBox.Text) || ((!string.IsNullOrWhiteSpace(outputFileTextBox.Text.Trim())) &&(!Directory.Exists(Path.GetDirectoryName(outputFileTextBox.Text))))))
            {
                defaultLocation = Directory.GetCurrentDirectory();
            }
            else
            {
                defaultLocation = inputFileTextBox.Text;
            }

            Stream myStream = null;
            OpenFileDialog inputFilePath = new OpenFileDialog();

            //Set OpenFileDialog settings
            inputFilePath.InitialDirectory = defaultLocation;
            inputFilePath.Filter = "CSV and TXT Files|*.csv;*.txt|CSV Files|*.csv|TXT Files|*.txt|All files (*.*)|*.*";
            inputFilePath.FilterIndex = 1;
            inputFilePath.RestoreDirectory = true;

            if (inputFilePath.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = inputFilePath.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            inputFileTextBox.Text = inputFilePath.FileName;
                            this.inputFilePath = inputFilePath.FileName;
                            inputFileProgressCheckBox.Checked = true;
                            inputColumnProgressCheckBox.Checked = false;
                            inputAliases = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        //Allow the user to manually input an input directory and press enter to validate it
        private void inputFileTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(inputFileTextBox.Text))
                {
                    statusTextBox.Text = "";
                    inputFileProgressCheckBox.Checked = false;
                }
                else
                {
                    if (Directory.Exists(Path.GetDirectoryName(inputFileTextBox.Text)))
                    {
                        this.inputFilePath = inputFileTextBox.Text;
                        inputColumnProgressCheckBox.Checked = false;
                        inputAliases = null;
                        statusTextBox.Text = "";
                        inputFileProgressCheckBox.Checked = true;
                    }
                    else
                    {
                        statusTextBox.Text = "Invalid input file path.";
                        inputFileProgressCheckBox.Checked = false;
                    }
                }
            }
        }

        //Allow the user to manually input an input directory and leave the textbox to validate it
        private void inputFileTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputFileTextBox.Text))
            {
                statusTextBox.Text = "";
                inputFileProgressCheckBox.Checked = false;
            }
            else
            {
                if (Directory.Exists(Path.GetDirectoryName(inputFileTextBox.Text)))
                {
                    this.inputFilePath = inputFileTextBox.Text;
                    inputColumnProgressCheckBox.Checked = false;
                    inputAliases = null;
                    statusTextBox.Text = "";
                    inputFileProgressCheckBox.Checked = true;
                }
                else
                {
                    statusTextBox.Text = "Invalid input file path.";
                    inputFileProgressCheckBox.Checked = false;
                }
            }
        }

        //Set Default Output File in GUI when the Input file is selected
        private void defaultOutputFile() 
        {
            if (!string.IsNullOrEmpty(this.inputFilePath)) 
            {
                this.outputFilePath = Path.Combine(Path.GetDirectoryName(this.inputFilePath), Path.GetFileNameWithoutExtension(this.inputFilePath) + "_OUTPUT" + Path.GetExtension(this.inputFilePath));
                outputFileProgressCheckBox.Checked = true;
                outputFileTextBox.Text = outputFilePath;
            }
        }

        //Select Output File in GUI
        private void outputFileSelectButton_Click(object sender, EventArgs e)
        {
            string defaultLocation;

            //Check textbox if it is empty, if it's not then check if the path is valid then open that directory
            //Otherwise open the current directory
            if ((String.IsNullOrEmpty(outputFileTextBox.Text) || (!Directory.Exists(Path.GetDirectoryName(outputFileTextBox.Text)))))
            {
                defaultLocation = Directory.GetCurrentDirectory();
            }
            else
            {
                defaultLocation = outputFileTextBox.Text;
            }

            //Create output file dialog and directory
            using (SaveFileDialog outputFilePath = new SaveFileDialog())
            {
                outputFilePath.InitialDirectory = Directory.GetCurrentDirectory();
                outputFilePath.Filter = "CSV and TXT Files|*.csv;*.txt|CSV Files|*.csv|TXT Files|*.txt|All files (*.*)|*.*";
                outputFilePath.FilterIndex = 1;
                outputFilePath.RestoreDirectory = true;

                if (outputFilePath.ShowDialog() == DialogResult.OK)
                {
                    outputFileTextBox.Text = outputFilePath.FileName;
                    this.outputFilePath = outputFilePath.FileName;
                    outputFileProgressCheckBox.Checked = true;
                }
            }
        }

        //Allow the user to manually input an output directory and press enter to validate it
        private void outputFileTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(outputFileTextBox.Text))
                {
                    statusTextBox.Text = "";
                    outputFileProgressCheckBox.Checked = false;
                }
                else
                {
                    if (Directory.Exists(Path.GetDirectoryName(outputFileTextBox.Text)))
                    {
                        this.outputFilePath = outputFileTextBox.Text;
                        statusTextBox.Text = "";
                        outputFileProgressCheckBox.Checked = true;
                    }
                    else
                    {
                        statusTextBox.Text = "Invalid output file path.";
                        outputFileProgressCheckBox.Checked = false;
                    }
                }
            }
        }

        //Allow the user to manually input an output directory and leave the textbox to validate it
        private void outputFileTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(outputFileTextBox.Text))
            {
                statusTextBox.Text = "";
                outputFileProgressCheckBox.Checked = false;
            }
            else 
            {
                if (Directory.Exists(Path.GetDirectoryName(outputFileTextBox.Text)))
                {
                    this.outputFilePath = outputFileTextBox.Text;
                    statusTextBox.Text = "";
                    outputFileProgressCheckBox.Checked = true;
                }
                else 
                {
                    statusTextBox.Text = "Invalid output file path.";
                    outputFileProgressCheckBox.Checked = false;
                }
            }
        }

        //Process the file when the run button is clicked with backgroundworker
        private void runButton_Click(object sender, EventArgs e)
        {
            //If we are not running yet, we are in a "passive state", meaning that the run button asks the user to run and will say "Run"
            if (runButton.Text.Equals("Run"))
            {
                //If the output file exists already, prompt the user to overwrite or not
                if (File.Exists(this.outputFilePath))
                {
                    //Show the "Overwrite Warning" form initially
                    //The user can select not to show it again, if they do, we will remember their choice and not show the form again
                    //Unless the application is closed and reopened, then it will display again
                    if (!this.doNotShowOverwritePrompt)
                    {
                        OverwriteWarningForm overwriteWarningForm = new OverwriteWarningForm();
                        overwriteWarningForm.StartPosition = FormStartPosition.CenterParent;
                        overwriteWarningForm.Focus();
                        overwriteWarningForm.ShowDialog();

                        this.doNotShowOverwritePrompt = overwriteWarningForm.doNotShowPrompt;
                        this.overwriteFile = overwriteWarningForm.overwrite;
                    }
                    if (!this.overwriteFile)
                    {
                        this.statusTextBox.Text = "Please rename output file.";
                        return;
                    }
                }

                serviceObject = Activator.CreateInstance(serviceType) as IWS;
                serviceObject.serviceOptions = new Dictionary<string, string>(MainForm.serviceOptions);
                serviceObject.userLicense = MainForm.userLicense;

                //If the run button is clicked
                //Display and enable the pause button
                //Change the run button size
                //Start the worker thread
                if ((inputColumnProgressCheckBox.Checked) || (this.setInputAliases()))
                {
                    this.changeRunButtonSize();
                    statusTextBox.Text = "";
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    statusTextBox.Text = "Input columns were not able to be autodetected.\r\nPlease set manually.";
                }
            }
            //If we are not in a passive state, we are in a run state, records are running, and the run button says "Cancel"
            else
            {
                //If the cancel button is selected while we are paused, then resume the backgroundworker thread but flag for cancellation
                //If the cancel button is selected while we are not paused, then just flag backgroundworker for cancellation
                if (!manualResetEvent.WaitOne(0))
                {
                    manualResetEvent.Set();
                }
                backgroundWorker1.CancelAsync();
            }
        }

        //Change the size of the run button, just to have the GUI look a little bit cleaner by default
        //Essentially, the pause button will only appear when we are currently running records (in a "run" state)
        private void changeRunButtonSize() 
        {
            //If we are in a passive state, the run button is large and "welcoming" and the pause button is hidden
            if (runButton.Text == "Run")
            {
                runButton.Text = "Cancel";
                pauseButton.Enabled = true;
                runButton.Height = 29;
                pauseButton.Visible = true;
            }
            //If we are in a run state, the run button is half the size to make room for the pause button that was previously hidden
            else 
            {
                runButton.Text = "Run";
                pauseButton.Enabled = false;
                runButton.Height = 63;
                pauseButton.Visible = false;
            }
        }

        //Pause processing when the pause button is clicked
        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (pauseButton.Text == "Pause")
            {
                manualResetEvent.Reset();
                pauseButton.Text = "Resume";
            }
            else
            {
                manualResetEvent.Set();
                pauseButton.Text = "Pause";
            }
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Buttons that make new windows (Config, Input Columns, Preview)
        //For the set configuration button, save options
        private void setConfigurationButton_Click(object sender, EventArgs e)
        {
            //This is for the options configurations window
            //Open window, fill grid
            //When the grid is closed options are passed back to service
            if (serviceProgressCheckBox.Checked)
            {
                statusTextBox.Text = "";
                SetConfigurationForm setConfigurationForm = new SetConfigurationForm(configurationProgressCheckBox);
                setConfigurationForm.StartPosition = FormStartPosition.CenterParent;
                setConfigurationForm.Focus();
                setConfigurationForm.ShowDialog();
            }
            else 
            {
                statusTextBox.Text = "Please set:\r\nService";
            }
        }

        //To preview the input file with the selected qualifiers and delimiters
        private void inputFilePreviewButton_Click(object sender, EventArgs e)
        {
            if (inputFileProgressCheckBox.Checked)
            {
                InputPreviewForm inputPreviewForm = new InputPreviewForm(inputFilePath, getInputDelimiter(), getInputQualifier());
                inputPreviewForm.StartPosition = FormStartPosition.CenterParent;
                inputPreviewForm.Focus();
                inputPreviewForm.ShowDialog();
            }
            else 
            {
                statusTextBox.Text = "Please set:\r\nInput file";
            }
        }

        //Set input columns
        private void setInputsButton_Click(object sender, EventArgs e)
        {
            //Make new form
            //Fill datagrid in form
            //Auto detect fields
            if (inputFileProgressCheckBox.Checked && serviceProgressCheckBox.Checked)
            {
                Input inputFile = new Input(inputFilePath, getInputDelimiter(), getInputQualifier());
                statusTextBox.Text = "";
                SetInputsForm setInputsForm = new SetInputsForm(inputFilePath, getInputDelimiter(), getInputQualifier(), inputColumnProgressCheckBox);
                setInputsForm.StartPosition = FormStartPosition.CenterParent;
                setInputsForm.Focus();
                setInputsForm.ShowDialog();
            }
            else
            {
                string statusMessage = "Please set:";
                if (!inputFileProgressCheckBox.Checked) 
                {
                    statusMessage += "\r\nInput file";
                }
                if (!serviceProgressCheckBox.Checked) 
                {
                    statusMessage += "\r\nService";
                }

                statusTextBox.Text = statusMessage;
            }
        }

        //Set selected outputs
        private void setOutputsButton_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(setLicenseTextBox.Text) && serviceProgressCheckBox.Checked)
            {
                statusTextBox.Text = "";
                SetOutputsForm setOutputsForm = new SetOutputsForm(outputColumnProgressCheckBox);
                setOutputsForm.StartPosition = FormStartPosition.CenterParent;
                setOutputsForm.Focus();
                setOutputsForm.ShowDialog();
            }
            else
            {
                statusTextBox.Text = "Please set:";
                if (!string.IsNullOrEmpty(setLicenseTextBox.Text)) 
                {
                    statusTextBox.Text += "\r\nLicense";
                }
                if (!serviceProgressCheckBox.Checked) 
                {
                    statusTextBox.Text += "\r\nService";
                }
            }
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Background Worker
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Create input and output objects, initialize input and output queue, clear status texbox
                Input inputFile = new Input(inputFilePath, getInputDelimiter(), getInputQualifier());
                Output outputFile = new Output(outputFilePath, getOutputDelimiter(), getOutputQualifier());
                inputQueue = new ConcurrentQueue<Record[]>();
                outputQueue = new ConcurrentQueue<Record[]>();
                statusTextBox.Invoke(new Action(() => statusTextBox.Text += ""));
                int recordCount = 0;
                int numThreads = (numberOfThreadsMenuStripTextBox.Text.Equals("")) ? 1 : int.Parse(numberOfThreadsMenuStripTextBox.Text);

                #region Multi-thread WS send logic
                if (!serviceObject.needsAllRecords)
                {
                    //Keep looping until
                    //1. User wants to cancel OR
                    //2. Input file is done being read AND input queue is empty AND output queue is empty
                    while (!inputFile.checkForEnd())
                    {
                        //If user clicks cancel, break from loop
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }

                        //Pause if user wants to pause
                        if (!manualResetEvent.WaitOne(0))
                        {
                            statusTextBox.Invoke(new Action(() => (statusTextBox.Text += " (Paused)").Trim()));
                        }
                        manualResetEvent.WaitOne(Timeout.Infinite);

                        //Write to input queue while we have records and one record array for each thread
                        while (inputFiletoInputQueue(ref recordCount, inputFile))
                        {
                            if (inputQueue.Count >= numThreads)
                            {
                                break;
                            }
                        }

                        //Queue a "send to service" event for each batch in the input queue
                        int numObjects = inputQueue.Count;
                        ManualResetEvent[] doneEvents = new ManualResetEvent[numObjects];
                        for (int i = 0; i < numObjects; i++)
                        {
                            doneEvents[i] = new ManualResetEvent(false);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(sendRecordToService), doneEvents[i]);
                            statusTextBox.Invoke(new Action(() => statusTextBox.Text = generateProgressString((i + 1), outputFile.numberOfLinesWritten())));
                        }

                        WaitHandle.WaitAll(doneEvents.ToArray());

                        //Write records to file when we are done
                        while (!outputQueue.IsEmpty)
                        {
                            writeFromOutputQueueToFile(outputFile);
                            statusTextBox.Invoke(new Action(() => statusTextBox.Text = generateProgressString(0, outputFile.numberOfLinesWritten())));
                        }
                    }
                }
                #endregion
                #region Matchup WS Logic
                else
                {
                    //This is a service that needs all records before returning (like Matchup WS)
                    List<Record> inputHolder = new List<Record>();
                    while (!serviceObject.serviceFinishedProcessing)
                    {
                        //If user clicks cancel, break from loop
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }

                        //Send all records to the service
                        while (!inputFile.checkForEnd())
                        {
                            Record[] fullInputRecords = inputFile.getRecords(serviceObject.maxRecsPerRequest);
                            this.createRecordID(ref recordCount, fullInputRecords);
                            inputHolder.AddRange(fullInputRecords);
                            Record[] aliasedRecords = this.aliasRecords(fullInputRecords);
                            serviceObject.sendToService(aliasedRecords);
                        }

                        //Mark us as done when we're done sending records
                        if (!serviceObject.inputRecordsFinished)
                        {
                            serviceObject.inputRecordsFinished = true;
                        }
                    }

                    //When we're done processing, combine and write the records
                    Record[] outputRecords = serviceObject.outputRecords;
                    if (passThroughCheckBox.Checked) outputRecords = this.combineRecordsIfPassThrough(inputHolder.ToArray(), outputRecords);
                    outputFile.sendRecords(outputRecords);
                }
                #endregion

                inputFile.closeReader();
                outputFile.closeWriter();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                Console.Read();
            }
        }

        //Method to give each record a recordID
        private void createRecordID(ref int recordCount, Record[] inputRecords) 
        {
            foreach (Record inputRecord in inputRecords) 
            {
                inputRecord.recordID = recordCount;
                recordCount++;
            }
        }

        //Generate the string to display to the user
        private string generateProgressString(int threads, int recordsWritten) 
        {
            string returnStr = "";

            if (threads > 0)
            {
                returnStr += "Using " + threads + " threads:";
            }

            if (recordsWritten > 0)
            {
                if (!string.IsNullOrEmpty(returnStr))
                {
                    returnStr += "\r\n";
                }

                returnStr += recordsWritten + " records written.";
            }
            else 
            {
                returnStr += "\r\nProcessing records.";
            }
            return returnStr;
        }

        //Read from the input file and send to the input queue
        private bool inputFiletoInputQueue(ref int recordCount, Input inputFile) 
        {
            if (inputFile.checkForEnd())
            {
                return false;
            }

            Record[] fullInputRecords = inputFile.getRecords(serviceObject.maxRecsPerRequest);
            this.createRecordID(ref recordCount, fullInputRecords);
            foreach (Record fullInputRecord in fullInputRecords)
            {
                fullInputRecord.recordID = recordCount;
                recordCount++;
            }
            inputQueue.Enqueue(fullInputRecords);
            return true;
        }

        //Method to control writing to an output file
        private void writeFromOutputQueueToFile(Output outputFile) 
        {
            while (!outputQueue.IsEmpty) 
            {
                Record[] outputRecords;
                if (outputQueue.TryDequeue(out outputRecords)) 
                {
                    outputFile.sendRecords(outputRecords);
                }
            }
        }

        //Method to send records to a specific service object, and apply user options
        private void sendRecordToService(object state)
        {
            IWS localService = Activator.CreateInstance(serviceType) as IWS;
            localService.userLicense = MainForm.userLicense;
            localService.serviceOptions = new Dictionary<string,string>(MainForm.serviceOptions);

            Record[] inputRecords;
            if (inputQueue.TryDequeue(out inputRecords))
            {
                Record[] aliasedRecords = this.aliasRecords(inputRecords);
                localService.sendToService(aliasedRecords);
                Record[] localOutputRecords = localService.outputRecords;

                if (selectedOutputs != null) localOutputRecords = pruneNotSelectedOutputs(localOutputRecords);
                if (passThroughCheckBox.Checked) localOutputRecords = combineRecordsIfPassThrough(inputRecords, localOutputRecords);

                outputQueue.Enqueue(localOutputRecords);
                ((ManualResetEvent)state).Set();
            }
        }

        //Method used to combine input and output records if passthrough is selected
        private Record[] combineRecordsIfPassThrough(Record[] inputRecords, Record[] outputRecords) 
        {
            Dictionary<int, Record> inputPlaceHolder = inputRecords.ToDictionary(s => s.recordID);
            foreach (Record tempOutputRecord in outputRecords) 
            {
                if (inputPlaceHolder.ContainsKey(tempOutputRecord.recordID)) 
                {
                    Record tempInRecord = new Record(inputPlaceHolder[tempOutputRecord.recordID]);
                    Record tempOutRecord = new Record(tempOutputRecord);
                    tempInRecord.combineRecord(tempOutRecord);
                    outputRecords[Array.IndexOf(outputRecords, tempOutputRecord)] = tempInRecord;
                }
            }
            return outputRecords;
        }

        //Method to prune the unselected outputs from the output record
        private Record[] pruneNotSelectedOutputs(Record[] outputRecords) 
        {
            List<Record> tempRecords = new List<Record>();
            foreach (Record tempRecord in outputRecords) 
            {
                Record newRecord = new Record();
                foreach (string selectedOutput in selectedOutputs) 
                {
                    foreach (KeyValuePair<string, string> entry in tempRecord.fieldAndData) 
                    {
                        if ((entry.Key.Contains(selectedOutput)) || (entry.Key.ToLowerInvariant().Contains("results"))) 
                        {
                            newRecord.addField(entry.Key, entry.Value);
                        }
                    }
                }
                newRecord.recordID = tempRecord.recordID;
                tempRecords.Add(newRecord);
            }
            return tempRecords.ToArray();
        }

        //Method to translate input file header names to service input record names
        private Record[] aliasRecords(Record[] fullInputRecords) 
        {
            Record[] aliasedRecords = new Record[fullInputRecords.Length];
            foreach (Record fullInputRecord in fullInputRecords) 
            {
                Record aliasedRecord = new Record();

                foreach (KeyValuePair<string, string> fieldAndData in fullInputRecord.fieldAndData) 
                {
                    if (inputAliases.ContainsKey(fieldAndData.Key)) 
                    {
                        aliasedRecord.addField(inputAliases[fieldAndData.Key], fieldAndData.Value);
                    }
                }
                aliasedRecord.recordID = fullInputRecord.recordID;
                aliasedRecords[Array.IndexOf(fullInputRecords, fullInputRecord)] = aliasedRecord;
            }
            return aliasedRecords;
        }

        //Enable button when backgroundworker is finished
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Hide and disable the pause button and reset the text on it
            //Set run button text to "Run"
            this.changeRunButtonSize();

            //If there was something wrong with processing, tell the customer
            if (serviceObject.errorStatus)
            {
                serviceObject.errorStatus = false;
                statusTextBox.Text = "Something went wrong with processing:\r\n" + serviceObject.statusMessage;

                //Get the current amount of credits for the license
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    this.getCreditCount();
                }).Start();
            }
            else
            {
                //If processing is cancelled by the user, cancel when it's safe
                //Otherwise run the report if it has been enabled
                if (e.Cancelled)
                {
                    statusTextBox.Text = "Processing Cancelled.";
                }
                else
                {
                    statusTextBox.Text = "Processing finished!";
                    if (reportingCheckBox.Checked)
                    {
                        this.createReport();
                    }
                }

                //Get the current amount of credits for the license
                //Get the amount of credits consumed for a successful run
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    creditAmt = ((string)creditsLabel.Invoke(new Func<string>(() => creditsLabel.Text))).Split(':', '(')[1].Trim();
                    this.getCreditCount();
                    this.getCreditsConsumed();
                }).Start();
            }
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Delimiter + Qualifier Radio Buttons
        //The following four methods allow the user to enter into the "other" textbox in the delimiter and qualifier boxes
        //if "other" is selected
        private void inputFileDelimiterOtherRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (inputFileDelimiterOtherRadio.Checked)
            {
                inputFileDelimiterOtherTextBox.Enabled = true;
            }
            else 
            {
                inputFileDelimiterOtherTextBox.Enabled = false;
            }
        }

        private void inputFileQualifierOtherRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (inputFileQualifierOtherRadio.Checked)
            {
                inputFileQualifierOtherTextBox.Enabled = true;
            }
            else
            {
                inputFileQualifierOtherTextBox.Enabled = false;
            }
        }

        private void outputFileDelimiterOtherRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (outputFileDelimiterOtherRadio.Checked)
            {
                outputFileDelimiterOtherTextBox.Enabled = true;
            }
            else
            {
                outputFileDelimiterOtherTextBox.Enabled = false;
            }
        }

        private void outputFileQualifierOtherRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (outputFileQualifierOtherRadio.Checked)
            {
                outputFileQualifierOtherTextBox.Enabled = true;
            }
            else
            {
                outputFileQualifierOtherTextBox.Enabled = false;
            }
        }

        //The next four methods get the respective delimiters/qualifiers from the radio buttons
        private string getInputDelimiter() 
        {
            if (inputFileDelimiterCommaRadio.Checked) 
            {
                return ",";
            }
            else if (inputFileDelimiterTabRadio.Checked) 
            {
                return "\t";
            }
            else if (inputFileDelimiterPipeRadio.Checked)
            {
                return "|";
            }
            else 
            {
                return inputFileDelimiterOtherTextBox.Text;
            }
        }

        private string getInputQualifier() 
        {
            if (inputFileQualifierNoneRadio.Checked) 
            {
                return null;
            }
            else if (inputFileQualifierDoubleQuoteRadio.Checked) 
            {
                return "\"";
            }
            else if (inputFileQualifierSingleQuoteRadio.Checked)
            {
                return "'";
            }
            else 
            {
                return inputFileQualifierOtherTextBox.Text;
            }
        }

        private string getOutputDelimiter() 
        {
            if (outputFileDelimiterCommaRadio.Checked)
            {
                return ",";
            }
            else if (outputFileDelimiterTabRadio.Checked)
            {
                return "\t";
            }
            else if (outputFileDelimiterPipeRadio.Checked)
            {
                return "|";
            }
            else
            {
                return outputFileDelimiterOtherTextBox.Text;
            }
        }

        private string getOutputQualifier() 
        {
            if (outputFileQualifierNoneRadio.Checked)
            {
                return null;
            }
            else if (outputFileQualifierDoubleQuoteRadio.Checked)
            {
                return "\"";
            }
            else if (outputFileQualifierSingleQuoteRadio.Checked)
            {
                return "'";
            }
            else
            {
                return outputFileQualifierOtherTextBox.Text;
            }
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Misc Methods
        //Attempt to auto translate file input columns to service inputs if they are not explicitly set
        private bool setInputAliases() 
        {
            inputAliases = new Dictionary<string, string>();

            Input tempInput = new Input(inputFilePath, getInputDelimiter(), getInputQualifier());

            string[] headers = tempInput.headerFieldNames;
            foreach (string serviceInput in serviceObject.inputColumns)
            {
                if (headers.Select(s => s.ToLowerInvariant()).Contains(serviceInput.ToLowerInvariant()))
                {
                    inputAliases.Add(headers[Array.IndexOf(headers.Select(s => s.ToLowerInvariant()).ToArray(), serviceInput.ToLowerInvariant())], serviceInput);
                }
                else 
                {
                    if (AutoDetectInputs.variationDictionary.ContainsKey(serviceInput.ToLowerInvariant())) 
                    {
                        foreach (string headerInput in headers)  
                        {
                            if (AutoDetectInputs.variationDictionary[serviceInput.ToLowerInvariant()].Contains(headerInput.ToLowerInvariant())) 
                            {
                                inputAliases.Add(headerInput, serviceInput);
                                break;
                            }
                        }
                    }
                }
            }

            return (inputAliases.Count > 0);
        }

        //Get the amount of credits the user has
        private void getCreditCount() 
        {
            if (!string.IsNullOrEmpty(userLicense))
            {
                //Send a request to the token server to get the amount of credits tied to the user license
                string creditRequest = String.Format("http://token.melissadata.net/v3/web/service.svc/QueryCustomerInfo?L={0}&P=&K=", HttpUtility.UrlEncode(userLicense));
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(creditRequest);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader responseReader = new StreamReader(responseStream);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(responseReader.ReadToEnd());

                    //If there is a result code in the response, then there is an error since the only result codes for token server are TE (error) codes
                    //So we first check if there is a result code, if not then update credits
                    if (!doc.DocumentElement.SelectSingleNode("Result").HasChildNodes)
                    {
                        creditsLabel.Invoke(new Action(() => creditsLabel.Visible = true));
                        creditsLabel.Invoke(new Action(() => creditsLabel.Text = "Available Credits: " + doc.DocumentElement.SelectSingleNode("TotalCredits").FirstChild.Value));
                    }
                    //Otherwise, we display invalid license
                    else
                    {
                        creditsLabel.Invoke(new Action(() => creditsLabel.Visible = true));
                        creditsLabel.Invoke(new Action(() => creditsLabel.Text = "Invalid License"));
                    }
                }
                catch (Exception e) 
                {
                    statusTextBox.Invoke(new Action(() => statusTextBox.Text = "Error getting credit amount:\n" + e.ToString()));
                }
            }
        }

        //Get the amount of credits that have been consumed
        private void getCreditsConsumed() 
        {
            int difference = Int32.Parse(((string)creditsLabel.Invoke(new Func<string>(() => creditsLabel.Text))).Split(':', '(')[1].Trim()) - Int32.Parse(creditAmt);
            creditsLabel.Invoke(new Action(() => creditsLabel.Text += " (" + Math.Abs(difference).ToString() + " credits consumed)"));
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Enable run button if prerequisites are passed
        //Basically just enable the run button if all the prerequisites are achieved
        private void serviceProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (serviceProgressCheckBox.Checked)
            {
                serviceProgressCheckBox.ForeColor = Color.DarkSeaGreen;
                this.outputFileGroupBox.Enabled = true;
                this.defaultOutputFile();
            }
            else
            {
                serviceProgressCheckBox.ForeColor = Color.Crimson;
            }

            if (!string.IsNullOrEmpty(setLicenseTextBox.Text) && serviceProgressCheckBox.Checked && inputFileProgressCheckBox.Checked && outputFileProgressCheckBox.Checked)
            {
                runButton.Enabled = true;
            }
            else
            {
                runButton.Enabled = false;
            }
        }

        private void inputFileProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (inputFileProgressCheckBox.Checked)
            {
                inputFileProgressCheckBox.ForeColor = Color.DarkSeaGreen;
                this.configurationGroupBox.Enabled = true;
            }
            else
            {
                inputFileProgressCheckBox.ForeColor = Color.Crimson;
            }

            if (!string.IsNullOrEmpty(setLicenseTextBox.Text) && serviceProgressCheckBox.Checked && inputFileProgressCheckBox.Checked && outputFileProgressCheckBox.Checked)
            {
                runButton.Enabled = true;
            }
            else 
            {
                runButton.Enabled = false;
            }
        }

        private void outputFileProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (outputFileProgressCheckBox.Checked)
            {
                outputFileProgressCheckBox.ForeColor = Color.DarkSeaGreen;
                this.enableReportingGroupBox();
            }
            else
            {
                outputFileProgressCheckBox.ForeColor = Color.Crimson;
            }

            if (!string.IsNullOrEmpty(setLicenseTextBox.Text) && serviceProgressCheckBox.Checked && inputFileProgressCheckBox.Checked && outputFileProgressCheckBox.Checked)
            {
                runButton.Enabled = true;
            }
            else 
            {
                runButton.Enabled = false;
            }
        }
        
        private void inputColumnProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (inputColumnProgressCheckBox.Checked)
            {
                inputColumnProgressCheckBox.ForeColor = Color.DarkSeaGreen;
            }
            else
            {
                inputColumnProgressCheckBox.ForeColor = Color.Crimson;
            }
        }

        private void configurationProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (configurationProgressCheckBox.Checked)
            {
                configurationProgressCheckBox.ForeColor = Color.DarkSeaGreen;
            }
            else
            {
                configurationProgressCheckBox.ForeColor = Color.Crimson;
            }
        }

        private void outputColumnProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (outputColumnProgressCheckBox.Checked)
            {
                outputColumnProgressCheckBox.ForeColor = Color.DarkSeaGreen;
            }
            else
            {
                outputColumnProgressCheckBox.ForeColor = Color.Crimson;
            }
        }

        //Enable groupboxes depending on if the user entered a license or not
        private void enableGroupBoxes() 
        {
            this.inputFileGroupBox.Enabled = true;
            this.outputFileGroupBox.Enabled = true;
            this.configurationGroupBox.Enabled = true;
            this.progressGroupBox.Enabled = true;
            this.enableReportingGroupBox();
        }

        private void enableInputProgressGroupBoxes() 
        {
            this.inputFileGroupBox.Enabled = true;
            this.progressGroupBox.Enabled = true;
        }

        //Disable groupboxes depending on if the user entered a license or not
        private void disableGroupBoxes()
        {
            this.inputFileGroupBox.Enabled = false;
            this.outputFileGroupBox.Enabled = false;
            this.configurationGroupBox.Enabled = false;
            this.progressGroupBox.Enabled = false;
            this.reportingGroupBox.Enabled = false;
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Menu Items
        //Go to URLs when pressing the toolstrip menu items
        private void aboutCreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.melissa.com/credits/developer");
        }

        private void purchaseCreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.melissa.com/credits/");
        }

        private void obtainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.melissadata.com/user/signin.aspx");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm(_version);
            aboutForm.StartPosition = FormStartPosition.CenterParent;
            aboutForm.Focus();
            aboutForm.ShowDialog();
        }

        private void numberOfThreadsMenuStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void numberOfThreadsToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (numberOfThreadsMenuStripTextBox.Text.Equals(""))
            {
                numberOfThreadsMenuStripTextBox.Text = "1";
            }
        }

        private void wikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.melissadata.com/index.php?title=Listware_Desktop");
        }

        private void gitlabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://git.melissadata.com/Listware/ListwareDesktopNET");
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Reporting

        private void enableReportingGroupBox() 
        {
            if(Directory.Exists("Reporting") && File.Exists(@"Reporting\ValidDescriptions.cfg") && File.Exists(@"Reporting\ValidFilters.cfg") && File.Exists(@"Reporting\ReportTemplate.html"))
            {
                reportingGroupBox.Enabled = true;
            }
        }
        //Enable or disable reporting textboxes depending on if the checkbox is checked
        private void reportingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (reportingCheckBox.Checked)
            {
                this.enableReportingFields();
            }
            else 
            {
                this.disableReportingFields();
            }
        }

        //Enable reporting fields depending on reporting checkbox
        private void enableReportingFields() 
        {
            reportingClientNameLabel.Enabled = true;
            reportingClientNameTextBox.Enabled = true;
            reportingJobDescriptionLabel.Enabled = true;
            reportingJobDescriptionTextBox.Enabled = true;
            reportingMelissaContactLabel.Enabled = true;
            reportingMelissaContactTextBox.Enabled = true;
        }

        //Disable reporting fields depending on reporting checkbox
        private void disableReportingFields() 
        {
            reportingClientNameLabel.Enabled = false;
            reportingClientNameTextBox.Enabled = false;
            reportingJobDescriptionLabel.Enabled = false;
            reportingJobDescriptionTextBox.Enabled = false;
            reportingMelissaContactLabel.Enabled = false;
            reportingMelissaContactTextBox.Enabled = false;
        }

        //Sets all the reporting data and instantiates the reporting object
        private void createReport() 
        {
            //Set reporting data
            HeaderData hd = new HeaderData();
            hd.Client = reportingClientNameTextBox.Text;
            hd.Contacts = reportingMelissaContactTextBox.Text;
            hd.IDENT = userLicense;
            hd.JobDescription = reportingJobDescriptionTextBox.Text;
            hd.InputFileName = Path.GetFileName(this.outputFilePath);

            //Generate report, request the result code table, and set the file path/delimiter/qualifier
            //Then get the reports we want and request a report form to be generated
            GenerateReport reportModule = new GenerateReport(hd);
            reportModule.requestedRCTable = true;
            reportModule.ReadFileCreateSortedDictionaryopenWith(this.outputFilePath, this.getOutputDelimiter(), this.getOutputQualifier());
            reportModule.GetRequestedReportsList(fillFilterList(reportModule));

            //Create sample charts and graphs to copy the report images from
            #region Create Pie Chart
            Chart c = new Chart();
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            Series series1 = new Series();
            reportModule.GetPieTemplate(c);
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.Area3DStyle.Inclination = 35;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.CursorX.LineColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            c.ChartAreas.Add(chartArea1);
            legend1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.Center;
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            c.Legends.Add(legend1);
            c.Location = new System.Drawing.Point(705, 217);
            c.Name = "c";
            c.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            c.PaletteCustomColors = new System.Drawing.Color[] {
                System.Drawing.Color.DodgerBlue,
                System.Drawing.Color.Silver,
                System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
                System.Drawing.Color.White,
                System.Drawing.Color.Gray
            };
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            c.Series.Add(series1);
            c.Size = new System.Drawing.Size(300, 200);
            c.TabIndex = 3;
            c.Text = "chart1";
            c.Visible = false;
            #endregion

            #region Create Bar Graph
            Chart col = new Chart();
            ChartArea chartArea2 = new ChartArea();
            Legend legend2 = new Legend();
            Series series2 = new Series();
            reportModule.GetColumnTemplate(col);
            chartArea2.Area3DStyle.Inclination = 40;
            chartArea2.Area3DStyle.IsRightAngleAxes = false;
            chartArea2.Area3DStyle.Rotation = 0;
            chartArea2.Area3DStyle.WallWidth = 2;
            chartArea2.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea2.Name = "ChartArea1";
            col.ChartAreas.Add(chartArea2);
            legend2.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.Center;
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            col.Legends.Add(legend2);
            col.Location = new System.Drawing.Point(705, 456);
            col.Name = "col";
            col.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series2.ChartArea = "ChartArea1";
            series2.IsValueShownAsLabel = true;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.ShadowOffset = 1;
            col.Series.Add(series2);
            col.Size = new System.Drawing.Size(300, 200);
            col.TabIndex = 11;
            col.Text = "chart1";
            col.Visible = false;
            #endregion

            reportModule.GenerateReportFile();
        }

        //Generates the list of different report types depending on what result codes are present in the file
        private List<string> fillFilterList(GenerateReport reportModule) 
        {
            bool AddedToList = false;
            string validTemplateCodes;
            string[] contributor;
            List<string> reportTypes = new List<string>();
            foreach (KeyValuePair<string, string> vt in reportModule.validTemplates)
            {
                AddedToList = false;
                validTemplateCodes = vt.Value;
                contributor = validTemplateCodes.Split(new char[] { ',' });
                foreach (string c in contributor)
                {
                    // go thru each result code to see if it is in a template
                    foreach (KeyValuePair<string, int> kvp in reportModule.openWith)
                    {
                        if (kvp.Key.Contains(c) == true)
                        {
                            reportTypes.Add(vt.Key);
                            AddedToList = true;
                            break;
                        }
                    }
                    if (AddedToList == true)
                        break;
                }
            }
            return reportTypes;
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Credits
        //Listware Desktop
        //Melissa Global Intelligence 2018
        //Created by Meelad Vahdat with the help of
        //Tim Sidor, Aria Ushani, Michael Bulotano, Michael Johnson, Samuel Chung, Joseph Vertido, Katie Blair, Oscar Li
        //Reporting module created by Tim Sidor and Aria Ushani with the help of Katie Blair and integrated by Meelad Vahdat
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
