using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Nodo
    {
        public Nodo Sig { get; set; }
        public Token Token { get; set; }
        public string Ambito { get; set; }

        public Nodo(Nodo siguiente, Token token, string ambito)
        {
            Ambito = ambito;
            Token = token;
            Sig = siguiente;
        }
        public Nodo()
        {
            Ambito = "";
            Token = new Token();
            Sig = null;
        }
        public Nodo(Token token)
        {
            Ambito = "";
            Token = token;
            Sig = null;
        }
        public Nodo(Nodo nodo)
        {
            Ambito = nodo.Ambito;
            Token = nodo.Token;
            Sig = nodo.Sig;
        }
    }

    #region No Terminales
    //listo
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
            if (pila.Peek().Token.Nombre == "null")
            {
                pila.Pop();
                listavar = null;
            }
            else
                listavar = (ListaVar)pila.Pop();//listavar
            id = new Id(pila);
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;
        }
    }
    //listo
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
            if (pila.Peek().Token.Nombre == "null")
            {
                pila.Pop();
                parametros = null;
            }
            else
            {
                parametros = (Parametros)pila.Pop();//parametros
            }
            pila.Pop();//estado
            pila.Pop();//(
            id = new Id(pila);
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;
        }


    }
    //listo
    public class Parametros : Nodo
    {
        string tipo;
        Id id;
        ListaParam listaParam;

        public Parametros(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            if (pila.Peek().Token.Nombre == "null")
            {
                pila.Pop();
                listaParam = null;
            }
            else
            {
                listaParam = (ListaParam)pila.Pop();//parametros
            }
            id = new Id(pila);
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;//tipo
        }
    }
    //listo
    public class ListaVar : Nodo
    {
        Id id;
        ListaVar listavar;

        public ListaVar(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            if (pila.Peek().Token.Nombre == "null")
            {
                pila.Pop();
                listavar = null;
            }
            else
            {
                listavar = new ListaVar(pila);//listavar
            }
            id = new Id(pila);
            pila.Pop();//estado
            pila.Pop();//,
        }
    }
    //listo
    public class ListaParam : Nodo
    {
        string tipo;
        Id id;
        ListaParam listaParam;

        public ListaParam(Stack<Nodo>  pila)
        {
            pila.Pop();//estado
            if (pila.Peek().Token.Nombre == "null")
            {
                pila.Pop();
                listaParam = null;
            }
            else
            {
                listaParam = (ListaParam)pila.Pop();//parametros
            }
            id = new Id(pila);//id
            pila.Pop();//estado
            tipo = pila.Pop().Token.Lexema;//tipo
            pila.Pop();//estado
            pila.Pop();//,
        }
    }
    //listo
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
    //listo
    public class ClaseIf : Nodo
    {
        string _if; 
        Nodo SentenciaBloque;
        Nodo Otro;
        Operacion expresion; 

        public ClaseIf(Stack<Nodo>pila)
        {
            pila.Pop();//estado
            if (pila.Peek().Token.Nombre == "null")
            {
                Otro = null;
                pila.Pop();
            }
            else Otro = pila.Pop();//otro
            pila.Pop();//estado
            if (pila.Peek().Token.Nombre == "null")
            {
                SentenciaBloque = null;//sentenciabloque
                pila.Pop();
            }
            else SentenciaBloque = pila.Pop();//sentenciabloque
            pila.Pop();//estado
            pila.Pop();//)
            expresion = new Operacion(pila);//Expresion
            pila.Pop();//estado
            pila.Pop();//(
            pila.Pop();//estado
            _if = pila.Pop().Token.Lexema;//if
        }
    }
    //listo
    public class ClaseWhile : Nodo
    {
        string _while;
        Nodo Bloque;
        Operacion expresion;

        public ClaseWhile(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            if (pila.Peek().Token.Nombre == "null")
            {
                pila.Pop();
                Bloque = null;
            }
            else Bloque = pila.Pop();
            pila.Pop();//estado
            pila.Pop();//)
            expresion = new Operacion(pila);//Expresion
            pila.Pop();//estado
            pila.Pop();//(
            pila.Pop();//estado
            _while = pila.Pop().Token.Lexema;//while
        }
    }
    //listo, aunque en sig muestra un nodo con nombre null pero se puede validar que sea null o Token.Nombre
    public class ClaseReturn : Nodo
    {
        Operacion expresion;
        public ClaseReturn(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            pila.Pop();//;
            if(pila.Peek().Token.Nombre == "null")
            {
                pila.Pop();
                expresion = null;
            }
            else
                expresion = new Operacion(pila);//expresion
            pila.Pop();//estado
            pila.Pop();//return
        }
    }
    //listo
    public class Id : Nodo
    {
        string lexema;
        public Id(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            lexema = pila.Pop().Token.Lexema;//id
            this.Token.Nombre = "Id";
        }
    }
    //listo
    public class Constante : Nodo
    {
        string lexema;
        public Constante(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            lexema = pila.Pop().Token.Lexema;//constante
            this.Token.Nombre = "Constante";
        }
    }
    //listo
    public class LlamadaFunc : Nodo
    {
        Id id;
        Nodo argumentos;
        public LlamadaFunc(Stack<Nodo> pila)
        {
            pila.Pop();//estado
            pila.Pop();//(
            pila.Pop();//estado
            if (pila.Peek().Token.Nombre == "null")
            {
                argumentos = null;
                pila.Pop();
            }
            else argumentos = pila.Pop();//argumentos
            pila.Pop();//estado
            pila.Pop();//)
            id = new Id(pila);
        }
    }
    //listo
    public class Operacion : Nodo
    {
        string operador;
        Operacion operando1, operando2;
        Id id1, id2;
        Constante constante1, constante2;
        LlamadaFunc llamadaFunc1, llamadaFunc2;
        
        public Operacion(Stack<Nodo> pila)
        {
            id1 = null;
            id2 = null;
            constante1 = null;
            constante2 = null;
            operador = "";
            operando1 = null;
            operando2 = null;
            llamadaFunc1 = null;
            llamadaFunc2 = null;
            this.Token.Nombre = "Operacion";

            pila.Pop();//estado
            Nodo aux = pila.Pop();
            if (aux.Token.Nombre == "Id") id2 = (Id)aux;
            else if (aux.Token.Nombre == "Constante") constante2 = (Constante)aux;
            else if (aux.Token.Nombre == "Operacion") operando2 = (Operacion)aux;
            else llamadaFunc2 = (LlamadaFunc)aux;
            Nodo temp = pila.Pop();//estado
            if (pila.Peek().Token.Lexema != "=" && pila.Peek().Token.Lexema != "return" && pila.Peek().Token.Lexema != "(" && pila.Peek().Token.Lexema != ",")
            {
                operador = pila.Pop().Token.Lexema;//operador
                pila.Pop();//estado
                if (pila.Peek().Token.Nombre == "Id") id1 = (Id)pila.Pop();
                else if (pila.Peek().Token.Nombre == "Constante") constante1 = (Constante)pila.Pop();
                else if (pila.Peek().Token.Nombre == "Operacion") operando1 = (Operacion)pila.Pop();
                else llamadaFunc1 = (LlamadaFunc)pila.Pop();
            }
            else pila.Push(temp);
        }
    }

    #endregion
}
