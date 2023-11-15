using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.DataStructs;

namespace Pikachu.Scripts.Common.Database
{
    [CreateAssetMenu(fileName = "Creature Database", menuName = "Scriptable Objects/Databases/Creature Database", order = 1)]
    public class CreatureDatabase : ScriptableObject
    {
        [SerializeField] private List<CreatureData> creatureImages;
        [SerializeField] private bool get;

        public int Size => creatureImages != null ? creatureImages.Count : 0;

        public List<CreatureData> CreatureImages => creatureImages;

        private void GetCreatures()
        {
            creatureImages?.Clear();
            Sprite[] creatures = Resources.LoadAll<Sprite>("Creatures");
            
            for (int i = 0; i < creatures.Length; i++)
            {
                creatureImages.Add(new CreatureData
                {
                    ID = i,
                    Creature = creatures[i]
                });
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (get)
            {
                get = false;
                GetCreatures();
            }
        }
#endif
    }
}
