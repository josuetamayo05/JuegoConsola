namespace Prueba_por_Clases_2;

public class IA
{
    private Jugador jugadorIA;

    public IA(string nombre, int fila, int columna)
    {
        jugadorIA = new Jugador(nombre, fila, columna);
    }

    public Jugador GetJugadorIA()
    {
        return jugadorIA; // Obtener jugador IA
    }

    public void Mover(Mapa mapa, Jugador jugadorHumano)
    {
        int[] posicionIA = jugadorIA.Position;
        int[] meta = new int[] { 20, 20 };
        List<Nodo> camino = BuscarCaminoLee(posicionIA, meta, mapa);

        if (camino.Count > 0)
        {
            Nodo siguientePaso = camino[0];
            jugadorIA.Position[0] = siguientePaso.Fila;
            jugadorIA.Position[1] = siguientePaso.Columna;

            if (mapa.GetFicha(jugadorIA.Position[0], jugadorIA.Position[1]) == "💰 ")
            {
                mapa.SetFicha(jugadorIA.Position[0], jugadorIA.Position[1], "   ");
                jugadorIA.Puntos++;
                Console.WriteLine($"{jugadorIA.Nombre} ha recogido una ficha de recompensa! Puntos: {jugadorIA.Puntos}");

            }
        }
        else
        {
            EvaluarObstaculos(posicionIA, mapa);
        }
    }

    private List<Nodo> BuscarCaminoLee(int[] inicio, int [] meta, Mapa mapa)
    {
        Queue<Nodo> cola = new Queue<Nodo>();
        HashSet<Nodo> visitados = new HashSet<Nodo>();
        Nodo nodoInicial = new Nodo(inicio[0], inicio [1]);
        cola.Enqueue(nodoInicial);
        visitados.Add(nodoInicial);
        nodoInicial.Distancia = 0; // Inicializar la distancia del nodo inicial

        while (cola.Count > 0)
        {
            Nodo nodoActual = cola.Dequeue();
            // Comprobar si es el objetivo
            if (nodoActual.Fila == meta[0] && nodoActual.Columna == meta[1])
            {
                return ReconstruirCamino(nodoActual); // Método para reconstruir el camino
            }

            foreach (Nodo vecino in ObtenerVecinos(nodoActual, mapa))
            {
                if (!visitados.Contains(vecino))
                {
                    visitados.Add(vecino);
                    cola.Enqueue(vecino);
                    vecino.Distancia = nodoActual.Distancia + 1; // Actualizar la distancia
                    vecino.Padre = nodoActual; // Guardar el nodo padre para reconstruir el camino
                }
            }
        }
        return new List<Nodo>(); // Retornar vacío si no se encuentra camino
    }
    
    private List<Nodo> ObtenerVecinos(Nodo nodoActual, Mapa mapa)
    {
        List<Nodo> vecinos = new List<Nodo>();
            // Definir movimientos posibles (arriba, abajo, izquierda, derecha)
        int[,] movimientos = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        for (int i = 0; i < movimientos.GetLength(0); i++)
        {
            int nuevaFila = nodoActual.Fila + movimientos[i, 0]; // Acceder a la fila
            int nuevaColumna = nodoActual.Columna + movimientos[i, 1]; // Acceder a la columna

                // Verificar límites y obstáculos
            if (mapa.EsPosicionValida(nuevaFila, nuevaColumna))
            {
                vecinos.Add(new Nodo(nuevaFila, nuevaColumna));
            }
        }
        return vecinos;
    }

    private List<Nodo> ReconstruirCamino(Nodo nodoObjetivo)
    {
        List<Nodo> camino = new List<Nodo>();
        Nodo nodoActual = nodoObjetivo;

            // Reconstruir el camino hacia atrás
        while (nodoActual != null)
        {
            camino.Add(nodoActual);
            nodoActual = nodoActual.Padre; // Asumiendo que cada nodo tiene un puntero al nodo padre
        }

        camino.Reverse(); // Invertir el camino para que vaya desde el inicio hasta el objetivo
        return camino;
    }

    private void EvaluarObstaculos(int[] posicionIA, Mapa mapa)
    {
        string fichaActual = mapa.GetFicha(posicionIA[0], posicionIA[1]);

        // Supongamos que "🌳" representa un árbol y que la IA tiene un poder especial para atravesarlo
        if (fichaActual == "🌳")
        {
            // Verificar si la IA tiene un poder especial
            if (jugadorIA.TienePoderEspecial)
            {
                Console.WriteLine($"{jugadorIA.Nombre} usa su poder especial para atravesar el árbol!");
                // Aquí podrías implementar la lógica para mover a la IA a la siguiente posición
                MoverAdyacente(posicionIA, mapa);
            }
            else
            {
                Console.WriteLine($"{jugadorIA.Nombre} no puede avanzar, hay un árbol en su camino y no tiene poder especial.");
            }
        }
        else
        {
            Console.WriteLine($"{jugadorIA.Nombre} no puede avanzar, pero no hay obstáculos en su camino.");
        }
    }

    private void MoverAdyacente(int[] posicionIA, Mapa mapa)
    {
        // Lógica para mover a la IA a una posición adyacente
            // Por ejemplo, mover a la derecha si es posible
        int nuevaFila = posicionIA[0];
        int nuevaColumna = posicionIA[1] + 1; // Mover a la derecha

        if (mapa.EsPosicionValida(nuevaFila, nuevaColumna))
        {
            jugadorIA.Position[0] = nuevaFila;
            jugadorIA.Position[1] = nuevaColumna;
            Console.WriteLine($"{jugadorIA.Nombre} se ha movido a la posición ({nuevaFila}, {nuevaColumna}).");
        }
        else
        {
            Console.WriteLine($"{jugadorIA.Nombre} no puede moverse a la posición ({nuevaFila}, {nuevaColumna}), evaluando otras opciones...");
                // Aquí podrías implementar lógica adicional para intentar mover en otras direcciones
        }
    }

    public class Nodo
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public int Distancia { get; set; }
        public Nodo Padre { get; set; } // Para reconstruir el camino

        public Nodo(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
            Distancia = int.MaxValue; // Inicializar con un valor alto
            Padre = null; // Inicialmente no tiene padre
        }

        // Sobrecarga de Equals y GetHashCode para usar en HashSet
        public override bool Equals(object obj)
        {
            if (obj is Nodo nodo)
            {
                return Fila == nodo.Fila && Columna == nodo.Columna;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Fila, Columna).GetHashCode();
        }
    }
    
}