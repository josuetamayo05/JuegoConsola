using System;
using System.Collections.Generic;
using System.Text;
namespace Prueba_por_Clases_2;


public class Juego
{
    private Mapa mapa;
    private Jugador[] jugadores;
    private int turnoActual;

    public Juego(int rows, int cols, Jugador[] jugadores)
    {
        this.mapa = new Mapa(rows, cols, jugadores);
        this.jugadores = jugadores;
        this.turnoActual = 0;
    }

    public void Iniciar()
    {
        bool juegoEnCurso = true;

        while (juegoEnCurso)
        {
            mapa.Imprimir();

            Jugador jugadorActual = jugadores[turnoActual];
            Console.WriteLine($"{jugadorActual.Nombre}, ingresa tu movimiento (W/A/S/D):");
            ConsoleKeyInfo tecla = Console.ReadKey(true);
            Console.WriteLine();
            
            int nuevaFila = jugadorActual.Position[0];
            int nuevaColumna = jugadorActual.Position[1];

            switch (tecla.Key)
            {
                case ConsoleKey.W: nuevaFila--; break; // Arriba
                case ConsoleKey.S: nuevaFila++; break; // Abajo
                case ConsoleKey.A: nuevaColumna--; break; // Izquierda
                case ConsoleKey.D: nuevaColumna++; break; // Derecha
                default:
                     Console.WriteLine("Movimiento no válido. Intenta de nuevo.");
                    continue;
            }
            if (mapa.MoverJugador(jugadorActual, nuevaFila, nuevaColumna))
            {
                // Si el movimiento fue exitoso, pasar al siguiente jugador
                turnoActual = (turnoActual + 1) % jugadores.Length; // Cambiar al siguiente jugador
            }
            else
            {
                Console.WriteLine("Movimiento fallido. Intenta de nuevo.");
            }           
        }
    }

    /*private bool JugadorHaAlcanzadoMeta(Jugador jugador)
    {
        return jugador.Position[0] == meta[0] && jugador.Position[1] == meta[1];
    }*/
}