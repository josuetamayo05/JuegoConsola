using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using System.Diagnostics;
namespace Prueba_por_Clases_2;
public class Juego
{
    private Stopwatch temporizador;
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
        temporizador = new Stopwatch();
    }

    public void IniciarJuego()
    {
        
        Console.Clear();
        AnsiConsole.Write(new FigletText("Hola").Color(Color.Aqua));
        Console.WriteLine("¡Bienvenido al juego!");
        Console.WriteLine("Presiona cualquier tecla para comenzar...");
        Console.ReadKey();

        temporizador.Start();
        int modoJuego = MostrarMenuJuego();
        var personajes = new[]
        {
            new { Nombre = "Invoker", Emoji = "👾" },
            new { Nombre = "Sofia", Emoji = "👧" },
            new { Nombre = "Luis", Emoji = "🤖" },
            new { Nombre = "Ana", Emoji = "👸" },
            new { Nombre = "Juan", Emoji = "🤓" },
        };

        if (modoJuego == 1)
        {

            Console.WriteLine("Elige un personaje:");
            for (int i = 0; i < personajes.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {personajes[i].Nombre} {personajes[i].Emoji}");
            }
            Console.Write("Ingrese el número del personaje que desea seleccionar: ");
            int seleccion = Convert.ToInt32(Console.ReadLine()) - 1;
            if (seleccion >= 0 && seleccion < personajes.Length)
            {
                var jugador = personajes[seleccion];
                jugadores[0] = new Jugador(jugador.Nombre, 1, 1, jugador.Emoji);
            }
            else
            {
                Console.WriteLine("Selección inválida");
            }

            InscribirJugador();
            CrearIA();
        }
        else
        {
            InscribirJugadores(); 
        }
        Jugar(modoJuego);
    }
    private void CrearIA()
    {
        ia = new IA();
        jugadores[1] = ia.GetJugadorIA(); 
    }

    private void InscribirJugadores()
    {
        int opcion = 0;
        bool entradaValida = false;
        var personajes = new[]
        {
            new { Nombre = "Invoker", Emoji = "👾" },
            new { Nombre = "Sofia", Emoji = "👧" },
            new { Nombre = "Luis", Emoji = "🤖" },
            new { Nombre = "Ana", Emoji = "👸" },
            new { Nombre = "Juan", Emoji = "🤓" },
        };

        for (int i = 0; i < 2; i++)
        {
            Console.Write($"Ingresa el nombre del Jugador {i + 1}: ");
            string nombre = Console.ReadLine()!;
            while(!entradaValida)
            {
                try
                {
                    Console.WriteLine("Elige un personaje:");
                    for (int j = 0; j < personajes.Length; j++)
                    {
                        Console.WriteLine($"{j + 1}. {personajes[j].Nombre} - {personajes[j].Emoji}");
                    }
                    opcion = int.Parse(Console.ReadLine()!);
                    if(opcion >= 1 || opcion <= personajes.Length) 
                    {
                        entradaValida = true;
                    }
                    else
                    {
                        Console.WriteLine("Opción inválida. Por favor, elige un número entre 1 y " + personajes.Length);
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Error: Debes ingresar un número válido. Inténtalo de nuevo.");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine("Error: El número ingresado es demasiado grande. Inténtalo de nuevo.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inesperado: " + ex.Message);
                }
            }
            int fila = 1;  
            int columna = i == 0 ? 1 : 25; 
            var personaje = personajes[opcion - 1];
            jugadores[i] = new Jugador(nombre, fila, columna, personaje.Emoji);

            entradaValida = false;
        }
    }
    private void InscribirJugador()
    {
        int opcion = 0;
        bool entradaValida = false;
        var personajes = new[]
        {
            new { Nombre = "Invoker", Emoji = "👾" },
            new { Nombre = "Sofia", Emoji = "👧" },
            new { Nombre = "Luis", Emoji = "🤖" },
            new { Nombre = "Ana", Emoji = "👸" },
            new { Nombre = "Juan", Emoji = "🤓" },
        };

        Console.Write("Ingresa el nombre del Jugador: ");
        string nombre = Console.ReadLine()!;

        while (!entradaValida)
        {
            try
            {
                Console.WriteLine("Elige un personaje:");
                for (int j = 0; j < personajes.Length; j++)
                {
                    Console.WriteLine($"{j + 1}. {personajes[j].Nombre} - {personajes[j].Emoji}");
                }
                opcion = int.Parse(Console.ReadLine()!);

                if (opcion >= 1 && opcion <= personajes.Length)
                {
                    entradaValida = true;
                }
                else
                {
                    Console.WriteLine("Opción inválida. Por favor, elige un número entre 1 y " + personajes.Length);
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error: Debes ingresar un número válido. Inténtalo de nuevo.");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine("Error: El número ingresado es demasiado grande. Inténtalo de nuevo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inesperado: " + ex.Message);
            }
        }

        int fila = 1;
        int columna = 1;

        var personaje = personajes[opcion - 1];
        jugadores[0] = new Jugador(nombre, fila, columna, personaje.Emoji);

        ia = new IA();
        jugadores[1] = ia.GetJugadorIA();
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
                if (VerificarVictoria(jugadores[0]))
                {
                    temporizador.Stop();
                    MostrarMensajeVictoria(jugadores[0].Nombre);
                    break;
                }
            }

            if (VerificarVictoria(jugadores[0]))
            {
                temporizador.Stop();
                MostrarMensajeVictoria(jugadores[0].Nombre);
                break;
            }

            if (modoJuego == 1)
            {
                ia.Mover(mapa, jugadores[0]);

                if (VerificarVictoria(ia.GetJugadorIA()))
                {
                    temporizador.Stop();
                    MostrarMensajeVictoria(ia.GetJugadorIA().Nombre);
                    break;
                }
            }

            else if (modoJuego == 2)
            {
                for( int i = 0; i < 3; i++)
                {
                    movimientoJugador.MoverJugador(2); 
                    if (VerificarVictoria(jugadores[1]))
                    {
                        temporizador.Stop();
                        MostrarMensajeVictoria(jugadores[1].Nombre);
                        break; 
                    }
                }
            }
            turnoActual = (turnoActual + 1) % jugadores.Length;
        }
    }

    private bool VerificarVictoria(Jugador jugador)
    {
        int filaMeta = 25;
        int columnaMeta = 13;

        return jugador.Position[0] == filaMeta && jugador.Position[1] == columnaMeta;
    }

    public void MostrarMenuInicio()
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("Welcome Maze Runner").Color(Color.Aqua));
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

        table.AddRow("[bold cyan]Lista de Jugadores[/]");

        table.Width = 80;
        AnsiConsole.Render(table);

        int opcion = 0;
        bool entradaValida = false;

        while (!entradaValida)
        {
            try
            {
                Console.Write("Elige una opción: ");
                opcion = int.Parse(Console.ReadLine()!);

                if (opcion >= 1 && opcion <= 6)
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
            case 6:
                ListaJugadores();
                break;
            default:
                Console.WriteLine("Opción inválida. Por favor, elige una opción válida.");
                MostrarMenuInicio();
                break;
        }
    }

    public void ListaJugadores()
    {
        Console.Clear();
        var panel = new Panel(new Markup(
            "[bold green]¡Elige tu personaje![/]\n\n" +
            "[bold yellow]1. 🏃 Corredor[/]\n" +
            "  - Velocidad: +20% de velocidad\n" +
            "  - Acción especial: Correr 2 casillas adicionales\n\n" +
            "[bold yellow]2. 👻 Fantasma[/]\n" +
            "  - Invisibilidad: No puede ser visto por la IA\n" +
            "  - Acción especial: Teletransportarse a una casilla aleatoria\n\n" +
            "[bold yellow]3. 🌀 Teleportador[/]\n" +
            "  - Teletransporte: Teletransportarse a una casilla aleatoria\n" +
            "  - Acción especial: Teletransportar a la IA a una casilla aleatoria\n\n" +
            "[bold yellow]4. 🛡️ Defensor[/]\n" +
            "  - Escudo: +20% de defensa\n" +
            "  - Acción especial: Bloquear un ataque de la IA\n\n" +
            "[bold yellow]5. 🕴️ Doble[/]\n" +
            "  - Doble movimiento: Moverse 2 casillas en un turno\n" +
            "  - Acción especial: Moverse 3 casillas en un turno\n\n" +
            "[bold green]Presiona el número correspondiente para elegir tu personaje...[/]"
        ))
        .Header(new PanelHeader("[bold cyan]Menú de personajes[/]"))
        .BorderColor(Color.Green)
        .Border(BoxBorder.Rounded);

        AnsiConsole.Render(panel);
        Console.ReadKey();
        MostrarMenuInicio();
    }
    private List<Personaje> personajes = new List<Personaje>()
    {
        new Personaje("Corredor", "", PoderEspecial.Velocidad),
        new Personaje("Fantasma", "", PoderEspecial.Invisibilidad),
        new Personaje("Teleportador", "", PoderEspecial.Teletransporte),
        new Personaje("Defensor", "", PoderEspecial.Escudo),
        new Personaje("Doble", "", PoderEspecial.DobleMovimiento)
    };

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
        int opcion = 0;
        bool entradaValida = false;
        while(!entradaValida)
        {
            try
            {
                Console.Write("Elige una opción: ");
                opcion = int.Parse(Console.ReadLine()!);
                if (opcion == 1 || opcion == 2) entradaValida = true;
                else
                {
                    Console.WriteLine("Opción inválida. Por favor, elige 1 para jugar contra la IA o 2 para jugar contra un jugador.");
                    Console.WriteLine("Presiona 'c' para cancelar y regresar al menú principal.");
                    string cancelar = Console.ReadLine()!.ToLower();

                    if (cancelar == "c")
                    {
                        MostrarMenuInicio();
                    }
                }                
            }
            catch(FormatException ex)
            {
                Console.WriteLine("Error: Debes ingresar un número válido. Inténtalo de nuevo.");
            }
            catch(OverflowException ex)  
            {
                Console.WriteLine("Error: El número ingresado es demasiado grande. Inténtalo de nuevo.");
            }
            catch(Exception ex) 
            {
               Console.WriteLine("Error inesperado: + ex.Message");
            }
        }
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

    
    private void MostrarMensajeVictoria(string nombreGanador)
    {
        TimeSpan tiempoTranscurrido = temporizador.Elapsed;
        string tiempoFormateado = string.Format("{0:00}:{1:00}:{2:00}", tiempoTranscurrido.Hours, tiempoTranscurrido.Minutes, tiempoTranscurrido.Seconds);   
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