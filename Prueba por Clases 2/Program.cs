using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using System.Threading;

namespace Prueba_por_Clases_2;

class Program
{
    static void Main(string[] args)
    {
        Jugador jugador1 = new Jugador("Jugador 1", 1, 1); // Posición inicial (1, 1)
        Jugador jugador2 = new Jugador("Jugador 2", 1, 25); // Posición inicial (1, 2)

        Jugador[] jugadores = new Jugador[] { jugador1, jugador2 };

        int filas = 27; // Número de filas del mapa
        int columnas = 27; // Número de columnas del mapa
        Mapa mapa = new Mapa(filas, columnas, jugadores);

        Juego juego = new Juego(filas, columnas, jugadores);
        juego.Jugar();
    }
}
