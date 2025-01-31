using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace Prueba_por_Clases_2;


public class Jugador
{
    public Personaje Personaje{ get; set; }
    public string Nombre { get; set; }
    public int Puntos { get; set; } 
    public int[] Position { get; set; }  // [fila, columna]
    public int[] PosicionInicial { get; set; }  // [fila, columna]
    public int PoderesCaptura { get; set; }
    public bool TienePoderEspecial { get; set; }
    public bool Inmune { get; set; }
    public int TurnosInmune { get; set; }
    public int PoderesInmunidad { get; set; }
    public string Emoji { get; set; }
    public int PoderesTeletransportacion { get; set; }

    public Jugador(string nombre, int fila, int columna, string emoji)
    {
        Nombre = nombre;
        PosicionInicial = new int[] { fila, columna };
        Position = new int[] { fila, columna };
        Puntos = 0;
        PoderesCaptura = 0;
        TienePoderEspecial = false;
        PoderesInmunidad = 0;
        Emoji = emoji;
        PoderesTeletransportacion = 0;
    }
    public void ActivarInmunidad()
    {
        Inmune = true;
        TurnosInmune = 2;
    }
    public void ActualizarInmunidad()
    {
        if (Inmune)
        {
            TurnosInmune--;
            if(TurnosInmune == 0)
            {
                Inmune = false;
            }
        }
    }
}