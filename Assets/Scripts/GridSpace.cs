using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{

    public Button button;
    public Text buttonText;

    private GameController gameController;

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

    public void SetSpace()
    {
        // Solo permitir la interacción si el turno es del jugador 'X' (el humano)
        if (gameController.GetPlayerSide() == "X") 
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
        }
    }
}



/*using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public string playerSide;
    private GameController gameController;
    public Text[] buttonList;

    public void SetSpace()
    {
        buttonText.text = playerSide; 
        button.interactable = false;
        gameController.EndTurn();

    }



    public void SetGameControllerReference(GameController controller)
    {
        buttonText.text = gameController.GetPlayerSide();
        button.interactable = false;
        gameController = controller;
    }


}
*/