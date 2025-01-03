﻿using Spectre.Console;
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

        ColocarFichasYObstaculos(5, 6, 2);       
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


    static void ColocarFichasYObstaculos(int cantidadFichas, int cantidadArboles, int cantidadFichasCaptura)
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

        for (int i = 0; i < cantidadFichasCaptura; i++)
        {
            int fila, columna;
            do
            {
                fila = random.Next(1, filas - 1);
                columna = random.Next(1, columnas - 1);
            } while (mapa[fila, columna] != "   "); // Asegurarse que el espacio esté vacío
            mapa[fila, columna] = "⚡ "; // Colocar ficha de
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

    // static bool poderInstruccionesMostradas = false;        
    
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
            if (!string.IsNullOrEmpty(mensajeRecompensa))
            {
                Console.WriteLine(mensajeRecompensa);
                mensajeRecompensa = ""; // Limpiar el mensaje de recompensa
            }
            
            if (!string.IsNullOrEmpty(mensajeTrampa))
            {
                Console.WriteLine(mensajeTrampa);
                mensajeTrampa = ""; // Limpiar el mensaje de recompensa
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
                    // Verificar si el jugador tiene puntos para usar el poder
                    if ((jugador == 1 && puntosJugador1 > 0) || (jugador == 2 && puntosJugador2 > 0))
                    {
                        // Preguntar si quiere usar el poder
                        AnsiConsole.MarkupLine($"{nombreJugadorActual}, has caído en un árbol. ¿Quieres usar el poder para atravesarlo? Se te restará un punto de tu contador. Presiona 'F' para usar el poder o cualquier otra tecla para volver a tu posición inicial.");
                        char decision = Console.ReadKey().KeyChar;
                        Console.WriteLine(); // Salto de línea después de la entrada


                        if (decision == 'f' || decision == 'F')
                        {
                            // Restar un punto al jugador
                            if (jugador == 1)
                            {
                                puntosJugador1--;
                            }
                            else
                            {
                                puntosJugador2--;
                            }

                            // Permitir al jugador moverse a la nueva posición
                            posicion[0] = nuevaFila;
                            posicion[1] = nuevaColumna;
                            AnsiConsole.MarkupLine($"{nombreJugadorActual} ha atravesado el árbol y se ha movido a la nueva posición.");
                        }                        
                                            
                        else
                        {
                            // Restaurar la posición inicial
                            mensajeTrampa = $"{nombreJugadorActual}, has caído en una trampa. Vuelves a tu posición inicial."; // Mensaje de trampa  
                            Console.WriteLine(mensajeTrampa); // Imprimir el mensaje de trampa
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
                            ImprimirMapa();
                            return;  // Salir del método para evitar más movimientétodo para evitar más movimientos
                        }
                    }
                    else
                    {
                        // Si el jugador no tiene puntos, no puede usar el poder
                        mensajeTrampa = $"{nombreJugadorActual}, no tienes suficientes puntos para usar el poder. Vuelves a tu posición inicial."; // Mensaje de trampa
                        Console.WriteLine(mensajeTrampa); // Imprimir el mensaje antes de restaurar la posición
                        
                        for (int j = 0; j < 3; j++)
                        {
                            Console.Write($"{nombreJugadorActual} retrocediendo a la posición inicial");
                            for (int k = 0; k < 3; k++)
                            {
                                Console.Write("."); // Mostrar puntos para la animación
                                System.Threading.Thread.Sleep(300); // Esperar un poco
                            }
                            Console.WriteLine(); // Salto de línea después de cada retroceso
                        }

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
                        ImprimirMapa();
                        return;  // Salir del método para evitar más movimientos
                    }    
                }

                // Verificar si puede moverse a un nuevo espacio
                if (mapa[nuevaFila, nuevaColumna] != "⬜ ") // No puede moverse a una pared
                {    // Actualizar posición del jugador
                    posicion[0] = nuevaFila;
                    posicion[1] = nuevaColumna;

                    if (nuevaFila == puerta1[0] && nuevaColumna == puerta1[1])
                    {
                        // Imprimir el mensaje de teletransportación
                        AnsiConsole.MarkupLine($"{nombreJugadorActual}, estás siendo teletransportado a la Puerta 2.");
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write(".");
                            System.Threading.Thread.Sleep(700);
                        }
                        Console.WriteLine();

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
                    }
                    

                    else if (nuevaFila == puerta2[0] && nuevaColumna == puerta2[1])
                    {
                        AnsiConsole.MarkupLine($"{nombreJugadorActual}, estás siendo teletransportado a la Puerta 1.");
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write(".");
                            System.Threading.Thread.Sleep(700);
                        }
                        Console.WriteLine();

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
                    }

                    // Comprobar si ha recogido una recompensa
                    Console.WriteLine("Evaluando posición");
                    if (mapa[nuevaFila, nuevaColumna] == "💰 ")
                    {
                        mapa[nuevaFila, nuevaColumna] = "   "; // limpiar la posición

                        if (jugador == 1)
                        {                            
                            puntosJugador1++;     
                            AnsiConsole.MarkupLine($"¡🎉 {nombreJugador1} has recogido una ficha de recompensa! 🎉 Puntos: {puntosJugador1}");
                            
                        }
                        else
                        {
                            puntosJugador2++;            
                            AnsiConsole.MarkupLine($"¡🎉 {nombreJugador2} has recogido una ficha de recompensa! 🎉 Puntos: {puntosJugador2}");
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            Console.WriteLine("🎊 🎊 🎊 ¡Felicidades, has recogido una ficha recompensa y puedes volver a jugar! 🎊 🎊 🎊");
                            System.Threading.Thread.Sleep(500); // Esperar medio segundo
                        }
                        
                        
                        ImprimirMapa(); // Imprimir mapa actualiz
                        movimientoExtra = true;
                    }
                    else
                    {
                        movimientoExtra = false;
                    }
                    
                    ImprimirMapa();

                    if (mapa[nuevaFila, nuevaColumna] == "⚡ ")
                    {
                        AnsiConsole.MarkupLine($"¡🎉 {nombreJugadorActual} ha recogido una ficha de captura! 🎉 Puedes usar el poder de captura.");
                        mapa[nuevaFila, nuevaColumna] = "   "; // Limpiar la posición
                        
                        AnsiConsole.MarkupLine($"¿Quieres usar el poder de captura? Presiona 'C' para capturar al otro jugador solo en caso de que ambos están en la misma fila o columna o cualquier otra tecla para continuar.");
                        char decision = Console.ReadKey().KeyChar;
                        Console.WriteLine(); // Salto de línea después de la en

                        if (decision == 'c' || decision == 'C')
                        {
                            // Verificar si el otro jugador está en la misma fila o columna
                            int[] otroJugador = jugador == 1 ? jugador2 : jugador1;
                            if (otroJugador[0] == nuevaFila || otroJugador[1] == nuevaColumna)
                            {
                                // Lógica para capturar al otro jugador
                                AnsiConsole.MarkupLine($"[bold red]{nombreJugadorActual} ha usado el poder de captura! 🎯[/]");
                                // Animación de captura
                                for (int i = 0; i < 3; i++)
                                {
                                    Console.Write("Capturando al otro jugador");
                                    for (int j = 0; j < 3; j++)
                                    {
                                        Console.Write("."); // Mostrar puntos para la animación
                                        System.Threading.Thread.Sleep(300); // Esperar un poco
                                    }
                                    Console.WriteLine(); // Salto de línea después de cada captura
                                }

                                // Mover al otro jugador a su posición inicial
                                if (jugador == 1)
                                {
                                    jugador2[0] = posicionInicialJugador2[0];
                                    jugador2[1] = posicionInicialJugador2[1];
                                }
                                else
                                {
                                    jugador1[0] = posicionInicialJugador1[0];
                                    jugador1[1] = posicionInicialJugador1[1];
                                }
                                ImprimirMapa(); // Imprimir mapa actualizado
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[bold red]¡No puedes usar el poder de captura! El otro jugador no está en la misma fila o columna.[/]");
                            }
                        }
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

            for (int i = 0; i < 5; i++)
            {
                // Imprimir el mensaje de "¡Felicidades!"
                Console.SetCursorPosition(0, 16); // Ajusta la posición según sea necesario
                AnsiConsole.MarkupLine("[bold yellow]🎊 🎊 🎊 ¡Felicidades! 🎊 🎊 🎊[/]");
                
                // Esperar medio segundo
                System.Threading.Thread.Sleep(500);

                // Imprimir el mensaje de "¡Victoria!"
                Console.SetCursorPosition(0, 16); // Ajusta la posición según sea necesario
                AnsiConsole.MarkupLine("[bold yellow]🎉 🎉 🎉 ¡Victoria! 🎉 🎉 🎉[/]");
                
                // Esperar medio segundo
                System.Threading.Thread.Sleep(500);
            }
            return true; // El jugador ha ganado
        }

        return false; // El jugador no ha ganado
    }   
}


