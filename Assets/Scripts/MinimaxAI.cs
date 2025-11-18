using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public partial class MinimaxAI
{
    private const string AI_SIDE = "O"; // El lado de la IA
    private const string PLAYER_SIDE = "X"; // El lado del jugador humano

    /// <summary>
    /// Función principal que la IA llama para obtener su mejor movimiento.
    /// </summary>
    public int FindBestMove(Text[] board)
    {
        Move bestMove = Minimax(board, AI_SIDE);
        return bestMove.index;
    }

    public int FindBestMove(Text[] board, string playerSymbol) // Ahora pasamos el símbolo para que el jugador sea dinámico
    {
        // Inicializa Alpha y Beta
        int alpha = int.MinValue;
        int beta = int.MaxValue;
        //El algoritmo Minimax calcula el mejor movimiento para 'playerSymbol'
        //Move bestMove = Minimax(board, playerSymbol, playerSymbol); 
        Move bestMove = Minimax(board, playerSymbol, playerSymbol, alpha, beta); //Poda alfa-beta
        return bestMove.index;
    }

    /// <summary>
    /// La implementación recursiva del algoritmo Minimax.
    /// </summary>
    private Move Minimax(Text[] currentBoard, string currentPlayer)
    {
        //1. Verificar el estado del juego (Función de Evaluación/Terminal)
        string winner = CheckWinner(currentBoard);
        if (winner == AI_SIDE)
            return new Move { score = 10 }; // Victoria de la IA
        if (winner == PLAYER_SIDE)
            return new Move { score = -10 }; // Victoria del Jugador
        if (GetAvailableMoves(currentBoard).Count == 0)
            return new Move { score = 0 }; // Empate (Tablas)

        // 2. Inicializar la mejor jugada (dependiendo de si es MAX o MIN)
        List<int> availableMoves = GetAvailableMoves(currentBoard);
        Move bestMove = new Move();

        if (currentPlayer == AI_SIDE) // MAXIMIZADOR
        {
            bestMove.score = int.MinValue;
            foreach (int move in availableMoves)
            {
                // Realizar el movimiento
                currentBoard[move].text = currentPlayer;
                
                // Llamada recursiva (al MINIMIZADOR)
                Move result = Minimax(currentBoard, PLAYER_SIDE);
                
                // Deshacer el movimiento (Backtracking)
                currentBoard[move].text = "";
                
                // Actualizar la mejor puntuación
                if (result.score > bestMove.score)
                {
                    bestMove.score = result.score;
                    bestMove.index = move;
                }
            }
        }
        else //MINIMIZADOR (PLAYER_SIDE)
        {
            bestMove.score = int.MaxValue;
            foreach (int move in availableMoves)
            {
                // Realizar el movimiento
                currentBoard[move].text = currentPlayer;
                
                // Llamada recursiva (al MAXIMIZADOR)
                Move result = Minimax(currentBoard, AI_SIDE);
                
                // Deshacer el movimiento (Backtracking)
                currentBoard[move].text = "";
                
                // Actualizar la mejor puntuación
                if (result.score < bestMove.score)
                {
                    bestMove.score = result.score;
                    bestMove.index = move;
                }
            }
        }

        return bestMove;
    }

    //Uno contra otro
    /// <summary>
    /// Implementación recursiva del algoritmo Minimax para dos jugadores inteligentes.
    /// </summary>
private Move Minimax(Text[] currentBoard, string currentPlayer, string startingPlayer)
    {
        string opponentPlayer = (currentPlayer == "X") ? "O" : "X"; 
        
        // 1. Verificar el estado del juego (Función de Evaluación/Terminal)
        string winner = CheckWinner(currentBoard);
        
        if (winner != "")
        {
            // La puntuación es relativa al 'startingPlayer'.
            if (winner == startingPlayer)
                return new Move { score = 10 }; // Victoria para el jugador inicial (MAX)
            else
                return new Move { score = -10 }; // Derrota para el jugador inicial (MIN)
        }
        if (GetAvailableMoves(currentBoard).Count == 0)
            return new Move { score = 0 }; // Empate (Tablas)

        // 2. Inicializar la mejor jugada
        List<int> availableMoves = GetAvailableMoves(currentBoard);
        Move bestMove = new Move();

        // MAXIMIZADOR (Si el jugador actual es el que inició la búsqueda)
        if (currentPlayer == startingPlayer) 
        {
            bestMove.score = int.MinValue;
            foreach (int move in availableMoves)
            {
                currentBoard[move].text = currentPlayer;
                
                // Llamada recursiva (al oponente)
                Move result = Minimax(currentBoard, opponentPlayer, startingPlayer); 
                
                currentBoard[move].text = "";
                
                // Actualizar la mejor puntuación (buscando el MÁXIMO)
                if (result.score > bestMove.score)
                {
                    bestMove.score = result.score;
                    bestMove.index = move;
                }
            }
        }
        // MINIMIZADOR (El oponente del jugador que inició la búsqueda)
        else 
        {
            bestMove.score = int.MaxValue;
            foreach (int move in availableMoves)
            {
                currentBoard[move].text = currentPlayer;
                
                // Llamada recursiva (al MAXIMIZADOR)
                Move result = Minimax(currentBoard, opponentPlayer, startingPlayer);
                
                currentBoard[move].text = "";
                
                // Actualizar la mejor puntuación (buscando el MÍNIMO)
                if (result.score < bestMove.score)
                {
                    bestMove.score = result.score;
                    bestMove.index = move;
                }
            }
        }

        return bestMove;
    }

/// <summary>
    /// Implementación recursiva del algoritmo Minimax con Poda Alpha-Beta.
    /// </summary>
    private Move Minimax(Text[] currentBoard, string currentPlayer, string startingPlayer, int alpha, int beta)
    {
        string opponentPlayer = (currentPlayer == "X") ? "O" : "X";

        // 1. Verificación Terminal y Evaluación de Puntuación
        string winner = CheckWinner(currentBoard);
        
        if (winner != "")
        {
            // La puntuación es relativa al 'startingPlayer'.
            if (winner == startingPlayer)
                return new Move { score = 10 }; // Victoria para el jugador inicial (MAX)
            else
                return new Move { score = -10 }; // Derrota para el jugador inicial (MIN)
        }
        if (GetAvailableMoves(currentBoard).Count == 0)
            return new Move { score = 0 }; // Empate (Tablas)

        // 2. Inicializar la mejor jugada
        List<int> availableMoves = GetAvailableMoves(currentBoard);
        Move bestMove = new Move();
        bestMove.index = -1; // Inicializar índice

        // MAXIMIZADOR (Si el jugador actual es el que inició la búsqueda)
        if (currentPlayer == startingPlayer) 
        {
            bestMove.score = int.MinValue;
            foreach (int move in availableMoves)
            {
                currentBoard[move].text = currentPlayer;
                
                // Llamada recursiva (al oponente)
                Move result = Minimax(currentBoard, opponentPlayer, startingPlayer, alpha, beta); 
                
                currentBoard[move].text = "";
                
                // Actualizar la mejor puntuación
                if (result.score > bestMove.score)
                {
                    bestMove.score = result.score;
                    bestMove.index = move;
                }
                
                // Poda Alpha: Actualizar Alpha y realizar el corte Beta.
                alpha = Mathf.Max(alpha, bestMove.score);
                if (beta <= alpha)
                {
                    break; // Poda: No se necesita explorar más esta rama.
                }
            }
        }
        // MINIMIZADOR (El oponente del jugador que inició la búsqueda)
        else 
        {
            bestMove.score = int.MaxValue;
            foreach (int move in availableMoves)
            {
                currentBoard[move].text = currentPlayer;
                
                // Llamada recursiva (al MAXIMIZADOR)
                Move result = Minimax(currentBoard, opponentPlayer, startingPlayer, alpha, beta);
                
                currentBoard[move].text = "";
                
                // Actualizar la mejor puntuación
                if (result.score < bestMove.score)
                {
                    bestMove.score = result.score;
                    bestMove.index = move;
                }

                // Poda Beta: Actualizar Beta y realizar el corte Alpha.
                beta = Mathf.Min(beta, bestMove.score);
                if (beta <= alpha)
                {
                    break; // Poda: No se necesita explorar más esta rama.
                }
            }
        }

        return bestMove;
    }




    /// <summary>
    /// Encuentra todos los espacios vacíos en el tablero.
    /// </summary>
    private List<int> GetAvailableMoves(Text[] board)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i].text == "")
            {
                available.Add(i);
            }
        }
        return available;
    }
    
    /// <summary>
    /// Verifica si hay un ganador en el estado actual del tablero.
    /// Esta lógica es una adaptación del GameController.EndTurn.
    /// </summary>
    private string CheckWinner(Text[] board)
    {
        // Filas
        if (board[0].text != "" && board[0].text == board[1].text && board[1].text == board[2].text) return board[0].text;
        if (board[3].text != "" && board[3].text == board[4].text && board[4].text == board[5].text) return board[3].text;
        if (board[6].text != "" && board[6].text == board[7].text && board[7].text == board[8].text) return board[6].text;
        
        // Columnas
        if (board[0].text != "" && board[0].text == board[3].text && board[3].text == board[6].text) return board[0].text;
        if (board[1].text != "" && board[1].text == board[4].text && board[4].text == board[7].text) return board[1].text;
        if (board[2].text != "" && board[2].text == board[5].text && board[5].text == board[8].text) return board[2].text;

        // Diagonales
        if (board[0].text != "" && board[0].text == board[4].text && board[4].text == board[8].text) return board[0].text;
        if (board[2].text != "" && board[2].text == board[4].text && board[4].text == board[6].text) return board[2].text;

        return ""; // No hay ganador
    }
}