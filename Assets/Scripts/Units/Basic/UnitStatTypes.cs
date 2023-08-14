using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitStatTypes : ScriptableObject
    {
        [System.Serializable]
        public class Base
        {
            public float costMinerals, costEnergium, costSupply, aggroRange, attackRange, attackSpeed, attack, health, armor, speed;
        }
    }
}


