/*using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace Prueba_por_Clases_2;
{
    public abstract class Habilidad
    {
        public abstract void Activar(Jugador jugador);
    }

    public class CorredorHabilidad : Habilidad
    {
        public override void Activar(Jugador jugador, MovimientoJugador movimientoJugador)
        {
            Console.WriteLine("Has elegido usar el poder del corredor. Realizarás 2 movimientos extras.");
            movimientoJugador.MoverJugador(1);
            movimientoJugador.MoverJugador(1);
        }
    }
}*/