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