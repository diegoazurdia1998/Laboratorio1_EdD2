using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1Consola.Models
{
    class OperationJson
    {
        public string operation { get; set; }
        public string json { get; set; }
        public OperationJson(string o, string j)
        {
            this.operation = o;
            this.json = j;
        }
    }
}
