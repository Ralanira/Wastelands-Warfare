using UnityEngine;

namespace Buildings.Player
{
    public class PlayerBuilding : MonoBehaviour
    {
        public BasicBuilding buildingType;
        [HideInInspector]
        public BuildingStatTypes.Base baseStats;
        public Units.UnitStatDisplay statDisplay;
        Units.BuildingConstructing buildingConstructing;

        private void Start() 
        {
            baseStats = buildingType.baseStats;
            statDisplay.SetStatDisplayBasicBuilding(baseStats, true);
            if(!BuildingsPlacementManager.instance.flyingBuilding)
            {
                buildingConstructing = gameObject.GetComponent<Units.BuildingConstructing>();
                if(buildingConstructing)
                {
                    buildingConstructing.FinishConstructing();
                    gameObject.AddComponent<HighlightUnderCursor>();
                    Destroy(GetComponent<Rigidbody>());
                }
            }
        }
    }
}