using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class BuildingsPlacementManager : MonoBehaviour
{
    public static BuildingsPlacementManager instance = null;
    private Vector2Int GridSize = new Vector2Int(200, 200);
    public GameObject flyingBuilding = null;
    private Camera mainCamera;
    public bool isPlaceable = false;
    private int buildingGridBorder = 10;
    private float buildingCostMinerals;
    private float buildingCostEnergium;
    private Buildings.BasicBuilding mybuilding;
    private HighlightUnderCursor highlightUnderCursor;
    private Units.BuildingConstructing building;
    private GameObject playerBuildingRoot;
    private Units.Player.PlayerUnit pU;
    [SerializeField] private AudioSource confirmBuildingSound;
    private string flyingBuildingName;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        
        mainCamera = Camera.main;  
        playerBuildingRoot = GameObject.Find("Player Buildings");
    }

    void Update()
    {
        if(flyingBuilding)
        {
            CheckIfFlyingBuildingIsPlaceable();
        }
    }

    public void CreateFlyingBuilding(string buildingName)
    {
        if(flyingBuilding != null)
        {
            DestroyFlyingBuilding();
        }
        if (IsBuilding(buildingName))
        {
            mybuilding = IsBuilding(buildingName);
        }
        else
        {
            Debug.Log($"{buildingName} is not a spawnable object");
        }

        flyingBuilding = Instantiate(mybuilding.buildingPrefab);
        flyingBuildingName = flyingBuilding.GetComponent<Buildings.Player.PlayerBuilding>().buildingType.name;
        flyingBuilding.transform.SetParent(playerBuildingRoot.transform.Find(flyingBuildingName.ToString() + "s").transform);
        building = flyingBuilding.GetComponent<Units.BuildingConstructing>();
    }

    private Buildings.BasicBuilding IsBuilding(string name)
    {
        if (UI.InGame.ActionPanel.ActionFrame.instance.actionsList.basicBuildings.Count > 0)
        {
            foreach(Buildings.BasicBuilding building in UI.InGame.ActionPanel.ActionFrame.instance.actionsList.basicBuildings)
            {
                if (building.name == name)
                {
                    return building;
                }
            }
        }
        return null;
    }

    private void CheckIfFlyingBuildingIsPlaceable()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            int x,y;
            x = Mathf.RoundToInt(worldPosition.x);
            y = Mathf.RoundToInt(worldPosition.z);

            if (x < - GridSize.x + buildingGridBorder || x > GridSize.x - buildingGridBorder) 
            {
                isPlaceable = false;
            } 
            else if (y < - GridSize.y + buildingGridBorder || y > GridSize.y - buildingGridBorder) 
            {
                isPlaceable = false; 
            }  
            else if (building.blockingUnits > 0)
            {
                isPlaceable = false;
            }
            else 
            {
                isPlaceable = true;
            }
                
            flyingBuilding.transform.position = new Vector3(x, 0, y);
            building.SetTransparent(isPlaceable);
        }
    }
    
    public void ConfirmBuildingPlacement()
    {
        buildingCostMinerals = flyingBuilding.gameObject.GetComponent<Buildings.Player.PlayerBuilding>().baseStats.costMinerals;
        buildingCostEnergium = flyingBuilding.gameObject.GetComponent<Buildings.Player.PlayerBuilding>().baseStats.costEnergium;
        if(!isPlaceable)
        {
            UI.InGame.MessageString.instance.PrintMessage("You can't build here!");
        }
        if(!UI.InGame.PlayerResources.PlayerBank.instance.HasEnoughResourses(buildingCostMinerals, buildingCostEnergium))
        {
            UI.InGame.MessageString.instance.PrintMessage("Not enough resources!");
        } 
        if (UI.InGame.PlayerResources.PlayerBank.instance.HasEnoughResourses(buildingCostMinerals, buildingCostEnergium) && isPlaceable)
        {
            UI.InGame.PlayerResources.PlayerBank.instance.WithdrawResources(buildingCostMinerals,buildingCostEnergium);
            foreach (Transform transform in InputManager.InputHandler.instance.selectedUnits)
            {
                if(transform.gameObject.TryGetComponent<Units.Basic.Workers.ResourceMiner>(out Units.Basic.Workers.ResourceMiner miningManager))
                {
                    pU = transform.gameObject.GetComponent<Units.Player.PlayerUnit>();
                    break;
                }
            }
            pU.CancelConstructingBuilding();
            pU.StopMining();
            pU.plannedBuilding = flyingBuilding;
            flyingBuilding = null;
            isPlaceable = false;
            confirmBuildingSound.time = 0.1f;
            confirmBuildingSound.Play();
            pU.MoveToBuild();
        }     
    }

    public void DestroyFlyingBuilding()
    {
        Destroy(flyingBuilding.gameObject);
    }
}
