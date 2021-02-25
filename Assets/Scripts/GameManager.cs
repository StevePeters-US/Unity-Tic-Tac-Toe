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

        public bool training = false;

        private int currentAgentIndex = 0;
        private int moveCount = 0;


        private void Awake()
        {
            for (int i = 0; i < gridSpaces.Length; i++)
            {
                gridSpaces[i].SetGameControllerReference(this);
                gridSpaces[i].gridSpaceIndex = i;
            }
        }
        private void Start()
        {
            RestartEpisode();
        }

        public void RestartEpisode()
        {
            gameOverPanel.SetActive(false);
            restartGameButton.SetActive(false);
            XControlTypeButton.gameObject.SetActive(false);
            OControlTypeButton.gameObject.SetActive(false);

            currentAgentIndex = 0;
            SetPlayerColors(playerAgents[0], playerAgents[1]);

            moveCount = 0;
            ResetAllButtons();
            ResetGridValues();

            GetAgentDecision();
        }


        public int GetCurrentAgentIndex()
        {
            return currentAgentIndex;
        }

        public string GetCurrentAgentName()
        {
            return playerAgents[currentAgentIndex].agentName;
        }

        public void selectGridSpace(int gridSpaceIndex)
        {
            // Check that valid grid space has been selected
            if (gridValues[gridSpaceIndex] == -1)
            {
                gridSpaces[gridSpaceIndex].SetSpace();
                gridValues[gridSpaceIndex] = GetCurrentAgentIndex();
                EndTurn();
            }

            else
            {
                Debug.LogWarning("invalid grid space selection " + gridSpaceIndex + " by agent num : " + currentAgentIndex);
                playerAgents[currentAgentIndex].InvalidDecisionPenalty();
                GetAgentDecision();
            }
        }

        public void EndTurn()
        {
            moveCount++;

            // Brute force win condition check
            bool win = (gridSpaces[0].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[1].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[2].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[3].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[5].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[6].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[7].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[8].GetButtonAgentIndex() == currentAgentIndex) ||

                (gridSpaces[0].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[3].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[6].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[1].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[7].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[2].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[5].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[8].GetButtonAgentIndex() == currentAgentIndex) ||

                (gridSpaces[0].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[8].GetButtonAgentIndex() == currentAgentIndex) ||
                (gridSpaces[6].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[4].GetButtonAgentIndex() == currentAgentIndex && gridSpaces[2].GetButtonAgentIndex() == currentAgentIndex);

            if (win) GameOver(false);

            else if (moveCount > 8)
            {
                GameOver(true);
                return;
            }

            else ChangeSides();
        }

        void ChangeSides()
        {
            currentAgentIndex = currentAgentIndex == 1 ? 0 : 1; // switch to other agent

            if (currentAgentIndex == 1) { SetPlayerColors(playerAgents[1], playerAgents[0]); }
            else { SetPlayerColors(playerAgents[0], playerAgents[1]); }

            Debug.Log("Player " + playerAgents[currentAgentIndex].agentName + "s turn");
            GetAgentDecision();
        }

        void GameOver(bool isDraw)
        {
            DisableAllButtons();

            gameOverPanel.SetActive(true);
            restartGameButton.SetActive(true);
            //  XControlTypeButton.gameObject.SetActive(true);
            //  OControlTypeButton.gameObject.SetActive(true);

            string gameResultsText;

            if (isDraw) gameResultsText = "It's a Draw";
            else gameResultsText = playerAgents[currentAgentIndex].agentName + " Wins!";

            gameOverText.text = gameResultsText;
            //  Debug.LogWarning(gameResultsText);

            playerAgents[currentAgentIndex].AddReward(1.0f);
            foreach (PlayerAgent agent in playerAgents) agent.EndEpisode();

            if (training) RestartEpisode();
        }


        void SetPlayerColors(PlayerAgent newPlayer, PlayerAgent oldPlayer)
        {
            newPlayer.panelImage.color = activePlayerColor.panelColor;
            newPlayer.panelText.color = activePlayerColor.textColor;
            oldPlayer.panelImage.color = inactivePlayerColor.panelColor;
            oldPlayer.panelText.color = inactivePlayerColor.textColor;
        }

        void EnableAvailableButtons()
        {
            foreach (GridSpace gridSpace in gridSpaces)
            {
                if (gridSpace.agentIndex == -1)
                {
                    gridSpace.EnableButton();
                }
            }
        }

        private void GetAgentDecision()
        {
            // Update input type - if human, enable all available buttons
            if (playerAgents[currentAgentIndex].humanControlled == true)
            {
                EnableAvailableButtons();
            }

            // Otherwise request decision from agent
            else
            {
                DisableAllButtons();

                // Give the illusion that the computer is thinking by adding a slight delay for AI decisions
                if (!training && !playerAgents[currentAgentIndex].humanControlled) Invoke("RequestAgentDecision", Random.Range(0.5f, 2.0f));
                //else Invoke("RequestAgentDecision", 0.1f);  // Apparently we need this delay since there's no way to order agents https://github.com/Unity-Technologies/ml-agents/issues/4991
                else RequestAgentDecision();
            }
        }

        private void RequestAgentDecision()
        {
            playerAgents[currentAgentIndex].AgentTakeAction();
        }

        private void DisableAllButtons()
        {
            foreach (GridSpace gridSpace in gridSpaces) gridSpace.DisableButton();
        }

        private void ResetAllButtons()
        {
            foreach (GridSpace gridSpace in gridSpaces)
            {
                gridSpace.EnableButton();
                gridSpace.agentIndex = -1;
            }
        }
        private void ResetGridValues()
        {
            for (int i = 0; i < gridValues.Length; i++)
            {
                gridValues[i] = -1;
            }
        }
    }
}