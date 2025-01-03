using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
namespace Prueba_por_Clases_2;

public class MovimientoJugador
{
    private Jugador[] jugadores;
    private Mapa mapa;
    private int[] puerta1 = {15, 15};
    private int[] puerta2 = {20, 24};
    private bool[] poderCapturaDisponible;

    public MovimientoJugador(Mapa mapa, Jugador[] jugadores)
    {
        this.mapa = mapa;
        this.jugadores = jugadores;
        this.poderCapturaDisponible = new bool[jugadores.Length]; // Bandera para controlar el mensaje de poder de captura

    }

    public void MoverJugador(int jugador )
    {
        bool movimientoExtra = false;
        string nombreJugadorActual = jugadores[jugador - 1].Nombre;
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

            AnsiConsole.MarkupLine($"[bold blue]🎲 Puntos {jugadores[0].Nombre} : [/][red]{jugadores[0].Puntos}[/] | [bold blue]🎲 Puntos {jugadores[1].Nombre} : [/][red]{jugadores[1].Puntos}[/]");
            Console.WriteLine();

            Console.WriteLine($"Turno de {nombreJugadorActual} (Jugador {jugador}), ingresa tu movimiento (W/A/S/D) o 'Q' para salir: ");
            char movimiento = Console.ReadKey().KeyChar;
            Console.WriteLine(); 

            if (movimiento == 'q' || movimiento == 'Q') 
            {
                Console.WriteLine("¡Gracias por jugar!");
                Environment.Exit(0); 
            }

            if (jugadores[jugador - 1].PoderesCaptura > 0 && (movimiento == 'c' || movimiento == 'C'))
            {    
                int[] otroJugador = jugador == 1 ? jugadores[1].Position : jugadores[0].Position;
                if (otroJugador[0] == jugadores[jugador - 1].Position[0] || otroJugador[1] == jugadores[jugador - 1].Position[1])
                {
                    AnsiConsole.MarkupLine($"[bold red]{nombreJugadorActual} ha usado el poder de captura! 🎯[/]");
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
                    jugadores[jugador - 1].PoderesCaptura--; // Decrementar el poder de captura
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]¡No puedes usar el poder de captura! El otro jugador no está en la misma fila o columna.[/]");
                    jugadores[jugador - 1].PoderesCaptura--; // Decrementar el poder de captura
                }
                continue;
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
                    if (jugadores[jugador - 1].Puntos > 0)
                    {
                        // Preguntar si quiere usar el poder
                        Console.WriteLine($"{nombreJugadorActual}, has caído en un árbol. ¿Quieres usar el poder para atravesarlo? Se te restará un punto de tu contador. Presiona 'F' para usar el poder o cualquier otra tecla para volver a tu posición inicial.");
                        char decision = Console.ReadKey().KeyChar;
                        Console.WriteLine(); // Salto de línea después de la entrada

                        if (decision == 'f' || decision == 'F')
                        {
                            jugadores[jugador - 1].Puntos--;

                            // Permitir al jugador moverse a la nueva posición
                            posicion[0] = nuevaFila;
                            posicion[1] = nuevaColumna;
                            Console.WriteLine($"{nombreJugadorActual} ha atravesado el árbol y se ha movido a la nueva posición.");
                        }                        
                        else
                        {
                            Console.WriteLine($"{nombreJugadorActual}, has decidido no usar el poder. Regresando a la posición inicial.");
                            posicion[0] = jugadores[jugador - 1].PosicionInicial[0];
                            posicion[1] = jugadores[jugador - 1].PosicionInicial[1];
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

                        Console.WriteLine($"{nombreJugadorActual}, no tienes suficientes puntos para usar el poder. Regresando a la posición inicial.");
                        posicion[0] = jugadores[jugador - 1].PosicionInicial[0];
                        posicion[1] = jugadores[jugador - 1].PosicionInicial[1];
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
                            System.Threading.Thread.Sleep(900); // Esperar medio segundo
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
                        System.Threading.Thread.Sleep(1000); // Esperar medio segundo

                        if (jugador == 1)
                        {
                            mapa.SetFicha(nuevaFila, nuevaColumna, "   ");
                            jugadores[0].PoderesCaptura++;
                        }                            
                        else
                        {
                            mapa.SetFicha(nuevaFila, nuevaColumna, "   ");
                            jugadores[1].PoderesCaptura++;
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
}