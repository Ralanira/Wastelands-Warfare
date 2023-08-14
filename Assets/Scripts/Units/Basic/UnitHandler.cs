using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Player;

namespace Units
{
    public class UnitHandler : MonoBehaviour
    {
        public static UnitHandler instance;
        [SerializeField] private BasicUnit worker, scout, tank, executor;
        public LayerMask pUnitLayer, eUnitLayer;

        private void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        public UnitStatTypes.Base GetBasicUnitStats(string type)
        {   
            BasicUnit unit;
            switch (type)
            {
                case "worker":
                    unit = worker;
                    break;
                case "scout":
                    unit = scout;
                    break;
                case "tank":
                    unit = tank;
                    break; 
                case "executor":
                    unit = executor;
                    break;   
                default:
                    Debug.Log($"Unit type: {type} could not be found or does not exist");
                    return null;
            }
            return unit.baseStats;
        }
    }
}