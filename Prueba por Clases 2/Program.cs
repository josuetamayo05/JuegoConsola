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
        int filas = 27; 
        int columnas = 27; 
        Juego juego = new Juego(filas, columnas);
        juego.MostrarMenuInicio();
    }
}
