using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1Consola.Utils.DataStructurs
{
    public class AVLNode<K, T>
    {
        public K key {get;set;}
        public T value { get; set; }
        public AVLNode<K, T> Izquierda { get; set; }
        public AVLNode<K, T> Derecha { get; set; }
        public int balance { get; set; }
        public AVLNode(K pKey, T pValue)
        {
            this.key = pKey;
            this.value = pValue;
        }
        

    }

    public class AVLNodeDoubleKey<K, T>
    {
        public K key { get; set; }
        public K key2 { get; set; }
        public T value { get; set; }
        public AVLNodeDoubleKey<K, T> Izquierda { get; set; }
        public AVLNodeDoubleKey<K, T> Derecha { get; set; }
        public int balance { get; set; }
        public AVLNodeDoubleKey(K pKey1, K pKey2, T pValue)
        {
            this.key = pKey1;
            this.key2 = pKey2;
            this.value = pValue;
        }
    }
}
