using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "New unit")]
    public class BasicUnit : ScriptableObject
    {
        [Header("Unit Settings")]
        [Space(15)]
        public UnitType Type;

        public string unitName;
        public GameObject friendlyUnitPrefab;
        public GameObject enemyUnitPrefab;
        public GameObject icon;
        public int spawnTime;
        public string description;
        [Space(40)]

        [Header("Unit base stats")]
        [Space(15)]
        public UnitStatTypes.Base baseStats;
    }
}

