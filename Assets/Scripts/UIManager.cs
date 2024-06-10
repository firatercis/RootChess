using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftwareKingdom.Chess.UI
{
    public class UIManager : MonoBehaviour
    {

        // Settings

        // Connections
        public event Action<int> OnPieceSelected;
        public GameObject selectPiecePanel;
        public GameObject seedTitle;
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

        public void OnPieceButton(int pieceID)
        {
            OnPieceSelected?.Invoke(pieceID);
        }

        public void ShowPiecesMenu(bool isWhite = true, bool isSeed = false)
        {
            selectPiecePanel.gameObject.SetActive(true);
            PieceSelectionUI pieceSelectionUI = selectPiecePanel.GetComponent<PieceSelectionUI>();
            pieceSelectionUI.Configure(isWhite, isSeed);
        }
        public void HidePiecesMenu()
        {
            selectPiecePanel.gameObject.SetActive(false);
        }

        public void ShowSeedSelectionMenu()
        {
            seedTitle.SetActive(true);
            selectPiecePanel.gameObject.SetActive(true);
            PieceSelectionUI pieceSelectionUI = selectPiecePanel.GetComponent<PieceSelectionUI>();
            //pieceSelectionUI.Configure()
        }

    }

}
