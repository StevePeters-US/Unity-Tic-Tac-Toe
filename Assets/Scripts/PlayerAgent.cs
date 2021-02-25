using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;
using TMPro;

namespace APG
{
    [System.Serializable]
    public class PlayerUI
    {
        public Image panelImage;
        public TextMeshProUGUI panelText;
    }

    public class PlayerAgent : Agent
    {
        public bool humanControlled = true;

        public Image panelImage;
        public TextMeshProUGUI panelText;
        public int agentIndex = 1;
        public string agentName = "X";

        private GameManager gameManager;

        private int heuristicRandomIndex = -1;

        private void Awake()
        {
            if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        }


        public override void OnEpisodeBegin()
        {

        }

        public void AgentTakeAction()
        {
            if (true)
            {
                bool validIndexFound = false;

                while (!validIndexFound)
                {
                    heuristicRandomIndex = Random.Range(0, 9);
                    if (gameManager.gridValues[heuristicRandomIndex] == -1) validIndexFound = true;
                }
            }
            RequestDecision();
        }

        // These are the observations that are fed to the model on decision request (defaults to every 5th fixed frame in the decision requester component attached to the agent).
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(agentIndex);

            // 9 observations Get board status from game manager
            for (int i = 0; i < gameManager.gridValues.Length; i++)
            {
                sensor.AddObservation(gameManager.gridValues[i]);
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            Debug.Log("Player " + agentName + " chose " + heuristicRandomIndex + " in heuristic function");
            var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut[0] = heuristicRandomIndex;
        }


        // Convert output from model into usable variables that can be used to pilot the agent.
        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            int gridNum = actionBuffers.DiscreteActions[0];

            Debug.Log("Player " + agentName + " chose " + gridNum + " in OnActionRecieved");

            if (gridNum < gameManager.gridValues.Length)
            {
                gameManager.selectGridSpace(gridNum);
            }
        }

        public void InvalidDecisionPenalty()
        {
            AddReward(-0.1f);
        }
    }
}