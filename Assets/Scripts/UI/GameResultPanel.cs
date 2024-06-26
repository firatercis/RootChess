

using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SoftwareKingdom.Chess.UI
{
    public class GameResultPanel : MonoBehaviour
    {
        public TextMeshProUGUI resultText;
        public Transform gamePanelTransform;
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void ShowGameResult(Core.GameResult result) {
            gamePanelTransform.DOScale(Vector3.one, 1.0f);
            // TODO: Add UI
            if (result.winningIndex == 0)
            {
                resultText.text = "It is a TIE!";
            }

            if (result.winningIndex == 1)
            {
                resultText.text = "White Wins!";
            }

            if (result.winningIndex == -1)
            {
                resultText.text = "Black Wins!";
            }


        }
    }

}