/* 
else if (mapa[nuevaFila, nuevaColumna] == "⚡ ")
{
    mapa[nuevaFila, nuevaColumna] = "   "; // Limpiar la posición
    AnsiConsole.MarkupLine($"¡🎉 {nombreJugadorActual} ha recogido una ficha de captura! 🎉 Puedes usar el poder de captura.");

    // Preguntar al jugador si quiere usar el poder
    AnsiConsole.MarkupLine($"¿Quieres usar el poder de captura? Presiona 'C' para capturar al otro jugador o cualquier otra tecla para continuar.");
    char decision = Console.ReadKey().KeyChar;
    Console.WriteLine(); // Salto de línea después de la entrada

    if (decision == 'c' || decision == 'C')
    {
        // Verificar si el otro jugador está en la misma fila o columna
        int[] otroJugador = jugador == 1 ? jugador2 : jugador1;
        if (otroJugador[0] == nuevaFila || otroJugador[1] == nuevaColumna)
        {
            // Lógica para capturar al otro jugador
            AnsiConsole.MarkupLine($"[bold red]{nombreJugadorActual} ha usado el poder de captura! 🎯[/]");
            // Animación de captura
            for (int i = 0; i < 3; i++)
            {
                Console.Write("Capturando al otro jugador");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write("."); // Mostrar puntos para la animación
                    System.Threading.Thread.Sleep(300); // Esperar un poco
                }
                Console.WriteLine(); // Salto de línea después de cada captura
            }

            // Mover al otro jugador a su posición inicial
            if (jugador == 1)
            {
                jugador2[0] = posicionInicialJugador2[0];
                jugador2[1] = posicionInicialJugador2[1];
            }
            else
            {
                jugador1[0] = posicionInicialJugador1[0];
                jugador1[1] = posicionInicialJugador1[1];
            }
            ImprimirMapa(); // Imprimir mapa actualizado
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]¡No puedes usar el poder de captura! El otro jugador no está en la misma fila o columna.[/]");
        }
    }
}

// Actualizar la posición del jugador
posicion[0] = nuevaFila;
posicion[1] = nuevaColumna;

// Imprimir el mapa después de mover
ImprimirMapa();*/
