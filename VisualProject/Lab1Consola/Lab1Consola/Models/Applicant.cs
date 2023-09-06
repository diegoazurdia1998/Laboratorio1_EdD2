using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1Consola.Models
{
    public class Applicant
    {
        public string name { get; set; }
       public string dpi { get; set; }
        public string dateBirth { get; set; }
        public string address { get; set; }
        public Applicant()
        { }
        public Applicant(string n, string d, string db, string a)
        {
            this.name = n;
            this.dpi = d;
            this.dateBirth = db;
            this.address = a;
        }
    }
}
