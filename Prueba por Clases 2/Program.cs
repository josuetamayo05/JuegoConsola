using System;
using System.Collections.Generic;
using System.Text;

namespace Prueba_por_Clases_2;

class Program
{
    static void Main(string[] args)
    {
        int rows = 15;
        int cols = 15;
        Mapa mapa = new Mapa(rows, cols);
        
        Console.WriteLine("Mapa Inicial:");
        mapa.Imprimir();
    }
}
