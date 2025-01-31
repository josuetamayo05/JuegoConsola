using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using System.Linq;


namespace Prueba_por_Clases_2;


public class Mapa
{
    private string[,] mapa;
    private string[,] mapaMenu;
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
        
        mapa[25, 13] = "🏠 ";
        mapa[8, 13] = "🚪 ";
        mapa[17, 23] = "🚪 ";
        ColocarFichasRandom(6, "⚡ ");
        ColocarFichasRandom(12, "💰 ");
        ColocarFichasRandom(5, "💊 ");
        ColocarFichasRandom(8, "🌳 ");
        ColocarFichasRandom(3, "🎁 ");
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

            if (nRow > 0 && nRow < Rows && nCol > 0 && nCol < Cols && mapa[nRow, nCol] == "⬜ ")
            {
                mapa[row + dRow / 2, col + dCol / 2] = "   ";
                GenerateMaze(nRow, nCol); 
                
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
                bool jugadorEncontrado = false;
                foreach (var jugador in jugadores)
                {
                    if (i == jugador.Position[0] && j == jugador.Position[1])
                    {
                        if(jugador.Nombre == "IA")
                        {
                            Console.Write("🤖 ");
                        }
                        else
                        {
                            Console.Write(jugador.Emoji + " ");     
                                              
                        }
                        jugadorEncontrado = true;
                        break;
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




