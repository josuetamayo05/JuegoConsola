using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace Prueba_por_Clases_2;

public class Personaje
{
    public string Nombre { get; set; }
    public string Simbolo { get; set; }
    public PoderEspecial Poder { get; set; }

    public Personaje(string nombre, string simbolo, PoderEspecial poder)
    {
        Nombre = nombre;
        Simbolo = simbolo;
        Poder = poder;
    }
}

public enum PoderEspecial
{
    Velocidad,
    Invisibilidad,
    Teletransporte,
    Escudo,
    DobleMovimiento
}