using System;

namespace Lab1Consola.Utils
{
    public class KeyComparer
    {
        /// <summary>
        /// Delegado comparador de strings
        /// </summary>
        /// <param name="x">Llave 1</param>
        /// <param name="y">Llave 2</param>
        /// <returns>* Si x es menor a y devuelve -1 * si x es igual a y devuleve 0 * si x es mayor a y devuelve 1</returns>
        public int ComparerString(string x, string y)
        {
            /*
             
             */
            return x.CompareTo(y);
        }

        public bool ContainsKey(string x, string y)
        {
            return x.Contains(y);
        }
        
    }
}
