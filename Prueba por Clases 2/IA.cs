namespace Prueba_por_Clases_2;

public class IA : Jugador
{
    private Jugador jugadorIA;
    private Random random;
    private int poderCapturaUsado = 0;

    public IA() : base("IA", 1, 25, null)
    {
        jugadorIA = new Jugador("IA", 1, 25, null);
        random = new Random();
    }

    public Jugador GetJugadorIA()
    {
        return jugadorIA; 
    }

    public void Mover(Mapa mapa, Jugador jugadorHumano)
    {
        int[] movimiento = DecidirMovimiento(jugadorHumano, mapa);

        if (movimiento != null)
        {
            int nuevaFila = jugadorIA.Position[0] + movimiento[0];
            int nuevaColumna = jugadorIA.Position[1] + movimiento[1];

              if (nuevaFila >= 0 && nuevaFila < mapa.Rows && nuevaColumna >= 0 && nuevaColumna < mapa.Cols)
            {
                jugadorIA.Position[0] = nuevaFila;
                jugadorIA.Position[1] = nuevaColumna;

                // Verificar si la IA ha atrapado al jugador
                if (jugadorIA.Position[0] == jugadorHumano.Position[0] || jugadorIA.Position[1] == jugadorHumano.Position[1])
                {
                    
                    if (poderCapturaUsado < 2)
                    {
                        Console.Write("Capturando al otro jugador");
                        for (int j = 0; j < 3; j++)
                        {
                            Console.Write("."); // Mostrar puntos para la animación
                            System.Threading.Thread.Sleep(500); // Esperar un poco
                        }
                        Console.WriteLine();
                        EnviarJugadorAPosicionInicial(jugadorHumano);
                        poderCapturaUsado++;
                    }  
                    else
                    {
                        Console.WriteLine("La IA no puede usar el poder de captura de nuevo.");
                    }  
                }
            }
        }
    }    

    private void EnviarJugadorAPosicionInicial(Jugador jugador)
    {
        jugador.Position[0] = 1;
        jugador.Position[1] = 1;
        Console.WriteLine($"{jugador.Nombre} ha sido enviado a su posición inicial.");
    }

    public int[] DecidirMovimiento(Jugador jugador, Mapa mapa)
    {
        int[] posicionIA = jugadorIA.Position;
        int[] posicionJugador = jugador.Position;

            // Prioridad 1: Atrapar al jugador si está en la misma fila o columna
        if (posicionIA[0] == posicionJugador[0] || posicionIA[1] == posicionJugador[1])
        {
            return MoverseHacia(posicionJugador[0], posicionJugador[1]);
        }

            // Prioridad 2: Moverse hacia la meta
        int[] posicionMeta = BuscarMeta(mapa);
        
        if (posicionMeta != null)
        {
            return MoverseHacia(posicionMeta[0], posicionMeta[1]);
        }

            // Si no hay jugador cerca ni meta, moverse aleatoriamente
        return MoverseAleatoriamente(mapa);
    }

    private int[] MoverseHacia(int objetivoX, int objetivoY)
    {
        int[] posicionIA = jugadorIA.Position;
        int[] movimiento = new int[2];

        if (objetivoX > posicionIA[0])
            movimiento[0] = 1; // Mover abajo
        else if (objetivoX < posicionIA[0])
            movimiento[0] = -1; // Mover arriba
        else
            movimiento[0] = 0;

        if (objetivoY > posicionIA[1])
            movimiento[1] = 1; // Mover derecha
        else if (objetivoY < posicionIA[1])
            movimiento[1] = -1; // Mover izquierda
        else
            movimiento[1] = 0;

        return movimiento;
    }

    private int[] BuscarMeta(Mapa mapa)
    {
        for (int i = 0; i < mapa.Rows; i++)
        {
            for (int j = 0; j < mapa.Cols; j++)
            {
                if (mapa.GetFicha(i, j) == "🏠 ")
                {
                    return new int[] { i, j };
                }
            }
        }
        return null; // No se encontró la meta
    }

    private int[] MoverseAleatoriamente(Mapa mapa)
    {
        var movimientos = new(int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        var movimiento = movimientos[random.Next(movimientos.Length)];

        int nuevaFila = jugadorIA.Position[0] + movimiento.Item1;
        int nuevaColumna = jugadorIA.Position[1] + movimiento.Item2;

        // Verificar si el movimiento es válido
        if (nuevaFila >= 0 && nuevaFila < mapa.Rows && nuevaColumna >= 0 && nuevaColumna < mapa.Cols)
        {
            return new int[] { movimiento.Item1, movimiento.Item2 };
        }
        else
        {
            // Si el movimiento no es válido, intentar otro movimiento aleatorio
            return MoverseAleatoriamente(mapa);
        }
    }
}

    
    
