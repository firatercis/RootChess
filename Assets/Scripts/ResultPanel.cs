using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using SoftwareKingdom.Chess.Core;


namespace SoftwareKingdom.Chess.UI
{
    public class ResultPanel : MonoBehaviour
    {

        // Settings

        // Connections
        public TextMeshProUGUI resultText;
        // State variables

        void Awake()
        {
            InitConnections();
        }

        void Start()
        {
            InitState();
        }

        void InitConnections()
        {
        }

        void InitState()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        //public void DisplayResult(int colourToWin)
        //{
        //    gameObject.SetActive(true);
        //    string winnerName = colourToWin == Piece.COLOR_WHITE ? "WHITE" : "BLACK";
        //    Color textColor = colourToWin == Piece.COLOR_WHITE  ? Color.white : Color.black;

        //    resultText.color = textColor;
        //    resultText.text = winnerName + " WINS!";

        //}

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

