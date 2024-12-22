using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
namespace Prueba_por_Clases_2;

public class Trampa
{
    public string Nombre { get; set; }
    public int PuntosPerdidos { get; set; }

    public Trampa(string nombre, int puntosPerdidos)
    {
        Nombre = nombre;
        PuntosPerdidos = puntosPerdidos;
    }

    public void AplicarEfecto(Jugador jugador)
    {
        jugador.Puntos -= PuntosPerdidos; // Reducir puntos del jugador
        Console.WriteLine($"{jugador.Nombre} ha perdido {PuntosPerdidos} puntos por caer en la trampa: {Nombre}!");
    }
}

