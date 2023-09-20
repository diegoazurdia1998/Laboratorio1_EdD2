using System;
using System.Collections.Generic;
using System.Text;
using Lab1Consola.Models;

namespace Lab1Consola.Utils
{
    public class CompressingOperations
    {
        public Applicant CompressApplicant(Applicant app)
        {
            string[] ParaComprimir = app.companies;
            List<String> compressedCompanies = new List<string>();
            foreach(string comp in ParaComprimir)
            {
                compressedCompanies.Add(CompressDPICompany(app.dpi, comp));
            }
            app.companies = compressedCompanies.ToArray();
            return app;
        }
        public String CompressDPICompany(String dpi, String company)
        {
            //Algoritmo utilizado LZ78

            String cadena = String.Empty, entrada = dpi + company;
            Dictionary<string, int> diccionario = new Dictionary<string, int>();
            List<ParCompreso> salidaCompresa = new List<ParCompreso>();
            foreach (var actual in entrada)
            {
                string tempCadena = cadena + actual;
                if (!diccionario.ContainsKey(tempCadena))
                {
                    int indice = cadena == "" ? 0 : diccionario[cadena];
                    salidaCompresa.Add(new ParCompreso(indice, actual));
                    diccionario[tempCadena] = diccionario.Count + 1;
                    cadena = String.Empty;
                }
                else
                {
                    cadena = tempCadena;
                }
            }
            if (cadena != "") salidaCompresa.Add(new ParCompreso(diccionario[cadena], ' '));
            String ParesAString = "";
            foreach(var par in salidaCompresa)
            {
                ParesAString = ParesAString + par.indice + '#' + par.entrada + ';';
            }
            return ParesAString;
        }
        private List<List<ParCompreso>> StrArrayToList(String[] compressedCompaniesArray)
        {
            List<List<ParCompreso>> listCompanies = new List<List<ParCompreso>>();
            foreach (var company in compressedCompaniesArray)
            {
                List<ParCompreso> pairsList = new List<ParCompreso>();
                String[] tempPairsArray = company.Split(';');
                foreach(var par in tempPairsArray)
                {
                    if (par != "")
                    {
                        String[] pares = par.Split('#');
                        if(pares[1] != "") pairsList.Add(new ParCompreso(int.Parse(pares[0]), pares[1][0]));
                    }
                }
                listCompanies.Add(pairsList);
            }
            return listCompanies;
        }
        
        public String[] DecompressDPICompany(Applicant applicant)
        {
            List<List<ParCompreso>> compressedCompaniesList = StrArrayToList(applicant.companies);
            List<String> companies = new List<string>();
            List<string> diccionario = new List<string>();
            string resultado = String.Empty;
            foreach(var company in compressedCompaniesList)
            {
                resultado = String.Empty;
                foreach (var par in company)
                {
                    string entrda = String.Empty;
                    if (par.indice != 0 && (par.indice - 1 < diccionario.Count)) entrda = diccionario[par.indice - 1];
                    entrda += par.entrada;
                    diccionario.Add(entrda);
                    resultado += entrda;

                }
                companies.Add(resultado.Substring(applicant.dpi.Length));
            }
            return companies.ToArray();   
        }
        

    }
}
