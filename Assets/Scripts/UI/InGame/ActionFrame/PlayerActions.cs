using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.ActionPanel
{
    [CreateAssetMenu(fileName = "NewPlayerActions", menuName = "PlayerActions")]
    public class PlayerActions : ScriptableObject
    {
        [Space(5)]
        [Header("Units")]
        public List<Units.BasicUnit> basicUnits = new List<Units.BasicUnit>();
        [Space(15)]
        [Header("Buildings")]
        public List<Buildings.BasicBuilding> basicBuildings = new List<Buildings.BasicBuilding>();
        [Space(15)]
        [Header("BasicCommands")]
        public List<Units.Player.PlayerUnitCommands> basicCommands = new List<Units.Player.PlayerUnitCommands>();
    }
}
