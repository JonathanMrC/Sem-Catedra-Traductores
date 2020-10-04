using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class token
    {
        public token(string nombre, string lexema, int id)
        {
            this.nombre = nombre;
            this.lexema = lexema;
            this.id = id;
        }
        string lexema, nombre;
        int id;

        public string Lexema { get => lexema; set => lexema = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int Id { get => id; set => id = value; }
    }
}
