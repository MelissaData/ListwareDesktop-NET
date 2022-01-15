using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ListwareDesktop.Framework;

namespace ListwareDesktop.Windows
{
    public partial class SetInputsForm : Form
    {
        IWS inputService;
        Input inputFile;
        private string inputFilePath;
        private string delimiter;
        private string qualifier;
        private CheckBox inputColumnsSetCheckBox;

        //Pass in IWS interface, so that any service that extends the interface can be passed in
        internal SetInputsForm(string inputFilePath, string delimiter, string qualifier, CheckBox inputColumnsSetCheckBox)
        {
            this.inputService = Activator.CreateInstance(MainForm.serviceType) as IWS;
            this.inputFilePath = inputFilePath;
            this.delimiter = delimiter;
            this.qualifier = qualifier;
            this.inputFile = new Input(inputFilePath, delimiter, qualifier);
            this.inputColumnsSetCheckBox = inputColumnsSetCheckBox;

            InitializeComponent();

            this.fillDGV();
            this.resizeForm();
            this.setDefaultValuesForInputs();
        }

        //Auto size the form to the options
        private void resizeForm()
        {
            int originalHeight = this.setInputsFormDataGridView.Height;
            int sum = this.setInputsFormDataGridView.ColumnHeadersHeight;

            foreach (DataGridViewRow row in this.setInputsFormDataGridView.Rows)
            {
                sum += row.Height;
            }

            this.setInputsFormDataGridView.Height = sum + 1;
            int heightDiff = originalHeight - sum;
            this.Height = this.Height - heightDiff;
            this.setInputsFormSaveButton.Location = new Point(this.setInputsFormSaveButton.Location.X, this.setInputsFormSaveButton.Location.Y - heightDiff);
            this.setInputsFormCancelButton.Location = new Point(this.setInputsFormCancelButton.Location.X, this.setInputsFormCancelButton.Location.Y - heightDiff);
        }

        //Datagridviewer filled with service inputs and columns from input file
        private void fillDGV() 
        {
            foreach(string innerString in inputService.inputColumns)
            {   
                setInputsFormDataGridView.Rows.Add(innerString);
            }

            List<string> inputHeaderFieldNames = inputFile.headerFieldNames.ToList<string>();

            inputHeaderFieldNames.Insert(0, " ");

            DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn.Name = "Values";
            dataGridViewComboBoxColumn.Items.AddRange(inputHeaderFieldNames.ToArray());
            setInputsFormDataGridView.Columns.Add(dataGridViewComboBoxColumn);
        }

        //This is hard to explain, this method basically tries to auto detect any columns that are using common names
        //Names that are detected can be edited in AutoDetectInput.cs
        private void setDefaultValuesForInputs() 
        {
            //Get headers from input file, change them to lowercase to make it easier
            string[] inputFileHeadersLowercase = inputFile.headerFieldNames.Select(s => s.ToLowerInvariant()).ToArray();

            //Iterate through each row in datagridview
            foreach (DataGridViewRow row in setInputsFormDataGridView.Rows) 
            {
                //Check current service input from datagrid, change to lowercase
                string currentInputHeaderService = row.Cells[0].Value.ToString().ToLowerInvariant();

                if (MainForm.inputAliases == null)
                {

                    //This is to check if we found an exact match
                    bool exactMatchFound = new bool();

                    //Check for exact name match
                    foreach (string inputFileIndividualHeader in inputFileHeadersLowercase)
                    {
                        if (inputFileIndividualHeader.Equals(currentInputHeaderService))
                        {
                            DataGridViewComboBoxCell currentCBCell = row.Cells[1] as DataGridViewComboBoxCell;
                            currentCBCell.Value = inputFile.headerFieldNames[Array.IndexOf(inputFileHeadersLowercase, inputFileIndividualHeader)];
                            exactMatchFound = true;
                            break;
                        }
                    }

                    //If no exact match, then check variant dictionary
                    if (!exactMatchFound)
                    {
                        if (AutoDetectInputs.variationDictionary.ContainsKey(currentInputHeaderService))
                        {
                            string[] possibleVariants = AutoDetectInputs.variationDictionary[currentInputHeaderService];

                            foreach (string inputFileIndividualHeader in inputFileHeadersLowercase)
                            {
                                if (Array.IndexOf(possibleVariants, inputFileIndividualHeader) > -1)
                                {
                                    //Set to inputFileIndividualHeader then break
                                    DataGridViewComboBoxCell currentCBCell = row.Cells[1] as DataGridViewComboBoxCell;
                                    currentCBCell.Value = inputFile.headerFieldNames[Array.IndexOf(inputFileHeadersLowercase, inputFileIndividualHeader)];
                                    break;
                                }
                            }
                        }
                    }
                }
                else 
                {
                    if (MainForm.inputAliases.Values.Select(s => s.ToLowerInvariant()).ToList().Contains(currentInputHeaderService))
                    {
                        DataGridViewComboBoxCell currentCBCell = row.Cells[1] as DataGridViewComboBoxCell;

                        foreach (KeyValuePair<string, string> innerPair in MainForm.inputAliases) 
                        {
                            if (innerPair.Value.ToLowerInvariant().Equals(currentInputHeaderService)) 
                            {
                                currentCBCell.Value = innerPair.Key;
                                break;
                            }
                        }
                    }
                }
            }

        }

        private void setInputsFormSaveButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> inputAliases = new Dictionary<string, string>();
            foreach (DataGridViewRow row in setInputsFormDataGridView.Rows)
            {
                DataGridViewComboBoxCell currentCBCell = row.Cells[1] as DataGridViewComboBoxCell;
                if ((currentCBCell.Value != null) && (!currentCBCell.Value.Equals(" ")))
                {
                    if (!inputAliases.ContainsKey(row.Cells[1].Value.ToString()))
                    {
                        inputAliases.Add(row.Cells[1].Value.ToString(), row.Cells[0].Value.ToString());
                    }
                    else 
                    {
                        inputAliases[row.Cells[1].Value.ToString()] = row.Cells[0].Value.ToString();
                    }
                }
            }
            MainForm.inputAliases = new Dictionary<string,string>(inputAliases);

            inputColumnsSetCheckBox.Checked = true;
            
            this.Close();
        }

        private void setInputsFormCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
