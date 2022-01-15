using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace ListwareDesktop.Reporting
{
    class GenerateReport
    {
        private HeaderData hd             { get; set; }
        private string OutputReportName { get; set; }
        private long Total { get; set; }
        private Chart pieCharttemplate { get; set; }
        private Chart columnCharttemplate { get; set; }

        private List<int> rcFields = new List<int>();
        internal SortedDictionary<string, int> openWith = new SortedDictionary<string, int>();
        internal Dictionary<string, string> validTemplates = new Dictionary<string, string>();
        private Dictionary<string, string> validDescriptions = new Dictionary<string, string>();
        private List<object> requestedReportCharts;
        internal bool requestedRCTable;

        public GenerateReport(HeaderData reportHeaderData)
        {
            this.hd = reportHeaderData;
            this.LoadFilters();
            this.LoadDescriptions();
            this.requestedReportCharts = new List<object>();
            this.requestedRCTable = false;
            this.Total = 0;
        }

        public void LoadDescriptions()
        {
            FileStream InCfg;
            StreamReader srCfg; 
            string cfgRecord;
            string[] Fields;

            try
            {
                InCfg = new FileStream(@"Reporting\ValidDescriptions.cfg", FileMode.Open);
                srCfg = new StreamReader(InCfg);
            }
            catch (Exception)
            {
                System.Console.WriteLine("Could not open Description Configuration File");
                return;
            }

            srCfg.ReadLine();  // ignore header
            while ((cfgRecord = srCfg.ReadLine()) != null)
            {
               Fields = cfgRecord.Split(new char[] { '\t' });
               this.validDescriptions.Add(Fields[0], Fields[1]);            
            }
            InCfg.Close();
            srCfg.Close();
        }

        public void LoadFilters() 
        {
            FileStream InCfg;
            StreamReader srCfg;
            string cfgRecord;
            string[] Fields;

            try
            {
                InCfg = new FileStream(@"Reporting\ValidFilters.cfg", FileMode.Open);
                srCfg = new StreamReader(InCfg);
            }
            catch (Exception)
            {
                System.Console.WriteLine("Could not open Report Filter Configuration File");
                return;
            }

            srCfg.ReadLine();  // ignore header
            while ((cfgRecord = srCfg.ReadLine()) != null)
            {
                Fields = cfgRecord.Split(new char[] { '\t' });
                this.validTemplates.Add(Fields[0], Fields[1]);
            }
            InCfg.Close();
            srCfg.Close();
        }


        public void ReadFileCreateSortedDictionaryopenWith(string dINPUTFILE, string delimiter, string qualifier)
        {
            FileStream InFile;
            StreamReader srFile;
            string Record;
            string[] Fields;
            long TTotal = 0; 

            #region ErrorCheckIncomingFile
            try
            {
                InFile = new FileStream(dINPUTFILE, FileMode.Open);
                srFile = new StreamReader(InFile); 
            }
            catch (Exception)
            {
                System.Console.WriteLine("Could not open \"{0}\"", dINPUTFILE);
                return;
            }
            #endregion ErrorCheckIncomingFile

            #region getResultcodefieldsfromheader
            Record = srFile.ReadLine();

            if (qualifier == null)
            {
                Fields = Record.Split(delimiter[0]).Select(s => s.Trim()).ToArray();
            }
            else
            {
                string pattern = string.Format(@"{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", Regex.Escape(delimiter), Regex.Escape(qualifier));
                string[] split = Regex.Split(Record, pattern);
                Fields = split.Select(s => s.Trim(qualifier[0], ' ')).ToArray();
            }

            int i = 0;
            foreach (string field in Fields)
            {
                if (field.ToLowerInvariant().Contains("results"))
                    rcFields.Add(i);
                i++;
            }
            #endregion getResultcodefieldsfromheader

            #region ReadFileCreateResultDictionary
            while ((Record = srFile.ReadLine()) != null)
            {
                if (qualifier == null)
                {
                    if (delimiter.Equals(","))
                    {
                        string pattern = string.Format(@"{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", Regex.Escape(delimiter), Regex.Escape("\""));
                        string[] split = Regex.Split(Record, pattern);
                        Fields = split.Select(s => s.Trim('"', ' ')).ToArray();
                    }
                    else 
                    {
                        Fields = Record.Split(delimiter[0]).Select(s => s.Trim()).ToArray();
                    }
                }
                else
                {
                    string pattern = string.Format(@"{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", Regex.Escape(delimiter), Regex.Escape(qualifier));
                    string[] split = Regex.Split(Record, pattern);
                    Fields = split.Select(s => s.Trim(qualifier[0], ' ')).ToArray();
                }

                foreach (int x in rcFields)
                {
                    string mdresults = "";
                    try
                    {
                        mdresults = Fields[x];
                    }
                    catch (IndexOutOfRangeException) 
                    {
                        mdresults = "XXXX";
                    }

                    string[] rowcodes = mdresults.Split(',');
                    foreach (string rcode in rowcodes)
                    {
                        if ((rcode.Length != 4) || ((rcode.Length == 4) && ((!char.IsLetter(rcode[0])) || (!char.IsLetter(rcode[1])) || (!char.IsDigit(rcode[2])) || (!char.IsDigit(rcode[3])))))
                        {
                            if (!this.openWith.ContainsKey("XXXX"))
                            {
                                this.openWith.Add("XXXX", 1);
                            }
                            else
                            {
                                this.openWith["XXXX"] = openWith["XXXX"] + 1;
                            }
                        }
                        else
                        {
                            if (!this.openWith.ContainsKey(rcode))
                            {
                                this.openWith.Add(rcode, 1);
                            }
                            else
                            {
                                this.openWith[rcode] = openWith[rcode] + 1;
                            }
                        }
                    }
                }

                TTotal++;

            }  // end while has records
            this.Total = TTotal;
            #endregion ReadFileCreateResultDictionary

            InFile.Close();
            srFile.Close();
        }

        public void GetRequestedReportsList(List<string> reqRepChrts) 
        {
            foreach (string s in reqRepChrts)
                requestedReportCharts.Add(s);
        }

        public mdRCTable CreateTable(string filter)
        {
            string validTemplateCodes;
            string rcDescription;
            string[] contibutor;

            mdRCTable rcT = new mdRCTable();
            DataTable dt = new DataTable();
            dt.Columns.Add("ResultCode", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("Percent", typeof(decimal));
            dt.Columns.Add("Category", typeof(string));
            validTemplateCodes = this.validTemplates[filter];
            contibutor = validTemplateCodes.Split(new char[] { ',' });   // MS = MS01,MS02,MS03,MS07
            foreach (string c in contibutor)
            {
                if (this.validDescriptions.TryGetValue(c, out rcDescription) == false)
                    rcDescription = c;

                if (openWith.ContainsKey(c))
                {
                    dt.Rows.Add(c, openWith[c], (Math.Round((openWith[c] * 100) / (decimal)this.Total, 2).ToString()), rcDescription);
                }
                else
                    dt.Rows.Add(c, 0, 0, rcDescription);
            }

            rcT.tableName = filter;
            rcT.rcTable = dt;
            if (filter.Contains("Levels"))
                rcT.chartType = "Pie";
            else if (filter.Contains("Quality"))
                rcT.chartType = "Column";
            else if (filter.Contains("Uplift"))
                rcT.chartType = "Column";
            else
                rcT.chartType = "Table";
            return rcT;
        }

        public string ConvertChartFileto64Base(string png)
        {
            FileStream fs = new FileStream(png, FileMode.Open, FileAccess.Read);
            byte[] filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            string imm = "data:image/png;base64," + Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
            fs.Close();
            fs.Dispose();
            return imm;
        }

        public void GetPieTemplate(Chart cp)
        {
            this.pieCharttemplate = cp;
        }

        public void GetColumnTemplate(Chart cc)
        {
            this.columnCharttemplate = cc;
        }




        public string CreateChartTypePie(mdRCTable rcT)
        {
            this.pieCharttemplate.Series.Clear();

            string seriesnameM = rcT.tableName;
            this.pieCharttemplate.Series.Add(seriesnameM);
            this.pieCharttemplate.Series[seriesnameM].ChartType = SeriesChartType.Pie;

            this.pieCharttemplate.Series[seriesnameM].XValueMember = rcT.rcTable.Rows[0].ToString();
            this.pieCharttemplate.Series[seriesnameM].YValueMembers = rcT.rcTable.Rows[1].ToString();
            this.pieCharttemplate.Series[seriesnameM].IsValueShownAsLabel = true;

            Color[] myPalette = new Color[6]{
                Color.FromKnownColor(KnownColor.DodgerBlue), 
                Color.FromKnownColor(KnownColor.SkyBlue), 
                Color.FromKnownColor(KnownColor.LightGray),
                Color.FromKnownColor(KnownColor.LightCyan),
                Color.FromKnownColor(KnownColor.Aqua),
                Color.FromKnownColor(KnownColor.Silver)
           };

            this.pieCharttemplate.Palette = ChartColorPalette.None;
            this.pieCharttemplate.PaletteCustomColors = myPalette;

            this.pieCharttemplate.Series[seriesnameM].Label = "#PERCENT{P2}";
            this.pieCharttemplate.Series[seriesnameM].LegendText = "#VALX";

            this.pieCharttemplate.DataBind();

            foreach (DataRow row in rcT.rcTable.Rows)
            {
                if (row[1].ToString() != "0")
                    this.pieCharttemplate.Series[seriesnameM].Points.AddXY(row[0], row[1]);
            }
            this.pieCharttemplate.SaveImage(rcT.tableName + ".png", ChartImageFormat.Png);

            return rcT.tableName + ".png";
        }


        public string CreateChartTypeColumn(mdRCTable rcT)
        {
            this.columnCharttemplate.Series.Clear();
            string seriesnameD = rcT.tableName;
            this.columnCharttemplate.Series.Add(seriesnameD);
            this.columnCharttemplate.Series[seriesnameD].ChartType = SeriesChartType.Column;

            KnownColor kculah = KnownColor.SkyBlue;
            if (seriesnameD.Contains("Uplift") == true) 
                kculah = KnownColor.MediumAquamarine;

            Color[] myPaletteD = new Color[1] { Color.FromKnownColor(kculah) };
            this.columnCharttemplate.Palette = ChartColorPalette.None;
            this.columnCharttemplate.PaletteCustomColors = myPaletteD;

            foreach (DataRow row in rcT.rcTable.Rows)
            {
                this.columnCharttemplate.Series[seriesnameD].Points.AddXY(row[0], row[1]);
            }
            columnCharttemplate.ResetAutoValues();
            this.columnCharttemplate.SaveImage(rcT.tableName + ".png", ChartImageFormat.Png);
            return rcT.tableName + ".png";
        }

        public void GenerateReportFile()
        {
            string chartToAdd = "";
            string strHtmlTemplate = File.ReadAllText(@"Reporting\ReportTemplate.html");

            strHtmlTemplate = strHtmlTemplate.Replace("[Client]", hd.Client);
            strHtmlTemplate = strHtmlTemplate.Replace("[IDENT]", hd.IDENT);
            strHtmlTemplate = strHtmlTemplate.Replace("[JOB_DESC]", hd.JobDescription);
            strHtmlTemplate = strHtmlTemplate.Replace("[Melissa_Contact]", hd.Contacts);
            strHtmlTemplate = strHtmlTemplate.Replace("[ProcessedFile]", hd.InputFileName);
            strHtmlTemplate = strHtmlTemplate.Replace("[Total]", Total.ToString());

            #region for each requested ReportChart Option create a table, create a chart, start the html doc.
            foreach (object itemChecked in requestedReportCharts)
            {
                //CreateTable(itemChecked.ToString());
                mdRCTable rcT = new mdRCTable();
                rcT = CreateTable(itemChecked.ToString());
                if (rcT.chartType == "Pie")
                    chartToAdd = CreateChartTypePie(rcT);
                else if (rcT.chartType == "Column")
                    chartToAdd = CreateChartTypeColumn(rcT);
                
                
                strHtmlTemplate += "<h3>" + rcT.tableName + "</h3>";
                strHtmlTemplate += "<table class=\"table4\" ><tr><td width=\"350\" height=\"200\">\r\n<table class=\"table2\">\r\n";
                strHtmlTemplate += "<tr><th>" + rcT.rcTable.Columns[3].ToString() + "</th><th>" + rcT.rcTable.Columns[0].ToString() + "</th><th>" + rcT.rcTable.Columns[1].ToString() + "</th></tr>\r\n";
                //for each row in table
                foreach (DataRow r in rcT.rcTable.Rows)
                {
                    strHtmlTemplate += "<tr><td>" + r[3] + "</td><td>" + r[0] + "</td><td>" + r[1] + "</td></tr>\r\n";
                }
                strHtmlTemplate += "</table></td><td width=\"350\" height=\"200\">\r\n";

                strHtmlTemplate += "<img src=\"" + ConvertChartFileto64Base(chartToAdd) + "\" align=\"right\" alt=\"" + chartToAdd + "\" height=\"200\" width=\"300\"></td></tr></table><br />\r\n\r\n";
            }

            if (requestedRCTable == true)
            {
                strHtmlTemplate += "<h3>" + "RESULT CODE COUNTS" + "</h3>";
                strHtmlTemplate += "<table class=\"table3\">\r\n";
                strHtmlTemplate += "<tr><th>" + "RESULT CODE" + "</th><th>" + "DESCRIPTION" + "</th><th>" + "COUNT" + "</th><th>" + "PERCENT" + "</th></tr>\r\n";

                foreach (KeyValuePair<string, int> kvp in this.openWith)
                {
                    if (this.validDescriptions.Keys.Contains(kvp.Key))
                    {
                        strHtmlTemplate += "<tr><td>" + kvp.Key + "</td><td>" + this.validDescriptions[kvp.Key] + "</td><td>" + kvp.Value + "</td><td>" + Math.Round((kvp.Value * 100) / (decimal)Total, 2).ToString() + "</td></tr>\r\n";
                    }
                }
                strHtmlTemplate += "</table>";
            }

            strHtmlTemplate += "</div></body></html>";
            #endregion for each checked list create a table, create a chart, start the html doc.

            File.WriteAllText("Report.html", strHtmlTemplate);
            List<string> filePaths = Directory.GetFiles(@"Reporting").ToList<string>();
            filePaths.AddRange(Directory.GetFiles(Directory.GetCurrentDirectory()));
            foreach (string filePath in filePaths)
            {
                if (filePath.Contains(".png"))
                    File.Delete(filePath);
            }
        }



    } // end class ReportData

    public class HeaderData
    {
        public string Client { get; set; }
        public string IDENT { get; set; }
        public string JobDescription { get; set; }
        public string Contacts { get; set; }
        public string InputFileName { get; set; } 
    }

    public class mdRCTable
    {
        public string tableName { get; set; }
        public DataTable rcTable { get; set; }
        public string chartType { get; set; }

        public mdRCTable() { }
        public mdRCTable(string tn, DataTable dt, string ct)
        {
            tableName = tn;
            rcTable = dt;
            chartType = ct;
        }
    }
}
