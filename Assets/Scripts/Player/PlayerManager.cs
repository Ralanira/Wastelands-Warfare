using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;

namespace My.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;
        public Transform playerUnits;
        public Transform enemyUnits;
        public Transform playerBuildings;
        public Transform enemyBuildings;

        private void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;

            SetBasicStats(playerUnits);
            SetBasicStats(enemyUnits);
            SetBasicStats(playerBuildings);
            SetBasicStats(enemyBuildings);
        }

        private void Update()
        {
            InputHandler.instance.HandleUnitMovement();
        }

        public void SetBasicStats(Transform type)
        {
            foreach (Transform child in type)
            {
                foreach (Transform tf in child)
                {
                    string name = child.name.Substring(0, child.name.Length - 1).ToLower();

                    if (type == playerUnits)
                    {
                        Units.Player.PlayerUnit pU = tf.GetComponent<Units.Player.PlayerUnit>();
                        pU.baseStats = Units.UnitHandler.instance.GetBasicUnitStats(name);
                        
                    }
                    else if (type == enemyUnits)
                    {
                        Units.Enemy.EnemyUnit eU = tf.GetComponent<Units.Enemy.EnemyUnit>();
                        eU.baseStats = Units.UnitHandler.instance.GetBasicUnitStats(name);
                    }
                    else if (type == playerBuildings)
                    {
                        Buildings.Player.PlayerBuilding pB = tf.GetComponent<Buildings.Player.PlayerBuilding>();
                        pB.baseStats =  Buildings.BuildingHandler.instance.GetBasicBuildingStats(name);
                    }
                    else if (type == enemyBuildings)
                    {
                        Buildings.Player.PlayerBuilding eU = tf.GetComponent<Buildings.Player.PlayerBuilding>();
                        eU.baseStats =  Buildings.BuildingHandler.instance.GetBasicBuildingStats(name);
                    }
                }
            }
        }
    }
}