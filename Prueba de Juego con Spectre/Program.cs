using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PruebaJuegoSpectre;

class Program
{
    static string[,] mapa;
    static int puntosJugador1 = 0;
    static int puntosJugador2 = 0;
    static string nombreJugador1; 
    static string nombreJugador2;
    static int[] posicionInicialJugador1 = { 1, 1 }; // Posición inicial del jugador 1
    static int[] posicionInicialJugador2 = { 1, 13 }; // Posición inicial del jugador 2
    static int[] jugador1 = (int[])posicionInicialJugador1.Clone(); // Posición actual del jugador 1
    static int[] jugador2 = (int[])posicionInicialJugador2.Clone(); // Posición actual del jugador 2
    static Random random = new Random(); 
    static int metaFila = 13;
    static int metaColumna = 7;
    static int[] puerta1 = { 3, 5 };
    static int[] puerta2 = { 10, 5 };


    static void Main(string[] args) // Iniciar el laberinto
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // Configurar codificación UTF-8

        MostrarMensajesBienvenida(); // Llamar al método de bienvenida
        MostrarInstrucciones();  // Llamar al método de instrucciones
        InicializarMapa(15, 15);  // Inicializar el mapa con 12 filas y 12 columnas
        ImprimirMapa();
        // Verificar si hay un camino a la meta
        if (!HayCamino(posicionInicialJugador1[0], posicionInicialJugador1[1], metaFila, metaColumna) || !HayCamino(posicionInicialJugador2[0], posicionInicialJugador2[1], metaFila, metaColumna))
        {
            Console.WriteLine("No hay un camino accesible a la meta. Regenerando el laberinto...");
            InicializarMapa(15, 15); // Regenerar el laberinto
        }
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
        // Crear un panel para mostrar el mensaje de bienvenida
        var panel = new Panel(new Markup(
            "[bold cyan]Bienvenidos al juego del laberinto 👋👾![/]\n" +
            "Presiona cualquier tecla para continuar..."
        ));

        // Configurar el panel
        panel.Border = BoxBorder.Rounded; // Bordes redondeados
        panel.Padding = new Padding(2); // Añadir un poco de espacio interno
        panel.Header = new PanelHeader("[bold yellow]¡Bienvenido![/]"); // Encabezado del panel

        // Renderizar el panel
        AnsiConsole.Render(panel);

        Console.ReadKey(); // Esperar a que el jugador presione una tecla
        Console.Clear(); // Limpiar la consola
    
