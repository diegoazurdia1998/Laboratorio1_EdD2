using System;
using System.Collections.Generic;
using System.Text;
using Lab1Consola.Services;
using Lab1Consola.Models;
using Lab1Consola.Utils;

namespace Lab1Consola.Views
{
    class Visualizer
    {
        private void printApplicant(Applicant applicant)
        {
            CompressingOperations operation = new CompressingOperations();
            String[] companies = operation.DecompressDPICompany(applicant);
            String impresion = "\n- - - - - - - - - - - - - - -\nDPI: " + applicant.dpi + "\nNombre y apellido: " + applicant.name + "\nFecha de nacimiento: " + applicant.datebirth + "\nDireción: " + applicant.address + "\nCompañias:\n";
            Console.Write(impresion);
            foreach (var company in companies)
            {
                Console.Write("\t" + company + '\n');
            }
            Console.Write("\n- - - - - - - - - - - - - - -\n");
        }
        public void Menu()
        {

            ApplicantService servicios = null;
            while (servicios == null)
            {
                Console.Write("Ingrese la ubicacion del archivo a cargar: ");
                string path = Console.ReadLine();
                try
                {
                    servicios = new ApplicantService(path);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.Write("Bienvenid@");
            string opcion = "";
            while (opcion != "5")
            {
                Console.Write("\n* * * * * * * * * * * * * *\n1. Buscar solicitante por DPI\n2. Buscar solicitante por nombre y apellido\n3. Extraer bitácora por nombre.\n4. Ver listado completo\n5. Salir\n\nIngrese el número correspondiente: ");
                opcion = Console.ReadLine();
                Console.Write("\n* * * * * * * * * * * * * *\n");
                switch (opcion)
                {
                    case "1":
                        Console.Write("Ingrese el DPI del solicitante a buscar: ");
                        string dpi = Console.ReadLine();
                        Applicant search = servicios.buscarDPI(dpi);
                        if (search != null)
                        {
                            printApplicant(search);
                        }
                        else Console.WriteLine("No se encontró al solicitante con DPI: " + dpi);
                        break;
                    case "2":
                        Console.Write("Ingrese el nombre de los solicitantes a buscar: ");
                        string nameSearch = Console.ReadLine();
                        try
                        {
                            List<Applicant> foudApplicants = servicios.buscarNombre(nameSearch);
                            foreach(var appl in foudApplicants)
                            {
                                printApplicant(appl);                            
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    case "3":
                        Console.Write("Ingrese un nombre, o deje en blanco, para la extración de su bitácora: ");
                        string name = Console.ReadLine();
                        try
                        {
                            servicios.ExtraerBitacoraNombre(name);
                            Console.Write("Su bitácora para los nombres que contienen: " + name + " fue extraida con exito.");
                        }
                        catch
                        {
                            Console.WriteLine("No se estrajo la bitacota.");
                        }
                        break;
                    case "4":
                        List<Applicant> list = servicios.extraerSolicitantes();
                        foreach(var appl in list)
                        {
                            printApplicant(appl);
                        }
                        break;
                }
            }
        }

    }
}
