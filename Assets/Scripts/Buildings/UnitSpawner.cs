using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject SpawnMarker = null;
    List<float> spawnQueue = new List<float>();
    List<GameObject> spawnOrder = new List<GameObject>();
    GameObject playerUnitRoot;
    
    void Start() 
    {
        playerUnitRoot = GameObject.Find("Player Units");
    }

    public void StartSpawnTimer(string objectToSpawn)
    {
        if (IsUnit(objectToSpawn))
        {
            Units.BasicUnit unit = IsUnit(objectToSpawn);
            if(UI.InGame.PlayerResources.PlayerBank.instance.HasEnoughResourses(unit.baseStats.costMinerals, unit.baseStats.costEnergium, unit.baseStats.costSupply))
            {
                UI.InGame.PlayerResources.PlayerBank.instance.ManageSupply(unit.baseStats.costSupply, 0);
                UI.InGame.PlayerResources.PlayerBank.instance.WithdrawResources(unit.baseStats.costMinerals,unit.baseStats.costEnergium);
                spawnQueue.Add(unit.spawnTime);
                spawnOrder.Add(unit.friendlyUnitPrefab);
            }
            else
            {
                UI.InGame.MessageString.instance.PrintMessage("Not enough resources!");
            }

        }
        else
        {
            Debug.Log($"{objectToSpawn} is not a spawnable object");
        }

        if (spawnQueue.Count == 1)
        {
            StartCoroutine(SpawnQueueTimer());
        }
        else if (spawnQueue.Count == 0)
        {
            StopAllCoroutines();
        }
    }

    private Units.BasicUnit IsUnit(string name)
    {
        if (UI.InGame.ActionPanel.ActionFrame.instance.actionsList.basicUnits.Count > 0)
        {
            foreach(Units.BasicUnit unit in UI.InGame.ActionPanel.ActionFrame.instance.actionsList.basicUnits)
            {
                if (unit.name == name)
                {
                    return unit;
                }
            }
        }
        return null;
    }

    private IEnumerator SpawnQueueTimer()
    {
        if (spawnQueue.Count > 0)
        {
            yield return new WaitForSeconds(spawnQueue[0]);
            SpawnUnit();
            if (spawnQueue.Count > 0)
            {
                StartCoroutine(SpawnQueueTimer());
            }
        }
    }

    private void SpawnUnit()
    {
        if(spawnOrder.Count > 0)
        {
            GameObject spawnedObject = Instantiate(spawnOrder[0], new Vector3(transform.position.x, 
                transform.parent.position.y, transform.position.z - 8), Quaternion.Euler(0,180,0));
            Units.Player.PlayerUnit pU = spawnedObject.GetComponent<Units.Player.PlayerUnit>();
            pU.transform.SetParent(playerUnitRoot.transform.Find(pU.unitType.unitName.ToString() + "s").transform);
            spawnedObject.GetComponent<Units.Player.PlayerUnit>().MoveOrderUnit(SpawnMarker.transform.position);
            spawnQueue.Remove(spawnQueue[0]);
            spawnOrder.Remove(spawnOrder[0]);
        }
    }
}
