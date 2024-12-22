using System;
using System.Collections.Generic;
using System.Text;
//using Spectre.Console;
namespace Prueba_por_Clases_2;

public class Trampa
{
    public string Name { get; set; }
    public int PointLoses { get; set; }

    public Trampa(string name, int pointLoses)
    {
        Name = name;
        PointLoses = pointLoses;
    }
}