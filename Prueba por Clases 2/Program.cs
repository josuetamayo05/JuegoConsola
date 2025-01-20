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
        int filas = 27; // Número de filas del mapa
        int columnas = 27; // Número de columnas del mapa
        Juego juego = new Juego(filas, columnas);
        juego.MostrarMenuInicio();
    }
}
