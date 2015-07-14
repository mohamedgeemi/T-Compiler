using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Theory
{
    public partial class Scanner : Form
    {
        public Scanner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add File Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string OpenedFilePath = openFileDialog1.FileName;//Get Path of file
                OpenFile newFile = new OpenFile();
                string [] FileData = newFile.GetFile(OpenedFilePath);//Get file in array of string
                FileDataBox.Text="";//Clear output list
                foreach (string line in FileData)
                {
                    FileDataBox.Text+=(line+"\r\n");
                }
            }
        }

        private void FillGrid(List<KeyValuePair<string, string>> ScannerData, ref DataGridView ScannerGridView)
        {
            ScannerGridView.RowCount = 1;
            foreach(var value in ScannerData)
            {
                ScannerGridView.RowCount++;
                ScannerGridView[0, ScannerGridView.RowCount - 2].Value = value.Key;
                ScannerGridView[1, ScannerGridView.RowCount - 2].Value = value.Value;
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            string[] FileData = FileDataBox.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            CompilerScanner newScanner = new CompilerScanner();
            List<KeyValuePair<string, string>> ScannerData = new List<KeyValuePair<string, string>>();
            newScanner.StartScanner(FileData, ref ScannerData);
            FillGrid(ScannerData, ref ScannerGridView);
        }
    }
}
