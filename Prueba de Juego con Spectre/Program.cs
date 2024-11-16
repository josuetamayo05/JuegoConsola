namespace PruebaJuegoSpectre;

using Spectre.Console;

class Program
{
    static string[,] mapa;
    static int puntosJugador1 = 0;
    static int puntosJugador2 = 0;
    static string nombreJugador1; 
    static string nombreJugador2;
    static int[] posicionInicialJugador1 = { 1, 1 }; // Posición inicial del jugador 1
    static int[] posicionInicialJugador2 = { 3, 1 }; // Posición inicial del jugador 2
    static int[] jugador1 = (int[])posicionInicialJugador1.Clone(); // Posición actual del jugador 1
    static int[] jugador2 = (int[])posicionInicialJugador2.Clone(); // Posición actual del jugador 2
    static Random random = new Random(); 


    static void Main(string[] args) // Iniciar el laberinto
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // Configurar codificación UTF-8

        MostrarMensajesBienvenida(); // Llamar al método de bienvenida
        MostrarInstrucciones();  // Llamar al método de instrucciones
        InicializarMapa(12, 12);  // Inicializar el mapa con 12 filas y 12 columnas
        ImprimirMapa();

        while (true)
        {
            MoverJugador(1);           
            if (ComprobarVictoria(jugador1))
            {
                break; // Salir del bucle si el jugador 1 ha ganado
            }

            MoverJugador(2);          
            if (ComprobarVictoria(jugador2))
            {
                break; // Salir del bucle si el jugador 2 ha ganado
            }
        }

