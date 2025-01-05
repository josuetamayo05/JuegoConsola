using System;

class Program
{
    static void Main(string[] args)
    {
        int jugadorX = 0;
        int jugadorY = 0;

        while (true)
        {
            Console.Clear();

            // Dibujar el mapa
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (x == jugadorX && y == jugadorY)
                    {
                        Console.Write("P"); // Dibujar al jugador
                    }
                    else
                    {
                        Console.Write("."); // Dibujar el terreno
                    }
                }
                Console.WriteLine();
            }

            // Solicitar movimiento
            Console.WriteLine("Ingresa tu movimiento (W/A/S/D): ");
            char movimiento = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Salto de línea después de la entrada

            // Calcular la nueva posición
            switch (movimiento)
            {
                case 'w': jugadorY--; break; // Arriba
                case 's': jugadorY++; break; // Abajo
                case 'a': jugadorX--; break; // Izquierda
                case 'd': jugadorX++; break; // Derecha
                default:
                    Console.WriteLine("Movimiento no válido. Intenta de nuevo.");
                    continue; // Si la tecla no es válida, volver a pedir movimiento
            }

            // Verificar límites del mapa
            if (jugadorX < 0 || jugadorX >= 10 || jugadorY < 0 || jugadorY >= 10)
            {
                Console.WriteLine("Movimiento fuera de los límites del mapa. Intenta de nuevo.");
                continue;
            }

            // Esperar un poco antes de seguir
            System.Threading.Thread.Sleep(100);
        }
    }
}
