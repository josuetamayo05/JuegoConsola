/*public class Guardian
{
    private string nombre;
    private int fila;
    private int columna;
    private int velocidad;

    public Guardián(string nombre, int fila, int columna, int velocidad)
    {
        this.nombre = nombre;
        this.fila = fila;
        this.columna = columna;
        this.velocidad = velocidad;
    }

    public void Mover(Mapa mapa, Jugador jugador)
    {
        // Calcula la distancia entre el guardián y el jugador
        int distanciaFila = Math.Abs(this.fila - jugador.Fila);
        int distanciaColumna = Math.Abs(this.columna - jugador.Columna);

        // Si el guardián está en la misma fila que el jugador, mueve hacia la columna del jugador
        if (distanciaFila == 0)
        {
            if (this.columna < jugador.Columna)
            {
                this.columna += this.velocidad;
            }
            else if (this.columna > jugador.Columna)
            {
                this.columna -= this.velocidad;
            }
        }
        // Si el guardián está en la misma columna que el jugador, mueve hacia la fila del jugador
        else if (distanciaColumna == 0)
        {
            if (this.fila < jugador.Fila)
            {
                this.fila += this.velocidad;
            }
            else if (this.fila > jugador.Fila)
            {
                this.fila -= this.velocidad;
            }
        }
        // Si el guardián no está en la misma fila ni columna que el jugador, mueve hacia la diagonal del jugador
        else
        {
            if (this.fila < jugador.Fila)
            {
                this.fila += this.velocidad;
            }
            else if (this.fila > jugador.Fila)
            {
                this.fila -= this.velocidad;  
            }

            if (this.columna < jugador.Columna)
            {
                this.columna += this.velocidad;
            }
            else if (this.columna > jugador.Columna)
            {
                this.columna -= this.velocidad;
            }
        }

        // Verifica si el guardián ha capturado al jugador
        if (this.fila == jugador.Fila && this.columna == jugador.Columna)
        {
            Console.WriteLine($"El guardián {this.nombre} ha capturado al jugador {jugador.Nombre}!");
        }
    }
}*/