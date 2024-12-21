using System;

namespace Prueba_Por_Clases;

public class Mapa
{
    private string[,] mapa;

    public Mapa(int filas, int columnas)
    {
        InicializarMapa(filas, columnas);
    }

    private void InicializarMapa(int filas, int columnas)
    {
        mapa = new string[filas, columnas];
        for (i = 0; i < filas; i++)
        {
            for (j = 0; j < columnas; j++)
            {
                // Colocar bordes
                if (i == 0 || i == filas - 1 || j == 0 || j == columnas - 1)
                {
                   mapa[i, j] = "⬜ ";
                }
                else
                {
                    mapa[i, j] = "   ";
                }
            }
        }
        // Asegurarse de que los jugadores tengan un camino inicial libre
        mapa[jugador1[0], jugador1[1]] = "   "; // Asegurarse que el espacio de inicio del Jugador 1 esté vacío
        mapa[jugador2[0], jugador2[1]] = "   "; // Jugador 2
        mapa[17, 17] = "🏠 "; // Meta


        CrearCamino(jugador1[0], jugador1[1], 10, 10);
        ColocarFichasYObstaculos(5, 30, 6);
    }
    
    private void CrearCamino(int inicioFila, int inicioColumna, int metaFila, int metaColumna)
    {
        int fila = inicioFila;
        int columna = inicioColumna;

        // Crear un camino simple hacia la meta
        while (fila != metaFila || columna != metaColumna)
        {
            mapa[fila, columna] = "   "; // Asegurarse que el camino esté libre

            if (fila < metaFila) fila++; // Mover hacia abajo
            else if (fila > metaFila) fila--; // Mover hacia arriba

            if (columna < metaColumna) columna++; // Mover hacia la derecha
            else if (columna > metaColumna) columna--; // Mover hacia la izquierda
        }    
    }
    

    private void ColocarFichasYObstaculos(int cantidadFichas, int porcentajeObstaculos, int cantidadArboles)
    {
        Random random = new Random();
        int filas = mapa.GetLength(0);
        int columnas = mapa.GetLength(1);
        int totalCeldas = (filas - 2) * (columnas - 2); // Total de celdas interiores

        // Calcular el número de obstáculos basados en el porcentaje
        int numeroObstaculos = totalCeldas * porcentajeObstaculos / 100;

        // Colocar fichas de recompensa
        for (int i = 0; i < cantidadFichas; i++)
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1);
                columna = random.Next(1, columnas - 1);
            } while (mapa[fila, columna] != "   "); // Asegurarse que el espacio esté vacío

            mapa[fila, columna] = "💰 "; // Colocar ficha de recompensa
        }

        // Colocar árboles
        for (int i = 0; i < cantidadArboles; i++)
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1);
                columna = random.Next(1, columnas - 1);
            } while (mapa[fila, columna] != "   "); // Asegurarse que el espacio esté vacío

            mapa[fila, columna] = "🌳 "; // Colocar árbol
        }

        // Colocar obstáculos
        for (int i = 0; i < numeroObstaculos; i++)
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1);
                columna = random.Next(1, columnas - 1);
            } while (mapa[fila, columna] != "   "); // Asegurarse que el espacio esté vacío

            mapa[fila, columna] = "⬜ "; // Colocar obstáculo
        }
    }

    public void ImprimirMapa(Jugador jugador1, Jugador jugador2)
    {
        int filas = mapa.GetLength(0);
        int columnas = mapa.GetLength(1);

        for (i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                // Verificar si la posición corresponde a un jugador
                if (i == jugador1.Posicion[0] && j == jugador1.Posicion[1])
                {
                    Console.Write("😠 ");
                }
                else if (i == jugador2.Posicion[0] && j == jugador2.Posicion[1])
                {
                    Console.Write("😎 ");
                }
                else
                {
                    Console.Write(mapa[i, j]); // Imprimir el contenido del mapa
                }
            }
            Console.WriteLine(); // Nueva línea al final de cada fila
        }
    }
}
