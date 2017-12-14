using System;
using System.Threading;
using System.Windows.Forms;

namespace TetsFormApp
{
    public partial class Form1 : Form
    {
        Search search;
        private Thread seachThread;

        public Form1()
        {

            search = new Search();
            seachThread = new Thread(() =>
            {
                search.SearchNow(folderBrowserDialog1.SelectedPath, "");
                taskdone();
            });

            InitializeComponent();
            label1.DataBindings.Add("Text",
                                        search,
                                        "Output",
                                        true,
                                        DataSourceUpdateMode.OnPropertyChanged);

            listBox1.DataBindings.Add("DataSource", search, "Found", true,
                DataSourceUpdateMode.OnPropertyChanged);
            listBox2.DataBindings.Add("DataSource", search, "NotSerached", true,
                DataSourceUpdateMode.OnPropertyChanged);

            textBox2.Text = "Søgeord";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Anders Søgeland";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            choosefolder();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void choosefolder()
        {
            if (seachThread.IsAlive)
            {
                seachThread.Abort();
            }
            if (string.IsNullOrEmpty(textBox2.Text) || textBox2.Text == "Søgeord")
            {
                MessageBox.Show("Skriv venligst  et søgeord");
                return;
            }

            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                if (checkBox2.Checked)
                {
                    seachThread = new Thread(() =>
                    {
                        search.SearchNow(folderBrowserDialog1.SelectedPath, textBox2.Text);
                        taskdone();
                    });

                }
                else
                {
                    seachThread = new Thread(() =>
                    {
                        search.SearchNow(folderBrowserDialog1.SelectedPath, textBox2.Text.ToLower());
                        taskdone();
                    });

                }

                seachThread.Start();

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (seachThread.IsAlive)
            {
                seachThread.Abort();
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Søgeord")
            {
                textBox2.Text = "";
            }

        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Søgeord";
            }
        }


        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                if (listBox1.SelectedItem.ToString().Length != 0)
                    if (checkBox1.Checked)
                    {
                        System.Diagnostics.Process.Start(listBox1.SelectedItem.ToString());

                    }
                    else
                    {
                        Clipboard.SetText(listBox1.SelectedItem.ToString());
                        MessageBox.Show("Filens sti er nu kopieret til udklipsholderen");
                    }

        }

        private void listbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (listBox1.SelectedItem != null)
                    if (listBox1.SelectedItem.ToString().Length != 0)
                        if (checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start(listBox1.SelectedItem.ToString());

                        }
                        else
                        {
                            Clipboard.SetText(listBox1.SelectedItem.ToString());
                            MessageBox.Show("Filens sti er nu kopieret til udklipsholderen");
                        }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            search.casesensitive = checkBox2.Checked;
        }

        private void taskdone()
        {
            MessageBox.Show("Søgningen er nu færdig og tog " + (DateTime.Now - search.Begintime).Seconds + " sekunder");

        }
    }
}
