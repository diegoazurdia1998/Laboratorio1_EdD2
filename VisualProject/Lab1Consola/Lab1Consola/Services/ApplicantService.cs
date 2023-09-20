using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lab1Consola.Utils.DataStructurs;
using Lab1Consola.Utils;
using Lab1Consola.Models;

namespace Lab1Consola.Services
{
    public class ApplicantService
    {
        private AVLTreeDoubleKey<string, Applicant> Solicitantes;
        private string path;

        public ApplicantService(string filePath)
        {
            Solicitantes = new AVLTreeDoubleKey<string, Applicant>(new KeyComparer().ComparerString, new KeyComparer().ContainsKey);
            this.path = filePath;
            ProcesarSolicitantes();
            if (Solicitantes.getCount() <= 0) throw new Exception("No se encontraron solicitantes");
        }
        public void ProcesarSolicitantes()
        {
            FileOperations reader = new FileOperations();
            JsonParser jsonParser = new JsonParser();
            int inseta = 0, elimina = 0, actualiza = 0, insertaError = 0;

            List<OperationJson> operaciones = reader.readFile(this.path);
            Applicant tempApplicant;
            foreach(var op in operaciones)
            {
                tempApplicant = jsonParser.jsonToApplicant(op.json);

                switch (op.operation)
                {
                    case "INSERT":
                        try
                        {
                            Solicitantes.Add(tempApplicant.dpi, tempApplicant.name, tempApplicant);
                            Console.WriteLine("+ Se insertó el solicitante con DPI: " + tempApplicant.dpi);
                            inseta++;
                        }
                        catch
                        {
                            insertaError++;
                            Console.WriteLine("! Error en la inserción del solicitante con DPI: " + tempApplicant.dpi);
                        }
                        break;
                    case "DELETE":
                        try
                        {
                            Solicitantes.Remove(tempApplicant.dpi);
                            Console.WriteLine("- Se eliminó el solicitante con DPI: " + tempApplicant.dpi);
                            elimina++;
                        }
                        catch
                        {
                            Console.WriteLine("! Error en la eliminación del solicitante con DPI: " + tempApplicant.dpi);
                        }
                        break;
                    case "PATCH":
                        try
                        {
                            Solicitantes.PatchT(tempApplicant.dpi, tempApplicant);
                            Console.WriteLine("* Se actualizó el solicitante con DPI: " + tempApplicant.dpi);
                            actualiza++;
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("! Error en la actualización del solicitante con DPI: " + tempApplicant.dpi);
                        }
                        break;
                    default:
                        Console.WriteLine("Operación no reconocida.");
                        break;
                }
            }
            Console.WriteLine("Se realizaron:\n" + inseta + " incerciones, hubo: " + insertaError + " errores al insertar.\n" + elimina + " eliminaciones\n" + actualiza + " actualizaciones");
        }
        public Applicant buscarDPI(string DPI)
        {
            Applicant search =  Solicitantes.Find(DPI);
            if (search != default) return search;
            else return null;
        }
        public List<Applicant> buscarNombre(string name)
        {
            List<Applicant> searchName = Solicitantes.FindSecondaryKey(name);
            if (searchName.Count > 0) return searchName;
            else throw new Exception("No se encontró el nombre del solicitante");
        }
        public List<Applicant> extraerSolicitantes()
        {
            return Solicitantes.TreeToList();
        }
        public void ExtraerBitacoraNombre(string applicantName)
        {
            List<Applicant> applicants;
            if (applicantName != "") applicants = Solicitantes.FindSecondaryKey(applicantName);            
            else applicants = Solicitantes.TreeToList();
            JsonParser extractJson = new JsonParser();
            string ApplicantsJsonCompressed = extractJson.ApplicantToJson(applicants);
            File.WriteAllText(@"D:\Documentos\1A URL\ED2\Laboratorios\Lab 1\BitacoraCompresa.txt", ApplicantsJsonCompressed);

            CompressingOperations operation = new CompressingOperations();
            foreach(var appl in applicants)
            {
                String[] companies = operation.DecompressDPICompany(appl);
                appl.companies = companies;
            }
            string ApplicantsJsonDecompressed = extractJson.ApplicantToJson(applicants);
            File.WriteAllText(@"D:\Documentos\1A URL\ED2\Laboratorios\Lab 1\BitacoraExtendida.txt", ApplicantsJsonDecompressed);
        }

    }
}
