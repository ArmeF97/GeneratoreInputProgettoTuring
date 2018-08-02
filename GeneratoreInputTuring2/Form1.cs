using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneratoreInputTuring2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random x;
        List<string> L;
        string path;

        private void Button1_Click(object sender, EventArgs e)
        {
            string name = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".txt";
            bool fatto = Genera(path, name);
            if (fatto)
                MessageBox.Show("Generato!");
        }

        private bool Genera(string path, string name)
        {
            L = new List<string>
            {
                "tr"
            };

            int n_stati = x.Next((int)numericUpDown1.Value,(int)numericUpDown2.Value);
            int n_acc = x.Next((int)numericUpDown3.Value,(int)numericUpDown4.Value);
            int n_transizioni = x.Next((int)numericUpDown6.Value, (int)numericUpDown7.Value);
            int n_char = x.Next((int)numericUpDown8.Value, (int)numericUpDown9.Value);
            int n_run = x.Next((int)numericUpDown10.Value, (int)numericUpDown11.Value);
            int max = x.Next((int)numericUpDown12.Value, (int)numericUpDown13.Value);

            if (n_acc>n_stati)
            {
                MessageBox.Show("Gli stati accettati non possono superare quelli esistenti!");
                return false;
            }

            int max_char = 'z' - 'a';
            if ((int)numericUpDown9.Value>max_char)
            {
                MessageBox.Show("Massimo " + max_char.ToString() + " caratteri.");
                return false;
            }


            RiempiTransizioni(n_transizioni, n_stati, n_char);

            L.Add("acc");

            RiempiAccettazione(n_acc, n_stati);

            L.Add("max");
            L.Add(max.ToString());

            L.Add("run");

            RiempiRun(n_run, n_char);

            File.WriteAllLines(path + name, L);

            return true;
        }

        private void RiempiRun(int n_run, int n_char)
        {
            for (int i=0; i<n_run;i++)
            {
                string s = "";
                int l_run = x.Next((int)numericUpDown14.Value,(int)numericUpDown15.Value);
                for (int j=0; j<l_run; j++)
                {
                    s += CarattereRandom(n_char,true);
                }
                L.Add(s);
            }
        }

        private char CarattereRandom(int n_char, bool test = false)
        {
            int soglia = (int)numericUpDown17.Value;
            if (test)
                soglia = (int)numericUpDown16.Value;

            int r2 = x.Next(0, 999);
            if (r2<soglia)
            {
                return '_';
            }

            int r = x.Next(0, n_char - 1);
            int c1 = (int)'a';
            int c2 = c1 + r;
            char c = (char)c2;
            return c;
        }

        private void RiempiAccettazione(int n_acc, int n_stati)
        {
            List<int> L2 = new List<int>();
            for (int i=0; i<n_acc; i++)
            {
                int r = x.Next(0, n_stati-1);
                int f = L2.IndexOf(r);
                if (f>=0 && f<L2.Count)
                {
                    i--;
                }
                else
                {
                    L2.Add(r);
                }
            }
            foreach (var y in L2)
            {
                L.Add(y.ToString());
            }
        }

        private void RiempiTransizioni(int n_transizioni, int n_stati, int n_char)
        {
            for (int i=0; i<n_transizioni; i++)
            {
                int a1 = x.Next(0, n_stati - 1);
                char a2 = CarattereRandom(n_char);
                char a3 = CarattereRandom(n_char);
                char a4 = SpostamentoRandom();
                int a5 = x.Next(0, n_stati - 1);
                string s = a1 + " " + a2 + " " + a3 + " " + a4 + " " + a5;
                L.Add(s);
            }

            if (checkBox3.Checked)
            {
                for (int i = 0; i < n_stati; i++)
                {
                    int do2 = x.Next(1, 100);
                    if (do2 <= numericUpDown5.Value)
                    {
                        int a1 = i;
                        char a2 = CarattereRandom(n_char);
                        char a3 = CarattereRandom(n_char);
                        char a4 = SpostamentoRandom();
                        int a5 = i;
                        string s = a1 + " " + a2 + " " + a3 + " " + a4 + " " + a5;
                        L.Add(s);
                    }
                }
            }

            if (checkBox2.Checked)
            {
                for (int i = 0; i < n_stati; i++)
                {
                    int a1 = x.Next(0, n_stati - 1);
                    char a2 = CarattereRandom(n_char);
                    char a3 = CarattereRandom(n_char);
                    char a4 = SpostamentoRandom();
                    int a5 = i;
                    string s = a1 + " " + a2 + " " + a3 + " " + a4 + " " + a5;
                    L.Add(s);
                }
            }

            if (checkBox1.Checked)
            {
                for (int i = 0; i < n_stati; i++)
                {
                    int a1 = i;
                    char a2 = CarattereRandom(n_char);
                    char a3 = CarattereRandom(n_char);
                    char a4 = SpostamentoRandom();
                    int a5 = x.Next(0, n_stati - 1);
                    string s = a1 + " " + a2 + " " + a3 + " " + a4 + " " + a5;
                    L.Add(s);
                }
            }
        }

        private char SpostamentoRandom()
        {
            int a = x.Next(0, 2);
            if (a == 0)
                return 'S';
            if (a == 1)
                return 'L';
            return 'R';
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            path = "C:\\git\\ProgettoApiTuring\\PossibiliInput\\";
            x = new Random();

            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = 1020 * 2100 * 1002;
            numericUpDown2.Minimum = numericUpDown1.Minimum;
            numericUpDown2.Maximum = numericUpDown1.Maximum;
            numericUpDown3.Minimum = numericUpDown1.Minimum;
            numericUpDown3.Maximum = numericUpDown1.Maximum;
            numericUpDown4.Minimum = numericUpDown1.Minimum;
            numericUpDown4.Maximum = numericUpDown1.Maximum;
            numericUpDown6.Minimum = numericUpDown1.Minimum;
            numericUpDown6.Maximum = numericUpDown1.Maximum;
            numericUpDown7.Minimum = numericUpDown1.Minimum;
            numericUpDown7.Maximum = numericUpDown1.Maximum;
            numericUpDown8.Minimum = numericUpDown1.Minimum;
            numericUpDown8.Maximum = numericUpDown1.Maximum;
            numericUpDown9.Minimum = numericUpDown1.Minimum;
            numericUpDown9.Maximum = numericUpDown1.Maximum;
            numericUpDown10.Minimum = numericUpDown1.Minimum;
            numericUpDown10.Maximum = numericUpDown1.Maximum;
            numericUpDown11.Minimum = numericUpDown1.Minimum;
            numericUpDown11.Maximum = numericUpDown1.Maximum;
            numericUpDown12.Minimum = numericUpDown1.Minimum;
            numericUpDown12.Maximum = numericUpDown1.Maximum;
            numericUpDown13.Minimum = numericUpDown1.Minimum;
            numericUpDown13.Maximum = numericUpDown1.Maximum;
            numericUpDown14.Minimum = numericUpDown1.Minimum;
            numericUpDown14.Maximum = numericUpDown1.Maximum;
            numericUpDown15.Minimum = numericUpDown1.Minimum;
            numericUpDown15.Maximum = numericUpDown1.Maximum;

            //n_stati
            numericUpDown1.Value = 1000;
            numericUpDown2.Value = 2000;

            //n_accettati
            numericUpDown3.Value = 3;
            numericUpDown4.Value = 5;

            //check_transizioni
            checkBox1.Checked = true;
            checkBox2.Checked = true;

            //probabilità autoanello
            checkBox3.Checked = true;
            numericUpDown5.Value = 50;

            //numero transizioni extra
            numericUpDown6.Value = 5000;
            numericUpDown7.Value = 7000;

            //numero caratteri usati
            numericUpDown8.Value = 4;
            numericUpDown9.Value = 8;

            //numero stringhe di input
            numericUpDown10.Value = 25;
            numericUpDown11.Value = 30;

            //numero di mosse massine
            numericUpDown12.Value = 10000;
            numericUpDown13.Value = 40000;

            //lunghezza nastro di input
            numericUpDown14.Value = 100;
            numericUpDown15.Value = 10000;

            //probabilità di _ nelle stringhe di input (su 1000 massimo)
            numericUpDown16.Value = 10;
            numericUpDown16.Minimum = 0;
            numericUpDown16.Maximum = 1000;

            //probabilità di _ nelle stringhe di transizione
            numericUpDown17.Value = 30;
            numericUpDown17.Minimum = 0;
            numericUpDown17.Maximum = 1000;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Process.Start(path);
        }
    }
}
