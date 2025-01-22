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

        var table = new Table();
        table.AddColumn("BIENVENIDOS");
        table.AddRow(new Panel(new Markup("[bold green]BIENVENIDOS[/]"))
            .Header(new PanelHeader("[bold cyan]MAZE RUNNERS[/]"))
            .BorderColor(Color.Green)
            .Border(BoxBorder.Rounded));

        AnsiConsole.Render(table);

        Thread.Sleep(2000); // Espera 2 segundos antes de continuar

        Console.Clear();

        var panel = new Panel(new Markup("[bold yellow]¡Bienvenidos al juego de Maze Runners![/]"))
            .Header(new PanelHeader("[bold cyan]¡Comienza la aventura![/]"))
            .BorderColor(Color.Yellow)
            .Border(BoxBorder.Rounded);

        AnsiConsole.Render(panel);

        Thread.Sleep(2000); // Espera 2 segundos antes de continuar

        Console.Clear();

        var mensaje = new Markup("[bold green]BIENVENIDOS[/] [bold blue]al juego de[/] [bold red]Maze Runners[/] [bold yellow]¡Comienza la aventura![/]");
        AnsiConsole.Write(mensaje);

        Thread.Sleep(2000); // Espera 2 segundos antes de continuar

        Console.Clear();

        var mensaje2 = new Markup("[bold cyan]¡Prepárate para[/] [bold green]correr[/] [bold blue]y[/] [bold red]evadir[/] [bold yellow]obstáculos![/]");
        AnsiConsole.Write(mensaje2);

        Thread.Sleep(2000); 
    }
}
