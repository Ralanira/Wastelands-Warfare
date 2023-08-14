using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(fileName = "Building", menuName = "New Building/Basic")]
    public class BasicBuilding : ScriptableObject
    {
        public enum buildingType
        {
            CommandCenter,
            Factory,
            SupplyDepot,
            AdvFactory,
            DefenceTower
        }

        [Space(15)]
        [Header("Building Settings")]
        public buildingType type;
        public new string name;
        public GameObject buildingPrefab;
        public GameObject icon;
        public string description;
 
        [Space(40)]
        [Header("Building base stats")]
        [Space(15)]
        public BuildingStatTypes.Base baseStats;
    }
}

