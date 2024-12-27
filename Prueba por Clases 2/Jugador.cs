using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace Prueba_por_Clases_2;


public class Jugador
{
    public string Nombre { get; set; }
    public int Puntos { get; set; } 
    public int[] Position { get; set; }  // [fila, columna]
    public int[] PosicionInicial { get; set; }  // [fila, columna]
    public int PoderesCaptura { get; set; }
    public bool TienePoderEspecial { get; set; }

    public Jugador(string nombre, int fila, int columna)
    {
        Nombre = nombre;
        PosicionInicial = new int[] { fila, columna };
        Position = new int[] { fila, columna };
        Puntos = 0;
        PoderesCaptura = 0;
        TienePoderEspecial = false;
    }

    public void RecogerRecompensa(int puntos)
    {
        Puntos += puntos;
    }

    
}