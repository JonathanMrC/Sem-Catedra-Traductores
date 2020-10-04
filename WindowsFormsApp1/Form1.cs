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
        Queue<KeyValuePair<string, string> > tokens;
        string cadena;

        public Form()
        {
            tokens = new Queue<KeyValuePair<string, string>>();
            InitializeComponent();
            dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
        }
        private void BtnAnalizar_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, string> pairT;
            dgv.Rows.Clear();                                                   //limpiar el datagridview
            cadena = txtborigen.Text+"$";
            AnalizadorL();
            //agregar las tokens al grid
            while(tokens.Count() > 0)
            {
                int temp = dgv.Rows.Count;
                dgv.Rows.Add();
                pairT = tokens.Dequeue();
                dgv[0, temp].Value = pairT.Key;
                dgv[1, temp].Value = pairT.Value;
            }
        }
        bool EsLetra(char x, int opc)//opc = 0 solo minusculas, opc = 1 solo mayus, opc > 1 ambas
        {
            bool min = false, may = false;
            if (x >= 'a' && x <= 'z') min = true;
            if (opc == 0) return min;
            if (x >= 'A' && x <= 'Z') may = true;
            if (opc == 1) return may;
            return min || may;
        }
        bool EsNum(char x)
        {
            return (x >= '0' && x <= '9');
        }


        void AnalizadorL()
        {
            char c = cadena[0];
            int e = 0, act = 1, fin = 2;
            string creada = "";
            while (fin > 0)
            {
                if(e == 0)
                {
                    creada = "";
                    if (c == 'i') e = 1;
                    else if (c == 'f') e = 5;
                    else if (c == 'c') e = 10;
                    else if (c == 'v') e = 14;
                    else if (c == 'w') e = 19;
                    else if (c == 'r') e = 24;
                    else if (c == 'e') e = 30;
                    else if (EsLetra(c, 2) || c == '_') e = 18;
                    else if (EsNum(c)) e = 34;
                    else if (c == '+' || c == '-') e = 36;
                    else if (c == '*' || c == '/') e = 37;
                    else if (c == ';') e = 46;
                    else if (c == ',') e = 47;
                    else if (c == '(') e = 48;
                    else if (c == ')') e = 49;
                    else if (c == '{') e = 50;
                    else if (c == '}') e = 51;
                    else if (c == '=') e = 52;
                    else if (c == '!') e = 38;
                    else if (c == '<' || c == '>') e = 40;
                    else if (c == '|') e = 42;
                    else if (c == '&') e = 44;
                    else if (c == ' ' || c == '\r' || c == '\n') e = 0;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("error", "" + c));
                    }
                }
                else if (e == 1)
                {
                    if (c == 'f') e = 2;
                    else if (c == 'n') e = 3;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 2)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("9", creada));
                    act--;
                    e = 0;
                }
                else if (e == 3)
                {
                    if (c == 't') e = 4;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 4)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("0", creada));
                    act--;
                    e = 0;
                }
                else if (e == 5)
                {
                    if (c == 'l') e = 6;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 6)
                {
                    if (c == 'o') e = 7;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 7)
                {
                    if (c == 'a') e = 8;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 8)
                {
                    if (c == 't') e = 9;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 9)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("0", creada));
                    act--;
                    e = 0;
                }
                else if (e == 10)
                {
                    if (c == 'h') e = 11;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 11)
                {
                    if (c == 'a') e = 12;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 12)
                {
                    if (c == 'r') e = 13;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 13)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("0", creada));
                    act--;
                    e = 0;
                }
                else if (e == 14)
                {
                    if (c == 'o') e = 15;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 15)
                {
                    if (c == 'i') e = 16;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 16)
                {
                    if (c == 'd') e = 17;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 17)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("0", creada));
                    act--;
                    e = 0;
                }
                else if (e == 18)
                {
                    if (!EsLetra(c, 2) && !EsNum(c) && c != '_')
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        act--;
                        e = 0;
                    }
                }
                else if (e == 19)
                {
                    if (c == 'h') e = 20;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 20)
                {
                    if (c == 'i') e = 21;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 21)
                {
                    if (c == 'l') e = 22;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 22)
                {
                    if (c == 'e') e = 23;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 23)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("10", creada));
                    act--;
                    e = 0;
                }
                else if (e == 24)
                {
                    if (c == 'e') e = 25;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 25)
                {
                    if (c == 't') e = 26;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 26)
                {
                    if (c == 'u') e = 27;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 27)
                {
                    if (c == 'r') e = 28;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 28)
                {
                    if (c == 'n') e = 29;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 29)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("11", creada));
                    act--;
                    e = 0;
                }
                else if (e == 30)
                {
                    if (c == 'l') e = 31;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 31)
                {
                    if (c == 's') e = 32;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 32)
                {
                    if (c == 'e') e = 33;
                    else if (EsLetra(c, 2) || EsNum(c) || c == '_') e = 18;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("1", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 33)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("12", creada));
                    act--;
                    e = 0;
                }
                else if (e == 34)
                {
                    if (c == '.') e = 35;
                    else if (!EsNum(c))
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("13", creada));
                        act--;
                        e = 0;
                    }
                }
                else if (e == 35)
                {
                    if (!EsNum(c))
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("13", creada));
                        act--;
                        e = 0;
                    }
                }
                else if (e == 36)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("14", creada));
                    act--;
                    e = 0;
                }
                else if (e == 37)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("16", creada));
                    act--;
                    e = 0;
                }
                else if (e == 38)
                {
                    if (c == '=') e = 39;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("error", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 39)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("17", creada));
                    act--;
                    e = 0;
                }
                else if (e == 40)
                {
                    if (c == '=') e = 41;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("17", creada));
                        act--;
                        e = 0;
                    }
                }
                else if (e == 41)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("17", creada));
                    act--;
                    e = 0;
                }
                else if (e == 42)
                {
                    if (c == '|') e = 43;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("error", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 43)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("15", creada));
                    act--;
                    e = 0;
                }
                else if (e == 44)
                {
                    if (c == '&') e = 45;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("error", creada));
                        e = 0;
                        act--;
                    }
                }
                else if (e == 45)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("15", creada));
                    act--;
                    e = 0;
                }
                else if (e == 46)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("2", creada));
                    act--;
                    e = 0;
                }
                else if (e == 47)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("3", creada));
                    act--;
                    e = 0;
                }
                else if (e == 48)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("4", creada));
                    act--;
                    e = 0;
                }
                else if (e == 49)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("5", creada));
                    act--;
                    e = 0;
                }
                else if (e == 50)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("6", creada));
                    act--;
                    e = 0;
                }
                else if (e == 51)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("7", creada));
                    act--;
                    e = 0;
                }
                else if (e == 52)
                {
                    if (c == '=') e = 53;
                    else
                    {
                        tokens.Enqueue(new KeyValuePair<string, string>("8", creada));
                        act--;
                        e = 0;
                    }
                }
                else if(e == 53)
                {
                    tokens.Enqueue(new KeyValuePair<string, string>("17", creada));
                    act--;
                    e = 0;
                }
                creada += c;
                c = cadena[act++];
                if (c == '$') fin--;
            }
            tokens.Enqueue(new KeyValuePair<string, string>("18", "$"));
        }
    }
}
