using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desafio2
{


    public partial class Form1 : Form
    {
        private Grafo grafo;
        public Form1()
        {
            InitializeComponent();
      
            InicializarGrafo();
            LlenarComboBoxes();
            DibujarGrafo(null);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inicio = comboBoxInicio.SelectedItem.ToString();
            string fin = comboBoxFin.SelectedItem.ToString();
            List<string> recorrido = grafo.Anchura(inicio, fin);
            MessageBox.Show($"Ruta más corta por Anchura: {string.Join(" -> ", recorrido)}");
            DibujarGrafo(recorrido);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string inicio = comboBoxInicio.SelectedItem.ToString();
            string fin = comboBoxFin.SelectedItem.ToString();
            List<string> recorrido = grafo.Profundidad(inicio, fin);
            MessageBox.Show($"Ruta más corta por Profundidad: {string.Join(" -> ", recorrido)}");
            DibujarGrafo(recorrido);

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string inicio = comboBoxInicio.SelectedItem.ToString();
            string fin = comboBoxFin.SelectedItem.ToString();
            var camino = grafo.Dijkstra(inicio, fin, out int distanciaTotal);
            MessageBox.Show($"Camino más corto de {inicio} a {fin} (Dijkstra): {string.Join(" -> ", camino)}\nDistancia total: {distanciaTotal}");
            DibujarGrafo(camino);
        }

        private void pictureBoxGrafo_Paint(object sender, PaintEventArgs e)
        {

        }


        private void InicializarGrafo()
        {
            grafo = new Grafo();

            grafo.AgregarVertice("Ahuachapan");
            grafo.AgregarVertice("Santa Ana");
            grafo.AgregarVertice("Sonsonate");
            grafo.AgregarVertice("Chalatenango");
            grafo.AgregarVertice("Cuscatlan");
            grafo.AgregarVertice("San Salvador");
            grafo.AgregarVertice("Cabañas");
            grafo.AgregarVertice("San Vicente");
            grafo.AgregarVertice("La Paz");
            grafo.AgregarVertice("La Libertad");
            grafo.AgregarVertice("Usulutan");
            grafo.AgregarVertice("San Miguel");
            grafo.AgregarVertice("Morazan");
            grafo.AgregarVertice("La Union");

         
            grafo.AgregarArista("Ahuachapan", "Santa Ana", 83);
            grafo.AgregarArista("Ahuachapan", "Sonsonate", 56);
            grafo.AgregarArista("Santa Ana", "Chalatenango", 53);
            grafo.AgregarArista("Santa Ana", "La Libertad", 45);
            grafo.AgregarArista("Santa Ana", "Sonsonate", 76);
            grafo.AgregarArista("Sonsonate", "La Libertad", 38);
            grafo.AgregarArista("Chalatenango", "La Libertad", 67);
            grafo.AgregarArista("Chalatenango", "Cuscatlan", 76);
            grafo.AgregarArista("Chalatenango", "San Salvador", 74);
            grafo.AgregarArista("Chalatenango", "Cabañas", 67);
            grafo.AgregarArista("Cuscatlan", "San Vicente", 49);
            grafo.AgregarArista("Cuscatlan", "San Salvador", 55);
            grafo.AgregarArista("Cuscatlan", "Cabañas", 56);
            grafo.AgregarArista("Cuscatlan", "La Paz", 88);
            grafo.AgregarArista("San Salvador", "La Libertad", 64);
            grafo.AgregarArista("San Salvador", "La Paz", 78);
            grafo.AgregarArista("San Vicente", "Usulutan", 82);
            grafo.AgregarArista("San Vicente", "Cabañas", 39);
            grafo.AgregarArista("San Vicente", "La Paz", 48);
            grafo.AgregarArista("San Vicente", "San Miguel", 97);
            grafo.AgregarArista("Usulutan", "San Miguel", 77);
            grafo.AgregarArista("San Miguel", "Morazan", 66);
            grafo.AgregarArista("San Miguel", "La Union", 67);
            grafo.AgregarArista("Morazan", "La Union", 54);
        }

        private void LlenarComboBoxes()
        {
            comboBoxInicio.Items.AddRange(grafo.Vertices.Keys.ToArray());
            comboBoxFin.Items.AddRange(grafo.Vertices.Keys.ToArray());

            comboBoxInicio.SelectedIndex = 0;
            comboBoxFin.SelectedIndex = 0;

            foreach (var vertice in grafo.Vertices.Values)
            {
                puntoInicio2.Items.Add(vertice.Nombre);
            }
        }

        private List<string> _currentPath;
        private int _currentIndex;
        private Timer _timer;

        private void DibujarGrafo(List<string> recorrido)
        {
            _currentPath = recorrido ?? new List<string>();
            _currentIndex = 0;

            // Configurar y comenzar el temporizador
            _timer = new Timer();
            _timer.Interval = 500; // Intervalo de tiempo en milisegundos (0.5 segundos)
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_currentIndex >= _currentPath.Count)
            {
                // Si hemos recorrido toda la ruta, detener el temporizador
                _timer.Stop();
                _timer.Dispose();
                return;
            }

            // Obtener el nombre del nodo actual
            string currentNodeName = _currentPath[_currentIndex];

            // Dibujar el grafo con el nodo actual coloreado
            Bitmap bitmap = new Bitmap(pictureBoxGrafo.Width, pictureBoxGrafo.Height);
            Graphics g = Graphics.FromImage(bitmap);

            // Dibujar aristas
            foreach (var vertice in grafo.Vertices.Values)
            {
                Point pos1 = ObtenerPosicion(vertice.Nombre);
                foreach (var arista in vertice.Adyacencias)
                {
                    Point pos2 = ObtenerPosicion(arista.Destino.Nombre);
                    bool esParteDelRecorrido = _currentPath.Contains(vertice.Nombre) && _currentPath.Contains(arista.Destino.Nombre);
                    Pen pen = esParteDelRecorrido ? new Pen(Color.Red, 2) : Pens.Black;
                    g.DrawLine(pen, pos1, pos2);
                    g.DrawString(arista.Distancia.ToString(), new Font("Arial", 8), Brushes.Black, (pos1.X + pos2.X) / 2, (pos1.Y + pos2.Y) / 2);
                }
            }

            // Dibujar vértices
            foreach (var vertice in grafo.Vertices.Values)
            {
                Point pos = ObtenerPosicion(vertice.Nombre);
                Brush brush = Brushes.LightBlue;
                if (vertice.Nombre == currentNodeName)
                {
                    brush = Brushes.Green; // Cambiar el color del nodo actual a verde
                }
                g.FillEllipse(brush, pos.X - 10, pos.Y - 10, 20, 20);
                g.DrawEllipse(Pens.Black, pos.X - 10, pos.Y - 10, 20, 20);
                g.DrawString(vertice.Nombre, new Font("Arial", 8), Brushes.Black, pos.X + 12, pos.Y);
            }

            pictureBoxGrafo.Image = bitmap;

            _currentIndex++;
        }

        private Point ObtenerPosicion(string nombre)
        {
            // Define posiciones fijas para cada vértice 
            switch (nombre)
            {
                case "Ahuachapan": return new Point(60,400); 
                case "Santa Ana": return new Point(120, 200); 
                case "Sonsonate": return new Point(230, 450); 
                case "Chalatenango": return new Point(300, 100); 
                case "Cuscatlan": return new Point(500, 200); 
                case "San Salvador": return new Point(400, 290);
                case "Cabañas": return new Point(650, 100); 
                case "San Vicente": return new Point(630, 250); 
                case "La Paz": return new Point(570, 365); 
                case "La Libertad": return new Point(330, 380); 
                case "Usulutan": return new Point(720, 430);
                case "San Miguel": return new Point(790, 290); 
                case "Morazan": return new Point(940, 200); 
                case "La Union": return new Point(990, 350); 
                default: return new Point(0, 0);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
           
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string inicio = puntoInicio2.SelectedItem.ToString();
            var recorrido = grafo.Anchura2(inicio);
            MessageBox.Show("Recorrido en Anchura: " + string.Join(" -> ", recorrido));
            DibujarGrafo(recorrido);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            string inicio = puntoInicio2.SelectedItem.ToString();
            var recorrido = grafo.Profundidad2(inicio);
            MessageBox.Show("Recorrido en Anchura : " + string.Join(" -> ", recorrido));
            DibujarGrafo(recorrido);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxInicio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxFin_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

