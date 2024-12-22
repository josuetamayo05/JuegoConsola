using System;
using System.Collections.Generic;
using System.Text;

namespace Prueba_por_Clases_2;

class Program
{
    static void Main(string[] args)
    {
        Jugador[] jugadores = new Jugador[]
        {
            new Jugador("Jugador 1", new int[] {1, 1}),
            new Jugador("Jugador 2", new int[] {1, 13})
        };
        int[] meta = new int[] { 8, 14};

        Juego juego = new Juego(15, 15, jugadores, meta);
        juego.Iniciar();
    }
}
