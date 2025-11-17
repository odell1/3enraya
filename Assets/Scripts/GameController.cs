using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Asegúrate de que esta línea esté presente para List

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
            return; // Detener la ejecución si hay ganador
        }
        
        if (moveCount >= 9)
        {
            GameOver("tablas");
            return;
        }
        
        ChangeSides();
    }

    // Método auxiliar para limpiar la lógica de EndTurn
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
        playerSide = (playerSide == "X") ? "O" : "X";
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
            // LA IA JUEGA AQUÍ
            StartCoroutine(AIMoveDelay()); 
        }
    }

    // NUEVO MÉTODO PARA LA JUGADA DE LA IA
    IEnumerator AIMoveDelay()
    {
        // Añade un pequeño retraso para una mejor experiencia de usuario
        yield return new WaitForSeconds(0.5f); 
        
        // La IA calcula el mejor movimiento
        int bestMoveIndex = aiPlayer.FindBestMove(buttonList);

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

    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }


    public void RestartGame()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        SetPlayerColors(playerX, playerO);
        SetBoardInteractable(true);

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