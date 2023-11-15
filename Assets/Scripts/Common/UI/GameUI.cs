using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pikachu.Scripts.Common.GameBoard;

namespace Pikachu.Scripts.Common.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button shuffleButton;
        [SerializeField] private Button suggestButton;
        [SerializeField] private PikachuGameBoard gameBoard;

        private void Awake()
        {
            shuffleButton?.onClick.AddListener(() =>
            {
                gameBoard.Shuffle();
            });

            suggestButton?.onClick.AddListener(() =>
            {
                gameBoard.Suggest();
            });
        }
    }
}
