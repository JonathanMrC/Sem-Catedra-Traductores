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
        Queue<string> tokens;
        string cadena;

        public Form()
        {
            tokens = new Queue<string>();
            InitializeComponent();
            //dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
        }
        private void BtnAnalizar_Click(object sender, EventArgs e)
        {   
            cadena = txtbox.Text+"$";

            AnalizadorLexico();
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
        
        void AnalizadorLexico()
        {

        }
    }
}
