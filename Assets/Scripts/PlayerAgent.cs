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
    // Should have 3 settings human, random, and inference, 
    // So a bool is set for human input or random if there is no model set ( which would run inference).
    // Observations per grid section are -1 for X, 1 for O, 0 if empty.
    // Agent can only choose 1 output out of 9 ( 1 hot encoded).

    public enum AgentControlType
    {
        human,
        random,
        ai
    }

    [System.Serializable]
    public class PlayerUI
    {
        public Image panelImage;
        public TextMeshProUGUI panelText;
    }

    public class PlayerAgent : Agent
    {
        [SerializeField] private AgentControlType agentControlType = AgentControlType.human; // If true, this script does nothing

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
            /*           var discreteActionsOut = actionsOut.DiscreteActions;

                       bool validIndexFound = false;
                       int randomIdx = 0;

                       while (!validIndexFound)
                       {
                           randomIdx = Random.Range(0, 9);
                           if (gameManager.gridValues[randomIdx] == -1) validIndexFound = true;
                       }
                       Debug.Log("Random index = " + randomIdx);
                       discreteActionsOut[0] = randomIdx;*/

            var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut[0] = heuristicRandomIndex;
        }


        // Convert output from model into usable variables that can be used to pilot the agent.
        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            int gridNum = actionBuffers.DiscreteActions[0];
            if (gridNum < gameManager.gridValues.Length)
            {
               // Debug.Log("GridSpace index = " + gridNum);
                gameManager.selectGridSpace(gridNum);
            }
        }

        public void InvalidDecisionPenalty()
        {
            AddReward(-0.1f);
        }
    }
}