using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form : System.Windows.Forms.Form
    {
        Queue<Token> tokens;
        HashSet<string> tipodato;
        Dictionary<string, int> reservadas;
        Dictionary<string, int> c_reservados;
        List<Point> reglas;
        List<List<int>> tabla;
        bool BALE;
        public Form()
        {
            c_reservados = new Dictionary<string, int> { { ";", 2 }, { ",", 3 }, { "(", 4 }, { ")", 5 }, { "{", 6 }, { "}", 7 } };
            reservadas = new Dictionary<string, int>{{ "if", 9 }, { "while", 10 }, { "return", 11 }, { "else", 12 } };
            tipodato = new HashSet<string> { "int", "float", "char", "void" };
            tabla = new List<List<int>>();
            tokens = new Queue<Token>();
            reglas = new List<Point>();
            CargarDatos();
            BALE = false;
            InitializeComponent();
            Formatodgv();
        }
        void Formatodgv()
        {
            dgv.Columns.Clear();
            dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
            dgv.Columns.Add("", "");
            dgv.Rows.Add("Lexema"); dgv.Rows.Add("Id");dgv.Rows.Add("Nombre");
            dgv.Refresh();
        }
        int StringtoInt(string s)
        {
            int t = 0, i = 0;
            bool n = false;
            if (s[0] == '-')
            {
                n = true;
                ++i;
            }
            for(;i < s.Count();++i)
            {
                t *= 10;
                t += s[i] - '0';
            }
            return n? -t : t;
        }
       
        #region Cargar Tabla de reglas y transiciones
        void CargarDatos()//Leer reglas y tabla
        {
            string dir_reglas = "GR2slrRulesId.txt", dir_tabla = "GR2slrTablebien.txt", linea;
            if (!File.Exists(dir_reglas) || !File.Exists(dir_tabla))
            {
                MessageBox.Show("No se encuentra el archivo "+dir_reglas+" o "+dir_tabla 
                                +"\r\nColocar estos archivos en la misma carpeta del ejecutable");
                return;
            }
            StreamReader archivo = new StreamReader(dir_reglas);
            linea = archivo.ReadLine();   
            while (linea != null)
            {
                string[] par = linea.Split('\t');
                reglas.Add(new Point(StringtoInt(par[0]), StringtoInt(par[1])));
                linea = archivo.ReadLine();
            }
            archivo.Close();
            tabla = new List<List<int>>();
            archivo = new StreamReader(dir_tabla);
            linea = archivo.ReadLine();
            while (linea != null)
            {
                string[] columnas = linea.Split('\t');
                List<int> temp = new List<int>();
                foreach(string s in columnas) temp.Add(StringtoInt(s));
                linea = archivo.ReadLine();
                tabla.Add(temp);
            }
            archivo.Close();
        }
        #endregion
        private void BtnAnalizar_Click(object sender, EventArgs e)
        {
            Formatodgv();
            tokens.Clear();
            AnalizadorLexico(txtbox.Text + "$");
            AgregarCont();
            if (BALE)           //bandera analizador lexico error
            {
                MessageBox.Show("Error en analisis lexico");
                BALE = false;
            }
            else if (reglas.Count != 0) AnalizadorSintactico();
            else MessageBox.Show("No se puede realizar el analizis sintactico, sin las tablas de reglas y transiciones");
        }
        
        #region Analizador Lexico       
        void AnalizadorLexico(string cadena)
        {
            string cad_act = "";
            int act = 0, e = 0;
            while(act < cadena.Length)
            {
                char c = cadena[act++];
                cad_act += ""+c;
                if (e == 0)
                {
                    if (c == ' ' || c == '\r' || c == '\n') cad_act = "";
                    else if (c_reservados.ContainsKey(cad_act)) EstadoFinal(cad_act, ref cad_act, c_reservados[cad_act], ref e);
                    else if (EsLetra(c, 0) || c == '_') e = 1;
                    else if (EsNum(c)) e = 2;
                    else if (c == '-' || c == '+') EstadoFinal("OpSuma", ref cad_act, 14, ref e);
                    else if (c == '*' || c == '/') EstadoFinal("OpMultiplicacion", ref cad_act, 16, ref e);
                    else if (c == '|') e = 8;
                    else if (c == '&') e = 9;
                    else if (c == '=') e = 3;
                    else if (c == '<' || c == '>') e = 5;
                    else if (c == '!') e = 6;
                    else if (c == '$') tokens.Enqueue(new Token("Fin", "$", 18));
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
                else if(e == 8)
                {
                    if (c == '|') EstadoFinal("OpLogico", ref cad_act, 15, ref e);
                    else
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        EstadoFinal("Error", ref cad_act, -1, ref e);
                    }
                }
                else if(e == 9)
                {

                    if (c == '&') EstadoFinal("OpLogico", ref cad_act, 15, ref e);
                    else
                    {
                        act--;
                        cad_act = cad_act.Substring(0, cad_act.Length - 1);
                        EstadoFinal("Error", ref cad_act, -1, ref e);
                    }
                }
            }
        }
        void EstadoFinal(string nombre,ref string lexema, int id, ref int e)
        {
            if (nombre == "Error") BALE = true;
            e = 0;
            tokens.Enqueue(new Token(nombre, lexema, id));
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
        void AgregarCont()
        {
            Queue<Token> temp = new Queue<Token>(tokens);
            while(temp.Count > 0){
                Token t = temp.Dequeue();
                int pos = dgv.Columns.Add("", "");
                dgv[pos, 0].Value = t.Lexema;
                dgv[pos, 1].Value = t.Id;
                dgv[pos, 2].Value = t.Nombre;
            }
            dgv.Refresh();
        }
        #endregion
       
        #region Analizador Sintactico
        void AnalizadorSintactico()
        {
            Stack<Nodo> pila = new Stack<Nodo>();
            pila.Push(new Nodo(new Token(0)));
            Stack<Token> pila_tokens = new Stack<Token>(tokens.Reverse());
            int valor_devuelto;
            Nodo fila_act;
            while (pila_tokens.Count > 0)
            {
                Token token_act = pila_tokens.Peek();
                fila_act = pila.Peek();
                valor_devuelto = tabla[fila_act.Token.Id][token_act.Id+1];
                if (valor_devuelto < 0)                             //reduccion
                {
                    if (valor_devuelto == -1) {                     //aceptada
                        MostrarR(true, pila.Peek());
                        return;
                    }
                    ++valor_devuelto;                               //decremento la posicion
                    int cant = reglas[-valor_devuelto].Y << 1;      //saco el lado derecho x2 de la pila
                    //esta parte es la que voy a reemplazar cuando entienda como va esta cosa xD
                    while (cant-- > 0) pila.Pop();              
                    //**************************************************************************
                    pila_tokens.Push(new Token("", "", reglas[-valor_devuelto].X));
                }
                else if (valor_devuelto > 0)                        //si cae aquí el token actual es descartado
                {                                                   //y se agrega su id en la pila junto con el anterior
                    pila_tokens.Pop();
                    pila.Push(new Nodo(token_act));
                    pila.Push(new Nodo(new Token(valor_devuelto)));
                }
                else break;                                         //no aceptada
            }
            MostrarR(false, pila.Peek());
            return;
        }
        void MostrarR(bool b, Nodo raiz)
        {
            if (b)
            {
                MessageBox.Show("Valido");
                txtbox.BackColor = Color.LightGreen;
            }
            else
            {
                MessageBox.Show("Invalido");
                txtbox.BackColor = Color.LightCoral;
            }
            MessageBox.Show("Quitar esto");
        }
        #endregion
        public Nodo NTaNT(Stack<Nodo> pila, int regla)
        {
            Nodo aux = new Nodo(new Token("FALSE", "", 0));
            //Programa -> Definiciones
            //Definicion -> DefVar
            //Definicion -> DefFunc
            //DefLocal -> DefVar
            //DefLocal->Sentencia
            //Expresion -> LlamadaFunc
            //SentenciaBloque -> Sentencia
            //SentenciaBloque->Bloque
            if (regla == 2 || regla == 5 || regla == 6 || regla == 18 || regla == 19 || regla == 34 || regla == 38 || regla == 39)
            {
                pila.Pop();//estado
                aux = pila.Pop();//definiciones
            }
            //BloqFunc -> { DefLocales }
            //Bloque -> { Sentencias }
            //Expresion -> ( Expresion )
            else if (regla == 15 || regla == 29 || regla == 40)
            {
                pila.Pop();//estado
                pila.Pop();//{
                pila.Pop();//estado
                aux = pila.Pop();//deflocales
                pila.Pop();//estado
                pila.Pop();//}
            }
            else if (regla == 26)//sentencia -> llamadafunc ;
            {
                pila.Pop();//estado
                pila.Pop();//;
                pila.Pop();//estado
                aux = pila.Pop();//llamadafunc
            }
            else if (regla == 28)//Otro -> else SentenciaBloque
            {
                pila.Pop();//estado
                aux = pila.Pop();//sentenciabloque
                pila.Pop();//estado
                pila.Pop();//else
            }
            return aux;
        }
        public Nodo ReglasGeneran2(Stack<Nodo> pila, int cant)
        {
            Nodo aux = new Nodo(), sig;
            pila.Pop();//estado
            sig = pila.Pop();//sig
            pila.Pop();//estado
            aux = pila.Pop();//nodo
            if(cant != 2)
            {
                pila.Pop();//estado
                pila.Pop();//,
            }
            return aux;
        }

    }



}
