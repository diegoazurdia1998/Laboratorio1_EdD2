using System;
using System.Collections.Generic;
using System.Text;
using Lab1Consola.Services;
using Lab1Consola.Models;

namespace Lab1Consola.Views
{
    class Visualizer
    {
        
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
                            Console.Write("\n- - - - - - - - - - - - - - -\nDPI: " + search.dpi + "\nNombre y apellido: " + search.name + "\nFecha de nacimiento: " + search.dateBirth + "\nDireción: " + search.address + "\n- - - - - - - - - - - - - - -\n");
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
                                Console.Write("\n- - - - - - - - - - - - - - -\nDPI: " + appl.dpi + "\nNombre y apellido: " + appl.name + "\nFecha de nacimiento: " + appl.dateBirth + "\nDireción: " + appl.address + '\n');
                            }
                            Console.Write("\n - - - - - - - - - - - - - - -\n");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    case "3":
                        Console.Write("Ingrese el nombre para la extración de su bitácora: ");
                        string name = Console.ReadLine();
                        try
                        {
                            servicios.ExtraerBitacoraNombre(name);
                            Console.Write("Su bitácora para los nombres que contienen: " + name + " fue extraida con exito.");
                        }
                        catch(Exception e)
                        {

                        }
                        break;
                    case "4":
                        List<Applicant> list = servicios.extraerSolicitantes();
                        foreach(var appl in list)
                        {
                            Console.Write("\n- - - - - - - - - - - - - - -\nDPI: " + appl.dpi + "\nNombre y apellido: " + appl.name + "\nFecha de nacimiento: " + appl.dateBirth + "\nDireción: " + appl.address + '\n');
                        }
                        Console.Write("\n - - - - - - - - - - - - - - -\n");
                        break;
                }
            }
        }

    }
}
