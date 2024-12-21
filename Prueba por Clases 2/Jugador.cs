using System;
using System.Collections.Generic;
using System.Text;
//using Spectre.Console;

namespace PruebaJuegoSpectre;

public class Jugador
{
    public string Name { get; set; }
    public int Points { get; set; }
    public int[] Position { get; set; }

    public Jugador(string name, int[] positionInitial)
    {
        Name = name;
        Points = 0;
        Position = positionInitial;
    }

    public void RecogerRecompensa()
    {
        Points++;
    }

    public void RestorePositionInitial(int[] positionInitial)
    {
        Position = positionInitial;
    }
}