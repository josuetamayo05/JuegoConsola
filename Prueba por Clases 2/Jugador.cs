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
    public int TrampasDetectadas{ get; set; }
    public int TrampasMaximas{ get; set; }

    public Jugador(string nombre, int fila, int columna, Personaje personaje)
    {
        Nombre = nombre;
        PosicionInicial = new int[] { fila, columna };
        Position = new int[] { fila, columna };
        Puntos = 0;
        PoderesCaptura = 0;
        TienePoderEspecial = false;
        Personaje = personaje;

    }
    public void Penalizar()
    {
        Console.WriteLine("Se te ha restado un punto por coger una trampa");
        Puntos--;
    }
    
    public void RecogerRecompensa(int puntos)
    {
        Puntos += puntos;
    }

    public void UsarPoder(Personaje personaje)
    {
        switch (personaje.Poder)
        {
            case PoderEspecial.Velocidad:
                // Aumentar la velocidad del jugador
                break;
            case PoderEspecial.Invisibilidad:
                // Hacer que el jugador sea invisible durante un turno
                break;
            case PoderEspecial.Teletransporte:
                // Teletransportar el jugador a una posición aleatoria en el mapa
                break;
            case PoderEspecial.Escudo:
                // Dar al jugador un escudo que lo proteja de un ataque
                break;
            case PoderEspecial.DobleMovimiento:
                // Permitir al jugador moverse dos veces en un turno
                break;
        }
    }
}