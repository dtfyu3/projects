using Cyotek.GhostScript.PdfConversion;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace pdf
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            
            InitializeComponent();
            //EmbeddedAssembly.Load("MyApp.gsdll32.dll", "gsdll32.dll");
            this.bwForm = new BackgroundWorker();
            this.bwForm.DoWork += new DoWorkEventHandler(this.BwForm_DoWork);
            this.bwForm.ProgressChanged += new ProgressChangedEventHandler(this.BwForm_ProgressChanged);
            this.bwForm.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BwForm_RunWorkerCompleted);
        }
        private BackgroundWorker bwForm;
        Pdf2Image pdf;
        string outputFileName = "";
        string path;
        string i1n = "";
        int firstpage = 1;
        int lastpage = 0;
        int counter = 0;
        int dpi = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                label3.Text = "";
                progressBar1.Value = 0;
                Regex reg = new Regex(@"^([^а-яА-Я]+)$");
                if (!reg.IsMatch(openFileDialog1.FileName))
                    MessageBox.Show("Полный путь к файлу не должен содержать кириллицы", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    button2.Enabled = true;
                    i1n = openFileDialog1.FileName;
                    textBox3.Text = i1n;
                    pdf = new Pdf2Image(i1n);
                    pdf.Settings.ImageFormat = Cyotek.GhostScript.ImageFormat.Png24;
                    textBox2.Text = $"{pdf.PageCount}";
                    textBox1.Text = "1";
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpi = Convert.ToInt32(comboBox1.SelectedItem);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Text = "";
            textBox3.Enabled = false;
            textBox4.Text = "";
            textBox4.Enabled = false;
            comboBox1.Enabled = false;
            bwForm.WorkerReportsProgress = true;
            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Regex reg = new Regex(@"^([^а-яА-Я]+)$");
                if (!reg.IsMatch(folderBrowserDialog1.SelectedPath))
                    MessageBox.Show("Путь к директории не должен содержать кириллицы", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    button3.Enabled = true;
                    path = folderBrowserDialog1.SelectedPath;
                    textBox4.Text = path;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    comboBox1.Enabled = true;
                }
            }
        }
        void BwForm_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = sender as BackgroundWorker;
            counter = firstpage;
            int y = 1;
            try
            {
                Disable();
                while (counter <= lastpage)
                {
                    outputFileName = path + $@"\{counter}.png";
                    double d = y * 100 / (lastpage - firstpage + 1);
                    int per = Convert.ToInt32(d);
                    pdf.ConvertPdfPageToImage(outputFileName, counter);
                    bgw.ReportProgress(per);
                    counter++;
                    y++;
                }

            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show($"Возникла ошибка\n{ex.Message}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Enable();
            }

        }
        void Enable()
        {
            button2.Enabled = true;
            button3.Enabled = true;
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            comboBox1.Enabled = true;
            button4.Enabled = true;
        }

        void Disable()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            comboBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }
        void BwForm_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
        void BwForm_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Enable();
            MessageBox.Show("Готово", "Сообщение");

        }
        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (Convert.ToInt32(textBox1.Text) <= Convert.ToInt32(textBox2.Text) && Convert.ToInt32(textBox2.Text) <= pdf.PageCount && Convert.ToInt32(textBox1.Text) != 0 && Convert.ToInt32(textBox2.Text) != 0)
                {

                    firstpage = Convert.ToInt32(textBox1.Text);
                    lastpage = Convert.ToInt32(textBox2.Text);
                    if (dpi != 0)
                    {
                        pdf.Settings.Dpi = dpi;
                        progressBar1.Visible = true;
                        bwForm.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Процесс будет запущен со значением dpi по умолчанию - 200", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bwForm.RunWorkerAsync();
                    }
                }
                else
                {
                    textBox1.Text = "1";
                    textBox2.Text = pdf.PageCount.ToString();
                    MessageBox.Show("Проверьте правильность ввода");
                }
            }
            else
            {
                textBox1.Text = "1";
                textBox2.Text = pdf.PageCount.ToString();
                MessageBox.Show("Проверьте правильность ввода");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (textBox1.Text == null || textBox1.Text == "")
            //{
            //    textBox1.Text = "1";
            //}
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //if (textBox2.Text == null || textBox2.Text == "")
            //{
            //    textBox2.Text = textBox2.Text = pdf.PageCount.ToString();
            //}
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {

        }

        private void comboBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (comboBox1.DropDownStyle != ComboBoxStyle.DropDownList)
            {
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox1.SelectedItem = "300";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
