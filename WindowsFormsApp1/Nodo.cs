using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Nodo
    {
        private Nodo _sig;
        private Token _token;
        private string _ambito, _clase;
        public Nodo Sig { get => _sig; set => _sig = value; }
        public Token Token { get => _token; set => _token = value; }
        public string Ambito { get => _ambito; set => _ambito = value; }
        public string Clase { get => _clase; set => _clase = value; }

        public Nodo(Nodo siguiente, Token token, string ambito, string clase)
        {
            _ambito = ambito;
            _token = token;
            _sig = siguiente;
            _clase = clase;
        }
        public Nodo()
        {
            _ambito = "";
            _token = new Token();
            _sig = null;
            _clase = "";
        }
        public Nodo(Token token)
        {
            _ambito = "";
            _token = token;
            _sig = null;
            _clase = "";
        }
        public Nodo(Nodo nodo)
        {
            _ambito = nodo.Ambito;
            _token = nodo.Token;
            _sig = nodo.Sig;
            _clase = nodo.Clase;
        }
    }
     
    #region No Terminales
    public class DefVar : Nodo
    {
        string tipo;
        Id id;
        ListaVar listavar;
        public DefVar(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            pila.Pop();//;
            pila.Pop();//estado
            if (pila.Pop().Token.Nombre == "null")
                listavar = null;
            else
            {
                pila.Push(new Nodo());
                listavar = new ListaVar(pila);//listavar
            }
            id = new Id(pila);
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;
        }
    }
    public class DefFunc : Nodo
    {
        string tipo; 
        Nodo BloqFunc;
        Id id;
        Parametros parametros;
        public DefFunc(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            BloqFunc = pila.Pop();//bloqfunc
            pila.Pop();//estado
            pila.Pop();//)
            pila.Pop();//estado
            if (pila.Pop().Token.Nombre == "null")
                parametros = null;
            else
            {
                pila.Push(new Nodo());
                parametros = new Parametros(pila);//parametros
            }
            pila.Pop();//estado
            pila.Pop();//(
            id = new Id(pila);
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;
        }


    }
    public class Parametros : Nodo
    {
        string tipo;
        Id id;
        ListaParam listaParam;

        public Parametros(Stack<Nodo> pila)
        {
            listaParam = new ListaParam(pila);
            id = new Id(pila);
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;//tipo
        }
    }
    public class ListaVar : Nodo
    {
        Id id;
        ListaVar listavar;

        public ListaVar(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            listavar = new ListaVar(pila);//listavar
            id = new Id(pila);
            pila.Pop();//estado
            pila.Pop();//,
        }
    }
    public class ListaParam : Nodo
    {
        string tipo;
        Id id;
        ListaParam listaParam;

        public ListaParam(Stack<Nodo>  pila)
        {
            pila.Pop();//estado
            listaParam = new ListaParam(pila);//listaparam
            id = new Id(pila);//id
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;//tipo
            pila.Pop();//estado
            pila.Pop();//,
        }
    }
    public class Asignacion : Nodo
    {
        Id id;
        Operacion expresion;

        public Asignacion(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            pila.Pop();//;
            expresion = new Operacion(pila);//expresion
            pila.Pop();//estado
            pila.Pop();//=
            id = new Id(pila);//id
        }
    }
    public class ClaseIf : Nodo
    {
        string _if; 
        Nodo SentenciaBloque;
        Nodo Otro;
        Operacion expresion; 

        public ClaseIf(Stack<Nodo>pila)
        {
            pila.Pop();//estado
            if (pila.Pop().Token.Nombre == "null")
                Otro = null;
            else Otro = pila.Pop();//otro
            pila.Pop();//estado
            SentenciaBloque = pila.Pop();//sentenciabloque
            pila.Pop();//estado
            pila.Pop();//)
            expresion = new Operacion(pila);//Expresion
            pila.Pop();//estado
            pila.Pop();//(
            pila.Pop();//estado
            _if = pila.Pop().Token.Lexema;//while
        }
    }
    public class ClaseWhile : Nodo
    {
        string _while;
        Nodo Bloque;
        Operacion expresion;

        public ClaseWhile(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            Bloque = pila.Pop();//Bloque
            pila.Pop();//estado
            pila.Pop();//)
            expresion = new Operacion(pila);//Expresion
            pila.Pop();//estado
            _while = pila.Pop().Token.Lexema;//while
        }
    }
    public class ClaseReturn : Nodo
    {
        Operacion expresion;
        public ClaseReturn(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            pila.Pop();//;
            expresion = new Operacion(pila);//expresion
            pila.Pop();//estado
            pila.Pop();//return
        }
    }
    public class Id : Nodo
    {
        string lexema;
        public Id(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            lexema = pila.Pop().Token.Lexema;//id
        }
    }
    public class Constante : Nodo
    {
        string lexema;
        public Constante(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            lexema = pila.Pop().Token.Lexema;//constante
        }
    }
    public class LlamadaFunc : Nodo
    {
        Id id;
        Nodo argumentos;
        public LlamadaFunc(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            pila.Pop();//(
            pila.Pop();//estado
            if (pila.Pop().Token.Nombre == "null")
                argumentos = null;
            else argumentos = pila.Pop();//argumentos
            pila.Pop();//estado
            pila.Pop();//)
            id = new Id(pila);
        }
    }
    public class Operacion : Nodo
    {
        string operando1, operador, operando2;
        public Operacion(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            operando2 = pila.Pop().Token.Lexema;
            pila.Pop();//estado
            operador = pila.Pop().Token.Lexema;
            pila.Pop();//estado
            operando1 = pila.Pop().Token.Lexema;
        }
    }

    #endregion
}