        MostrarResultados(); // Mostrar resultados 
    }
    static void MostrarMensajesBienvenida()
    {
        AnsiConsole.MarkupLine("[bold cyan]Bienvenidos al juego del laberinto 👋👾![/]");
        AnsiConsole.MarkupLine("Presiona cualquier tecla para continuar...");
        Console.ReadKey();
        Console.Clear();

        nombreJugador1 = AnsiConsole.Ask<string>("Ingrese el nombre del [bold yellow]Jugador 1[/]: ");
        nombreJugador2 = AnsiConsole.Ask<string>("Ingrese el nombre del [bold yellow]Jugador 2[/]: ");
        Console.Clear();
    }

    static void MostrarInstrucciones()
    {
        AnsiConsole.MarkupLine($"Bienvenidos [bold yellow]({nombreJugador1})[/] y [bold yellow]({nombreJugador2})[/] a este emocionante juego del laberinto!");
        AnsiConsole.MarkupLine("Instrucciones del juego: 📝");
        AnsiConsole.MarkupLine("1. Usa las teclas W, A, S, D para mover a tu jugador: 🏃‍♂️");
        AnsiConsole.MarkupLine("   - W: Arriba ⬆");
        AnsiConsole.MarkupLine("   - A: Izquierda ⬅");
        AnsiConsole.MarkupLine("   - S: Abajo ⬇");
        AnsiConsole.MarkupLine("   - D: Derecha ➡");
        AnsiConsole.MarkupLine("2. 🚧 Evita los obstáculos marcados con 'X'.");
        AnsiConsole.MarkupLine("3. 🎁 Recoge las fichas de recompensa marcadas con '$' para ganar puntos.");
        AnsiConsole.MarkupLine("4. 😋 Por cada ficha recompensa que cojas puedes hacer un movimiento extra.");
        AnsiConsole.MarkupLine("5. 🏁 Llega a la meta marcada con 'M' para ganar el juego.");
        AnsiConsole.MarkupLine("¡Buena Suerte! Presiona cualquier tecla para continuar... ⌨");
        Console.ReadKey(); // Esperar a que el jugador presione una tecla
        Console.Clear();
    }

    static string ObtenerEmoji(string tipo)
    {
        switch (tipo)
        {
            case "#":
            return "[bold white]█[/]"; // Pared
            case "X":
            return "[bold yellow]👾[/]"; // Obstáculo
            case "$":
            return "[bold magenta]🎁[/]"; // Ficha de recompensa
            default:
            return "[white]  [/]"; // Espacio vacío
        }
    }
    
    static void InicializarMapa(int filas, int columnas)
    {
        mapa = new string[filas, columnas];

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                // Colocar bordes
                if (i == 0 || i == filas - 1 || j == 0 || j == columnas - 1)    
                {
                    mapa[i, j] = "#"; // Pared adornada en negro
                }
                else
                {
                    mapa[i, j] = " "; // Espacio vacío
                }
            }
        }
        
        // Asegurarse de que los jugadores tengan un camino inicial libre
        mapa[jugador1[0], jugador1[1]] = " "; // Asegurarse que el espacio de inicio del Jugador 1 esté vacío
        mapa[jugador2[0], jugador2[1]] = " "; // Jugador 2
        mapa[3, 2] = " ";  // Espacio vacío para que el jugador 2 pueda moverse
        mapa[1, 2] = " ";  // Espacio vacío para que el jugador 1 pueda moverse
        mapa[10, 10] = "🎉"; // Meta

        CrearCamino(jugador1[0], jugador1[1], 10, 10);

        GenerarObstaculos(20); // 20% probabilidad de obstáculos (porciento)

        for (int i = 0; i < 5; i++) // Colocar 5 fichas recompensa
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1);
                columna = random.Next(1, columnas - 1);
            } while (mapa[fila, columna] != " " || (fila == jugador1[0] && columna == jugador1[1]) || (fila == jugador2[0] && columna == jugador2[1])); // Asegurarse de que el espacio esté libre y no sea la posición de los jugadores

            mapa[fila, columna] = "$"; // Ficha de recompensa
        }        
    }

    static void CrearCamino(int inicioFila, int inicioColumna, int metaFila, int metaColumna)
    {
        int fila = inicioFila;
        int columna = inicioColumna;

        // Crear un camino simple hacia la meta
        while (fila != metaFila || columna != metaColumna)
        {
            mapa[fila, columna] = " "; // Asegurarse que el camino esté libre

            if (fila < metaFila) fila++; // Mover hacia abajo
            else if (fila > metaFila) fila--; // Mover hacia arriba

            if (columna < metaColumna) columna++; // Mover hacia la derecha
            else if (columna > metaColumna) columna--; // Mover hacia la izquierda
        }    
    }

    static void GenerarObstaculos(int porcentaje)
    {
        int filas = mapa.GetLength(0);
        int columnas = mapa.GetLength(1);
        int totalCeldas = (filas - 2) * (columnas - 2); // Total de celdas interiores
        // Calcular el número de obstáculos basados en el porcentaje
        int numeroObstaculos = totalCeldas * porcentaje / 100;
        for (int i = 0; i < numeroObstaculos; i++)
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1); // Generar fila aleatoria
            columna = random.Next(1, columnas - 1); // Generar columna aleatoria
            } while (mapa[fila, columna] != " " || (fila == jugador1[0] && columna == jugador1[1]) || (fila == jugador2[0] && columna == jugador2[1])); // Asegurarse que no se sobrescriba una pared o las posiciones de los jugadores
            mapa[fila, columna] = "X"; // Colocar obstáculo
        }
    }

    
    static void ImprimirMapa()
    {
        Console.Clear();
        int filas = mapa.GetLength(0);
        int columnas = mapa.GetLength(1);

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (i == jugador1[0] && j == jugador1[1])
                    AnsiConsole.Markup("[green]😁 [/]"); // Representa al jugador 1
                else if (i == jugador2[0] && j == jugador2[1])
                    AnsiConsole.Markup("[blue]😜 [/]"); // Representa al jugador 2
                else
                    AnsiConsole.Markup(ObtenerEmoji(mapa[i, j])); // Obtener el emoji correspondiente
            }
            Console.WriteLine(); // Salto de línea al final de cada fila
        }

        AnsiConsole.MarkupLine($"[bold yellow]Puntos {nombreJugador1}:[/][green]{puntosJugador1}[/] | [bold yellow]Puntos {nombreJugador2}:[/][blue]{puntosJugador2}[/]");
    }

    static void MostrarResultados()
    {
        var table = new Table();
        table.AddColumn("Jugador");
        table.AddColumn("Puntos");

        table.AddRow(nombreJugador1, puntosJugador1.ToString());
        table.AddRow(nombreJugador2, puntosJugador2.ToString());

        AnsiConsole.Render(table);
        AnsiConsole.MarkupLine("Presiona cualquier tecla para salir...");
        Console.ReadKey(); // Esperar a que el jugador presione una tecla
    }
        
    
    static void MoverJugador(int jugador)
    {
        int filas = mapa.GetLength(0);
        int columnas = mapa.GetLength(1);
        bool movimientoExtra = false; // Controlar si se permite un movimiento extra
        string nombreJugadorActual = jugador == 1 ? nombreJugador1 : nombreJugador2;
            
        do
        {
            AnsiConsole.MarkupLine($"Turno de [bold yellow]{nombreJugadorActual}[/] (Jugador {jugador}), ingresa tu movimiento (W/A/S/D) o [bold red]Q[/] para salir: ");
            char movimiento = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Salto de línea después de la entrada

            if (movimiento == 'q' || movimiento == 'Q') // Condición de salida
            {
                AnsiConsole.MarkupLine("[bold green]¡Gracias por jugar![/]");
                Console.ReadLine();
                Environment.Exit(0); // Salir del juego
            }

            int[] posicion = jugador == 1 ? jugador1 : jugador2;
            int nuevaFila = posicion[0];
            int nuevaColumna = posicion[1];

            switch (movimiento)
            {
                case 'w': nuevaFila--; break; // Arriba
                case 's': nuevaFila++; break; // Abajo
                case 'a': nuevaColumna--; break; // Izquierda
                case 'd': nuevaColumna++; break; // Derecha
                default:
                    AnsiConsole.MarkupLine("[bold red]Movimiento no válido. Intenta de nuevo.[/]");
                    continue; // Si la tecla no es válida, volver a pedir movimiento
            }

            // Verificar límites
            if (nuevaFila > 0 && nuevaColumna > 0 && nuevaFila < filas - 1 && nuevaColumna < columnas - 1)
            {
                // Comprobar si ha caído en una trampa
                if (mapa[nuevaFila, nuevaColumna] == "X")
                {                    
                    // Restaurar la posición inicial
                    if (jugador == 1)
                    {
                        jugador1[0] = posicionInicialJugador1[0]; // Restaurar posición inicial del jugador 1
                        jugador1[1] = posicionInicialJugador1[1];
                    }
                    else
                    {
                        jugador2[0] = posicionInicialJugador2[0]; // Restaurar posición inicial del jugador 2
                        jugador2[1] = posicionInicialJugador2[1];
                    }
                    AnsiConsole.MarkupLine($"[bold red]{nombreJugadorActual}, has caído en una trampa![/]");
                    Console.ReadLine();
                    ImprimirMapa(); // Imprimir el mapa después de restaurar la posición
                    return; // Salir del método para evitar más movimientos
                }

                // Verificar si puede moverse a un nuevo espacio
                if (mapa[nuevaFila, nuevaColumna] != "X") // No puede moverse a un obstáculo
                {
                    // Actualizar posición del jugador
                    posicion[0] = nuevaFila;
                    posicion[1] = nuevaColumna;

                    // Comprobar si ha recogido una recompensa
                    if (mapa[nuevaFila, nuevaColumna] == "$")
                    {
                        mapa[nuevaFila, nuevaColumna] = " "; // Limpiar la posición
                        if (jugador == 1)
                        {                            
                            puntosJugador1++;
                            AnsiConsole.MarkupLine($"¡[bold green]{nombreJugador1}[/] ha recogido una ficha de recompensa! Puntos: [bold green]{puntosJugador1}[/]");                            
                        }
                        else
                        {
                            puntosJugador2++;
                            AnsiConsole.MarkupLine($"¡[bold blue]{nombreJugador2}[/] ha recogido una ficha de recompensa! Puntos: [bold blue]{puntosJugador2}[/]");                        
                        }
                        movimientoExtra = true; // Permitir un movimiento extra
                    }
                    // Imprimir mapa después de un movimiento válido
                    ImprimirMapa();

                    if (movimientoExtra)
                    {
                        AnsiConsole.MarkupLine($"{nombreJugadorActual} has recogido una recompensa.");
                        break; // Salir del bucle para permitir un solo movimiento extra
                    }
                    
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]¡No puedes moverte allí! Hay un obstáculo.[/]");
                    break; // Salir del bucle si hay un obstáculo
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]¡Te has salido de los límites![/]");
                break; // Salir del bucle si se sale de los límites
            }
            

        } while (movimientoExtra); // Continuar si se ha recogido una ficha y no se ha caído en una trampa
    }   
    
    static bool ComprobarVictoria(int[] jugador)
    {
        // Definir la posición de la meta
        int metaFila = 10;
        int metaColumna = 10;

        // Comprobar si la posición del jugador coincide con la posición de la meta
        if (jugador[0] == metaFila && jugador[1] == metaColumna)
        {
            string nombreJugadorActual = jugador == jugador1 ? nombreJugador1 : nombreJugador2;
            AnsiConsole.MarkupLine($"[bold green]{nombreJugadorActual} ha alcanzado la meta y ha ganado el juego! 🎉[/]");
            return true; // El jugador ha ganado
        }

        return false; // El jugador no ha ganado
    }   
}