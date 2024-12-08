namespace ProbarEmojiMapa;

using System;

class Program
{
    static string[,] mapa;
    static int filas = 10; // Puedes cambiar el tamaño del mapa
    static int columnas = 10;

    static void Main(string[] args)
    {
        InicializarMapa();
        ImprimirMapa();
        
        // Aquí puedes probar diferentes emojis
        ProbarEmojis();
        
    }

    static void InicializarMapa()
    {
        mapa = new string[filas, columnas];

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                // Colocar bordes
                if (i == 0 || i == filas - 1 || j == 0 || j == columnas - 1)
                {
                    mapa[i, j] = "⬜"; // Pared (2 caracteres)
                }
                else
                {
                    mapa[i, j] = "   "; // Espacio vacío (3 caracteres)
                }
            }
        }

        // Colocar la meta
        mapa[5, 5] = "🏁 "; // Meta (2 caracteres)

        // Colocar fichas de recompensa
        mapa[2, 2] = "💰 "; // Ficha de recompensa (2 caracteres)
        mapa[3, 3] = "🚪 "; // Otra ficha de recompensa (2 caracteres)
        mapa[6, 5] = "😠 "; // Otra ficha de recompensa (2 caracteres)
        mapa[8, 7] = "😎 "; // Otra ficha de recompensa (2 caracteres)
        mapa[4, 6] = "🏆 "; // Otra ficha de recompensa (2 caracteres)
        mapa[7, 8] = "🥇 "; // Otra ficha de recompensa (2 caracteres)
        mapa[5, 6] = "🌳 "; // Otra ficha de recompensa (2 caracteres)
        mapa[7, 6] = "🔑 "; // Otra ficha de recompensa (2 caracteres)
        mapa[2, 3] = "🏠 "; // Otra ficha de recompensa (2 caracteres)
        mapa[4, 5] = "🚧 "; // Otra ficha de recompensa (2 caracteres)
        mapa[2, 8] = "🎁 "; // Otra ficha de recompensa (2 caracteres)

    }

    static void ImprimirMapa()
    {
        Console.Clear();
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Console.Write(mapa[i, j]); // Imprimir cada celda
            }
            Console.WriteLine(); // Salto de línea al final de cada fila
        }
    }
    
    static void ProbarEmojis()
    {
        Console.WriteLine("Probar diferentes emojis para las paredes:");
        string[] emojis = { "🟫", "🟥", "⬜ ", "🟨", "🚧" }; // Diferentes emojis para las paredes

        foreach (var emoji in emojis)
        {
            // Reemplazar las paredes en el mapa
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (i == 0 || i == filas - 1 || j == 0 || j == columnas - 1)
                    {
                        mapa[i, j] = emoji; // Asignar el emoji a las paredes
                    }
                }
            }

            ImprimirMapa();
            Console.WriteLine("Presiona cualquier tecla para probar el siguiente emoji...");
            Console.ReadKey(); // Esperar a que el usuario presione una tecla
        }
    }
}
/*
static void InicializarMapa(int filas, int columnas)
    {
        mapa = new string[filas, columnas];

        for (int i = 0; i < filas; i++)
        {    
            for (int j = 0; j < columnas; j++)
            {    
                mapa[i, j] = "⬜ ";
            }    
        }
        GenerarLaberinto();
        
        // Asegurarse de que los jugadores tengan un camino inicial libre
        mapa[jugador1[0], jugador1[1]] = "   "; // Asegurarse que el espacio de inicio del Jugador 1 esté vacío
        mapa[jugador2[0], jugador2[1]] = "   "; // Jugador 2
        mapa[17, 17] = "🏠 "; // Meta

        ColocarFichasYObstaculos(8, 30, 10); // fichas, porcentaje de obstáculos, árboles       
    }

    private static void GenerarLaberinto()
    {
        bool[,] visitadas = new bool[mapa.GetLength(0), mapa.GetLength(1)];
        List<Tuple<int, int>> bordes = new List<Tuple<int, int>>();
        //Comenzar desde la celda (1, 1)
        visitadas[1, 1] = true;
        mapa[1, 1] = "   ";
        AgregarBordes(1, 1, bordes);

        while (bordes.Count > 0)
        {
            // Elegir borde aleatorio
            int index = random.Next(bordes.Count);
            var borde = bordes[index];
            bordes.RemoveAt(index);

            int fila = borde.Item1;
            int columna = borde.Item2;

            //Determinar las celdas adyacentes
            int adyacenteFila = fila % 2 == 0 ? fila + 1 : fila - 1;
            int adyacenteColumna = columna % 2 == 0 ? columna + 1 : columna - 1;

            // Verificar si la celda adyacente está dentro de los límites
            if (adyacenteFila > 0 && adyacenteFila < mapa.GetLength(0) && adyacenteColumna > 0 && adyacenteColumna < mapa.GetLength(1))
            {
                if (!visitadas[adyacenteFila, adyacenteColumna])
                {
                    //eliminar borde
                    mapa[fila, columna] = "   "; //Convertir la pared en un espacio en blanco
                    visitadas[adyacenteFila, adyacenteColumna] = true; // Marcar la celda adyacente como visitada
                    AgregarBordes(adyacenteFila, adyacenteColumna, bordes); //Agregar los bordes de la nueva celda

                }
            }
        }
    }

    private static void AgregarBordes(int fila, int columna, List<Tuple<int, int>> bordes)
    {
        // Agregar los bordes de las celdas adyacentes
        if (fila - 1 > 0) bordes.Add(new Tuple<int, int>(fila - 1, columna)); // Arriba
        if (fila + 1 < mapa.GetLength(0)) bordes.Add(new Tuple<int, int>(fila + 1, columna)); // Abajo
        if (columna - 1 > 0) bordes.Add(new Tuple<int, int>(fila, columna - 1)); // Izquierda
        if (columna + 1 < mapa.GetLength(1)) bordes.Add(new Tuple<int, int>(fila, columna + 1)); // Derecha*/