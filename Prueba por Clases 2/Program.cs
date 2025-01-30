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
        Introduction();
        juego.MostrarMenuInicio();
    }

    static void Introduction()
    {
        Console.Clear();

        AnsiConsole.Write(new FigletText("BIENVENIDOS").Color(Color.Aqua));
        Thread.Sleep(2000); 

        Console.Clear();

        // Mostrar "MAZE RUNNERS" en letras grandes
        AnsiConsole.Write(new FigletText("MAZE RUNNERS").Color(Color.Aqua));
        Thread.Sleep(2000); // Espera 2 segundos antes de continuar

        Console.Clear();

        // Mostrar "¡Bienvenidos al juego de Maze Runners!" en letras grandes
        AnsiConsole.Write(new FigletText("¡Bienvenidos al juego de Maze Runners!").Color(Color.Aqua));
        Thread.Sleep(2000); // Espera 2 segundos antes de continuar

        Console.Clear();

        // Mostrar "¡Comienza la aventura!" en letras grandes
        AnsiConsole.Write(new FigletText("¡Comienza la aventura!").Color(Color.Aqua));
        Thread.Sleep(2000); // Espera 2 segundos antes de continuar

        Console.Clear();

        // Mostrar "¡Prepárate para correr y evadir obstáculos!" en letras grandes
        AnsiConsole.Write(new FigletText("¡Prepárate para correr y evadir obstáculos!").Color(Color.Aqua));
        Thread.Sleep(2000); // Espera 2 segundos antes de continuar
    }
}
