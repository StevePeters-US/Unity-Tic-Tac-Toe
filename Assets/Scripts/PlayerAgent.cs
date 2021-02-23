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

        public Image panelImage;
        public TextMeshProUGUI panelText;
        public int agentIndex = 1;
        public string agentName = "X";

        private GameManager gameManager;

        private void Awake()
        {
            if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnEpisodeBegin()
        {

        }


        // These are the observations that are fed to the model on decision request (defaults to every 5th fixed frame in the decision requester component attached to the agent).
        public override void CollectObservations(VectorSensor sensor)
        {
            // 9 observations Get board status from game manager
            for (int i = 0; i < gameManager.gridValues.Length; i++)
            {
                sensor.AddObservation(gameManager.gridValues[i]);
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            int randomIdx = Random.Range(0, 10);

            discreteActionsOut[randomIdx] = 1;          
        }


        // Convert output from model into usable variables that can be used to pilot the agent.
        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            int gridNum = actionBuffers.DiscreteActions[0];
            if (gridNum < gameManager.gridValues.Length)
            {
                gameManager.selectGridSpace(gridNum);
            }
        }
    }
}