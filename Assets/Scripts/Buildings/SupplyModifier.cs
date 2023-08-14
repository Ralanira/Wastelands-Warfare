using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyModifier : MonoBehaviour
{
    private float supplyProvided;
    Units.BuildingConstructing buildingConstructing = null;

    void Start()
    {
        supplyProvided = GetComponent<Buildings.Player.PlayerBuilding>().baseStats.supplyProvided;
        UI.InGame.PlayerResources.PlayerBank.instance.ManageSupply(0, supplyProvided);
    }

    void OnDisable()
    {
        buildingConstructing = GetComponent<Units.BuildingConstructing>();
        if (buildingConstructing == null)
        {
            UI.InGame.PlayerResources.PlayerBank.instance.ManageSupply(0, -supplyProvided);
        }
    }
}
