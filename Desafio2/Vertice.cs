using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Desafio2
{


    public class Vertice
    {
        public string Nombre { get; set; }
        public List<Arista> Adyacencias { get; set; } = new List<Arista>();

        public Vertice(string nombre)
        {
            Nombre = nombre;
        }
    }

    public class Arista
    {
        public Vertice Destino { get; set; }
        public int Distancia { get; set; }

        public Arista(Vertice destino, int distancia)
        {
            Destino = destino;
            Distancia = distancia;
        }
    }

    public class Grafo
    {
        public Dictionary<string, Vertice> Vertices { get; set; } = new Dictionary<string, Vertice>();

        public void AgregarVertice(string nombre)
        {
            Vertices[nombre] = new Vertice(nombre);
        }

        public void AgregarArista(string origen, string destino, int distancia)
        {
            Vertices[origen].Adyacencias.Add(new Arista(Vertices[destino], distancia));
            Vertices[destino].Adyacencias.Add(new Arista(Vertices[origen], distancia)); // Si el grafo es no dirigido
        }

        public List<string> Dijkstra(string inicio, string fin, out int distanciaTotal)
        {
            var distancias = new Dictionary<Vertice, int>();
            var previos = new Dictionary<Vertice, Vertice>();
            var pq = new PriorityQueue<Vertice, int>();

            foreach (var vertice in Vertices.Values)
            {
                distancias[vertice] = int.MaxValue;
                previos[vertice] = null;
            }
            distancias[Vertices[inicio]] = 0;
            pq.Enqueue(Vertices[inicio], 0);

            while (pq.Count > 0)
            {
                var verticeActual = pq.Dequeue();

                if (verticeActual.Nombre == fin)
                {
                    break;
                }

                foreach (var arista in verticeActual.Adyacencias)
                {
                    var distancia = distancias[verticeActual] + arista.Distancia;
                    if (distancia < distancias[arista.Destino])
                    {
                        distancias[arista.Destino] = distancia;
                        previos[arista.Destino] = verticeActual;
                        pq.Enqueue(arista.Destino, distancia);
                    }
                }
            }

            distanciaTotal = distancias[Vertices[fin]];
            var camino = new List<string>();
            for (var v = Vertices[fin]; v != null; v = previos[v])
            {
                camino.Add(v.Nombre);
            }
            camino.Reverse();
            return camino;
        }

        public List<string> Anchura(string inicio, string fin)
        {
            var visitados = new HashSet<Vertice>();
            var cola = new Queue<Vertice>();
            var previos = new Dictionary<Vertice, Vertice>();

            cola.Enqueue(Vertices[inicio]);
            visitados.Add(Vertices[inicio]);

            while (cola.Count > 0)
            {
                var vertice = cola.Dequeue();

                if (vertice.Nombre == fin)
                {
                    break;
                }

                foreach (var arista in vertice.Adyacencias)
                {
                    if (!visitados.Contains(arista.Destino))
                    {
                        visitados.Add(arista.Destino);
                        cola.Enqueue(arista.Destino);
                        previos[arista.Destino] = vertice;
                    }
                }
            }

            var camino = new List<string>();
            Vertice actual = Vertices[fin];
            while (actual != null)
            {
                camino.Add(actual.Nombre);
                previos.TryGetValue(actual, out actual);
            }
            camino.Reverse();
            return camino;
        }

        public List<string> Profundidad(string inicio, string fin)
        {
            var visitados = new HashSet<Vertice>();
            var pila = new Stack<Vertice>();
            var previos = new Dictionary<Vertice, Vertice>();

            pila.Push(Vertices[inicio]);

            while (pila.Count > 0)
            {
                var vertice = pila.Pop();
                if (!visitados.Contains(vertice))
                {
                    visitados.Add(vertice);

                    if (vertice.Nombre == fin)
                    {
                        break;
                    }

                    foreach (var arista in vertice.Adyacencias)
                    {
                        if (!visitados.Contains(arista.Destino))
                        {
                            pila.Push(arista.Destino);
                            previos[arista.Destino] = vertice;
                        }
                    }
                }
            }

            var camino = new List<string>();
            Vertice actual = Vertices[fin];
            while (actual != null)
            {
                camino.Add(actual.Nombre);
                previos.TryGetValue(actual, out actual);
            }
            camino.Reverse();
            return camino;
        }

        public List<string> Anchura2(string inicio)
        {
            var visitados = new HashSet<Vertice>();
            var cola = new Queue<Vertice>();
            var recorrido = new List<string>();

            cola.Enqueue(Vertices[inicio]);
            visitados.Add(Vertices[inicio]);

            while (cola.Count > 0)
            {
                var vertice = cola.Dequeue();
                recorrido.Add(vertice.Nombre);

                foreach (var arista in vertice.Adyacencias)
                {
                    if (!visitados.Contains(arista.Destino))
                    {
                        visitados.Add(arista.Destino);
                        cola.Enqueue(arista.Destino);
                    }
                }
            }

            return recorrido;
        }

        public List<string> Profundidad2(string inicio)
        {
            var visitados = new HashSet<Vertice>();
            var pila = new Stack<Vertice>();
            var recorrido = new List<string>();

            pila.Push(Vertices[inicio]);

            while (pila.Count > 0)
            {
                var vertice = pila.Pop();
                if (!visitados.Contains(vertice))
                {
                    visitados.Add(vertice);
                    recorrido.Add(vertice.Nombre);

                    foreach (var arista in vertice.Adyacencias)
                    {
                        if (!visitados.Contains(arista.Destino))
                        {
                            pila.Push(arista.Destino);
                        }
                    }
                }
            }

            return recorrido;
        }
    }

    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly SortedList<TPriority, Queue<TElement>> _dict = new SortedList<TPriority, Queue<TElement>>();

        public int Count { get; private set; }

        public void Enqueue(TElement element, TPriority priority)
        {
            if (!_dict.TryGetValue(priority, out var q))
            {
                q = new Queue<TElement>();
                _dict.Add(priority, q);
            }
            q.Enqueue(element);
            Count++;
        }

        public TElement Dequeue()
        {
            if (Count == 0) throw new InvalidOperationException("The queue is empty");
            var kv = _dict.First();
            var v = kv.Value.Dequeue();
            if (kv.Value.Count == 0)
            {
                _dict.RemoveAt(0);
            }
            Count--;
            return v;
        }
    }
}

