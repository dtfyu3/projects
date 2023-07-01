using Cyotek.GhostScript.PdfConversion;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdf
{
    public partial class Form1 : Form
    {
        string path = "";
        Pdf2Image pdf;
        string outputFile = "";
        int i = 1;
        public Form1()
        {
            //скачать x32 gsdll, x64dll, установить 32 версию (Program files x86), установить 64 версию(Program files) и положить в эту папку x32 dll
            InitializeComponent();
            progressBar1.Style = ProgressBarStyle.Continuous;

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            progressBar1.Maximum = progressBar1.Maximum * (pdf.PageCount-1);
            progressBar1.Step = progressBar1.Maximum/ (pdf.PageCount - 1);
            pdf.Settings.ImageFormat = Cyotek.GhostScript.ImageFormat.Png24;
            while (i <= pdf.PageCount)
            {
                outputFile = outputFile + $@"\{i}.png";
                cl();
                progressBar1.PerformStep();
                int pos = outputFile.LastIndexOf('\\');
                outputFile = outputFile.Substring(0, pos);
            }
            MessageBox.Show("done");
            this.Close();
        }
        private async Task cl() {
            pdf.ConvertPdfPageToImage(outputFile, i);
            i++;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = true;
                path = openFileDialog1.FileName;
                pdf = new Pdf2Image(path);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                outputFile = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                button3.Enabled = true;
                pdf.Settings.Dpi = Convert.ToInt32(comboBox1.SelectedItem);
            }
        }
    }
}
