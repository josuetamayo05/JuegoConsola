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
    private int filas; // Declarar filas
    private int columnas;
    private IA ia;


    public Juego(int rows, int cols)
    {
        this.filas = rows; // Inicializar filas
        this.columnas = cols;
        this.mapa = new Mapa(rows, cols);
        this.jugadores = new Jugador[2];
        this.turnoActual = 0;
        this.movimientoJugador = new MovimientoJugador(mapa, jugadores);
    }

    public void IniciarJuego()
    {
        int modoJuego = ElegirModoJuego();
        if (modoJuego == 1)
        {
            InscribirJugador(); // inscrib humano
            CrearIA();
        }
        else
        {
            InscribirJugadores(); //inscrib dos jugad
        }
        MostrarInstrucciones();
        Jugar(modoJuego);
    }

    private int ElegirModoJuego()
    {
        Console.WriteLine("¿Cuántos jugadores van a jugar?");
        Console.WriteLine("1. Un jugador (contra la IA)");
        Console.WriteLine("2. Dos jugadores (multijugador)");
        Console.Write("Elige una opción (1 o 2): ");
        int opcion = int.Parse(Console.ReadLine());
        return opcion;
    }

    private void InscribirJugador()
    {
        Console.Write("Ingrese el nombre del Jugador: ");
        string nombre = Console.ReadLine();
        jugadores[0] = new Jugador(nombre, 1, 1); //jug humano en (0,0)
    }

    private void CrearIA()
    {
        ia = new IA("IA", 1, 25);
        jugadores[1] = ia.GetJugadorIA(); // IA 2do jugador
    }

    private void InscribirJugadores()
    {
        for (int i = 0; i < 2; i++)
        {
            Console.Write($"Ingresa el nombre del Jugador {i + 1}: ");
            string nombre = Console.ReadLine();
            // Asignar posiciones iniciales
            int fila = 1;  // Fila para ambos jugadores
            int columna = i == 0 ? 1 : 25; // Jugador 1 en (1, 1), Jugador 2 en (1, 26)
            jugadores[i] = new Jugador(nombre, fila, columna);
        }
    }

    private void MostrarInstrucciones()
    {
        var panel = new Panel(new Markup(
            "[bold yellow]Instrucciones del Juego:[/]\n" +
            "1. Usa las teclas W, A, S, D para mover a tu jugador: 🏃‍♂️\n" +
            "   - W: Arriba ⬆\n" +
            "   - A: Izquierda ⬅\n" +
            "   - S: Abajo ⬇\n" +
            "   - D: Derecha ➡\n" +
            "2. 🚧 Evita los obstáculos marcados con '⛔'.\n" +
            "3. 🎁 Recoge las fichas de recompensa marcadas con '💰' para ganar puntos.\n" +
            "4. 😋 Por cada ficha recompensa que cojas puedes hacer un movimiento extra.\n" +
            "5. 🏁 Llega a la meta marcada con '🚩' para ganar el juego.\n" +
            "¡Buena Suerte! Presiona cualquier tecla para continuar... ⌨"
        ));

        panel.Border = BoxBorder.Rounded; 
        panel.Header = new PanelHeader("[bold cyan]Instrucciones[/]"); // Encabezado del panel
        AnsiConsole.Render(panel);
        Console.ReadKey(); 
        Console.Clear();
    }

    public void Jugar(int modoJuego)
    {

        while (true)
        {
            if (jugadores == null || jugadores.Length < 2)
            {
                Console.WriteLine("Error: Los jugadores no están correctamente inicializados.");
                return; // Salir del método si hay un problema
            }
            
            movimientoJugador.MoverJugador(1);

            if (VerificarVictoria(jugadores[0]))
            {
                MostrarMensajeVictoria(jugadores[0].Nombre);
                MostrarResultados();
                break;
            }

            if (modoJuego == 1)
            {
                ia.Mover(mapa, jugadores[0]);

                if (VerificarVictoria(ia.GetJugadorIA()))
                {
                    MostrarMensajeVictoria(ia.GetJugadorIA().Nombre);
                    MostrarResultados();
                    break;
                }
            }

            else if (modoJuego == 2)
            {
                movimientoJugador.MoverJugador(2); // Mover al segundo jugador

                if (VerificarVictoria(jugadores[1]))
                {
                    MostrarMensajeVictoria(jugadores[1].Nombre);
                    MostrarResultados();
                    break; // Salir del bucle si el jugador 2 gana
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
        for (int i = 0; i < 5; i++)
        {
            // Imprimir el mensaje de "¡Felicidades!"
            Console.SetCursorPosition(0, 16); // Ajusta la posición según sea necesario
            AnsiConsole.MarkupLine($"[bold yellow]🎊 🎊 🎊 ¡Felicidades, {nombreGanador}! 🎊 🎊 🎊[/]");

            // Esperar medio segundo
            System.Threading.Thread.Sleep(500);

            // Imprimir el mensaje de "¡Victoria!"
            Console.SetCursorPosition(0, 16); // Ajusta la posición según sea necesario
            AnsiConsole.MarkupLine($"[bold yellow]🎉 🎉 🎉 ¡Victoria, {nombreGanador}! 🎉 🎉 🎉[/]");

            // Esperar medio segundo
            System.Threading.Thread.Sleep(500);
        }
    }
    private void MostrarResultados()
    {
        var table = new Table();
        table.AddColumn("Jugador");
        table.AddColumn("Puntos");

        table.AddRow(jugadores[0].Nombre, jugadores[0].Puntos.ToString());
        table.AddRow(jugadores[1].Nombre, jugadores[1].Puntos.ToString());

        AnsiConsole.Render(table);

        AnsiConsole.MarkupLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}