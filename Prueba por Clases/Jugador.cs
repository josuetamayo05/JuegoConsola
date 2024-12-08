using System;

namespace Company.ClassLibrary1;

public class Jugador
{
    public string Nombre { get; set; }
    public int Fila { get; set; }
    public int Columna { get; set; }

    public Jugador(string nombre, int fila, int columna)
    {
        Nombre = nombre;
        Fila = fila;
        Columna = columna;
    }
}