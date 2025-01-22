using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
namespace Prueba_por_Clases_2;


public class Juego
{
    private Mapa mapa;
    private Jugador[] jugadores;
    private int turnoActual;
    private bool movimientoExtra;
    private MovimientoJugador movimientoJugador;
    private int filas; 
    private int columnas;
    private IA ia;

    public Juego(int rows, int cols)
    {
        this.filas = rows; 
        this.columnas = cols;
        this.mapa = new Mapa(rows, cols);
        this.jugadores = new Jugador[2];
        this.turnoActual = 0;
        this.movimientoJugador = new MovimientoJugador(mapa, jugadores);
    }

    public void IniciarJuego()
    {
        Console.Clear();
        Console.WriteLine("¡Bienvenido al juego!");
        Console.WriteLine("Presiona cualquier tecla para comenzar...");
        Console.ReadKey();

        int modoJuego = MostrarMenuJuego();

        if (modoJuego == 1)
        {
            InscribirJugador(); 
            CrearIA();
        }
        else
        {
            InscribirJugadores(); 
        }
        Jugar(modoJuego);

    }

    public void Introduction()
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

    public void MostrarMenuInicio()
    {
        Introduction();
        Console.Clear();
        AnsiConsole.MarkupLine("[blue]¡Bienvenidos al Maze Runner![/]");
        Console.WriteLine();
        ImprimirMatrizMenu();
        Console.WriteLine();

        var table = new Table();
        table.AddColumn("Menú de inicio");

        table.AddRow("[bold green]Jugar[/]");

        table.AddRow("[bold blue]Instrucciones[/]");

        table.AddRow("[bold red]Salir[/]");

        table.AddRow("[bold yellow]Créditos[/]");

        table.AddRow("[bold cyan]Acerca de[/]");
        table.Width = 80;
        AnsiConsole.Render(table);

        int opcion = 0;
        bool entradaValida = false;

        while (!entradaValida)
        {
            try
            {
                Console.Write("Elige una opción: ");
                opcion = int.Parse(Console.ReadLine());

                if (opcion >= 1 && opcion <= 5)
                {
                    entradaValida = true;
                }
                else
                {
                    Console.WriteLine("Opción inválida. Por favor, elige un número entre 1 y 5.");
                }
            }
            catch (FormatException) 
            {
                Console.WriteLine("Error: Debes ingresar un número válido. Inténtalo de nuevo.");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }
        }

        switch (opcion)
        {
            case 1:
                IniciarJuego();
                break;
            case 2:
                MostrarInstrucciones();
                break;
            case 3:
                Salir();
                break;
            case 4:
                MostrarCreditos();
                break;
            case 5:
                MostrarAcercaDe();
                break;
            default:
                Console.WriteLine("Opción inválida. Por favor, elige una opción válida.");
                MostrarMenuInicio();
                break;
        }
    }

