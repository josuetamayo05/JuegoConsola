using System;
using System.Collections.Generic;
using System.Text;
//using Spectre.Console;

namespace Prueba_por_Clases_2;


public class Mapa
{
    private string[,] mapa;
    public int Rows { get; set;}
    public int Cols { get; set; }
    Random random = new Random();
    private Jugador[] jugadores;

    public Mapa(int rows, int cols, Jugador[] jugadores)
    {
        Rows = rows;
        Cols = cols;
        this.jugadores = jugadores;
        InicializarMapa(rows, cols);
    }
    

    private void InicializarMapa(int rows, int cols)
    {
        mapa = new string[rows, cols];
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                mapa[i , j] = "⬜ ";
            }
        }

        GenerateMaze(1, 1);
        mapa[13, 7] = "🏠 ";
        PlaceChips(3, 6, 5);
    }

    public void GenerateMaze(int row, int col)
    {
        mapa[row, col] = "   ";
        var move = new(int, int)[]
        {
            (-2, 0),
            (2, 0),
            (0, 2),
            (0, -2)
        };

        move = move.OrderBy(x => random.Next()).ToArray();

        foreach(var (dRow, dCol) in move)
        {
            int nRow = row + dRow;
            int nCol = col + dCol;

            //Verificar si la new posicion está dentro de los límites
            if (nRow > 0 && nRow < mapa.GetLength(0) && nCol > 0 && nCol < mapa.GetLength(1) && mapa[nRow, nCol] == "⬜ ")
            {
                mapa[row + dRow / 2, col + dCol / 2] = "   ";
                GenerateMaze(nRow, nCol); 
                // Aquí se elimina la pared entre la celda actual y la nueva celda 
                // Luego se llama a la Recursión
            }
        }
    }
    
    public void Imprimir()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Console.Write(mapa[i, j]);
            }
            Console.WriteLine();
        }
    }

    public void PlaceChips(int cantidadPower, int cantidadReward, int cantidadArbol)
    {
        for (int i = 0; i < cantidadPower; i++)
        {
            PlaceChipsAleatorio("⚡ ");
        }

        for (int i = 0; i < cantidadReward; i++)
        {
            PlaceChipsAleatorio("💰 ");
        }

        for (int i = 0; i < cantidadArbol; i++)
        {
            PlaceChipsAleatorio("🌳 ");
        }
    }

    private void PlaceChipsAleatorio(string simbolo)
    {
        int row, col;
        do
        {
            row = random.Next(1, Rows - 1);
            col = random.Next(1, Cols - 1);
        } while(mapa[row, col] != "⬜ ");
        
        mapa[row, col] = simbolo;
    }

    public void ManejarInteraccion(Jugador jugador)
    {
        int row = jugador.Position[0];
        int col = jugador.Position[1];

        if (mapa[row, col] == "⚡ ")
        {
            Power poderCaptura = new Power("Poder Extra", 1, 3);
            Jugador jugadorCapturado = jugadores[1];

            poderCaptura.Capturar(jugador, jugadorCapturado);
            mapa[row, col] = "   ";

        }
        else if (mapa[row, col] == "🚩")
        {
            Trampa trampa = new Trampa("Trampa de Puntos", 3); // Ejemplo de trampa
            jugador.Puntos -= trampa.PuntosPerdidos;
            mapa[row, col] = "   "; // Reemplazar la trampa con un espacio en blanco
            Console.WriteLine($"{jugador.Nombre} ha caído en una trampa: {trampa.Nombre}!");
        }
        // Verificar si hay una recompensa en la posición del jugador
        else if (mapa[row, col] == "💰 ")
        {
            Reward reward = new Reward("Recompensa de Puntos", 10); // Ejemplo de recompensa
            jugador.RecogerRecompensa(reward.Puntos);
            mapa[row, col] = "   "; // Reemplazar la recompensa con un espacio en blanco
            Console.WriteLine($"{jugador.Nombre} ha recogido una recompensa: {reward.Nombre}!");
        }
        else if (mapa[row, col] == "🌳 ")
        {
            // Lógica para manejar la interacción con el árbol
            Console.WriteLine($"{jugador.Nombre} ha encontrado un árbol.");
        }
    }
}




