using System;
using System.Collections.Generic;
using System.Text;
//using Spectre.Console;
namespace Prueba_por_Clases_2;

public class Power
{
    public string Nombre { get; set; }
    public int Puntos { get; set; }

    public Power(string nombre, int puntos)
    {
        Nombre = nombre;
        Puntos = puntos;
    }
}

public class Reward
{
    public string Name { get; set; }
    public int Points { get; set; }

    public Reward(string name, int points)
    {
        Name = name;
        Points = points;
    }
}
