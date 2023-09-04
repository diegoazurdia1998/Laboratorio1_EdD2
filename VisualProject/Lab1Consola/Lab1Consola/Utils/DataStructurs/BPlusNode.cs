using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1Consola.Utils.DataStructurs
{
    public class BPlusNode<K, V>
    {
        public int Grado { get; set; }
        public int MinClaves;
        public K[] Claves { get; set; }
        public V[] Valores { get; set; }
        public BPlusNode<K, V> Padre { get; set; }
        public BPlusNode<K, V>[] Hijos { get; set; }
        public BPlusNode<K, V> Siguinte { get; set; }
        public BPlusNode(int g)
        {
            this.Grado = g;
            this.MinClaves = (Grado / 2) - 1;
            this.Claves = new K[Grado];
            this.Valores = new V[Grado];
            this.Hijos = new BPlusNode<K, V>[Grado + 1];
        }

        public bool IsFull()
        {
            if (Claves.Length < Grado - 1) return true;
            return false;
        }

    }
    
}
