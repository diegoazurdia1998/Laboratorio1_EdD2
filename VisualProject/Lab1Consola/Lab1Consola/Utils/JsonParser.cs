using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Lab1Consola.Models;

namespace Lab1Consola.Utils
{
    class JsonParser
    {
        public Applicant jsonToApplicant(string json)
        {
            Applicant appl =  new Applicant();
            try
            {
                appl = JsonSerializer.Deserialize<Applicant>(json);
            }
            catch
            {
                Console.WriteLine("Error al convertir json en solicitante.");
            }

            return appl;
        }

        public string ApplicantToJson(List<Applicant> applicants)
        {
            string json = "";
            foreach(var app in applicants)
            {
                json = json + JsonSerializer.Serialize(app) + '\n';
            }
            return json;
        }
    }
}
