namespace Prueba_por_Clases_2;

public class IA
{
    private Jugador jugadorIA;

    public IA(string nombre, int fila, int columna)
    {
        jugadorIA = new Jugador(nombre, fila, columna);
    }

    public void Mover(Mapa mapa, Jugador jugadorHumano)
    {
        Random rand = new Random(); 
        List<(int, int)> posiblesMovimientos = new List<(int, int)>();

        int[][] movimientos = new int[][]
        {
            new int[] { -1, 0 }, // Arriba
            new int[] { 1, 0 },  // Abajo
            new int[] { 0, -1 }, // Izquierda
            new int[] { 0, 1 }   // Derecha
        };

        foreach (var movimiento in movimientos)
        {
            int nuevaFila = jugadorIA.Position[0] + movimiento[0];
            int nuevaColumna = jugadorIA.Position[1] + movimiento[1];

            // Verificar límites y si la nueva posición es un espacio vacío o una ficha
            if (nuevaFila >= 0 && nuevaFila < mapa.Rows && nuevaColumna >= 0 && nuevaColumna < mapa.Cols)
            {
                string ficha = mapa.GetFicha(nuevaFila, nuevaColumna);
                if (ficha == "   " || ficha == "💰 ") // Espacio vacío o ficha de recompensa
                {
                    posiblesMovimientos.Add((nuevaFila, nuevaColumna));
                }
            }
        }

        if (posiblesMovimientos.Count > 0)
        {
            var movimientoElegido = posiblesMovimientos[rand.Next(posiblesMovimientos.Count)];
            jugadorIA.Position[0] = movimientoElegido.Item1;
            jugadorIA.Position[1] = movimientoElegido.Item2;

            // Verificar si ha recogido una ficha de recompensa
            if (mapa.GetFicha(jugadorIA.Position[0], jugadorIA.Position[1]) == "💰 ")
            {
                mapa.SetFicha(jugadorIA.Position[0], jugadorIA.Position[1], "   "); // Limpiar la ficha
                jugadorIA.Puntos++; // Incrementar puntos
                Console.WriteLine($"{jugadorIA.Nombre} ha recogido una ficha de recompensa! Puntos: {jugadorIA.Puntos}");
            }
        }
    }

    public Jugador GetJugadorIA()
    {
        return jugadorIA; // Obtener jugador IA
    }
}