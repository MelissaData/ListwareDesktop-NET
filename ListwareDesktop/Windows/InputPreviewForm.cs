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
    internal partial class InputPreviewForm : Form
    {
        private string inputFilePath;
        private string delimiter;
        private string qualifier;

        internal InputPreviewForm(string inputFilePath, string delimiter, string qualifier)
        {
            this.inputFilePath = inputFilePath;
            this.delimiter = delimiter;
            this.qualifier = qualifier;
            
            InitializeComponent();

            this.fillDGV();
            this.resizeForm();

            ClientSize = new Size(previewDataGridView.Width,previewDataGridView.Height);
        }

        private void resizeForm() 
        {
            int originalHeight = this.previewDataGridView.Height;
            int originalWidth = this.previewDataGridView.Width;
            int sumHeight = this.previewDataGridView.ColumnHeadersHeight;
            int sumWidth = 0;

            foreach (DataGridViewRow row in this.previewDataGridView.Rows)
            {
                sumHeight += row.Height;
            }

            foreach (DataGridViewColumn column in this.previewDataGridView.Columns) 
            {
                sumWidth += column.Width;
            }

            int heightDiff = originalHeight - sumHeight;
            int widthDiff = originalWidth - sumWidth;

            if (heightDiff > 0) 
            {
                this.previewDataGridView.Height = sumHeight + 1;
                this.Height = this.Height - heightDiff;
            }

            if (widthDiff > 0) 
            {
                this.previewDataGridView.Width = sumWidth + 1;
                this.Width = this.Width - widthDiff;
            }
        }

        private void fillDGV() 
        {
            Input inputPreview = new Input(inputFilePath, delimiter, qualifier);
            Record[] previewRecs = inputPreview.getRecordsForPreview();

            previewDataGridView.ColumnCount = inputPreview.headerFieldNames.Length;
            previewDataGridView.ColumnHeadersVisible = true;

            foreach(string header in inputPreview.headerFieldNames)
            {
                previewDataGridView.Columns[Array.IndexOf(inputPreview.headerFieldNames, header)].HeaderCell.Value = header;
            }

            foreach(Record tempPreviewRec in previewRecs)
            {
                DataGridViewRow row = new DataGridViewRow();
                List<string> dataListPreview = new List<string>();
                foreach (string header in inputPreview.headerFieldNames) 
                {
                    dataListPreview.Add(tempPreviewRec.fieldAndData[header]);
                }
                previewDataGridView.Rows.Add(dataListPreview.ToArray());
            }

            inputPreview.closeReader();
        }
    }
}
