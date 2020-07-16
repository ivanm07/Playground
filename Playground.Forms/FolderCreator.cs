using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playground.Forms
{
    public partial class FolderCreator : Form
    {
        public FolderCreator()
        {
            InitializeComponent();
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            BackgroundWorker bg = new BackgroundWorker()
            {
                WorkerReportsProgress = true
            };
            progressBar1.Visible = true;
            bg.DoWork += (s, args) =>
            {
                int i = 0;
                var total = ListBoxResults.Items.Count;
                foreach (string item in ListBoxResults.Items)
                {
                    i++;
                    Directory.CreateDirectory(Path.Combine(TxtDestination.Text, item));
                    Directory.CreateDirectory(Path.Combine(TxtDestination.Text, item, @"BANKING INFO"));
                    Directory.CreateDirectory(Path.Combine(TxtDestination.Text, item, @"FM-269"));
                    Directory.CreateDirectory(Path.Combine(TxtDestination.Text, item, @"OTHER"));
                    Directory.CreateDirectory(Path.Combine(TxtDestination.Text, item, @"W-9 - RFC"));
                    bg.ReportProgress(i * 100 / total);
                }
            };
            bg.ProgressChanged += (s, args) =>
            {
                progressBar1.Value = args.ProgressPercentage;
            };
            bg.RunWorkerCompleted += (s, args) =>
            {
                MessageBox.Show("Done");
                progressBar1.Visible = !true;
            };
            bg.RunWorkerAsync();
        }

        private void TxtSourcePath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var reader = new StreamReader(TxtSourcePath.Text))
                {
                    while (!reader.EndOfStream)
                    {
                        ListBoxResults.Items.Add(reader
                            .ReadLine()
                            .Replace('"', ' ')
                            .TrimStart()
                            .TrimEnd());
                    }
                }
                lblResults.Text = $"Results: {ListBoxResults.Items.Count} items";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString(), ex.Message);
            }
        }
    }
}