        nombreJugador1 = AnsiConsole.Ask<string>("Ingrese el nombre del [bold yellow]Jugador 1[/]: ");
        nombreJugador2 = AnsiConsole.Ask<string>("Ingrese el nombre del [bold yellow]Jugador 2[/]: ");
        Console.Clear();
    }

    static void MostrarInstrucciones()
    {
        // Crear un panel para mostrar las instrucciones
        var panel = new Panel(new Markup(
            "[bold yellow]Instrucciones del Juego:[/]\n" +
            "1. Usa las teclas W, A, S, D para mover a tu jugador: 🏃‍♂️\n" +
            "   - W: Arriba ⬆\n" +
            "   - A: Izquierda ⬅\n" +
            "   - S: Abajo ⬇\n" +
            "   - D: Derecha ➡\n" +
            "2. 🚧 Evita los obstáculos marcados con '⛔'.\n" +
            "3. 🎁 Recoge las fichas de recompensa marcadas con '💰' para ganar puntos.\n" +
            "4. 😋 Por cada ficha recompensa que cojas puedes hacer un movimiento extra.\n" +
            "5. 🏁 Llega a la meta marcada con '🚩' para ganar el juego.\n" +
            "¡Buena Suerte! Presiona cualquier tecla para continuar... ⌨"
        ));

        // Configurar el panel
        panel.Border = BoxBorder.Rounded; // Bordes redondeados
        panel.Header = new PanelHeader("[bold cyan]Instrucciones[/]"); // Encabezado del panel

        // Renderizar el panel
        AnsiConsole.Render(panel);

        Console.ReadKey(); // Esperar a que el jugador presione una tecla
        Console.Clear(); // Limpiar la consola
    }
    
    static void InicializarMapa(int filas, int columnas)
    {
        mapa = new string[filas, columnas];

        // Inicializar el laberinto con paredes
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                mapa[i, j] = "⬜ "; // Pared
            }
        }

        
        GenerarLaberinto(1, 1);
        
        mapa[posicionInicialJugador1[0], posicionInicialJugador1[1]] = "   "; // Jugador 1
        mapa[posicionInicialJugador2[0], posicionInicialJugador2[1]] = "   "; // Jugador 2
        mapa[metaFila, metaColumna] = "🏠 "; // Meta
        mapa[puerta1[0], puerta1[1]] = "🚪 "; // Puerta 1
        mapa[puerta2[0], puerta2[1]] = "🚪 "; // Puerta 2

        ColocarFichasYObstaculos(5, 6);       
    }

    static void GenerarLaberinto(int fila, int columna)
    {
        // Marcar la celda actual como parte del camino
        mapa[fila, columna] = "   ";

        // Definir los movimientos posibles (arriba, abajo, izquierda, derecha)
        var movimientos = new (int, int)[]
        {
            (-2, 0), // Arriba
            (2, 0),  // Abajo
            (0, -2), // Izquierda
            (0, 2)   // Derecha
        };

        // Mezclar los movimientos para aleatoriedad
        movimientos = movimientos.OrderBy(x => random.Next()).ToArray();

        foreach (var (dRow, dCol) in movimientos)
        {
            int nuevaFila = fila + dRow;
            int nuevaColumna = columna + dCol;

            // Verificar si la nueva posición está dentro de los límites
            if (nuevaFila > 0 && nuevaFila < mapa.GetLength(0) && nuevaColumna > 0 && nuevaColumna < mapa.GetLength(1) && mapa[nuevaFila, nuevaColumna] == "⬜ ")
            {
                // Eliminar la pared entre la celda actual y la nueva celda
                mapa[fila + dRow / 2, columna + dCol / 2] = "   "; // Espacio vacío
                GenerarLaberinto(nuevaFila, nuevaColumna); // Recursión
            }
        }
    }

    
    static bool HayCamino(int startRow, int startCol, int metaRow, int metaCol)
    {
        // Crear una cola para la búsqueda
        Queue<(int, int)> queue = new Queue<(int, int)>();
        bool[,] visitado = new bool[mapa.GetLength(0), mapa.GetLength(1)];

        // Agregar la posición inicial a la cola y marcarla como visitada
        queue.Enqueue((startRow, startCol));
        visitado[startRow, startCol] = true;

        // Definir los movimientos posibles (arriba, abajo, izquierda, derecha)
        int[,] movimientos = new int[,]
        {
            { -2, 0 }, // Arriba
            { 2, 0 },  // Abajo
            { 0, -2 }, // Izquierda
            { 0, 2 }   // Derecha
        };

        while (queue.Count > 0)
        {
            var (filaActual, colActual) = queue.Dequeue();

            // Comprobar si hemos llegado a la meta
            if (filaActual == metaRow && colActual == metaCol)
            {
                return true; // Hay un camino a la meta
            }

            // Explorar las celdas adyacentes
            for (int i = 0; i < movimientos.GetLength(0); i++)
            {
                int nuevaFila = filaActual + movimientos[i, 0];
                int nuevaColumna = colActual + movimientos[i, 1];

                // Verificar si la nueva posición está dentro de los límites y es un espacio vacío
                if (nuevaFila > 0 && nuevaFila < mapa.GetLength(0) - 1 &&
                    nuevaColumna > 0 && nuevaColumna < mapa.GetLength(1) - 1 &&
                    mapa[nuevaFila, nuevaColumna] == "   " && !visitado[nuevaFila, nuevaColumna])
                {
                    // Marcar la nueva posición como visitada y agregarla a la cola
                    visitado[nuevaFila, nuevaColumna] = true;
                    queue.Enqueue((nuevaFila, nuevaColumna));
                }
            }
        }

        return false; // No hay camino a la meta
    }


    static void ColocarFichasYObstaculos(int cantidadFichas, int cantidadArboles)
    {
        int filas = mapa.GetLength(0);
        int columnas = mapa.GetLength(1);
        int totalCeldas = (filas - 2) * (columnas - 2); // Total de celdas interiores

        // Calcular el número de obstáculos basados en el porcentaje
        //int numeroObstaculos = totalCeldas * porcentajeObstaculos / 100;

        // Colocar fichas de recompensa
        for (int i = 0; i < cantidadFichas; i++)
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1);
                columna = random.Next(1, columnas - 1);
            } while (mapa[fila, columna] != "   "); // Asegurarse que el espacio esté vacío

            mapa[fila, columna] = "💰 "; // Colocar ficha de recompensa
        }

        // Colocar árboles
        for (int i = 0; i < cantidadArboles; i++)
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1);
                columna = random.Next(1, columnas - 1);
            } while (mapa[fila, columna] != "   " || (fila == puerta1[0] && columna == puerta1[1] || fila == puerta2[0] && columna == puerta2[1])); // Asegurarse que el espacio esté vacío

            if (EsPosicionEstrategica(fila, columna))
            {
                mapa[fila, columna] = "🌳 "; // Colocar árbol
            }    
        }
    }

    static bool EsPosicionEstrategica(int fila, int columna)
    {
        // Verificar si hay un camino desde la posición (fila, columna) a la meta
        bool caminoMeta = HayCamino(fila, columna, metaFila, metaColumna);

        bool caminoPuerta1 = HayCamino(fila, columna, puerta1[0], puerta1[1]);

        bool caminoPuerta2 = HayCamino(fila, columna, puerta2[0], puerta2[1]);

        return !caminoMeta || !caminoPuerta1 || !caminoPuerta2;
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
                    Console.Write("😠 "); // Representa al jugador 1                   
                else if (i == jugador2[0] && j == jugador2[1])
                    Console.Write("😎 "); // Representa al jugador 2
                else
                Console.Write(mapa[i, j] + ""); // Imprimir cada carácter sin salto de línea
            }
            Console.WriteLine(); // Salto de línea al final de cada fila
        }

        AnsiConsole.MarkupLine($"[bold blue]🎲 Puntos {nombreJugador1} : [/][red]{puntosJugador1}[/] | [bold blue]🎲 Puntos {nombreJugador2} : [/][red]{puntosJugador2}[/]");
        Console.WriteLine();
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
        string mensajeRecompensa = "";
        string mensajeTrampa = "";
            
        do
        {
            // Mostrar el mensaje de recompensa si existe
            if (!string.IsNullOrEmpty(mensajeRecompensa))
            {
                Console.WriteLine(mensajeRecompensa);
                mensajeRecompensa = ""; // Limpiar el mensaje de recompensa
            }
            
            if (!string.IsNullOrEmpty(mensajeTrampa))
            {
                Console.WriteLine(mensajeTrampa);
                mensajeTrampa = ""; // Limpiar el mensaje de recompensa
                ImprimirMapa(); // Imprimir el mapa después de mostrar el mensaje de trampa
                break; // Salir del bucle para no seguir pidiendo movimiento
            }    


            AnsiConsole.MarkupLine($"Turno de [bold blue]{nombreJugadorActual}[/] (Jugador {jugador}), ingresa tu movimiento (W/A/S/D) o [bold red]Q[/] para salir: ");
            char movimiento = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Salto de línea después de la entrada

            if (movimiento == 'q' || movimiento == 'Q') // Condición de salida
            {
                AnsiConsole.MarkupLine("[bold white]¡Gracias por jugar![/]");
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
                if (mapa[nuevaFila, nuevaColumna] == "🌳 ")
                {                    
                    posicion[0] = nuevaFila;
                    posicion[1] = nuevaColumna;
                    // Restaurar la posición inicial
                    
                    if (jugador == 1)
                    {
                        jugador1[0] = posicionInicialJugador1[0]; // Restaurar posición inicial del jugador 1
                        jugador1[1] = posicionInicialJugador1[1];
                        mensajeTrampa = $"{nombreJugadorActual}, has caído en una trampa. Vuelves a tu posición inicial."; // Mensaje de trampa

                    }
                    else
                    {
                        jugador2[0] = posicionInicialJugador2[0]; // Restaurar posición inicial del jugador 2
                        jugador2[1] = posicionInicialJugador2[1];
                        mensajeTrampa = $"{nombreJugadorActual}, has caído en una trampa. Vuelves a tu posición inicial."; // Mensaje de trampa

                    }
                    ImprimirMapa();
                    
                    return; // Salir del método para evitar más movimientos
                }


                // Verificar si puede moverse a un nuevo espacio
                if (mapa[nuevaFila, nuevaColumna] != "⬜ ") // No puede moverse a una pared
                {    // Actualizar posición del jugador
                    posicion[0] = nuevaFila;
                    posicion[1] = nuevaColumna;

                    if (nuevaFila == puerta1[0] && nuevaColumna == puerta1[1])
                    {
                        if (jugador == 1)
                        {    
                            jugador1[0] = puerta2[0]; // Mover jugador 1 a
                            jugador1[1] = puerta2[1];
                        }
                        else
                        {
                            jugador2[0] = puerta2[0];
                            jugador2[1] = puerta2[1];
                        }
                        mensajeRecompensa = $"{nombreJugadorActual}, has sido teletransportado a la Puerta 2.";
                    }
                    

                    else if (nuevaFila == puerta2[0] && nuevaColumna == puerta2[1])
                    {
                        if (jugador == 1)
                        {
                            jugador1[0] = puerta1[0];
                            jugador1[1] = puerta1[1];
                        }
                        else 
                        {
                            jugador2[0] = puerta1[0];
                            jugador2[1] = puerta1[1];
                        }
                        mensajeRecompensa = $"{nombreJugadorActual}, has sido teletransportado a la Puerta 1.";
                    }

                    // Comprobar si ha recogido una recompensa
                    Console.WriteLine("Evaluando posicion");
                    if (mapa[nuevaFila, nuevaColumna] == "💰 ")
                    {
                        mapa[nuevaFila, nuevaColumna] = "   "; // limpiar la posición

                        if (jugador == 1)
                        {                            
                            puntosJugador1++;     
                            mensajeRecompensa = $"¡{nombreJugador1} has recogido una ficha de recompensa! Puedes realizar otro movimiento extra. Puntos: {puntosJugador1}";                  
                        }
                        else
                        {
                            puntosJugador2++;            
                            mensajeRecompensa = $"¡{nombreJugador2} has recogido una ficha de recompensa! Puedes realizar otro movimiento extra. Puntos: {puntosJugador2}";                
                        }
                        movimientoExtra = true;
                    }
                    else
                    {
                        movimientoExtra = false;
                    }

                    ImprimirMapa();

                    // Opción para usar pode
                    AnsiConsole.MarkupLine($"¿Quieres usar un poder para atravesar un árbol? (S/N): ");
                    char usarPoder = Console.ReadKey().KeyChar;
                    Console.WriteLine(); // Salto de línea

                    if (usarPoder == 'f' || usarPoder == 'F')
                    {
                        UsarPoder(jugador); // LLamar al método UsarPoder
                    }
                }                                                       
                else 
                {
                    AnsiConsole.MarkupLine("[bold red]¡No puedes moverte allí! Hay una pared.[/]");
                    break; // Salir del bucle si se sale de los límites
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
        int metaFila = 13;
        int metaColumna = 7;

        // Comprobar si la posición del jugador coincide con la posición de la meta
        if (jugador[0] == metaFila && jugador[1] == metaColumna)
        {
            string nombreJugadorActual = jugador == jugador1 ? nombreJugador1 : nombreJugador2;
            AnsiConsole.MarkupLine($"[bold blue]{nombreJugadorActual} ha alcanzado la meta y ha ganado el juego! 🎉[/]");
            return true; // El jugador ha ganado
        }

        return false; // El jugador no ha ganado
    }   

    static void UsarPoder(int jugador)
    {
        // Determinar el jugador actual y sus puntos
        string nombreJugadorActual = jugador == 1 ? nombreJugador1 : nombreJugador2;
        int puntosJugadorActual = jugador == 1 ? puntosJugador1 : puntosJugador2;

        // Verificar si el jugador tiene suficientes puntos para usar el poder
        if (puntosJugadorActual >= 1) // Supongamos que se necesita 1 punto para usar el poder
        {
            // Gastar un punto para usar el poder
            if (jugador == 1)
            {
                puntosJugador1--;
                AnsiConsole.MarkupLine($"{nombreJugadorActual} ha usado un poder para atravesar un árbol.");
            }
            else
            {
                puntosJugador2--;
                AnsiConsole.MarkupLine($"{nombreJugadorActual} ha usado un poder para atravesar un árbol.");
            }

            // Obtener la posición actual del jugador
            int[] posicion = jugador == 1 ? jugador1 : jugador2;
            int filaActual = posicion[0];
            int columnaActual = posicion[1];

            // Permitir al jugador moverse a la siguiente posición
            AnsiConsole.MarkupLine($"[bold blue]{nombreJugadorActual}[/] elige una nueva posición para moverse (W/A/S/D): ");
            char movimiento = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Salto de línea después de la entrada

            int nuevaFila = filaActual;
            int nuevaColumna = columnaActual;

            switch (movimiento)
            {
                case 'w': nuevaFila--; break; // Arriba
                case 's': nuevaFila++; break; // Abajo
                case 'a': nuevaColumna--; break; // Izquierda
                case 'd': nuevaColumna++; break; // Derecha
                default:
                    AnsiConsole.MarkupLine("[bold red]Movimiento no válido. Intenta de nuevo.[/]");
                    return; // Salir si el movimiento no es válido
            }

            // Verificar límites y si puede moverse a la nueva posición
            if (nuevaFila > 0 && nuevaColumna > 0 && nuevaFila < mapa.GetLength(0) - 1 && nuevaColumna < mapa.GetLength(1) - 1)
            {
                // Permitir el movimiento a través de un árbol
                if (mapa[nuevaFila, nuevaColumna] == "🌳 ")
                {
                    // Actualizar la posición del jugador
                    posicion[0] = nuevaFila;
                    posicion[1] = nuevaColumna;
                    AnsiConsole.MarkupLine($"{nombreJugadorActual} ha atravesado un árbol y se ha movido a la nueva posición.");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]¡No puedes moverte allí! No hay un árbol para atravesar.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]¡Te has salido de los límites![/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]¡No tienes suficientes puntos para usar un poder![/]");
        }
    }
}