    private void ImprimirMatrizMenu()
    {
        var matrizEjemplo = new string[10, 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (i == 0 && j == 0)
                {
                    matrizEjemplo[i, j] = "🏃"; 
                }
                else if (i == 9 && j == 9)
                {
                    matrizEjemplo[i, j] = "🏆"; 
                }
                else if (i == 5 && j == 5)
                {
                    matrizEjemplo[i, j] = "🚪"; 
                }
                else if (i == 2 && j == 7)
                {
                    matrizEjemplo[i, j] = "⚡"; 
                }
                else if (i == 3 && j == 3)
                {
                    matrizEjemplo[i, j] = "🌳"; 
                }
                else if (i == 7 && j == 2)
                {
                    matrizEjemplo[i, j] = "💰"; 
                }
                else
                {
                    matrizEjemplo[i, j] = "⬜"; 
                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write(matrizEjemplo[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public int MostrarMenuJuego()
    {
        var table = new Table();
        table.AddColumn("Menú de juego");
        table.AddRow("[bold green]Juego contra la IA[/]");
        table.AddRow("[bold blue]Juego contra otro jugador[/]");

        AnsiConsole.Render(table);

        Console.Write("Elige una opción: ");
        int opcion = int.Parse(Console.ReadLine());

        return opcion;
    }
    public void Salir()
    {
        Console.Clear();
        Console.WriteLine("¡Gracias por jugar!");
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
        Environment.Exit(0);
    }
    public void MostrarCreditos()
    {
        Console.Clear();
        Console.WriteLine("Créditos:");
        Console.WriteLine("-------------------------");
        Console.WriteLine("Desarrollador: [Josué Alejandro Tamayo Ayrado]");
        Console.WriteLine("Versión: 1.0");
        Console.WriteLine("Fecha de lanzamiento: [En modificación ún]");
        Console.WriteLine("-------------------------");
        Console.WriteLine("Presiona cualquier tecla para regresar al menú...");
        Console.ReadKey();
        MostrarMenuInicio();
    }

    public void MostrarAcercaDe()
    {
        Console.Clear();
        Console.WriteLine("Acerca de:");
        Console.WriteLine("-------------------------");
        Console.WriteLine("Este juego es un proyecto de [Josué Alejandro Tamayo Ayrado] para [Volver a mi Niñez].");
        Console.WriteLine("El juego es un ejemplo de cómo se puede crear un juego en C# utilizando la biblioteca Spectre.Console.");
        Console.WriteLine("-------------------------");
        Console.WriteLine("Presiona cualquier tecla para regresar al menú...");
        Console.ReadKey();
        MostrarMenuInicio();
    }

    private void InscribirJugador()
    {
        Console.Write("Ingrese el nombre del Jugador: ");
        string nombre = Console.ReadLine();
        jugadores[0] = new Jugador(nombre, 1, 1); 
    }

    private void CrearIA()
    {
        ia = new IA("IA", 1, 25);
        jugadores[1] = ia.GetJugadorIA(); 
    }

    private void InscribirJugadores()
    {
        for (int i = 0; i < 2; i++)
        {
            Console.Write($"Ingresa el nombre del Jugador {i + 1}: ");
            string nombre = Console.ReadLine();
            int fila = 1;  // Fila para ambos jugadores
            int columna = i == 0 ? 1 : 25; // Jugador 1 en (1, 1), Jugador 2 en (1, 26)
            jugadores[i] = new Jugador(nombre, fila, columna);
        }
    }

    

    private void MostrarInstrucciones()
    {
        Console.Clear();
        var panel = new Panel(new Markup(
            "[bold yellow]Instrucciones del Juego:[/]\n" +
            "1. [bold blue]Objetivo del juego:[/] El objetivo del juego es evitar que la IA te capture mientras intentas llegar a la meta.\n" +
            "2. [bold blue]Movimientos:[/] Puedes moverte en cualquier dirección (arriba, abajo, izquierda, derecha) utilizando las teclas W, A, S, D.\n" +
            "3. [bold blue]IA:[/] La IA se moverá en un patrón de movimiento determinado y intentará capturarte si te detecta.\n" +
            "4. [bold blue]Detección:[/] La IA solo puede detectarte si estás dentro de un rango determinado de casillas.\n" +
            "5. [bold blue]Captura:[/] Si la IA te detecta, tiene una probabilidad de capturarte. Si te captura, el juego terminará.\n" +
            "6. [bold blue]Meta:[/] La meta es el objetivo final del juego. Debes intentar llegar a la meta sin que la IA te capture.\n" +
            "\n" +
            "[bold green]Consejos:[/]\n" +
            "1. [bold blue]Mantén la distancia:[/] Intenta mantener la distancia con la IA para evitar que te detecte.\n" +
            "2. [bold blue]Utiliza el patrón de movimiento:[/] Utiliza el patrón de movimiento de la IA para anticipar sus movimientos y evitar que te capture.\n" +
            "3. [bold blue]Mantén la calma:[/] No te desanimes si la IA te detecta. Intenta mantener la calma y encontrar una forma de escapar.\n" +
            "\n" +
            "[bold red]Notas:[/]\n" +
            "1. [bold blue]La IA se vuelve más agresiva:[/] A medida que avanzas en el juego, la IA se vuelve más agresiva y tiene más probabilidad de capturarte.\n" +
            "2. [bold blue]La meta se vuelve más difícil de alcanzar:[/] A medida que avanzas en el juego, la meta se vuelve más difícil de alcanzar debido a la presencia de la IA.\n" +
            "\n" +
            "[bold blue]¡Buena suerte! Presiona cualquier tecla para continuar...[/]"
        ))
        .Header(new PanelHeader("[bold cyan]Instrucciones[/]"))
        .BorderColor(Color.Green)
        .Border(BoxBorder.Rounded);

        AnsiConsole.Render(panel);
        Console.ReadKey();
        MostrarMenuInicio();

    }

    public void Jugar(int modoJuego)
    {
        while (true)
        {
            if (jugadores == null || jugadores.Length < 2)
            {
                Console.WriteLine("Error: Los jugadores no están correctamente inicializados.");
                return; 
            }
            for(int i = 0; i < 3; i++)
            {
                movimientoJugador.MoverJugador(1);
            }

            if (VerificarVictoria(jugadores[0]))
            {
                MostrarMensajeVictoria(jugadores[0].Nombre);
                break;
            }

            if (modoJuego == 1)
            {
                ia.Mover(mapa, jugadores[0]);

                if (VerificarVictoria(ia.GetJugadorIA()))
                {
                    MostrarMensajeVictoria(ia.GetJugadorIA().Nombre);
                    break;
                }
            }

            else if (modoJuego == 2)
            {
                for( int i = 0; i < 3; i++)
                    movimientoJugador.MoverJugador(2); 

                if (VerificarVictoria(jugadores[1]))
                {
                    MostrarMensajeVictoria(jugadores[1].Nombre);
                    break; 
                }
            }
            turnoActual = (turnoActual + 1) % jugadores.Length;
        }
    }
    
    
    private bool VerificarVictoria(Jugador jugador)
    {
        int filaMeta = 20;
        int columnaMeta = 20;

        return jugador.Position[0] == filaMeta && jugador.Position[1] == columnaMeta;
    }

    private void MostrarMensajeVictoria(string nombreGanador)
    {
        
         AnsiConsole.Status()
        .Spinner(Spinner.Known.CircleQuarters)
        .Start("Celebrando la victoria...", (_) =>
        {
            AnsiConsole.Clear();
            var panel = new Panel(new Markup($"¡Felicidades, {nombreGanador}!"))
                .Header(new PanelHeader("¡Victoria!"))
                .BorderColor(Color.Green)
                .Border(BoxBorder.Rounded);
            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[green]¡Has demostrado ser el mejor en este juego![/]");
            AnsiConsole.MarkupLine("[blue]¡Tu habilidad y estrategia te han llevado a la victoria![/]");
            AnsiConsole.MarkupLine("[red]¡No te rindas, sigue adelante y sigue mejorando![/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold blue]🎲 Puntos {nombreGanador} : [/][red]1000[/]");
            AnsiConsole.WriteLine();
            var table = new Table();
            table.AddColumn("Estadísticas de la partida");
            table.AddRow($"Nombre del jugador: {nombreGanador}");
            table.AddRow($"Puntuación: 1000");
            table.AddRow($"Tiempo de juego: 10 minutos");
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold blue]¡Gracias por jugar! Esperamos verte de nuevo pronto.[/]");
            AnsiConsole.MarkupLine("[bold blue]Presiona cualquier tecla para salir...[/]");
            Console.ReadKey(); // Esperar a que el jugador presione una tecla
        });
        
    }
}