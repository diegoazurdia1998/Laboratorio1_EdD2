using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using Lab1Consola.Models;

namespace Lab1Consola.Utils
{
    class FileOperations
    {
        public List<OperationJson> readFile(string filePath)
        {
            List<OperationJson> operationsList = new List<OperationJson>();
            string operation;
            string json;
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    //Mientras hayan lineas por leer
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        // Procesar cada campo
                        if (fields.Length > 1)
                        {
                            operation = fields[0];//almacena la operacion
                            json = fields[1];// almacena el json 
                            operationsList.Add(new OperationJson(operation, json)); //alacena el objeto en la lista
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error al leer el archivo.");
            }
            return operationsList;
        }
    }
}
