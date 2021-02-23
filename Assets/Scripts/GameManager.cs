using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace APG
{
    [System.Serializable]
    public class PlayerColor
    {
        public Color panelColor;
        public Color textColor;
    }

    public class GameManager : MonoBehaviour
    {
        public GridSpace[] gridSpaces;
        public int[] gridValues = new int[9];
        public GameObject gameOverPanel;
        public TextMeshProUGUI gameOverText;
        public GameObject restartGameButton;

        public PlayerAgent[] playerAgents;

        public PlayerColor activePlayerColor;
        public PlayerColor inactivePlayerColor;

        public Button XControlTypeButton;
        public Button OControlTypeButton;

      //  private string playerSide; // Replacing this with index
        private int currentAgentIndex = 0;
        private int moveCount;

        private void Awake()
        {
            foreach (GridSpace gridSpace in gridSpaces) gridSpace.SetGameControllerReference(this);

            RestartEpisode();
        }

        public void RestartEpisode()
        {
            gameOverPanel.SetActive(false);
            restartGameButton.SetActive(false);
            XControlTypeButton.gameObject.SetActive(false);
            OControlTypeButton.gameObject.SetActive(false);

            currentAgentIndex = 0;
            SetPlayerColors(playerAgents[0] , playerAgents[1]);

            moveCount = 0;
            foreach (GridSpace gridSpace in gridSpaces)
            {
                gridSpace.EnableButton();
                gridSpace.agentIndex = -1;
            }

            for (int i = 0; i < gridValues.Length; i++)
            {
                gridValues[i] = -1;
            }
        }

        public int GetCurrentAgentIndex()
        {
            return currentAgentIndex;
        }

        public string GetCurrentAgentName()
        {
            return playerAgents[currentAgentIndex].agentName;
        }

        public void EndTurn()
        {
            moveCount++;
            if (moveCount >= 9) GameOver(true);

            bool win = (gridSpaces[0].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[1].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[2].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[3].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[5].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[6].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[7].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[8].GetButtonAgentIndex() == currentAgentIndex) ||

                (gridSpaces[0].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[3].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[6].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[1].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[7].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[2].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[5].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[8].GetButtonAgentIndex() == currentAgentIndex) ||

                (gridSpaces[0].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[8].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[6].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[2].GetButtonAgentIndex() == currentAgentIndex);
            
            if (win) GameOver(false);

            else ChangeSides();
        }

        public void selectGridSpace(int gridSpaceIdx)
        {
            gridSpaces[gridSpaceIdx].SetSpace();
            gridValues[gridSpaceIdx] = GetCurrentAgentIndex();
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
            else gameOverText.text =  playerAgents[currentAgentIndex].agentName + " Wins!";
        }

        void ChangeSides()
        {
            currentAgentIndex = currentAgentIndex == 1 ? 0 : 1; // switch to other agent

            if (currentAgentIndex == 1) {SetPlayerColors(playerAgents[1], playerAgents[0]); }
            else { SetPlayerColors(playerAgents[0], playerAgents[1]); }
        }

        void SetPlayerColors(PlayerAgent newPlayer, PlayerAgent oldPlayer)
        {
            newPlayer.panelImage.color = activePlayerColor.panelColor;
            newPlayer.panelText.color = activePlayerColor.textColor;
            oldPlayer.panelImage.color = inactivePlayerColor.panelColor;
            oldPlayer.panelText.color = inactivePlayerColor.textColor;
        }
    }
}