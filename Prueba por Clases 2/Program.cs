using System;
using System.Collections.Generic;
using System.Text;

namespace Prueba_por_Clases_2;

class Program
{
    static void Main(string[] args)
    {
        Mapa mapa = new Mapa(15, 15);
        /*mapa.GenerarLaberinto(1, 1);
        Jugador[] jugadores = new Jugador[]
        {
            new Jugador("Jugador 1", new int[] {1, 1}),
            new Jugador("Jugador 2", new int[] {1, 13})
        };*/
        
        Console.WriteLine("Mapa Inicial:");        
        mapa.Imprimir();
    }
}
