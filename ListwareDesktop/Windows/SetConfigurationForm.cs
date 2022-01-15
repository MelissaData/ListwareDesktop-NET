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
    internal partial class SetConfigurationForm : Form
    {
        IWS inputService;
        CheckBox setConfigurationFormCheckBox;
        DataGridViewCell currentMultipleCell;
        Dictionary<string, string> localServiceOptions;
        Dictionary<string, string> localOptionNameTranslations = new Dictionary<string, string>();

        //Get which service we are using in constructor
        internal SetConfigurationForm(CheckBox setConfigurationFormCheckBox)
        {
            this.inputService = Activator.CreateInstance(MainForm.serviceType) as IWS;
            this.setConfigurationFormCheckBox = setConfigurationFormCheckBox;
            localServiceOptions = MainForm.serviceOptions;
            InitializeComponent();
            this.fillDGV();
            this.resizeForm();
        }

        //Auto size the form to the options
        private void resizeForm() 
        {
            int originalHeight = this.setConfigurationFormDataGridView.Height;
            int sum = this.setConfigurationFormDataGridView.ColumnHeadersHeight;

            foreach (DataGridViewRow row in this.setConfigurationFormDataGridView.Rows)
            {
                sum += row.Height;
            }

            this.setConfigurationFormDataGridView.Height = sum + 1;
            int heightDiff = originalHeight - sum;
            this.Height = this.Height - heightDiff;
            this.setConfigurationFormSaveButton.Location = new Point(this.setConfigurationFormSaveButton.Location.X, this.setConfigurationFormSaveButton.Location.Y - heightDiff);
            this.setConfigurationFormCancelButton.Location = new Point(this.setConfigurationFormCancelButton.Location.X, this.setConfigurationFormCancelButton.Location.Y - heightDiff);
        }

        //Fill datagridview for options from current service
        private void fillDGV()
        {
            foreach (KeyValuePair<string, List<string>> innerKVP in inputService.settingsList) 
            {
                string optionName = innerKVP.Key;
                if (optionName.Contains('_')) 
                {
                    string optionNameShort = innerKVP.Key.Split('_')[1];
                    this.localOptionNameTranslations.Add(optionNameShort, optionName);
                    optionName = optionNameShort;
                    
                }
                string optionType = innerKVP.Value[0];
                List<string> optionValues = innerKVP.Value.Where((v, i) => i != 0).ToList();

                //Go through each option in the service settings list and set the default values depending on the option type
                switch (optionType.ToLowerInvariant()) 
                {
                    case "manual":
                        if (optionValues.Count > 0)
                        {
                            setConfigurationFormDataGridView.Rows.Add(optionName, optionValues[0]);
                        }
                        else 
                        {
                            setConfigurationFormDataGridView.Rows.Add(optionName, "");
                        }
                        break;
                    case "single":
                        setConfigurationFormDataGridView.Rows.Add(optionName, "");
                        break;
                    case "multiple":
                        setConfigurationFormDataGridView.Rows.Add(optionName, "Click to Select");
                        break;
                }
            }

            //Go through the datagridview row by row, get the option type, and set the cell type depending on what type of option it is
            foreach (DataGridViewRow row in setConfigurationFormDataGridView.Rows) 
            {
                string optionName = this.translateColumn(row.Cells[0].Value.ToString());
                string optionType = inputService.settingsList[optionName][0];
                List<string> optionValues = inputService.settingsList[optionName].Where((v, i) => i != 0).ToList();

                if (inputService.settingsList.ContainsKey(optionName)) 
                {
                    switch (optionType.ToLowerInvariant())
                    {
                        case "manual":
                            if (localServiceOptions.ContainsKey(optionName)) 
                            {
                                row.Cells[1].Value = localServiceOptions[optionName];
                            }
                            row.Cells[1].ReadOnly = false;
                            break;
                        case "single":
                            DataGridViewComboBoxCell comboboxCell = new DataGridViewComboBoxCell();
                            comboboxCell.Items.AddRange(inputService.settingsList[this.localOptionNameTranslations[row.Cells[0].Value.ToString()]].Where((v, i) => i != 0).ToArray());
                            if (localServiceOptions.ContainsKey(optionName)) 
                            {
                                comboboxCell.Value = localServiceOptions[optionName];
                            }
                            row.Cells[1] = comboboxCell;
                            break;
                        case "multiple":
                            if (localServiceOptions.ContainsKey(optionName))
                            {
                                row.Cells[1].Value = localServiceOptions[optionName];
                            }
                            row.Cells[1].ReadOnly = true;
                            break;
                    }
                }
            }
       }

        private string translateColumn(string optionName) 
        {
            if (this.localOptionNameTranslations.ContainsKey(optionName))
            {
                return this.localOptionNameTranslations[optionName];
            }

            return optionName;
        }

        //Method to save all of the cells to the service options dictionary
        private void saveCellsToServiceOptions()
        {
            foreach (DataGridViewRow row in setConfigurationFormDataGridView.Rows)
            {
                string optionName = this.translateColumn(row.Cells[0].Value.ToString());
                string optionType = inputService.settingsList[optionName][0];
                List<string> optionValues = inputService.settingsList[optionName].Where((v, i) => i != 0).ToList();


                //Go through each row in the datagridview and save the values to userService.serviceOptions
                if (row.Cells[1].Value != null)
                {
                    if (localServiceOptions.ContainsKey(optionName))
                    {
                        if (row.Cells[1].Value.ToString().Trim().Equals(""))
                        {
                            localServiceOptions.Remove(optionName);
                        }
                        else if(!row.Cells[1].Value.ToString().Equals("Click to Select"))
                        {
                            localServiceOptions[optionName] = row.Cells[1].Value.ToString();
                        }
                    }
                    else if (!row.Cells[1].Value.ToString().Equals("Click to Select")) 
                    {
                        localServiceOptions.Add(optionName, row.Cells[1].Value.ToString());
                    }
                }
            }
        }

        //When the save button is hit, add all settings and values to the service
        private void setConfigurationFormSaveButton_Click(object sender, EventArgs e)
        {
            if (setConfigurationFormCheckedListBox.Visible) 
            {
                setConfigurationFormCheckedListBox.Hide();
                setCellValue(currentMultipleCell);
                setConfigurationFormCheckedListBox.Update();
            }
            this.saveCellsToServiceOptions();
            this.Close();
            MainForm.serviceOptions = new Dictionary<string, string>(localServiceOptions);
            if (localServiceOptions.Count > 0)
            {
                setConfigurationFormCheckBox.Checked = true;
            }
        }

        //When cancel is hit, close the window
        private void setConfigurationFormCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Method to move the checkedlistbox
        private void moveCheckedListBox(DataGridViewCell inputCell) 
        {
            int RowHeight1 = setConfigurationFormDataGridView.Rows[inputCell.RowIndex].Height;
            Rectangle CellRectangle1 = setConfigurationFormDataGridView.GetCellDisplayRectangle(inputCell.ColumnIndex, inputCell.RowIndex, false);
            CellRectangle1.X += setConfigurationFormDataGridView.Left;
            CellRectangle1.Y += setConfigurationFormDataGridView.Top + RowHeight1;
            setConfigurationFormCheckedListBox.Left = CellRectangle1.X;
            setConfigurationFormCheckedListBox.Top = CellRectangle1.Y;
        }

        //This one is a little complicated, since there is no way to created a checkedlistbox cell other than to overload the method
        //Essentially, we have a floating checkedlistbox that appears and disappears based on the user clicks
        //It changes location based on what cell is clicked
        //And it saves values when it is closed
        private void setConfigurationFormDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 1))
            {
                DataGridViewCell nameCell = setConfigurationFormDataGridView.Rows[e.RowIndex].Cells[0];
                DataGridViewCell valueCell = setConfigurationFormDataGridView.Rows[e.RowIndex].Cells[1];
                string optionName = this.translateColumn(nameCell.Value.ToString());
                string optionType = inputService.settingsList[optionName][0];
                List<string> optionValues = inputService.settingsList[optionName].Where((v, i) => i != 0).ToList();

                //If the checkedlistbox is invisible, and we are on the proper option type
                //Move the checkedlistbox to the proper location, fill in the listbox items, set the proper checkmarks, show, then update
                if (!setConfigurationFormCheckedListBox.Visible)
                {
                    if (optionType.ToLowerInvariant().Equals("multiple"))
                    {
                        this.moveCheckedListBox(valueCell);
                        this.setCheckedListBoxItems(valueCell);
                        this.setCheckedListBoxChecks(valueCell);
                        setConfigurationFormCheckedListBox.Show();
                        setConfigurationFormCheckedListBox.Update();
                        currentMultipleCell = valueCell;
                    }
                }
                else 
                {
                    //If the listbox is already visible, if we move to a different "multiple" option type, update the previous cell then
                    //Update the listbox, checks, and location
                    if ((optionType.ToLowerInvariant().Equals("multiple")) && (valueCell.RowIndex != currentMultipleCell.RowIndex))
                    {
                        this.setCellValue(currentMultipleCell);
                        this.setCheckedListBoxItems(valueCell);
                        this.setCheckedListBoxChecks(valueCell);
                        this.moveCheckedListBox(valueCell);
                        currentMultipleCell = valueCell;
                    }
                    //If the listbox is visible and we click on the same option as before, set the cell value and hide the listbox
                    else
                    {
                        this.setCellValue(valueCell);
                        setConfigurationFormCheckedListBox.Hide();
                    }
                }
            }
            //If we click somewhere not in column 1, then hide the listbox and update the previous cell if applicable
            else
            {
                if (setConfigurationFormCheckedListBox.Visible)
                {
                    setConfigurationFormCheckedListBox.Hide();
                    setCellValue(currentMultipleCell);
                }
            }
        }

        //Helper method that sets the cell value
        private void setCellValue(DataGridViewCell inputCell) 
        {
            string checkedItems = "";
            foreach (object checkedItem in setConfigurationFormCheckedListBox.CheckedItems) 
            {
                checkedItems += checkedItem.ToString() + ",";
            }
            checkedItems = checkedItems.TrimEnd(',');
            inputCell.Value = checkedItems;
        }

        //Set the items in the checkedlistbox based on the cell
        private void setCheckedListBoxItems(DataGridViewCell inputCell) 
        {
            string optionName = setConfigurationFormDataGridView.Rows[inputCell.RowIndex].Cells[0].Value.ToString();
            string optionType = inputService.settingsList[optionName][0];
            List<string> optionValues = inputService.settingsList[optionName].Where((v, i) => i != 0).ToList();
            setConfigurationFormCheckedListBox.Items.Clear();
            setConfigurationFormCheckedListBox.Items.AddRange(optionValues.ToArray());
        }

        //Set the checks in the checkedlistbox based on the cell value
        private void setCheckedListBoxChecks(DataGridViewCell inputCell) 
        {
            if (!string.IsNullOrEmpty(inputCell.Value.ToString().Trim())) 
            {
                string[] setOptions = inputCell.Value.ToString().Split(',');
                for (int i = 0; i < setConfigurationFormCheckedListBox.Items.Count; i++) 
                {
                    if (setOptions.Contains(setConfigurationFormCheckedListBox.Items[i].ToString())) 
                    {
                        setConfigurationFormCheckedListBox.SetItemChecked(i, true);
                    }
                }
            }
        }
    }
}
