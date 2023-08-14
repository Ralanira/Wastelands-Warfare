using UnityEngine;

namespace Buildings
{
    public class BuildingHandler : MonoBehaviour
    {
        public static BuildingHandler instance;
        [SerializeField] private BasicBuilding factory, commandCenter, supplyDepot;

        private void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        public BuildingStatTypes.Base GetBasicBuildingStats(string type)
        {
            BasicBuilding building;
            switch (type)
            {
                case "factory":
                    building = factory;
                    break;
                case "command center":
                    building = commandCenter;
                    break;   
                case "supply depot":
                    building = supplyDepot;
                    break; 
                default:
                    Debug.Log($"Unit type: {type} could not be found or does not exist");
                    return null;
                }
                return building.baseStats;
            }
        } 
}
