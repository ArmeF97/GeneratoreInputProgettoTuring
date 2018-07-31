using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        const int MILLE = 1000;


        public Form1()
        {
            InitializeComponent();
        }
        Random x;
        List<string> L;
        private void Button1_Click(object sender, EventArgs e)
        {
            string path = "C:\\git\\ProgettoApiTuring\\PossibiliInput\\";

            string name = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".txt";
            Genera(path, name);
            MessageBox.Show("Generato!");
        }

        private void Genera(string path, string name)
        {
            L = new List<string>
            {
                "tr"
            };

            int n_stati = x.Next(90 * MILLE, 500 * MILLE);
            int n_transizioni = x.Next(5000 * MILLE, 6000 * MILLE);
            int n_char = x.Next(5, 10);
            RiempiTransizioni(n_transizioni, n_stati, n_char);
            L.Add("acc");
            int n_acc = x.Next(1000, 1500);
            RiempiAccettazione(n_acc, n_stati);
            L.Add("max");
            L.Add(x.Next(1000*MILLE,2000*MILLE).ToString());
            L.Add("run");
            int n_run = x.Next(10, 20);
            RiempiRun(n_run, n_char);

            File.WriteAllLines(path + name, L);
        }

        private void RiempiRun(int n_run, int n_char)
        {
            for (int i=0; i<n_run;i++)
            {
                string s = "";
                int l_run = x.Next(100, 10000);
                for (int j=0; j<l_run; j++)
                {
                    s += CarattereRandom(n_char);
                }
                L.Add(s);
            }
        }

        private char CarattereRandom(int n_char)
        {
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
            x = new Random();
        }
    }
}
