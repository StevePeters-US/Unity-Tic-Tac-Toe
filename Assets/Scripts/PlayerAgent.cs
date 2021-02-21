using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Should have 3 settings human, random, and inference, 
// So a bool is set for human input or random if there is no model set ( which would run inference).
// Observations per grid section are -1 for X, 1 for O, 0 if empty.
// Agent can only choose 1 output out of 9 ( 1 hot encoded).


public class PlayerAgent : MonoBehaviour
{
    [SerializeField] private bool humanControlled; // If true, this script does nothing

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
