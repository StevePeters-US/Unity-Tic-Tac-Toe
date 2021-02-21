using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

[System.Serializable]
public class PlayerUI
{
    public Image panelImage;
    public TextMeshProUGUI panelText;
}

public class GameManager : MonoBehaviour
{
    public GridSpace[] gridSpaces;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public GameObject restartGameButton;

    public PlayerUI playerX;
    public PlayerUI playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    public Button XControlTypeButton;
    public Button OControlTypeButton;

    private string playerSide;
    private int moveCount;

    private void Awake()
    {
        foreach (GridSpace gridSpace in gridSpaces)
        {
            gridSpace.SetGameControllerReference(this);
        }

        RestartEpisode();
    }

    public void RestartEpisode()
    {
        gameOverPanel.SetActive(false);
        restartGameButton.SetActive(false);
        XControlTypeButton.gameObject.SetActive(false);
        OControlTypeButton.gameObject.SetActive(false);

        playerSide = "X";
        SetPlayerColors(playerX, playerO);
       
        moveCount = 0;
        foreach (GridSpace gridSpace in gridSpaces) gridSpace.EnableButton();
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;
        if (moveCount >= 9) GameOver(true);

        //Check win condition
        bool win = (gridSpaces[0].GetButtonText() == playerSide && gridSpaces[1].GetButtonText() == playerSide && gridSpaces[2].GetButtonText() == playerSide) ||
            (gridSpaces[3].GetButtonText() == playerSide && gridSpaces[4].GetButtonText() == playerSide && gridSpaces[5].GetButtonText() == playerSide) ||
            (gridSpaces[6].GetButtonText() == playerSide && gridSpaces[7].GetButtonText() == playerSide && gridSpaces[8].GetButtonText() == playerSide) ||

            (gridSpaces[0].GetButtonText() == playerSide && gridSpaces[3].GetButtonText() == playerSide && gridSpaces[6].GetButtonText() == playerSide) ||
            (gridSpaces[1].GetButtonText() == playerSide && gridSpaces[4].GetButtonText() == playerSide && gridSpaces[7].GetButtonText() == playerSide) ||
            (gridSpaces[2].GetButtonText() == playerSide && gridSpaces[5].GetButtonText() == playerSide && gridSpaces[8].GetButtonText() == playerSide) ||

            (gridSpaces[0].GetButtonText() == playerSide && gridSpaces[4].GetButtonText() == playerSide && gridSpaces[8].GetButtonText() == playerSide) ||
            (gridSpaces[6].GetButtonText() == playerSide && gridSpaces[4].GetButtonText() == playerSide && gridSpaces[2].GetButtonText() == playerSide);

        if (win) GameOver(false);

        else ChangeSides();
    }

    void GameOver(bool isDraw)
    {
        // Disable all buttons
        foreach (GridSpace gridSpace in gridSpaces) gridSpace.DisableButton();

        gameOverPanel.SetActive(true);
        restartGameButton.SetActive(true);
        XControlTypeButton.gameObject.SetActive(true);
        OControlTypeButton.gameObject.SetActive(true);

        if (isDraw) gameOverText.text = "It's a Draw";
        else gameOverText.text = playerSide + " Wins!";
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X"; // Note: Capital Letters for "X" and "O"

        if (playerSide == "X") { SetPlayerColors(playerX, playerO); }
        else { SetPlayerColors(playerO, playerX); }
    }

    void SetPlayerColors(PlayerUI newPlayer, PlayerUI oldPlayer)
    {
        newPlayer.panelImage.color = activePlayerColor.panelColor;
        newPlayer.panelText.color = activePlayerColor.textColor;
        oldPlayer.panelImage.color = inactivePlayerColor.panelColor;
        oldPlayer.panelText.color = inactivePlayerColor.textColor;
    }
}
