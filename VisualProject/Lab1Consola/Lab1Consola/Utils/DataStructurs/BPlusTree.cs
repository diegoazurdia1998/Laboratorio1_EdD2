using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab1Consola.Utils.DataStructurs
{
    public delegate int CompareKeyDelegate<K>(K key1, K key2);

    class BPlusTree<K, V>
    {
        public int Grado { get; set; }
        public BPlusNode<K, V> Raiz { get; set; }
        private CompareKeyDelegate<K> CompareKeys;
        public BPlusTree(int g, CompareKeyDelegate<K> com)
        {
            this.Grado = g;
            this.Raiz = new BPlusNode<K, V>(g);
            this.CompareKeys = com;
        }
        public void Insert(K key, V value)
        {
            //Busca la hoja donde toca insertar
            BPlusNode<K, V> tempNode = BuscarHoja(key);

            //Si el nodo no esta lleno insertar
            if (!tempNode.IsFull())
            {
                InsertInNode(tempNode, key, value);
            }
            else
            {
                //Dividir el nodo y propagar la division de ser necesario
                DivideNode(tempNode, key, value);
            }

        }
        private BPlusNode<K, V> BuscarHoja(K key)
        {
            BPlusNode<K, V> tempNode = Raiz;
            int i;
            while (tempNode.Hijos[0] != null)
            {
                i = 0;
                while (i < tempNode.Claves.Length && CompareKeys(key, tempNode.Claves[i]) > 0)
                {
                    i++;
                }
                tempNode = tempNode.Hijos[i];
            }

            return tempNode;
        }
        private void InsertInNode(BPlusNode<K, V> node, K key, V value)
        {
            int i = 0;
            //Buscar posicion correcta en el nodo
            while (i < node.Claves.Length && CompareKeys(key, node.Claves[i]) > 0)
            {
                i++;
            }
            //Hacer espacio para insertar la nueva llave y valor
            for (int j = node.Claves.Length - 1; j > i; j--)
            {
                node.Claves[j] = node.Claves[j - 1];
                node.Valores[j] = node.Valores[j - 1];
            }
            node.Claves[i] = key;
            node.Valores[i] = value;

        }

        private void DivideNode(BPlusNode<K, V> node, K key, V value)
        {
            //Nodo nuevo para almacenar la mitad ed las claves
            BPlusNode<K, V> nodoDivision = new BPlusNode<K, V>(Grado);
            //Se calcula el punto medio de las claves
            int t = Grado / 2;
            //Si la clave que se insertara es mayor a la del punto medio se corre el punto medio una posicion
            if (CompareKeys(key, node.Claves[t]) > 0) t++;
            //Se mueven la mitad de las claves y valores al nuevo nodo
            for (int i = t; i < Grado; i++)
            {
                nodoDivision.Claves[i - t] = node.Claves[i];
                nodoDivision.Valores[i - t] = node.Valores[i];
                node.Claves[i] = default;
                node.Valores[i] = default;
            }
            //Si el nodo tiene hijos se mueven la mitad al nuevo nodo
            if (node.Hijos[0] != null)
            {
                for (int i = t + 1; i <= Grado; i++)
                {
                    nodoDivision.Hijos[i - t - 1] = node.Hijos[i];
                    nodoDivision.Hijos[i - t - 1].Padre = nodoDivision;
                    node.Hijos[i] = null;
                }
            }
            //Se insertan clave y valor en el nodo correspondente
            if(CompareKeys(key, nodoDivision.Claves[0]) < 0)
            {
                InsertInNode(node, key, value);
            }
            else
            {
                InsertInNode(nodoDivision, key, value);
            }
            //Si el nodo es una hoja, se apunta al siguiente nodo hoja
            if(node.Siguinte != null)
            {
                nodoDivision.Siguinte = node.Siguinte;
                node.Siguinte = nodoDivision;
            }

            //Propagar division
            DivisionDivision(node, nodoDivision);
        }

        private void DivisionDivision(BPlusNode<K, V> node, BPlusNode<K,V> nuevoNodo)
        {
            //si el nodo es la raiz, se crea una nueva raiz
            if(node.Padre == null)
            {
                BPlusNode<K, V> nuevaRaiz = new BPlusNode<K, V>(Grado);
                nuevaRaiz.Claves[0] = nuevoNodo.Claves[0];
                nuevaRaiz.Hijos[0] = node;
                nuevaRaiz.Hijos[1] = nuevoNodo;
                node.Padre = nuevaRaiz;
                nuevoNodo.Padre = nuevaRaiz;
                Raiz = nuevaRaiz;
                return;
            }
            //si el padre no esta lleno insetar clave y mover el puntero al nuevo nodo
            if(!node.Padre.IsFull())
            {
                InsertarEnPadre(node.Padre, nuevoNodo.Claves[0], nuevoNodo);
            }
            else
            {
                //Si el padre sta lleno propagar la Division 
                DividirPadre(node.Padre, nuevoNodo.Claves[0], nuevoNodo);
            }
        }
        private void InsertarEnPadre(BPlusNode<K, V> padre, K key, BPlusNode<K, V> hijo)
        {
            int i = 0;
            //buscar posiscion para insertar
            while(i < padre.Claves.Length && CompareKeys(key, padre.Claves[i]) > 0)  i++;
            //Se hace espacio para el valor a insertar
            for (int j = padre.Claves.Length - 1; j > i; j--)
            {
                padre.Claves[j] = padre.Claves[j - 1];
                padre.Hijos[j + 1] = padre.Hijos[j];
            }
            //Se inserta clave y puntero en la posicion adecuada
            padre.Claves[i] = key;
            padre.Hijos[i + 1] = hijo;
        }

        private void DividirPadre(BPlusNode<K, V> padre, K key, BPlusNode<K, V> hijo)
        {
            //Nuevo nodo que almacena la mitad de las claves y putneros
            BPlusNode<K, V> nuevoPadre = new BPlusNode<K, V>(Grado);
            //punto medio
            int t = Grado / 2;
            // Mover la mitad de las claves y los punteros a los hijos al nuevo nodo
            for (int i = t; i < Grado; i++)
            {
                nuevoPadre.Claves[i - t] = padre.Claves[i];
                nuevoPadre.Hijos[i - t] = padre.Hijos[i + 1];
                nuevoPadre.Hijos[i - t].Padre = nuevoPadre;
                padre.Claves[i] = default;
                padre.Hijos[i + 1] = null;
            }
            // Insertar la clave y el puntero al hijo en el nodo correspondiente
            if (CompareKeys(key, nuevoPadre.Claves[0]) < 0)
            {
                InsertarEnPadre(padre, key, hijo);
            }
            else
            {
                InsertarEnPadre(nuevoPadre, key, hijo);
            }

            //Prooagar la division
            DivisionDivision(padre, nuevoPadre);
        }

        public V BuscarValor(K key)
        {
            BPlusNode<K, V> nodo = Raiz;
            //Mientras el nodo no sea hoja
            while(nodo.Hijos[0] != null)
            {
                int i = 0;
                //Buscar el puntero al nodo donde se buscara la llave
                while (i < nodo.Claves.Length && CompareKeys(key, nodo.Claves[i]) > 0) i++;
                //desplazarse al noso correspondiente
                nodo = nodo.Hijos[i];
            }
            //Buscar la clave en la hoja
            for(int i = 0; i < nodo.Claves.Length; i++)
            {
                if(CompareKeys(nodo.Claves[i], key) == 0)
                {
                    return nodo.Valores[i];
                }
            }
            //Si no se encuentra la llave
            return default;
        }

        public bool Eliminar(K key)
        {
            bool eliminado = false;

            //Buscar el valor a eliminar
            BPlusNode<K, V> NodoBuscado = BuscarNodo(key);
            //Si no se encunetra la llave no hay que eliminar nada
            if (NodoBuscado == null)
            {
                Console.WriteLine("No se encontró al solicitante.");
            }
            else
            {
                //Si se encuentra la llave hayq ue eliminarla
                //Verificar caso de eliminacion
                if (LlaveSoloEnHoja(NodoBuscado, key))
                {
                    //CASO A: La llave que se eliminará esta SOLO en una hoja
                    int indiceKey = Array.IndexOf(NodoBuscado.Claves, key);
                    if(NodoBuscado.Claves.Length > NodoBuscado.MinClaves)
                    {
                        //A.1: En el nodo donde esta la llave hay mas del minimo de llaves
                        //Se crea un nuevo arreglo pero sin la clave buscada, lo mismo para los valores
                        K[] nuevasClaves = NodoBuscado.Claves.Where(x => !x.Equals(key)).ToArray();
                        V[] nuevosValores = NodoBuscado.Valores.Where((x, i) => i != indiceKey).ToArray();
                        NodoBuscado.Claves = nuevasClaves;
                        NodoBuscado.Valores = nuevosValores;
                    }
                    else
                    {
                        //A.2: En el nodo donde esta la llave hay examente el minimo de llaves

                    }

                }
                else
                {

                }
            }

            return eliminado;
        }
        private BPlusNode<K, V> BuscarNodo(K key)
        {
            BPlusNode<K, V> nodo = Raiz;
            //Mientras el nodo no sea hoja
            while (nodo.Hijos[0] != null)
            {
                int i = 0;
                //Buscar el puntero al nodo donde se buscara la llave
                while (i < nodo.Claves.Length && CompareKeys(key, nodo.Claves[i]) > 0) i++;
                //desplazarse al nodo correspondiente
                nodo = nodo.Hijos[i];
            }
            //Si la llave existe en la hoja retorna el nodo
            for(int i = 0; i < nodo.Claves.Length; i++) 
                if(CompareKeys(key, nodo.Claves[i]) == 0) return nodo;
            return null;
        }
        private bool LlaveSoloEnHoja(BPlusNode<K, V> leafNode, K key)
        {
            //Nodo temporal de busqueda
            BPlusNode<K, V> tempNode = leafNode;
            //Mintras no lleguemos a la raiz
            while(tempNode.Padre != null)
            {
                //Se revisa si en el nodo no existe la clave buscada, si existe es que no esta solo en la hoja
                for(int i = 0; i < tempNode.Padre.Claves.Length; i++)
                {
                    if (CompareKeys(key, tempNode.Padre.Claves[i]) == 0) return false;
                }
                tempNode = tempNode.Padre;
            }
            return true;
        }
    }
}
