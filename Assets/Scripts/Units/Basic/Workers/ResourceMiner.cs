using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Basic.Workers
{
    public class ResourceMiner : MonoBehaviour
    {
        [SerializeField]bool isWorking = false;
        [SerializeField]GameObject targetMine;
        [SerializeField] GameObject cargoModel;
        Units.Player.PlayerUnit pU;
        bool canMine = true;
        float miningRange = 5;
        float timeToMine = .5f;
        float returningResoursesDistance = 9f;
        float resoursePerItem = 100;
        Resources.InventoryManager inventoryManager;
        Transform commandCentersParent;
        Transform closestCommandCenter;
        float distanceToCC, minDistanceToCC;
        List<Transform> commandCenters = new List<Transform>();

        void Start()
        {
            inventoryManager = GetComponent<Resources.InventoryManager>();
            pU = GetComponent<Units.Player.PlayerUnit>();
            commandCentersParent = GameObject.Find("Player Buildings").transform.Find("Command Centers").transform;
        }

        void Update()
        {
            if(isWorking)
            {
                GatherOrReturnResources();
            }
        }

        public void StartWorking(GameObject miningTarget)
        {
            targetMine = miningTarget;
            isWorking = true;
            commandCenters.Clear();
            pU.MoveOrderUnit(targetMine.transform.position);
            pU.navAgent.stoppingDistance = miningRange;
        }

        public void StopWorking()
        {
            targetMine = null;
            isWorking = false;
            StopAllCoroutines();
            canMine = true;
        }

        private void GatherOrReturnResources()
        {
            if(inventoryManager.carriedItems.Count < inventoryManager.maxItemsInInventory)
            {
                if(Vector3.Distance(targetMine.transform.position, transform.position) <= miningRange)
                {
                    if(canMine)
                    {
                        StartCoroutine(Mine());
                    }
                }
            }
            else
            {
                if(commandCenters.Count == 0)
                {
                    FindClosestCommandCenter();
                    pU.MoveOrderUnit(closestCommandCenter.position);
                }

                if(Vector3.Distance(closestCommandCenter.position, transform.position) <= returningResoursesDistance)
                {
                    ReturnResources();
                    StartWorking(targetMine);
                }     
            }
        }
    
        private IEnumerator Mine()
        {
            canMine = false;
            yield return new WaitForSecondsRealtime(timeToMine);
            if(Vector3.Distance(targetMine.transform.position, transform.position) <= miningRange)
            {
                inventoryManager.carriedItems.Add(targetMine.GetComponent<Resources.ResourceType>().resource);
                cargoModel.SetActive(true);
            }
            canMine = true;
        }

        private void FindClosestCommandCenter()
        {
            foreach(Transform child in commandCentersParent)
            {
                commandCenters.Add(child);
            }
                    
            int k = 0;
            minDistanceToCC = Vector3.Distance(transform.position, commandCenters[0].position);
            for (int i = 1;  i < commandCenters.Count; i++)
            {
                if(!commandCenters[i].gameObject.transform.TryGetComponent<BuildingConstructing>(out BuildingConstructing bs))
                {
                    distanceToCC = Vector3.Distance(transform.position, commandCenters[i].position);
                    if(distanceToCC < minDistanceToCC)
                    {
                        minDistanceToCC = distanceToCC;
                        k = i;
                    }
                }
            }
            closestCommandCenter = commandCenters[k];
        }

        private void ReturnResources()
        {
            foreach(Resources.ItemType item in inventoryManager.carriedItems)
            {
                switch(item)
                {
                    case Resources.ItemType.Minerals:
                        UI.InGame.PlayerResources.PlayerBank.instance.DepositResources(resoursePerItem, 0);
                        break;
                    case Resources.ItemType.Energium:
                        UI.InGame.PlayerResources.PlayerBank.instance.DepositResources(0, resoursePerItem);
                        break;
                }
            }
            inventoryManager.carriedItems.Clear();
            cargoModel.SetActive(false);
        }
    }
}
