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
            // 3 observation
            //  sensor.AddObservation(Normalization.Sigmoid(agentRB.velocity, 0.25f));
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
  /*          var continuousActionsOut = actionsOut.ContinuousActions;

            if (playerInputHandler)
            {
                //Debug.Log(Mathf.Pow(Vector3.Dot(Vector3.up, transform.right), moveCorrectionDampening));

                // If we have forward input add / subtract it from the appropriate rotors, Otherwise add a stabilization mod based on the dot product of the transform forward vector from the global up vector.
                float forwardMod = (Mathf.Abs(playerInputHandler.moveForward) > 0.1f) ? playerInputHandler.moveForward * moveAmount : Vector3.Dot(Vector3.up, transform.forward) * moveCorrectionDampening * moveAmount;
                float rightMod = (Mathf.Abs(playerInputHandler.moveRight) > 0.1f) ? playerInputHandler.moveRight * moveAmount : Vector3.Dot(Vector3.up, transform.right) * moveCorrectionDampening * moveAmount;

                // Modifier for rotating drone about z axis by modulating power to diagonally opposite rotors
                // Debug.Log(playerInputHandler.lookRight);
                float turnMod = playerInputHandler.lookRight * turnAmount;

                continuousActionsOut[0] = playerInputHandler.throttleTotal - forwardMod + rightMod - turnMod;
                continuousActionsOut[1] = playerInputHandler.throttleTotal - forwardMod - rightMod + turnMod;
                continuousActionsOut[2] = playerInputHandler.throttleTotal + forwardMod + rightMod + turnMod;
                continuousActionsOut[3] = playerInputHandler.throttleTotal + forwardMod - rightMod - turnMod;
            }*/
        }


        // Convert output from model into usable variables that can be used to pilot the agent.
        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
/*            throttleFL = Mathf.Clamp(actionBuffers.ContinuousActions[0], 0f, 1f);
            throttleFR = Mathf.Clamp(actionBuffers.ContinuousActions[1], 0f, 1f);
            throttleRL = Mathf.Clamp(actionBuffers.ContinuousActions[2], 0f, 1f);
            throttleRR = Mathf.Clamp(actionBuffers.ContinuousActions[3], 0f, 1f);*/
        }
    }
}