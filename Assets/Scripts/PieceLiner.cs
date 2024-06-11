using DG.Tweening;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SoftwareKingdom.Chess.UI {
    public class PieceLiner : MonoBehaviour
    {

        // Settings

        // Connections

        public PieceSet pieceSet; // TODO: Scriptable object

        public string[] pieceNotations;
        public GameObject[] piecePrefabs; // TODO: These two will be exported to a scriptable object.

        public PieceController basePiecePrefab;

        Dictionary<string, Sprite> pieceSpritesDict;
        Dictionary<string, GameObject> piecePrefabsDict;
        Dictionary<Coord, GameObject> coordPieceDictionary;
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
            RegisterPiecePrefabs();
            RegisterPieceSprites();
        }

        void InitState()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        void RegisterPiecePrefabs()
        {
            piecePrefabsDict = new Dictionary<string, GameObject>();
            for (int i = 0; i < piecePrefabs.Length; i++)
            {
                piecePrefabsDict.Add(pieceNotations[i], piecePrefabs[i]);
            }
        }

        void RegisterPieceSprites()
        {
            pieceSpritesDict = new Dictionary<string, Sprite>();
            for (int i = 0; i < pieceSet.pieceSprites.Length; i++)
            {
                pieceSpritesDict.Add(pieceSet.pieceDefinitions[i], pieceSet.pieceSprites[i]);
            }
        }

        public void DrawPiece(string pieceNotation, Vector3 absolutePosition, Coord coord)
        {
            if (piecePrefabsDict == null) RegisterPiecePrefabs();

           

            //GameObject pieceGO = Instantiate(piecePrefabsDict[pieceNotation]);
            PieceController pieceController = Instantiate(basePiecePrefab);
            pieceController.SetMainSprite(pieceSpritesDict[pieceNotation]);
            pieceController.SetPieceMode();
            pieceController.transform.position = absolutePosition;
            RegisterPieceToCoordinate(pieceController.gameObject, coord);
        }

        public void AddSeed(string pieceNotation, Vector3 absolutePosition, Coord coord, int seedTurns)
        {
            PieceController pieceController = Instantiate(basePiecePrefab);
            pieceController.SetMainSprite(pieceSpritesDict[pieceNotation]);
            pieceController.Sprout(seedTurns);
            pieceController.transform.position = absolutePosition;
            RegisterPieceToCoordinate(pieceController.gameObject, coord);
        }

        public void SeedPiece(string pieceNotation, Vector3 absolutePosition, Coord coord)
        {
            // TODO.
        }

        public void MovePiece(Coord startCoord, Coord targetCoord, Vector3 absolutePosition) {
            GameObject piece = coordPieceDictionary[startCoord];
            if (piece == null) return;
            coordPieceDictionary.Remove(startCoord);
            

            CheckClearSquare(targetCoord);

            coordPieceDictionary.Add(targetCoord, piece);
            piece.transform.DOJump(absolutePosition, 1.0f, 1, 0.5f); // TODO: Magic Number, and piece can move itself
        }

        public void CheckClearSquare(Coord targetCoord) {
            GameObject pieceInTargetSquare;
            bool thereIsAPieceInTarget = coordPieceDictionary.TryGetValue(targetCoord, out pieceInTargetSquare);

            if (thereIsAPieceInTarget)
            {
                coordPieceDictionary.Remove(targetCoord);
                pieceInTargetSquare.SetActive(false);
            }
        }

        void RegisterPieceToCoordinate(GameObject pieceGO, Coord coord)
        {
            if (coordPieceDictionary == null) coordPieceDictionary = new Dictionary<Coord, GameObject>();
            coordPieceDictionary.Add(coord, pieceGO);
        }

    }

}
