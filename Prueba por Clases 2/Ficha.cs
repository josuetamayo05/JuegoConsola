using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace Prueba_por_Clases_2;

public enum TipoFicha
{
    Poder,
    Recompensa,
    Trampa,
    Puerta,
    Meta,
    Arbol,
    Vacio,
    Pared
}


public class Ficha
{
    public TipoFicha Tipo { get; private set; }
    public int Valor { get; private set; }
    public int[] Posicion {get; private set; }

    public Ficha(TipoFicha tipo, int valor, int[] posicion)
    {
        Tipo = tipo;
        Valor = valor;
        Posicion = posicion;
    }

    public void Activar(Jugador jugador)
    {
        switch (Tipo)
        {
            case TipoFicha.Poder:
                //
                break;
            case TipoFicha.Recompensa:
                //
                break;
            case TipoFicha.Trampa:
                //
                break;
            case TipoFicha.Puerta:
                //
                break;
            case TipoFicha.Meta:
                //
                break;
            case TipoFicha.Arbol:
                //
                break;
            case TipoFicha.Vacio:
                //
                break;
            case TipoFicha.Pared:
                //
                break;
            
            
        }
    }
}

