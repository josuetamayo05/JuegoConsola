using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
namespace Prueba_por_Clases_2;

public class MovimientoJugador
{
    private Juego juego;
    private Jugador[] jugadores;
    private Mapa mapa;
    private int[] puerta1 = {8, 13};
    private int[] puerta2 = {17, 23};
    private bool[] poderCapturaDisponible;


    public MovimientoJugador(Juego juego)
    {
        this.juego = juego; 
    }
    public MovimientoJugador(Mapa mapa, Jugador[] jugadores)
    {
        this.mapa = mapa;
        this.jugadores = jugadores;
        this.poderCapturaDisponible = new bool[jugadores.Length]; 

    }

    public void MoverJugador(int jugador )
    {
        bool movimientoExtra = false;
        string nombreJugadorActual = jugadores[jugador - 1].Nombre;

        do
        {
            mapa.Imprimir(jugadores);
            
            AnsiConsole.MarkupLine($"[bold blue]🎲 Puntos {jugadores[0].Nombre} : [/][red]{jugadores[0].Puntos}[/] | [bold blue]🎲 Puntos {jugadores[1].Nombre} : [/][red]{jugadores[1].Puntos}[/]");
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[red]Opción[/]")
                .AddColumn("[red]Descripción[/]")
                .AddColumn("[red]Controles[/]")
                .AddColumn("[red]Activar[/]");

            table.AddRow("[red]C[/]", "Captura '⚡'", "Flecha arriba", "Presiona 'C' para activar")
                .AddRow("[red]P[/]", "Inmunidad'💊'", "Flecha abajo", "Presiona 'P' para activar")
                .AddRow("[red]T[/]", "Teletransportación'🎁'", "Flecha izquierda", "Presiona 'T' para activar");

            AnsiConsole.Write(table);
                   
            Console.WriteLine($"Turno de {nombreJugadorActual} (Jugador {jugador}), ingresa tu movimiento (W/A/S/D) o 'Q' para salir: ");
            char movimiento = Console.ReadKey().KeyChar;
            Console.WriteLine(); 

            if (movimiento == 'q' || movimiento == 'Q') 
            {
                Console.WriteLine("¡Gracias por jugar!");
                Environment.Exit(0); 
            }
            if (movimiento == 'p' || movimiento == 'P')
            {
                if (jugadores[jugador - 1].PoderesInmunidad > 0)
                {
                    jugadores[jugador-1].ActivarInmunidad();
                    Console.WriteLine("Has activado el poder de inmunidad.");
                    System.Threading.Thread.Sleep(2000); 
                    jugadores[jugador-1].PoderesInmunidad--;
                }
                else
                {
                    Console.WriteLine("No tienes poderes de inmunidad disponibles.");
                }
            }
            
            if (movimiento == 't' || movimiento == 'T')
            {
                if (jugadores[jugador - 1].PoderesTeletransportacion > 0)
                {
                    TeletransportarJugador(jugador, nombreJugadorActual);
                    jugadores[jugador - 1].PoderesTeletransportacion--;
                }                
            }

            if (jugadores[jugador - 1].PoderesCaptura > 0 && (movimiento == 'c' || movimiento == 'C'))
            {    
                int[] otroJugador = jugador == 1 ? jugadores[1].Position : jugadores[0].Position;
                if (otroJugador[0] == jugadores[jugador - 1].Position[0] || otroJugador[1] == jugadores[jugador - 1].Position[1])
                {
                    if(jugador == 1 && jugadores[1].Inmune)
                    {
                        Console.WriteLine("El jugador 2 está inmune a la captura.");
                        System.Threading.Thread.Sleep(2000); 
                        jugadores[0].PoderesCaptura--;
                    }
                    else if (jugador == 2 && jugadores[0].Inmune)
                    {
                        Console.WriteLine("El jugador 1 está inmune a la captura.");
                        System.Threading.Thread.Sleep(2000); 
                        jugadores[1].PoderesCaptura--;
                    }
                    
                    else
                    {
                        AnsiConsole.MarkupLine($"[bold red]{nombreJugadorActual} ha usado el poder de captura! 🎯[/]");
                        System.Threading.Thread.Sleep(500); 
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write("Capturando al otro jugador");
                            for (int j = 0; j < 3; j++)
                            {
                                Console.Write("."); 
                                    System.Threading.Thread.Sleep(300); 
                            }
                            Console.WriteLine(); 
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
                        jugadores[jugador - 1].PoderesCaptura--; 
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]¡No puedes usar el poder de captura! El otro jugador no está en la misma fila o columna.[/]");
                    System.Threading.Thread.Sleep(300); 
                    jugadores[jugador - 1].PoderesCaptura--;
                }
                continue;
            }
            

            int[] posicion = jugadores[jugador - 1].Position;
            int nuevaFila = posicion[0];
            int nuevaColumna = posicion[1];

            switch (movimiento)
            {
                case 'w': nuevaFila--; break; 
                case 's': nuevaFila++; break; 
                case 'a': nuevaColumna--; break; 
                case 'd': nuevaColumna++; break; 
                default:
                    Console.WriteLine("Movimiento no válido. Intenta de nuevo.");
                    continue; 
            }

            

            if (nuevaFila >= 0 && nuevaColumna >= 0 && nuevaFila < mapa.Rows && nuevaColumna < mapa.Cols)
            {
                if (mapa.GetFicha(nuevaFila, nuevaColumna) == "🌳 ")
                {                   
                    if (jugadores[jugador - 1].Puntos > 0)
                    {
                        Console.WriteLine($"{nombreJugadorActual}, has caído en un árbol. ¿Quieres usar el poder para atravesarlo? Se te restará un punto de tu contador. Presiona 'F' para usar el poder o cualquier otra tecla para volver a tu posición inicial.");
                        char decision = Console.ReadKey().KeyChar;
                        Console.WriteLine(); 

                        if (decision == 'f' || decision == 'F')
                        {
                            jugadores[jugador - 1].Puntos--;               
                            posicion[0] = nuevaFila;
                            posicion[1] = nuevaColumna;
                            Console.WriteLine($"{nombreJugadorActual} ha atravesado el árbol y se ha movido a la nueva posición.");
                        }                        
                        else
                        {
                            Console.WriteLine($"{nombreJugadorActual}, has decidido no usar el poder. Regresando a la posición inicial.");
                            posicion[0] = jugadores[jugador - 1].PosicionInicial[0];
                            posicion[1] = jugadores[jugador - 1].PosicionInicial[1];
                            return;  
                        }
                    }
                    else
                    {
                        Console.WriteLine("Has caído en un árbol");
                        System.Threading.Thread.Sleep(400); 
                        
                        for (int j = 0; j < 3; j++)
                        {
                            Console.Write($"{nombreJugadorActual} retrocediendo a la posición inicial");
                            for (int k = 0; k < 3; k++)
                            {
                                Console.Write("."); 
                                System.Threading.Thread.Sleep(600); 
                            }
                            Console.WriteLine(); 
                        }

                        Console.WriteLine($"{nombreJugadorActual}, no tienes suficientes puntos para usar el poder. Regresando a la posición inicial.");
                        System.Threading.Thread.Sleep(600); 

                        posicion[0] = jugadores[jugador - 1].PosicionInicial[0];
                        posicion[1] = jugadores[jugador - 1].PosicionInicial[1];
                        return;  
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
                            System.Threading.Thread.Sleep(850);
                        }
                        Console.WriteLine();

                        posicion[0] = puerta2[0]; 
                        posicion[1] = puerta2[1];
                    }
                        
                    else if (nuevaFila == puerta2[0] && nuevaColumna == puerta2[1])
                    {
                        AnsiConsole.MarkupLine($"{nombreJugadorActual}, estás siendo teletransportado a la Puerta 1.");
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write(".");
                            System.Threading.Thread.Sleep(850);
                        }
                        Console.WriteLine();

                        posicion[0] = puerta1[0];
                        posicion[1] = puerta1[1];
                    }

                    Console.WriteLine("Evaluando posición");
                    if (mapa.GetFicha(nuevaFila, nuevaColumna) == "💰 ")
                    {
                        mapa.SetFicha(nuevaFila, nuevaColumna, "   "); 

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
                            System.Threading.Thread.Sleep(1500); 
                        }
                                
                        movimientoExtra = true;
                    }
                    else
                    {
                        movimientoExtra = false;
                    }
                        
                    if (mapa.GetFicha(nuevaFila, nuevaColumna) == "⚡ ")
                    {
                        AnsiConsole.MarkupLine($"¡🎉 {nombreJugadorActual} ha recogido una ficha de captura! 🎉 Puedes usar el poder de captura.");
                        System.Threading.Thread.Sleep(1500); 

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
                    if(mapa.GetFicha(nuevaFila, nuevaColumna) == "💊 ")
                    {
                        mapa.SetFicha(nuevaFila, nuevaColumna, "   ");
                        jugadores[jugador -1].PoderesInmunidad++;
                        Console.WriteLine("Has cogido la ficha 💊. Estás inmune a los poderes de captura durante 2 turnos.");
                        System.Threading.Thread.Sleep(1000); 
                    }

                    if (mapa.GetFicha(nuevaFila, nuevaColumna) == "🎁 ")
                    {
                        mapa.SetFicha(nuevaFila, nuevaColumna, "   ");
                        jugadores[jugador - 1].PoderesTeletransportacion++;
                        Console.WriteLine("Has cogido una ficha de teletransportación.");
                        System.Threading.Thread.Sleep(1000); 
                    }
                    
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]¡No puedes moverte allí! Hay una pared.[/]");
                    System.Threading.Thread.Sleep(500); 
                    break;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]¡Te has salido de los límites![/]");
                System.Threading.Thread.Sleep(500); 

