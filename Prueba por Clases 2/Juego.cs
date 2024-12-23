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
    private int[] puerta1 = {15, 15};
    private int[] puerta2 = {20, 24};
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
            MoverJugador(1);
            mapa.Imprimir(jugadores);

            if (VerificarVictoria(jugadores[0]))
            {
                Console.WriteLine($"{jugadores[0].Nombre} ha llegado a la meta y ha ganado el juego!");
                break;
            }

            if (modoJuego == 1)
            {
                ia.Mover(mapa, jugadores[0]);
                mapa.Imprimir(jugadores);

                if (VerificarVictoria(ia.GetJugadorIA()))
                {
                    Console.WriteLine($"{ia.GetJugadorIA().Nombre} ha llegado a la meta y ha ganado el juego!");
                    break;
                }
            }

            else if (modoJuego == 2)
            {
                MoverJugador(2); // Mover al segundo jugador
                mapa.Imprimir(jugadores); // Asegúrate de pasar el arreglo de jugadores

                if (VerificarVictoria(jugadores[1]))
                {
                    Console.WriteLine($"{jugadores[1].Nombre} ha llegado a la meta y ha ganado el juego!");
                    break; // Salir del bucle si el jugador 2 gana
                }
            }
            turnoActual = (turnoActual + 1) % jugadores.Length;
        }
    }
    

    private void MoverJugador(int jugador)
    {
        bool movimientoExtra = false; // Controlar si se permite un movimiento extra
        string nombreJugadorActual = jugadores[jugador - 1].Nombre; // Obtener el nombre del jugador actual
        string mensajeRecompensa = "";
        string mensajeTrampa = "";

        do
        {
            // Mostrar mensajes de recompensa y trampa
            if (!string.IsNullOrEmpty(mensajeRecompensa))
            {
                Console.WriteLine(mensajeRecompensa);
                mensajeRecompensa = ""; // Limpiar el mensaje de recompensa
            }

            if (!string.IsNullOrEmpty(mensajeTrampa))
            {
                Console.WriteLine(mensajeTrampa);
                mensajeTrampa = ""; // Limpiar el mensaje de trampa
            }
            mapa.Imprimir(jugadores);

            // Solicitar movimiento
            Console.WriteLine($"Turno de {nombreJugadorActual} (Jugador {jugador}), ingresa tu movimiento (W/A/S/D) o 'Q' para salir: ");
            char movimiento = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Salto de línea después de la entrada

            if (movimiento == 'q' || movimiento == 'Q') // Condición de salida
            {
                Console.WriteLine("¡Gracias por jugar!");
                Environment.Exit(0); // Salir del juego
            }

            int[] posicion = jugadores[jugador - 1].Position;
            int nuevaFila = posicion[0];
            int nuevaColumna = posicion[1];

            switch (movimiento)
            {
                case 'w': nuevaFila--; break; // Arriba
                case 's': nuevaFila++; break; // Abajo
                case 'a': nuevaColumna--; break; // Izquierda
                case 'd': nuevaColumna++; break; // Derecha
                default:
                    Console.WriteLine("Movimiento no válido. Intenta de nuevo.");
                    continue; // Si la tecla no es válida, volver a pedir movimiento
            }

            // Verificar límites
            if (nuevaFila >= 0 && nuevaColumna >= 0 && nuevaFila < mapa.Rows && nuevaColumna < mapa.Cols)
            {
                if (mapa.GetFicha(nuevaFila, nuevaColumna) == "🌳 ")
                {                   
                    // Verificar si el jugador tiene puntos para usar el poder
                    if ((jugador == 1 && jugadores[0].Puntos > 0) || (jugador == 2 && jugadores[1].Puntos > 0))
                    {
                        // Preguntar si quiere usar el poder
                        Console.WriteLine($"{nombreJugadorActual}, has caído en un árbol. ¿Quieres usar el poder para atravesarlo? Se te restará un punto de tu contador. Presiona 'F' para usar el poder o cualquier otra tecla para volver a tu posición inicial.");
                        char decision = Console.ReadKey().KeyChar;
                        Console.WriteLine(); // Salto de línea después de la entrada

                        if (decision == 'f' || decision == 'F')
                        {
                            // Restar un punto al jugador
                            if (jugador == 1)
                            {
                                jugadores[0].Puntos--;
                            }
                            else
                            {
                                jugadores[1].Puntos--;
                            }

                            // Permitir al jugador moverse a la nueva posición
                            posicion[0] = nuevaFila;
                            posicion[1] = nuevaColumna;
                            Console.WriteLine($"{nombreJugadorActual} ha atravesado el árbol y se ha movido a la nueva posición.");
                        }                        
                        else
                        {
                            //string mensajeTrampa = $"{nombreJugadorActual}, has caído en una trampa. Vuelves a tu posición inicial."; // Mensaje de trampa  
                            Console.WriteLine(mensajeTrampa); // Imprimir el mensaje de trampa
                            if (jugador == 1)
                            {
                                jugadores[0].Position[0] = jugadores[0].PosicionInicial[0]; // Restaurar posición inicial del jugador 1
                                jugadores[0].Position[1] = jugadores[0].PosicionInicial[1];
                            }
                            else
                            {
                                jugadores[1].Position[0] = jugadores[1].PosicionInicial[0]; // Restaurar posición inicial del jugador 2
                                jugadores[1].Position[1] = jugadores[1].PosicionInicial[1];
                            }
                            //mapa.Imprimir(); // Imprimir el mapa después de restaurar la posición
                            return;  // Salir del método para evitar más movimientos
                        }
                    }
                    else
                    {
                        //string mensajeTrampa = $"{nombreJugadorActual}, no tienes suficientes puntos para usar el poder. Vuelves a tu posición inicial."; // Mensaje de trampa
                        Console.WriteLine(mensajeTrampa); // Imprimir el mensaje antes de restaurar la posición
                        
                        for (int j = 0; j < 3; j++)
                        {
                            Console.Write($"{nombreJugadorActual} retrocediendo a la posición inicial");
                            for (int k = 0; k < 3; k++)
                            {
                                Console.Write("."); // Mostrar puntos para la animación
                                System.Threading.Thread.Sleep(300); // Esperar un poco
                            }
                            Console.WriteLine(); // Salto de línea después de cada retroceso
                        }

                        if (jugador == 1)
                        {
                            jugadores[0].Position[0] = jugadores[0].PosicionInicial[0]; // Restaurar posición inicial del jugador 1
                            jugadores[0].Position[1] = jugadores[0].PosicionInicial[1];
                        }
                        else
                        {
                            jugadores[1].Position[0] = jugadores[1].PosicionInicial[0]; // Restaurar posición inicial del jugador 2
                            jugadores[1].Position[1] = jugadores[1].PosicionInicial[1];
                        }
                        //mapa.Imprimir(); // Imprimir el mapa después de restaurar la posición
                        return;  // Salir del método para evitar más movimientos
                    }    
                }
                
            
                if (mapa.GetFicha(nuevaFila, nuevaColumna) != "⬜ ")
                {
                    posicion[0] = nuevaFila;
                    posicion[1] = nuevaColumna;

                    if (nuevaFila == puerta1[0] && nuevaColumna == puerta1[1])
                    {
                        AnsiConsole.MarkupLine($"{nombreJugadorActual}, estás siendo teletransportado a la Puerta 2.");
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write(".");
                            System.Threading.Thread.Sleep(700);
                        }
                        Console.WriteLine();

                        posicion[0] = puerta2[0]; // Mover jugador 1 a
                        posicion[1] = puerta2[1];
                        //mapa.Imprimir();
                    }
                        
                    else if (nuevaFila == puerta2[0] && nuevaColumna == puerta2[1])
                    {
                        AnsiConsole.MarkupLine($"{nombreJugadorActual}, estás siendo teletransportado a la Puerta 1.");
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write(".");
                            System.Threading.Thread.Sleep(700);
                        }
                        Console.WriteLine();

                        posicion[0] = puerta1[0];
                        posicion[1] = puerta1[1];
                        //mapa.Imprimir();
                    }

                    Console.WriteLine("Evaluando posición");
                    if (mapa.GetFicha(nuevaFila, nuevaColumna) == "💰 ")
                    {
                        mapa.SetFicha(nuevaFila, nuevaColumna, "   "); // limpiar la posición

                        if (jugador == 1)
                        {                            
                            jugadores[0].Puntos++;     
                            AnsiConsole.MarkupLine($"¡🎉 {nombreJugadorActual} has recogido una ficha de recompensa! 🎉 Puntos: {jugadores[0].Puntos}");
                            
                        }
                        else
                        {
                            jugadores[0].Puntos++;            
                            AnsiConsole.MarkupLine($"¡🎉 {nombreJugadorActual} has recogido una ficha de recompensa! 🎉 Puntos: {jugadores[1].Puntos}");
                        }

                        for (int j = 0; j < 1; j++)
                        {
                            Console.WriteLine("🎊 🎊 🎊 ¡Felicidades, has recogido una ficha recompensa y puedes volver a jugar! 🎊 🎊 🎊");
                            System.Threading.Thread.Sleep(600); // Esperar medio segundo
                        }
                            
                            
                        //mapa.Imprimir(); // Imprimir mapa actualiz
                        movimientoExtra = true;
                    }
                    else
                    {
                        movimientoExtra = false;
                    }
                        
                    //mapa.Imprimir();

                    if (mapa.GetFicha(nuevaFila, nuevaColumna) == "⚡ ")
                    {
                        AnsiConsole.MarkupLine($"¡🎉 {nombreJugadorActual} ha recogido una ficha de captura! 🎉 Puedes usar el poder de captura.");
                        mapa.SetFicha(nuevaFila, nuevaColumna, "   ");
                        if (jugador == 1)
                        {
                            jugadores[0].PoderesCaptura++;
                        }                            
                        else
                        {
                            jugadores[1].PoderesCaptura++;
                        }
                    }    
                    if (jugadores[jugador - 1].PoderesCaptura > 0)
                    {    
                        AnsiConsole.MarkupLine($"¿Quieres usar el poder de captura? Presiona 'C' para capturar al otro jugador solo en caso de que ambos están en la misma fila o columna o cualquier otra tecla para continuar.");
                        char decision = Console.ReadKey().KeyChar;
                        Console.WriteLine(); // Salto de línea después de la en

                        if (decision == 'c' || decision == 'C')
                        {
                            // Verificar si el otro jugador está en la misma fila o columna
                            int[] otroJugador = jugador == 1 ? jugadores[1].Position : jugadores[0].Position;
                            if (otroJugador[0] == posicion[0] || otroJugador[1] == posicion[1])
                            {
                                AnsiConsole.MarkupLine($"[bold red]{nombreJugadorActual} ha usado el poder de captura! 🎯[/]");
                                // Animación de captura
                                for (int i = 0; i < 3; i++)
                                {
                                    Console.Write("Capturando al otro jugador");
                                    for (int j = 0; j < 3; j++)
                                    {
                                        Console.Write("."); // Mostrar puntos para la animación
                                        System.Threading.Thread.Sleep(300); // Esperar un poco
                                    }
                                    Console.WriteLine(); // Salto de línea después de cada captura
                                }

                                // Mover al otro jugador a su posición inicial
                                if (jugador == 1)
                                {
                                    jugadores[1].Position[0] = jugadores[1].PosicionInicial[0];
                                    jugadores[1].Position[1] = jugadores[1].PosicionInicial[1];
                                }
                                else
                                {
                                    jugadores[0].Position[0] = jugadores[0].PosicionInicial[0];
                                    jugadores[0].Position[1] = jugadores[0].PosicionInicial[1];
                                }
                                jugadores[jugador - 1].PoderesCaptura--;
                                mapa.Imprimir(jugadores);
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[bold red]¡No puedes usar el poder de captura! El otro jugador no está en la misma fila o columna.[/]");
                            }
                        }
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]¡No puedes moverte allí! Hay una pared.[/]");
                    break; // Salir del bucle si se sale de los límites
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]¡Te has salido de los límites![/]");
                break;
            }
        
        } while(movimientoExtra);
    }    

    private bool VerificarVictoria(Jugador jugador)
    {
        // Supongamos que la meta está en la posición (5, 5)
        int filaMeta = 20;
        int columnaMeta = 26;

        // Verificar si el jugador ha llegado a la meta
        return jugador.Position[0] == filaMeta && jugador.Position[1] == columnaMeta;
    }
}