using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using System.Linq;

namespace Prueba_por_Clases_2;


public class Mapa
{
    private string[,] mapa;
    public int Rows { get; set;}
    public int Cols { get; set; }
    Random random = new Random();
    private Jugador[] jugadores;

    public Mapa(int rows, int cols)
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
                mapa[i, j] = "⬜ ";
            }
        }

        GenerateMaze(1, 1);
        
        mapa[20, 20] = "🏠 ";
        mapa[15, 15] = "🚪 ";
        mapa[20, 24] = "🚪 ";

        
        ColocarFichasRandom(6, "⚡ ");
        ColocarFichasRandom(12, "💰 ");
        ColocarFichasRandom(12, "🚩 ");
        ColocarFichasRandom(8, "🌳 ");
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
            if (nRow > 0 && nRow < Rows && nCol > 0 && nCol < Cols && mapa[nRow, nCol] == "⬜ ")
            {
                mapa[row + dRow / 2, col + dCol / 2] = "   ";
                GenerateMaze(nRow, nCol); 
                // Aquí se elimina la pared entre la celda actual y la nueva celda 
                // Luego se llama a la Recursión
            }
        }
    }
    
   public void Imprimir(Jugador[] jugadores)
    {
        Console.Clear(); 
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                // Verificar si hay un jugador en la posición actual
                bool jugadorEncontrado = false;
                foreach (var jugador in jugadores)
                {
                    if (i == jugador.Position[0] && j == jugador.Position[1])
                    {
                        // Imprimir el símbolo del jugador
                        Console.Write(jugador.Nombre == "IA" ? "🤖 " : "😃 "); // Usar un emoji diferente para la IA
                        jugadorEncontrado = true;
                        break; // Salir del bucle si se encontró un jugador
                    }
                }

                
                if (!jugadorEncontrado)
                {
                    Console.Write(mapa[i, j]);
                }
            }
            Console.WriteLine(); 
        }
    }

    private void ColocarFichasRandom(int cantidad, string tipo)
    {
        for (int i = 0; i < cantidad; i++)
        {
            ColocarFichasAleatorias(tipo);
        }
    }

    private void ColocarFichasAleatorias(string tipo)
    {
        int row, col;
        do
        {
            row = random.Next(1, Rows - 1);
            col = random.Next(1, Cols - 1);
        } while(mapa[row, col] != "⬜ ");
        
        mapa[row, col] = tipo;
    }

    /*private int GetValorPorTipo(TipoFicha tipo)
    {
        return tipo switch // dar valor a las fichas
        {
            TipoFicha.Poder => 3,
            TipoFicha.Recompensa => 10,
            TipoFicha.Trampa => 3,
            _ => 0, // valor por defecto si el tipo no coincide
        };
    }*/
    

    public string GetFicha(int fila, int columna)
    {
        if (fila < 0 || fila >= Rows || columna < 0 || columna >= Cols)
        {
            throw new IndexOutOfRangeException("Índice fuera de los límites del mapa.");
        }
        return mapa[fila, columna];
    }

    public void SetFicha(int fila, int columna, string ficha)
    {
        if (fila < 0 || fila >= Rows || columna < 0 || columna >= Cols)
        {
            throw new IndexOutOfRangeException("Índice fuera de los límites del mapa.");
        }
        mapa[fila, columna] = ficha;
    }
}




