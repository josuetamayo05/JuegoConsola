using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace Prueba_por_Clases_2;


public class Jugador
{
    public string Nombre { get; set; }
    public int Puntos { get; set; } = 0;
    public int[] Position { get; set; } = new int[2]; // [fila, columna]
    public int[] PosicionInicial { get; set; } = new int[2]; // [fila, columna]
    public int PoderesCaptura { get; set; }

    public Jugador(string nombre, int filaInicial, int columnaInicial)
    {
        Nombre = nombre;
        Position[0] = filaInicial;
        Position[1] = columnaInicial;
        PosicionInicial[0] = filaInicial; // Inicializar posición inicial
        PosicionInicial[1] = columnaInicial;
        Puntos = 0;
        PoderesCaptura = 0;
    }

    public void RecogerRecompensa(int puntos)
    {
        Puntos += puntos;
    }

    
}