using System;
using System.Collections.Generic;
using System.Text;
//using Spectre.Console;

namespace Prueba_por_Clases_2;


public class Juego
{
    private Mapa mapa;
    private Jugador[] jugadores;
    private int[] meta;

    public Juego(int rows, int cols, Jugador[] jugadores, int[] meta)
    {
        this.mapa = new Mapa(rows, cols, jugadores);
        this.jugadores = jugadores;
        this.meta = meta;
    }

    public void Iniciar()
    {
        bool juegoEnCurso = true;

        while (juegoEnCurso)
        {
            foreach (var jugador in jugadores)
            {
                mapa.Imprimir();
                MoverJugador(jugador);
                if (JugadorHaAlcanzadoMeta(jugador))
                {
                    Console.WriteLine($"{jugador.Nombre} ha ganado!");
                    juegoEnCurso = false;
                    break;
                }
            }
        }
    }

    private void MoverJugador(Jugador jugador)
    {
        Console.WriteLine($"{jugador.Nombre}, ingresa tu movimiento (W/A/S/D): ");
        string input = Console.ReadLine().ToUpper();

        int nuevaFila = jugador.Position[0];
        int nuevaColumna = jugador.Position[1];

        switch (input)
        {
            case "W": // Mover hacia arriba
                nuevaFila--;
                break;
            case "S": // Mover hacia abajo
                nuevaFila++;
                break;
            case "A": // Mover hacia la izquierda
                nuevaColumna--;
                break;
            case "D": // Mover hacia la derecha
                nuevaColumna++;
                break;
            default:
                Console.WriteLine("Movimiento no válido. Intenta de nuevo.");
                return; // Salir del método si el movimiento no es válido
        }

        // Verificar límites del mapa
        if (nuevaFila >= 0 && nuevaFila < mapa.Rows && nuevaColumna >= 0 && nuevaColumna < mapa.Cols)
        {
            // Actualizar la posición del jugador
            jugador.Position[0] = nuevaFila;
            jugador.Position[1] = nuevaColumna;
        }
        else
        {
            Console.WriteLine("Movimiento fuera de límites. Intenta de nuevo.");
        }
    }

    private bool JugadorHaAlcanzadoMeta(Jugador jugador)
    {
        return jugador.Position[0] == meta[0] && jugador.Position[1] == meta[1];
    }
}