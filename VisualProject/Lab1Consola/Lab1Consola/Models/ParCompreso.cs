using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1Consola.Models
{
    public class ParCompreso
    {
        public int indice { get; set; }
        public char entrada { get; set; }

        public ParCompreso(int i, char e)
        {
            this.indice = i;
            this.entrada = e;
        }
    }
}
