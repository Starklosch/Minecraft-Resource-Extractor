using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Minecraft_Resource_Extractor
{
    public partial class Form1 : Form
    {
        string indexFile;
        string[] files;

        string content = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            files = Directory.GetFiles(Program.INDEXS_PATH);

            string[] names = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i].Split('\\').Last();
                names[i] = file.Substring(0, file.IndexOf(".json"));
            }

            cmbIndex.Items.AddRange(names);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Program.OBJECTS_PATH) && cmbIndex.SelectedIndex > -1)
            {
                try
                {
                    indexFile = files[cmbIndex.SelectedIndex];

                    content = File.ReadAllText(indexFile);

                    btnExtract.Enabled = false;
                    btnCancel.Enabled = true;
                    backgroundWorker.RunWorkerAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.EXTRACT_PATH = folderBrowserDialog1.SelectedPath;
                tbOutputPath.Text = Program.EXTRACT_PATH;
            }
        }

        private void TbOutputPath_TextChanged(object sender, EventArgs e)
        {
            Program.EXTRACT_PATH = tbOutputPath.Text;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;

                Resource[] res;

                res = ExtractTask.GetResources(content);

                for (int i = 0; i < res.Length; i++)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        float newValue = (float)i / res.Length;

                        worker.ReportProgress((int)(newValue * 100));

                        string file = Program.OBJECTS_PATH + @"\" + res[i].hash.Substring(0, 2) + @"\" + res[i].hash;
                        
                        if (File.Exists(file))
                        {
                            ExtractTask.CopyFile(file, Program.EXTRACT_PATH + @"\" + res[i].name, true);
                        }
                    }
                    
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                progressBar1.Value = 100;
                MessageBox.Show("Completado!");

            }


            btnExtract.Enabled = true;
            btnCancel.Enabled = false;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();

            btnCancel.Enabled = false;

            MessageBox.Show("Cancelado :'(");
        }
    }
}
