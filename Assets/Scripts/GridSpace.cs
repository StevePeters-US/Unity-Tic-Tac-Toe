using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace APG
{
    public class GridSpace : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI buttonText;

        private GameManager gameManager;
        public void SetSpace()
        {
            buttonText.text = gameManager.GetPlayerSide();
            DisableButton();

            gameManager.EndTurn();
        }

        public void EnableButton()
        {
            button.interactable = true;
            buttonText.text = "";
        }

        public void DisableButton()
        {
            button.interactable = false;
        }


        public void SetGameControllerReference(GameManager manager)
        {
            gameManager = manager;
        }

        public string GetButtonText()
        {
            return buttonText.text.ToString();
        }
    }
}
