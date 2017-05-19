using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excel_Accession
{
    public partial class Form1 : Form
    {
        static List<List<string>> SpeciesData;
        static bool seq1 = false;
        static bool seq2 = false;
        public Form1()
        {
            InitializeComponent();
            radioButton3.Checked = true;
            radioButton2.Checked = true;
            radioButton6.Checked = true;
            groupBox7.Enabled = false;
            groupBox1.Enabled = false;
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DNA_ALignment.Reset();
            Protein_Alignment.Reset();

            bool error = false;

            if (groupBox1.Enabled) //Accession#
            {
                string[] count = new string[2];
                count[0] = textBox1.Text;
                count[1] = textBox5.Text;

                // string[] count = textBox1.Text.Split(',', ' ');

                try
                {
                    if (radioButton1.Checked == true)   //Protein
                    {
                        getSeq(textBox1.Text + "," + textBox5.Text, "2");
                    }
                    else if (radioButton2.Checked == true)   //DNA
                    {
                        getSeq(textBox1.Text + "," + textBox5.Text, "1");
                    }
                }

                catch
                {
                    error = true;
                    MessageBox.Show("There is no such accession numbers!!");
                    textBox1.Clear();
                    textBox5.Clear();
                    button1.Enabled = false;
                }
            }
            else    //Manually
            {
                SpeciesData = new List<List<string>>();
                List<string> test = new List<string>();
                test.Add("");
                test.Add("");

                test.Add(textBox3.Text.ToUpper());

                SpeciesData.Add(test);

                test = new List<string>();
                test.Add("");
                test.Add("");
                test.Add(textBox4.Text.ToUpper());
                SpeciesData.Add(test);
            }

            if (!error)
            {
                if (radioButton6.Checked)//Local
                {
                    if (radioButton2.Checked)//DNA
                    {
                        Form2 obj = new Form2();
                        obj.Visible = true;
                        obj.DNALocal(SpeciesData, numericUpDown1.Text);
                    }

                    else //Protein
                    {
                        Form2 obj = new Form2();
                        obj.Visible = true;
                        obj.ProteinLocal(SpeciesData, numericUpDown1.Text);
                    }
                }

                else  //Global
                {
                    if (radioButton8.Checked)   //Linear
                    {
                        if (radioButton2.Checked) //DNA
                        {
                            Form2 obj = new Form2();
                            obj.Visible = true;
                            obj.DNAGlobalLinear(SpeciesData, numericUpDown2.Text);
                        }

                        else //Protein
                        {
                            Form2 obj = new Form2();
                            obj.Visible = true;
                            obj.ProteinGlobalLinear(SpeciesData, numericUpDown2.Text);
                        }
                    }

                    else  //Affine
                    {
                        if (radioButton2.Checked) //DNA
                        {
                            Form2 obj = new Form2();
                            obj.Visible = true;
                            obj.DNAAffine(SpeciesData, numericUpDown3.Text, numericUpDown2.Text);
                        }

                        else //Protein
                        {
                            Form2 obj = new Form2();
                            obj.Visible = true;
                            obj.ProteinAffine(SpeciesData, numericUpDown3.Text, numericUpDown2.Text);

                        }
                    }
                }
            }
        }
        public void getSeq(string AccN, string DBchoose)
        {
            SpeciesData = new List<List<string>>();

            WebClient client = new WebClient();
            List<char> acSub = new List<char>();
            List<string> ac = new List<string>();
            //List<string> seq = new List<string>();
            string accc = "";


            for (int i = 0; i < AccN.Length; i++)
            {
                if (AccN[i] != ',' && AccN[i] != ' ')
                {
                    acSub.Add(AccN[i]);
                }
                else if (AccN[i] == ',' || AccN[i] == ' ')
                {
                    accc = string.Join("", acSub.ToArray());
                    ac.Add(accc);
                    acSub.Clear();
                    accc = "";
                }
            }
            accc = string.Join("", acSub.ToArray());
            ac.Add(accc);
            acSub.Clear();
            accc = "";

            for (int u = 0; u < ac.Count; u++)
            {
                if (u == 0)
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = 3;
                }

                else if (u == 1)
                {
                    progressBar2.Value = 0;
                    progressBar2.Maximum = 3;
                }
                List<string> test = new List<string>();
                string AccN2 = ac[u];
                int flags = Convert.ToInt32(DBchoose);
                string database = "";
                if (flags == 1)
                    database = "nuccore";
                else if (flags == 2)
                    database = "protein";

                if (u == 0)
                    progressBar1.Increment(+1);

                else if (u == 1)
                    progressBar2.Increment(+1);

                string url = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db="
                    + database + "&id=" + AccN2 + "&rettype=fasta&retmode=text";

                if (u == 0)
                    progressBar1.Increment(+1);

                else if (u == 1)
                    progressBar2.Increment(+1);

                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                string[] gene = s.Split('\n');
                s = "";
                for (int i = 1; i < gene.Length; i++)
                    s += gene[i];
                //seq.Add(gene[0]);
                //seq.Add(s);

                string[] name = gene[0].Split(' ');
                string Name = name[1];
                for (int i = 2; i < name.Length; i++)
                    Name += " " + name[i];

                test.Add(Name);
                test.Add(ac[u]);
                test.Add(s);
                SpeciesData.Add(test);

                if (u == 0)
                    progressBar1.Increment(+1);

                else if (u == 1)
                    progressBar2.Increment(+1);

            }

            //MessageBox.Show("Retreiving is successfully completed!!");

            //return seq;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox6.Enabled = true;

            if (textBox3.Text != "")
                seq1 = true;

            else seq1 = false;

            if (textBox4.Text != "")
                seq2 = true;

            else seq2 = false;

            if (seq1 && seq2)
                button1.Enabled = true;

            else button1.Enabled = false;

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox6.Enabled = false;

            if (textBox1.Text.Length >= 6 && textBox1.Text.Length <= 8)
                seq1 = true;

            else seq1 = false;

            if (textBox5.Text.Length >= 6 && textBox5.Text.Length <= 8)
                seq2 = true;

            else seq2 = false;

            if (seq1 && seq2)
                button1.Enabled = true;

            else button1.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

            groupBox7.Enabled = true;
            radioButton7.Checked = true;

            label7.Enabled = false;
            numericUpDown1.Enabled = false;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            label7.Enabled = true;
            numericUpDown1.Enabled = true;
            groupBox7.Enabled = false;

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            label6.Enabled = true;
            numericUpDown3.Enabled = true;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            label6.Enabled = false;
            numericUpDown3.Enabled = false;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                int n;
                bool isNumeric = int.TryParse(textBox3.Text[textBox3.Text.Length - 1].ToString(), out n);
                if (isNumeric)
                {
                    MessageBox.Show("Please, No numbers!!");
                    textBox3.Text = textBox3.Text.Substring(0, textBox3.Text.Length - 1);
                    if (textBox3.Text != "")
                        seq1 = true;
                }

                else
                {
                    if (radioButton2.Checked)
                    {
                        if (textBox3.Text[textBox3.Text.Length - 1] != 'a' && textBox3.Text[textBox3.Text.Length - 1] != 'A' && textBox3.Text[textBox3.Text.Length - 1] != 'C' && textBox3.Text[textBox3.Text.Length - 1] != 'c' && textBox3.Text[textBox3.Text.Length - 1] != 'T' && textBox3.Text[textBox3.Text.Length - 1] != 't' && textBox3.Text[textBox3.Text.Length - 1] != 'G' && textBox3.Text[textBox3.Text.Length - 1] != 'g')
                        {
                            MessageBox.Show("Please, only Nucleotides are allowed!!");
                            textBox3.Text = textBox3.Text.Substring(0, textBox3.Text.Length - 1);
                            if (textBox3.Text != "")
                                seq1 = true;
                        }
                        else seq1 = true;

                    }

                    else
                    {
                        if (textBox3.Text[textBox3.Text.Length - 1] == 'o' || textBox3.Text[textBox3.Text.Length - 1] == 'O' || textBox3.Text[textBox3.Text.Length - 1] == 'u' || textBox3.Text[textBox3.Text.Length - 1] == 'U')
                        {
                            MessageBox.Show("Please, only Amino acids are allowed!!");
                            textBox3.Text = textBox3.Text.Substring(0, textBox3.Text.Length - 1);
                            if (textBox3.Text != "")
                                seq1 = true;
                        }
                        else seq1 = true;

                    }
                }
            }

            else
            {
                seq1 = false;
            }

            if (seq1 && seq2)
                button1.Enabled = true;

            else button1.Enabled = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                int n;
                bool isNumeric = int.TryParse(textBox4.Text[textBox4.Text.Length - 1].ToString(), out n);
                if (isNumeric)
                {
                    MessageBox.Show("Please, No numbers!!");
                    textBox4.Text = textBox4.Text.Substring(0, textBox4.Text.Length - 1);
                    if (textBox4.Text != "")
                        seq2 = true;
                }

                else
                {
                    if (radioButton2.Checked)
                    {
                        if (textBox4.Text[textBox4.Text.Length - 1] != 'a' && textBox4.Text[textBox4.Text.Length - 1] != 'A' && textBox4.Text[textBox4.Text.Length - 1] != 'C' && textBox4.Text[textBox4.Text.Length - 1] != 'c' && textBox4.Text[textBox4.Text.Length - 1] != 'T' && textBox4.Text[textBox4.Text.Length - 1] != 't' && textBox4.Text[textBox4.Text.Length - 1] != 'G' && textBox4.Text[textBox4.Text.Length - 1] != 'g')
                        {
                            MessageBox.Show("Please, only Nucleotides are allowed!!");
                            textBox4.Text = textBox4.Text.Substring(0, textBox4.Text.Length - 1);
                            if (textBox4.Text != "")
                                seq2 = true;
                        }
                        else seq2 = true;

                    }

                    else
                    {
                        if (textBox4.Text[textBox4.Text.Length - 1] == 'o' || textBox4.Text[textBox4.Text.Length - 1] == 'O' || textBox4.Text[textBox4.Text.Length - 1] == 'u' || textBox4.Text[textBox4.Text.Length - 1] == 'U')
                        {
                            MessageBox.Show("Please, only Amino acids are allowed!!");
                            textBox4.Text = textBox4.Text.Substring(0, textBox4.Text.Length - 1);
                            if (textBox4.Text != "")
                                seq2 = true;
                        }
                        else seq2 = true;

                    }
                }
            }
            else
            {
                seq2 = false;
            }

            if (seq1 && seq2)
                button1.Enabled = true;

            else button1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;

            if (textBox1.Text.Length >= 6)
            {
                if (textBox1.Text.Length <= 8)
                {

                    if (radioButton2.Checked)
                    {
                        if (textBox1.Text.Length == 6)
                            seq1 = true;

                        else seq1 = false;
                    }
                    else
                    {
                        if (textBox1.Text.Length == 8)
                            seq1 = true;

                        else seq1 = false;
                    }
                }
                else
                {
                    if (radioButton2.Checked)
                        textBox1.Text = textBox1.Text.Substring(0, 6);

                    else textBox1.Text = textBox1.Text.Substring(0, 8);

                    seq1 = true;
                }
            }
            else seq1 = false;

            if (seq1 && seq2)
                button1.Enabled = true;

            else button1.Enabled = false;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            progressBar2.Value = 0;

            if (textBox5.Text.Length >= 6)
            {
                if (textBox5.Text.Length <= 8)
                {

                    if (radioButton2.Checked)
                    {
                        if (textBox5.Text.Length == 6)
                            seq2 = true;

                        else seq2 = false;
                    }
                    else
                    {
                        if (textBox5.Text.Length == 8)
                            seq2 = true;

                        else seq2 = false;
                    }
                }
                else
                {
                    if (radioButton2.Checked)
                        textBox5.Text = textBox5.Text.Substring(0, 6);

                    else textBox5.Text = textBox5.Text.Substring(0, 8);

                    seq2 = true;
                }
            }
            else seq2 = false;

            if (seq1 && seq2)
                button1.Enabled = true;

            else button1.Enabled = false;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value < numericUpDown2.Value)
            {
                MessageBox.Show("Gap open can't be < Gap extension!!");
                numericUpDown2.Value = numericUpDown3.Value;
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value < numericUpDown2.Value)
            {
                MessageBox.Show("Gap open can't be < Gap extension!!");
                numericUpDown2.Value = numericUpDown3.Value;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox1.Clear();
            textBox5.Clear();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox1.Clear();
            textBox5.Clear();
        }

    }
}