                break;
            }
        } while(movimientoExtra);
    }
    private void TeletransportarJugador(int jugador, string name)
    {
        Random aleatorio = new Random();
        int nuevaFila, nuevaColumna;

        do
        {
            nuevaFila = aleatorio.Next(0, mapa.Rows);
            nuevaColumna = aleatorio.Next(0, mapa.Cols);
        } while (mapa.GetFicha(nuevaFila, nuevaColumna) == "⬜ " || mapa.GetFicha(nuevaFila, nuevaColumna) == "🌳 " || mapa.GetFicha(nuevaFila, nuevaColumna) == "🏠 " || mapa.GetFicha(nuevaFila, nuevaColumna) == "💰 " || mapa.GetFicha(nuevaFila, nuevaColumna) == "⚡ " || mapa.GetFicha(nuevaFila, nuevaColumna) == "💊 " || mapa.GetFicha(nuevaFila, nuevaColumna) == "🚪 " );

        jugadores[jugador - 1].Position[0] = nuevaFila;
        jugadores[jugador - 1].Position[1] = nuevaColumna;

        AnsiConsole.MarkupLine($"{name}, estás siendo teletransportado a un Lugar Random del Mapa.");
        for (int i = 0; i < 3; i++)
        {
            Console.Write(".");
            System.Threading.Thread.Sleep(850);
        }
    }

    
}