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
        int movimiento = rand.Next(4); // 0: Arriba, 1: Abajo, 2: Izquierda, 3: Derecha

        int nuevaFila = jugadorIA.Position[0];
        int nuevaColumna = jugadorIA.Position[1];

        switch (movimiento)
        {
            case 0: nuevaFila--; break; //arrib
            case 1: nuevaFila++; break; //abaj
            case 2: nuevaColumna--; break;
            case 3: nuevaColumna++; break;
        }

        if (nuevaFila >= 0 && nuevaColumna >= 0 && nuevaFila < mapa.Rows && nuevaColumna < mapa.Cols)
        {
            jugadorIA.Position[0] = nuevaFila;
            jugadorIA.Position[1] = nuevaColumna;
            Console.WriteLine($"{jugadorIA.Nombre} se ha movido a la posición ({nuevaFila}, {nuevaColumna}).");
        }
    }

    public Jugador GetJugadorIA()
    {
        return jugadorIA;
    }
}