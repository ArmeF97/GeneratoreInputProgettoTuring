using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        Thread t_gen_automatico = null;

        Random x;
        List<string> L;
        string path;

        int n_char = 0;

        public decimal soglia = 0;

        public List<int> stati_accettati = null;

        private void Button1_Click(object sender, EventArgs e)
        {
            Genera2(false,1);
        }

        private void Genera2(bool v, int quanti)
        {
            bool fatto = false;
            decimal mille = 1000M;
            soglia = numericUpDown19.Value / mille;
            for (int i = 0; i < quanti; i++)
            {
                string name = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + ".txt";
                fatto = Genera(name, v);
            }
            if (fatto)
                MessageBox.Show("Generati ("+quanti.ToString()+")!");
        }

        private bool Genera(string name, bool piu_randomico)
        {
            L = new List<string>
            {
                "tr"
            };

            int a5 = (int)Pezzo_Random(piu_randomico, numericUpDown9.Value);

            int max_char = 'z' - 'a';
            if (a5 > max_char)
            {
                a5 = max_char - 1;
            }

            int n_stati = RandFixed((int)Pezzo_Random(piu_randomico, numericUpDown1.Value),(int)Pezzo_Random(piu_randomico, numericUpDown2.Value));
            int n_acc = RandFixed((int)Pezzo_Random(piu_randomico, numericUpDown3.Value), (int)Pezzo_Random(piu_randomico, numericUpDown4.Value));
            int n_transizioni = RandFixed((int)Pezzo_Random(piu_randomico, numericUpDown6.Value), (int)Pezzo_Random(piu_randomico, numericUpDown7.Value));
            n_char = RandFixed((int)Pezzo_Random(piu_randomico, numericUpDown8.Value), a5);
            int n_run = RandFixed((int)Pezzo_Random(piu_randomico, numericUpDown10.Value), (int)Pezzo_Random(piu_randomico, numericUpDown11.Value));
            int max = RandFixed((int)Pezzo_Random(piu_randomico, numericUpDown12.Value), (int)Pezzo_Random(piu_randomico, numericUpDown13.Value));

            Metti_A_Uno(ref n_stati);
            Metti_A_Uno(ref n_acc);
            Metti_A_Uno(ref n_transizioni);
            Metti_A_Uno(ref n_char);
            Metti_A_Uno(ref n_run);
            Metti_A_Uno(ref max);

            if (n_stati <= n_acc)
                n_stati = n_acc * 2 + 2;
            else if (n_stati <= n_acc * 2 + 2)
                n_stati = n_acc * 3 + 3;

            n_run += 10;
            n_run = (int)(n_run * 1.5);
            n_run += 2000;
            n_run = (int)(n_run * 1.5);
            n_run += 2000;

            if (n_acc>n_stati)
            {
                //nota: è praticamente impossibile che questo accada, visto il codice sopra
                MessageBox.Show("Gli stati accettati non possono superare quelli esistenti!");
                return false;
            }

            if (n_char > 25)
            {
                n_char = 25;
            }
            else if (n_char<3)
            {
                n_char = 3;
            }

            //ABBIAMO FINITO DI PERFEZIONARE I VALORI, ORA SI INIZIA

            DefinisciAccettazione(n_acc, n_stati);

            RiempiTransizioni(n_transizioni, n_stati);

            L.Add("acc");

            RiempiAccettazione();

            L.Add("max");
            L.Add(max.ToString());

            L.Add("run");

            RiempiRun(n_run);

            Scrivi_File(path + name);



            return true;
        }

        private void Scrivi_File(string v)
        {
            int r = x.Next(0, 1000);
            if (r > 500)
            {
                File.WriteAllLines(v, L);
            }
            else
            {
                FileStream f = null;
                try
                {
                    f = File.OpenWrite(v);

                    byte[] n = new byte[1];
                    n[0] = (byte)'\n';

                    for (int i = 0; i < L.Count - 1; i++)
                    {
                        byte[] b1 = String_To_B_Array(L[i]);
                        f.Write(b1, 0, b1.Length);
                        f.Write(n, 0, 1);
                    }

                    byte[] b2 = String_To_B_Array(L[L.Count - 1]);
                    f.Write(b2, 0, b2.Length);

                    f.Close();
                }
                catch
                {
                    throw new IOException();
                }
            }
        }

        private static byte[] String_To_B_Array(string v)
        {
            return Encoding.ASCII.GetBytes(v);
        }

        private static void Metti_A_Uno(ref int n)
        {
            if (n < 2)
                n = 2;
        }

        private int RandFixed(int v1, int v2)
        {
            if (v1 == v2)
                return v1;
            if (v1>v2)
            {
                return x.Next(v2, v1);
            }
            return x.Next(v1, v2);
        }

        private decimal Pezzo_Random(bool piu_randomico, decimal value)
        {
            if (piu_randomico == false)
                return value;

            int lato = x.Next(0, 1000);
            if (lato > 500)
            {
                decimal r2 = (decimal)x.NextDouble();
                r2 *= soglia;
                return value + (value*r2);
            }
            else
            {
                decimal r3 = (decimal)x.NextDouble();
                return value - (value * r3);
            }
        }


        private void DefinisciAccettazione(int n_acc, int n_stati)
        {
            stati_accettati = new List<int>();
            for (int i = 0; i < n_acc; i++)
            {
                int r = x.Next(0, n_stati - 1);
                int f = stati_accettati.IndexOf(r);
                if (f >= 0 && f < stati_accettati.Count || r == 0)
                {
                    i--;
                }
                else
                {
                    stati_accettati.Add(r);
                }
            }
        }

        private void RiempiRun(int n_run)
        {
            int ripetizioni2 = n_run;
            List<string> LT = new List<string>();
            int volte = 0;
            int l1 = (int)Pezzo_Random(true, numericUpDown14.Value);
            int l2 = (int)Pezzo_Random(true, numericUpDown15.Value);
            if (l1 < 1)
                l1 = 1;
            if (l2 < 1)
                l2 = 1;

            while (ripetizioni2 > 0 && volte<100)
            {
                for (int i = 0; i < ripetizioni2; i++)
                {
                    string s = "";
                    int l_run = RandFixed(l1, l2);
                    for (int j = 0; j < l_run; j++)
                    {
                        s += CarattereRandom(true);
                    }
                    LT.Add(s);
                }

                /*
                ripetizioni2 = Ripetizioni(ref LT);
                if (ripetizioni2 > 0)
                {
                    Console.WriteLine(ripetizioni2 + " ripetizioni run");
                }
                */
                ripetizioni2 = 0;

                volte++;
            }

            foreach (string st in LT)
            {
                L.Add(st);
            }
        }

        private char CarattereRandom(bool test = false)
        {
            int soglia = 0; 
            if (test)
                soglia = (int)Pezzo_Random(true, numericUpDown16.Value);
            else
                soglia = (int)Pezzo_Random(true, numericUpDown17.Value);

            if (soglia < 0)
                soglia = 0;

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

        private void RiempiAccettazione()
        {
            foreach (var y in stati_accettati)
            {
                L.Add(y.ToString());
            }
        }

        private void RiempiTransizioni(int n_transizioni, int n_stati)
        {
            List<string> LT = new List<string>();
            for (int i=0; i<n_transizioni; i++)
            {
                int a1 = x.Next(0, n_stati - 1);
                char a2 = CarattereRandom();
                char a3 = CarattereRandom();
                char a4 = SpostamentoRandom();
                int a5 = x.Next(0, n_stati - 1);
                string s = a1 + "  " + a2 + "  " + a3 + "  " + a4 + "  " + a5;
                bool acc = IsAccettazione(a1);
                if (acc)
                {
                    i--;
                }
                else
                {
                    LT.Add(s);
                }
            }

            if (checkBox3.Checked)
            {
                int q1 = x.Next(1, 3);
                for (int j = 0; j < q1; j++)
                {
                    for (int i = 0; i < n_stati; i++)
                    {
                        int do2 = x.Next(1, 100);
                        if (do2 <= numericUpDown5.Value)
                        {
                            int a1 = i;
                            char a2 = CarattereRandom();
                            char a3 = CarattereRandom();
                            char a4 = SpostamentoRandom();
                            int a5 = i;
                            string s = a1 + "  " + a2 + "  " + a3 + "  " + a4 + "  " + a5;
                            bool acc = IsAccettazione(a1);
                            if (acc)
                            {
                                i--;
                            }
                            else
                            {
                                LT.Add(s);
                            }
                        }
                    }
                }
            }

            if (checkBox2.Checked)
            {
                for (int i = 0; i < n_stati; i++)
                {
                    int a1 = x.Next(0, n_stati - 1);
                    char a2 = CarattereRandom();
                    char a3 = CarattereRandom();
                    char a4 = SpostamentoRandom();
                    int a5 = i;
                    string s = a1 + "  " + a2 + "  " + a3 + "  " + a4 + "  " + a5;
                    bool acc = IsAccettazione(a1);
                    if (acc)
                    {
                        i--;
                    }
                    else
                    {
                        LT.Add(s);
                    }
                }
            }

            if (checkBox1.Checked)
            {
                for (int i = 0; i < n_stati; i++)
                {
                    int a1 = i;
                    char a2 = CarattereRandom();
                    char a3 = CarattereRandom();
                    char a4 = SpostamentoRandom();
                    int a5 = x.Next(0, n_stati - 1);
                    string s = a1 + " " + a2 + "  " + a3 + "  " + a4 + "  " + a5;
                    bool acc = IsAccettazione(a1);
                    if (acc)
                    {
                        ;
                    }
                    else
                    {
                        LT.Add(s);
                    }
                }
            }

            int ripetizioni2 = Ripetizioni(ref LT);
            if (ripetizioni2>0)
            {
                Console.WriteLine(ripetizioni2 + " ripetizioni transizioni");
            }
            foreach (string st in LT)
            {
                L.Add(st);
            }
        }

        public static int Ripetizioni(ref List<string> LT)
        {
            int r = 0;

            for (int i=0; i<LT.Count; i++)
            {
                int q = QuanteVolte(LT, i);
                if (q>1)
                {
                    LT.RemoveAt(i);
                    i--;
                    r++;
                }
            }

            return r;
        }

        private static int QuanteVolte(List<string> LT, int n)
        {
            int q = 0;
            for (int i = 0; i < LT.Count; i++)
            {
                if (LT[i] == LT[n])
                    q++;
            }
            return q;
        }

        private bool IsAccettazione(int n)
        {
            int r = stati_accettati.IndexOf(n);
            if (r >= 0 && r < stati_accettati.Count)
                return true;
            return false;
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
            path = @"C:\Users\Arme\Downloads\Telegram Desktop\SimulatoreMacchinaTuringND\SimulatoreMacchinaTuringND\bin\Debug\input\";
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
            numericUpDown1.Value = 150;
            numericUpDown2.Value = 250;

            //n_accettati
            numericUpDown3.Value = 1;
            numericUpDown4.Value = 2;

            //check_transizioni
            checkBox1.Checked = true;
            checkBox2.Checked = true;

            //probabilità autoanello
            checkBox3.Checked = true;
            numericUpDown5.Value = 85;

            //numero transizioni extra
            numericUpDown6.Value = 400;
            numericUpDown7.Value = 500;

            //numero caratteri usati
            numericUpDown8.Value = 4;
            numericUpDown9.Value = 6;

            //numero stringhe di input
            numericUpDown10.Value = 25;
            numericUpDown11.Value = 30;

            //numero di mosse massine
            numericUpDown12.Value = 50000;
            numericUpDown13.Value = 80000;

            //lunghezza nastro di input
            numericUpDown14.Value = 100;
            numericUpDown15.Value = 300;

            //probabilità di _ nelle stringhe di input (su 1000 massimo)
            numericUpDown16.Value = 15;
            numericUpDown16.Minimum = 0;
            numericUpDown16.Maximum = 1000;

            //probabilità di _ nelle stringhe di transizione
            numericUpDown17.Value = 35;
            numericUpDown17.Minimum = 0;
            numericUpDown17.Maximum = 1000;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Process.Start(path);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (t_gen_automatico == null)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Genera3));
                t.Start(null);
                t_gen_automatico = t;
                button3.Enabled = false;
                button4.Enabled = true;
            }
            else
            {
                MessageBox.Show("E' già in corso qualcosa! Killalo");
            }

        }

        private void Genera3(object a)
        {
            Genera2(true, (int)numericUpDown18.Value);
        }

        private void TabPage2_Click(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                t_gen_automatico.Abort();
            }
            catch
            {
                ;
            }
            t_gen_automatico = null;
        }


    }
}
