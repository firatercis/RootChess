using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoftwareKingdom.Chess.UI
{
    [CreateAssetMenu(menuName = "Theme/PieceSet")]
    public class PieceSet : ScriptableObject
    {

        // Settings
        public string[] pieceDefinitions;
        public Sprite[] pieceSprites;

        // Connections

        // State variables
        public Dictionary<string, Sprite> defToSpriteDictionary;

        public void Load()
        {
            defToSpriteDictionary = new Dictionary<string, Sprite>();
            for(int i=0; i< pieceDefinitions.Length; i++)
            {
                defToSpriteDictionary.Add(pieceDefinitions[i], pieceSprites[i]);
            }
        }

        // Update is called once per frame
       public Sprite GetPieceSprite(string piece)
       {


            return defToSpriteDictionary[piece];
       }
    }

}
