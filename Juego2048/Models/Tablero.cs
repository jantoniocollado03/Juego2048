using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Juego2048.Models
{
     public enum Direccion
    {
        Arriba,
        Abajo,
        Izquierda,
        Derecha
    }
    public class Tablero
    {
        private readonly Random _random = new Random();
        private int[,] _celdas;
        private int _puntuacion;
        public int this[int fila, int columna] => _celdas[fila, columna];

        public Tablero()
        {
            _celdas = new int[4, 4];
            _puntuacion = 0;
        }

        // Exponer la puntuación actual para que la UI pueda mostrarla
        public int Puntuacion => _puntuacion;

        public void Reiniciar()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _celdas[i, j] = 0;
                }
            }

            _puntuacion = 0;

            // Al iniciar una nueva partida, las fichas iniciales siempre deben ser 2
            ColocarFichaInicial();
            ColocarFichaInicial();
        }

        // Coloca una ficha inicial con valor 2 en una posición aleatoria vacía
        private void ColocarFichaInicial()
        {
            var vacias = new List<(int Fila, int Columna)>();

            for (int f = 0; f < 4; f++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (_celdas[f, c] == 0)
                    {
                        vacias.Add((f, c));
                    }
                }
            }

            if (vacias.Count == 0) return;

            var (fila, columna) = vacias[_random.Next(vacias.Count)];

            _celdas[fila, columna] = 2;
        }

        private (int[] fila, int puntos) FusionarFilaIzquierda(int[] fila)
        {
            int[] compactada = fila.Where(v => v != 0).ToArray();

            var resultado = new List<int>();
            int puntosGanados = 0;
            int i = 0;

            while (i < compactada.Length)
            {
                if (i + 1 < compactada.Length && compactada[i] == compactada[i + 1])
                {
                    int fusion = compactada[i] * 2;
                    resultado.Add(fusion);
                    puntosGanados += fusion;

                    i += 2;
                }
                else
                { 
                    resultado.Add(compactada[i]);
                    i += 1;
                }
            }

            while (resultado.Count < 4)
            {
                resultado.Add(0);
            }

            return (resultado.ToArray(), puntosGanados);
        }

        private void Invertir()
        {
            for (int f = 0; f < 4; f++)
            {
                int temp0 = _celdas[f, 0];
                int temp1 = _celdas[f, 1];

                _celdas[f, 0] = _celdas[f, 3];
                _celdas[f, 1] = _celdas[f, 2];
                _celdas[f, 2] = temp1;
                _celdas[f, 3] = temp0;
            }
        }

        private void Transponer()
        {
            int[,] nuevaMatriz = new int[4, 4];
            for (int f = 0; f < 4; f++)
            {
                for (int c = 0; c < 4; c++)
                {
                    nuevaMatriz[c, f] = _celdas[f, c];
                }
            }
            _celdas = nuevaMatriz;
        }


        public bool Mover(Direccion direccion)
        { 
            int[,] original = (int[,])_celdas.Clone();

            if (direccion == Direccion.Derecha) Invertir();
            else if (direccion == Direccion.Arriba) Transponer();
            else if (direccion == Direccion.Abajo) { Transponer(); Invertir(); }

            for (int f = 0; f < 4; f++)
            {
                int[] filaOriginal = { _celdas[f, 0], _celdas[f, 1], _celdas[f, 2], _celdas[f, 3] };
                var resultado = FusionarFilaIzquierda(filaOriginal);

                _celdas[f, 0] = resultado.fila[0];
                _celdas[f, 1] = resultado.fila[1];
                _celdas[f, 2] = resultado.fila[2];
                _celdas[f, 3] = resultado.fila[3];

                _puntuacion += resultado.puntos;
            }

            if (direccion == Direccion.Derecha) Invertir();
            else if (direccion == Direccion.Arriba) Transponer();
            else if (direccion == Direccion.Abajo) { Invertir(); Transponer(); }

            bool algoCambio = false;
            for (int f = 0; f < 4; f++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (original[f, c] != _celdas[f, c])
                    {
                        algoCambio = true;
                    }
                }
            }

            return algoCambio;
        }

        public void ColocarFichaAleatoria()
        {
            var vacias = new List<(int Fila, int Columna)>();

            for (int f = 0; f < 4; f++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (_celdas[f, c] == 0)
                    {
                        vacias.Add((f, c));
                    }
                }
            }

            if (vacias.Count == 0) return;

            var (fila, columna) = vacias[_random.Next(vacias.Count)];

            _celdas[fila, columna] = _random.Next(10) == 0 ? 4 : 2;
        }

        public bool HayFichaGanadora()
        {
            for (int f = 0; f < 4; f++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (_celdas[f, c] == 2048) return true;
                }
            }
            return false;
        }

        public bool HayMovimientosPosibles()
        {
            for (int f = 0; f < 4; f++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (_celdas[f, c] == 0) return true;
                }
            }

            for (int f = 0; f < 4; f++)
            {
                for (int c = 0; c < 4; c++)
                {
                    int actual = _celdas[f, c];

                    if (c < 3 && actual == _celdas[f, c + 1]) return true;

                    if (f < 3 && actual == _celdas[f + 1, c]) return true;
                }
            }

            return false;
        }
    }
}
