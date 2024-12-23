/*using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
namespace Prueba_por_Clases_2;

public class Power
{
    public string Nombre { get; set; }
    public int Puntos { get; set; }
    public int Duracion { get; set; }

    public Power(string nombre, int puntos, int duracion)
    {
        Nombre = nombre;
        Puntos = puntos;
        Duracion = duracion;
    }

    public void AplicarEfecto(Jugador jugador)
    {
        jugador.Puntos += Puntos;
        Console.WriteLine($"{jugador.Nombre} ha ganado {Puntos} puntos por usar el poder : {Nombre}!");
    }

    public void Capturar(Jugador jugadorCaptor, Jugador jugadorCapturado)
    {
        if (jugadorCaptor.Position[0] == jugadorCapturado.Position[0] || 
            jugadorCaptor.Position[1] == jugadorCapturado.Position[1])
        {
            jugadorCapturado.RestaurarPositionInicial();
            Console.WriteLine($"{jugadorCaptor.Nombre} ha capturado a {jugadorCapturado.Nombre}!");
        }
        else
        {
            Console.WriteLine($"{jugadorCaptor.Nombre} no puede capturar a {jugadorCapturado.Nombre} porque no están en la misma fila o columna.");
        }
    }
}*/


