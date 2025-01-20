/*public class Guardian
{
    private int x;
    private int y;
    private int velocidad;

    public Guardian(int x, int y, int velocidad)
    {
        this.x = x;
        this.y = y;
        this.velocidad = velocidad;
    }

    public void Mover(Jugador jugador)
    {
        // Calcula la distancia entre el guardián y el jugador
        int distanciaX = Math.Abs(this.x - jugador.X);
        int distanciaY = Math.Abs(this.y - jugador.Y);

        // Si el guardián está en la misma fila que el jugador, mueve hacia la columna del jugador
        if (distanciaY == 0)
        {
            if (this.x < jugador.X)
            {
                this.x += this.velocidad;
            }
            else if (this.x > jugador.X)
            {
                this.x -= this.velocidad;
            }
        }
        // Si el guardián está en la misma columna que el jugador, mueve hacia la fila del jugador
        else if (distanciaX == 0)
        {
            if (this.y < jugador.Y)
            {
                this.y += this.velocidad;
            }
            else if (this.y > jugador.Y)
            {
                this.y -= this.velocidad;
            }
        }
        // Si el guardián no está en la misma fila ni columna que el jugador, mueve hacia la diagonal del jugador
        else
        {
            if (this.x < jugador.X)
            {
                this.x += this.velocidad;
            }
            else if (this.x > jugador.X)
            {
                this.x -= this.velocidad;
            }

            if (this.y < jugador.Y)
            {
                this.y += this.velocidad;
            }
            else if (this.y > jugador.Y)
            {
                this.y -= this.velocidad;
            }
        }

        // Verifica si el guardián ha atrapado al jugador
        if (this.x == jugador.X && this.y == jugador.Y)
        {
            Console.WriteLine("El guardián ha atrapado al jugador!");
        }
    }
}*/