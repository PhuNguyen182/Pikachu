using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.DataStructs;
using Pikachu.Scripts.Common.Interfaces;
using Pikachu.Scripts.Common.Enumerations;

namespace Pikachu.Scripts.Common.GameBoard
{
    public class CreatureCell : MonoBehaviour, ICreatureCell
    {
        [SerializeField] private SpriteRenderer cellBackground;
        [SerializeField] private SpriteRenderer creatureSprite;

        [Header("Background Color")]
        [SerializeField] private Color normalColor;
        [SerializeField] private Color chooseColor;
        [SerializeField] private Color suggestColor;

        private int _id;
        private CreatureData _data;

        public int ID => _id;

        public CreatureData Data => _data;

        public void SetCellData(CreatureData data)
        {
            _data = data;
            _id = data.ID;
            creatureSprite.sprite = data.Creature;
        }

        public void SetChooseBorderActive(CreatureCellState state)
        {
            switch (state)
            {
                case CreatureCellState.Normal:
                cellBackground.color = normalColor;
                    break;
                case CreatureCellState.Choose:
                    cellBackground.color = chooseColor;
                    break;
                case CreatureCellState.Suggest:
                    cellBackground.color = suggestColor;
                    break;
            }
        }

        public void Clear()
        {
            SimplePool.Despawn(this.gameObject);
        }

        public void ResetCell()
        {
            cellBackground.color = normalColor;
        }
    }
}
