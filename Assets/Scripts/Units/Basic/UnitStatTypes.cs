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
            [SerializeField] private float ñostMinerals;
            [SerializeField] private float costEnergium;
            [SerializeField] private float costSupply;
            [SerializeField] private float aggroRange;
            [SerializeField] private float attackRange;
            [SerializeField] private float attackSpeed;
            [SerializeField] private float attack;
            [SerializeField] private float health;
            [SerializeField] private float armor;
            [SerializeField] private float speed;

            public float CostMinerals => ñostMinerals;
            public float CostEnergium => costEnergium;
            public float CostSupply => costSupply;
            public float AggroRange => aggroRange;
            public float AttackRange => attackRange;
            public float AttackSpeed => attackSpeed;
            public float Attack => attack;
            public float Health => health;
            public float Armor => armor;
            public float Speed => speed;
        }
    }
}


