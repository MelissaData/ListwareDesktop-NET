﻿using System;
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
    public partial class SetOutputsForm : Form
    {
        private string[] outputs;
        private IWS inputService;
        CheckBox setOutputsFormCheckBox;

        //Set the service and get all current outputs from a sample request
        public SetOutputsForm(CheckBox setOutputsFormCheckBox)
        {
            InitializeComponent();
            this.inputService = Activator.CreateInstance(MainForm.serviceType) as IWS;
            this.setOutputsFormCheckBox = setOutputsFormCheckBox;
            outputs = inputService.outputColumns;
            this.fillDGV();
            this.resizeForm();
        }

        //Auto size the form to the options
        private void resizeForm()
        {
            int originalHeight = this.setOutputsFormDataGridView.Height;
            int sum = this.setOutputsFormDataGridView.ColumnHeadersHeight;

            foreach (DataGridViewRow row in this.setOutputsFormDataGridView.Rows)
            {
                sum += row.Height;
            }

            int heightDiff = originalHeight - sum;
            if (heightDiff > 0)
            {
                this.setOutputsFormDataGridView.Height = sum + 1;
                this.Height = this.Height - heightDiff;
                this.setOutputsFormSaveButton.Location = new Point(this.setOutputsFormSaveButton.Location.X, this.setOutputsFormSaveButton.Location.Y - heightDiff);
                this.setOutputsFormSelectAllButton.Location = new Point(this.setOutputsFormSelectAllButton.Location.X, this.setOutputsFormSelectAllButton.Location.Y - heightDiff);
                this.setOutputsFormCancelButton.Location = new Point(this.setOutputsFormCancelButton.Location.X, this.setOutputsFormCancelButton.Location.Y - heightDiff);
            }
        }

        private void fillDGV() 
        {
            foreach(string individualOutput in outputs)
            {
                setOutputsFormDataGridView.Rows.Add(individualOutput);
            }

            if (MainForm.selectedOutputs != null)
            {
                foreach (DataGridViewRow row in setOutputsFormDataGridView.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["SelectColumn"];

                    if (MainForm.selectedOutputs.Contains(row.Cells["FieldName"].Value)) 
                    {
                        
                        chk.Value = chk.TrueValue;
                    }
                    else 
                    {
                        chk.Value = chk.FalseValue;
                    }
                }
            }
        }

        private void setOutputsFormSaveButton_Click(object sender, EventArgs e)
        {
            List<string> selectedOutputs = new List<string>();
            foreach (DataGridViewRow row in setOutputsFormDataGridView.Rows) 
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["SelectColumn"];

                if (chk.Value == chk.TrueValue) 
                {
                    selectedOutputs.Add(row.Cells["FieldName"].Value.ToString());
                }
            }

            MainForm.selectedOutputs = selectedOutputs.ToArray();
            this.setOutputsFormCheckBox.Checked = true;
            this.Close();
            this.Dispose();
        }

        private void setOutputsFormSelectAllButton_Click(object sender, EventArgs e)
        {
            bool checkedAlready = new bool();
            foreach (DataGridViewRow row in setOutputsFormDataGridView.Rows) 
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["SelectColumn"];

                if (chk.Value == chk.FalseValue)
                {
                    checkedAlready = false;
                    break;
                }
                else 
                {
                    checkedAlready = true;
                }
            }

            foreach (DataGridViewRow row in setOutputsFormDataGridView.Rows) 
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["SelectColumn"];

                if (!checkedAlready)
                {
                    chk.Value = chk.TrueValue;
                }
                else 
                {
                    chk.Value = chk.FalseValue;
                }
            }
        }

        private void setOutputsFormCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
