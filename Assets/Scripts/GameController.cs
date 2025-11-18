using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    // INICIO: MODIFICACIONES PARA MINIMAX
    private MinimaxAI aiPlayer;
    // FIN: MODIFICACIONES PARA MINIMAX

    private string playerSide;
    private int moveCount;
    
    // NUEVA VARIABLE PARA EL MODO IA VS IA
    private bool aiVsAiMode = false;

    void Awake()
    {
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        gameOverPanel.SetActive(false);
        moveCount = 0;
        restartButton.SetActive(false);
        SetPlayerColors(playerX, playerO);
        
        // INICIALIZACIÓN DE LA IA
        aiPlayer = new MinimaxAI();
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        // Lógica de comprobación de victoria (se mantiene igual)
        if (CheckForWin())
        {
            GameOver(playerSide);
            return; 
        }
        
        if (moveCount >= 9)
        {
            GameOver("tablas");
            return;
        }
        
        ChangeSides();
    }

    //Método auxiliar para limpiar la lógica de EndTurn
    bool CheckForWin()
    {
        // Filas
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide) return true;
        if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide) return true;
        if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide) return true;

        // Columnas
        if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide) return true;
        if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide) return true;
        if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide) return true;

        // Diagonales
        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide) return true;
        if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide) return true;
        
        return false;
    }
    
    void ChangeSides()
    {
        //El cambio de lado es normal
        playerSide = (playerSide == "X") ? "O" : "X"; 
        
        //Actualizar colores, el turno
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
        
        //Verificar si la IA debe jugar AHORA
        if (aiVsAiMode || playerSide == "O")
        {
            StartCoroutine(AIMoveDelay()); 
        }
    }

    // Método para iniciar el modo IA vs IA

public void StartAIVsAI()
    {
        // Reiniciamos el juego, pero establecemos el modo de IA.
        RestartGame(); 
        
        aiVsAiMode = true; 
        SetBoardInteractable(false); // Bloqueamos la interacción del usuario
        
        // La IA 'X' comienza
        playerSide = "X";
        SetPlayerColors(playerX, playerO);
        StartCoroutine(AIMoveDelay());
    }

    //MÉTODO MODIFICADO PARA LA JUGADA DE LA IA
    IEnumerator AIMoveDelay()
{
        // Solo para evitar errores si la coroutine se llama accidentalmente.
        if (!aiVsAiMode && playerSide == "X") yield break; 

        // Añade un pequeño retraso
        yield return new WaitForSeconds(0.5f); 
        
        // **CORRECCIÓN CLAVE:** Pasamos el símbolo del jugador actual (playerSide) a la IA.
        int bestMoveIndex = aiPlayer.FindBestMove(buttonList, playerSide);

        // La IA realiza el movimiento
        buttonList[bestMoveIndex].text = GetPlayerSide();
        buttonList[bestMoveIndex].GetComponentInParent<Button>().interactable = false;

        // Finaliza el turno para verificar la victoria y cambiar de lado
        EndTurn();
    }


    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }


    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);
        if (winningPlayer == "tablas")
        {
            SetGameOverText("¡Son tablas!");
        }
        else
        {
            SetGameOverText(winningPlayer + " gana!");
        }
        restartButton.SetActive(true);
        
        // Desactivamos el modo IA vs IA si termina el juego
        aiVsAiMode = false;
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }


    public void RestartGame()
    {
        // IMPORTANTE: Al reiniciar, aseguramos que el modo IA vs IA esté apagado
        aiVsAiMode = false; 
        
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        SetPlayerColors(playerX, playerO);
        SetBoardInteractable(true); // Se reestablece la interacción para el juego Humano vs IA

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
        restartButton.SetActive(false);
    }

    void SetBoardInteractable(bool toggle) 
    {
        for (int i = 0; i<buttonList.Length; i++) 
        { 
             buttonList[i].GetComponentInParent<Button>().interactable = toggle; 
        }
    }
}