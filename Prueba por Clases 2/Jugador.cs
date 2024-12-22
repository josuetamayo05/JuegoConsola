using System;
using System.Collections.Generic;
using System.Text;
//using Spectre.Console;

namespace Prueba_por_Clases_2;


public class Jugador
{
    public string Nombre { get; set; }
    public int Puntos { get; set; }
    public int[] Position { get; set; }
    private int[] PositionInicial { get; set;}

    public Jugador(string nombre, int[] positionInicial)
    {
        Nombre = nombre;
        Puntos = 0;
        Position = positionInicial;
        PositionInicial = positionInicial; //guardo posic inicial
    }

    public void RecogerRecompensa(int puntos)
    {
        Puntos += puntos;
    }

    public void RestaurarPositionInicial()
    {
        Position[0] = PositionInicial[0];
        Position[1] = PositionInicial[1];
        Console.WriteLine($"{Nombre} ha regresado a su posición inicial.");
    }
}