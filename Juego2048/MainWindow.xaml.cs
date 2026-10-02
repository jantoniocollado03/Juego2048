using Juego2048.Models;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Juego2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Border[,] _celdas = new Border[4, 4];
        private readonly Tablero _tablero = new Tablero();

        public MainWindow()
        {
            InitializeComponent();

            for (int fila = 0; fila < 4; fila++)
            {
                for (int columna = 0; columna < 4; columna++)
                {
                    Border border = new Border();
                    TextBlock textBlock = new TextBlock();

                    border.Child = textBlock;

                    GridBoard.Children.Add(border);

                    _celdas[fila, columna] = border;
                }
            }

            _tablero.Reiniciar();
            
            ActualizarTablero();

            this.KeyDown += Window_KeyDown;

            BtnNewGame.Click += btnNuevaPartida_Click_1;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Direccion? direccion = e.Key switch
            {
                Key.Up or Key.W => Direccion.Arriba,
                Key.Down or Key.S => Direccion.Abajo,
                Key.Left or Key.A => Direccion.Izquierda,
                Key.Right or Key.D => Direccion.Derecha,
                _ => null
            };

            if (direccion != null)
            {
                bool algoCambio = _tablero.Mover(direccion.Value);

                if (algoCambio)
                {
                    _tablero.ColocarFichaAleatoria();
                    ActualizarTablero();
                }
            }
        }

        private void ActualizarTablero()
        {
            for (int fila = 0; fila < 4; fila++)
            {
                for (int columna = 0; columna < 4; columna++)
                {
                    int valor = _tablero[fila, columna];

                    Border border = _celdas[fila, columna];
                    TextBlock textBlock = (TextBlock)border.Child;

                    ActualizarAparienciaCelda(border, textBlock, valor);
                }
            }

            TxtScore.Text = _tablero.Puntuacion.ToString();
        }
        private void btnNuevaPartida_Click_1(object sender, RoutedEventArgs e)
        {
            _tablero.Reiniciar();
            ActualizarTablero();

            this.Focus();
        }

        private void ActualizarAparienciaCelda(Border border, TextBlock textBlock, int valor)
        {
            string colorFondo = "";
            string colorTexto = "#FFFFFF";

            switch (valor)
            {
                case 0:
                    colorFondo = "#CDC1B4";
                    textBlock.Text = "";
                    break;
                case 2:
                    colorFondo = "#EEE4DA";
                    colorTexto = "#776E65";
                    textBlock.Text = "2";
                    break;
                case 4:
                    colorFondo = "#EDE0C8";
                    colorTexto = "#776E65";
                    textBlock.Text = "4";
                    break;
                case 8:
                    colorFondo = "#F2B179";
                    textBlock.Text = "8";
                    break;
                case 16:
                    colorFondo = "#F59563";
                    textBlock.Text = "16";
                    break;
                case 32:
                    colorFondo = "#F67C5F";
                    textBlock.Text = "32";
                    break;
                case 64:
                    colorFondo = "#F65E3B";
                    textBlock.Text = "64";
                    break;
                default:
                    colorFondo = "#EDCF72";
                    textBlock.Text = valor.ToString();
                    break;
            }

            var converter = new BrushConverter();
            border.Background = (SolidColorBrush)converter.ConvertFromString(colorFondo);

            if (valor > 0)
            {
                textBlock.Foreground = (SolidColorBrush)converter.ConvertFromString(colorTexto);
            }
        }
    }
}