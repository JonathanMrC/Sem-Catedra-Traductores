using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form : System.Windows.Forms.Form
    {
        Queue<token> tokens;
        HashSet<string> tipodato;
        Dictionary<string, int> reservadas;
        Dictionary<string, int> c_reservados;

        public Form()
        {
            c_reservados = new Dictionary<string, int> { { ";", 2 }, { ",", 3 }, { "(", 4 }, { ")", 5 }, { "{", 6 }, { "}", 7 } };
            reservadas = new Dictionary<string, int>{{ "if", 9 }, { "while", 10 }, { "return", 11 }, { "else", 12 } };
            tipodato = new HashSet<string> { "int", "float", "char", "void" };
            tokens = new Queue<token>();
            InitializeComponent();
            Formatodgv();
        }
        private void BtnAnalizar_Click(object sender, EventArgs e)
        {
            Formatodgv();
            AnalizadorLexico(txtbox.Text + "$");
            AgregarCont();
        }
        void AgregarCont()
        {
            while(tokens.Count > 0)
            {
                token t = tokens.Dequeue();
                int pos = dgv.Columns.Add("", "");
                dgv[pos, 0].Value = t.Lexema;
                dgv[pos, 1].Value = t.Id;
                dgv[pos, 2].Value = t.Nombre;
            }
            dgv.Refresh();
        }
        void AnalizadorLexico(string cadena)
        {
            string cad_act = "";
            int act = 0, e = 0;
            while(act < cadena.Length)
            {
                char c = cadena[act++];
                cad_act += c;
                if (e == 0)
                {
                    if (c == ' ' || c == '\r' || c == '\n') cad_act = "";
                    else if (c_reservados.ContainsKey(cad_act)) EstadoFinal(cad_act, ref cad_act, c_reservados[cad_act], ref e);
                    else if (EsLetra(c, 0) || c == '_') e = 1;
                    else if (EsNum(c)) e = 2;
                    else if (c == '-' || c == '+') EstadoFinal("OpSuma", ref cad_act, 14, ref e);
                    else if (c == '*' || c == '/') EstadoFinal("OpMultiplicacion", ref cad_act, 16, ref e);
                    else if (cad_act == "||" || cad_act == "&&") EstadoFinal("OpLogico", ref cad_act, 15, ref e);
                    else if (c == '=') e = 3;
                    else if (c == '<' || c == '>') e = 5;
                    else if (c == '!') e = 6;
                    else if (c == '$') tokens.Enqueue(new token("Fin", "$", 18));
                    else EstadoFinal("Error", ref cad_act, -1, ref e);
                }
                else if (e == 1)
                {
                    if (!EsLetra(c, 0) && c != '_' && !EsNum(c))
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        if (reservadas.ContainsKey(cad_act)) EstadoFinal(cad_act, ref cad_act, reservadas[cad_act], ref e);
                        else if (tipodato.Contains(cad_act)) EstadoFinal(cad_act, ref cad_act, 0, ref e);
                        else EstadoFinal("Id", ref cad_act, 1, ref e);
                    }
                }
                else if (e == 2)
                {
                    if (c == '.') e = 4;
                    else if(!EsNum(c))
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        EstadoFinal("Constante", ref cad_act, 13, ref e);
                    }
                }
                else if (e == 3)
                {
                    if (c == '=') EstadoFinal("OpRelacional", ref cad_act, 17, ref e);
                    else
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        EstadoFinal("=", ref cad_act, 8, ref e);
                    }
                }
                else if (e == 4)
                {
                    if (EsNum(c)) e = 7;
                    else 
                    {
                        act-=2;
                        cad_act = cad_act.Substring(0, cad_act.Length - 2);
                        EstadoFinal("Constante", ref cad_act, 13, ref e);
                    }
                }
                else if(e == 5)
                {
                    if (c == '=') EstadoFinal("OpRelacional", ref cad_act, 17, ref e);
                    else
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        EstadoFinal("OpRelacional", ref cad_act, 17, ref e);
                    }
                }
                else if(e == 6)
                {
                    if (c == '=') EstadoFinal("OpRelacional", ref cad_act, 17, ref e);
                    else
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        EstadoFinal("Error", ref cad_act, -1, ref e);
                    }
                }
                else if(e == 7)
                {
                    if (!EsNum(c))
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        EstadoFinal("Constante", ref cad_act, 17, ref e);
                    }
                }
                else
                {
                    MessageBox.Show("Error estado no encontrado, cadena actual: "+cad_act);
                    return;
                }
            }
        }
        void EstadoFinal(string nombre,ref string lexema, int id, ref int e)
        {
            e = 0;
            tokens.Enqueue(new token(nombre, lexema, id));
            lexema = "";
            return;
        }
        bool EsLetra(char x, int opc)//opc < 0 solo minusculas, opc > 0 solo mayus, opc == 0 ambas
        {
            bool min = false, may = false;
            if (x >= 'a' && x <= 'z') min = true;
            if (opc < 0) return min;
            if (x >= 'A' && x <= 'Z') may = true;
            if (opc > 0) return may;
            return min || may;
        }
        bool EsNum(char x)
        {
            return (x >= '0' && x <= '9');
        }
        void Formatodgv()
        {
            dgv.Columns.Clear();
            dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
            dgv.Columns.Add("", "");
            dgv.Rows.Add("Lexema");
            dgv.Rows.Add("Id");
            dgv.Rows.Add("Nombre");
            dgv.Refresh();
        }
    }
}
