using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class BuildingConstructing : MonoBehaviour
    {
        [SerializeField] Renderer MainRenderer;
        [SerializeField] public NavMeshObstacle navMeshObstacle;
        [SerializeField] public GameObject unitStatDisplay;
        [SerializeField] private GameObject mainBuilding;
        [SerializeField] private GameObject constructionModel;
        [SerializeField] private GameObject minimapMarker;
        public bool isConstructing = false;

        private SupplyModifier supplyModifier;
        private Buildings.Player.PlayerBuilding playerBuilding;
        private float spawnTime;
        private BoxCollider boxCol;
        private UnitStatDisplay unitStats;
        private Interactables.IBuilding iBuilding;

        private int _blockingUnits = 0;
        public int blockingUnits
        {
            get
            {
                return _blockingUnits;
            }
        }

        void Start() 
        {   
            if(BuildingsPlacementManager.instance.flyingBuilding)
            {
                CreateBuildingTemplate();
            }
            playerBuilding = GetComponent<Buildings.Player.PlayerBuilding>();
            spawnTime = playerBuilding.baseStats.constructionTime;
        }

        void Update() 
        {
            if (isConstructing)
            {
                unitStats.currentHealth += Mathf.Ceil(0.9f * unitStats.maxHealth * Time.deltaTime / spawnTime);
                if(unitStats.currentHealth >= unitStats.maxHealth)
                {
                    unitStats.currentHealth = unitStats.maxHealth;
                    StopCoroutine(Constructing());
                    FinishConstructing();
                }
            } 
        }

        public void SetTransparent(bool isPlaceable)
        {
            if (isPlaceable)
            {
                MainRenderer.material.color = Color.green;
            }
            else
            {
                MainRenderer.material.color = Color.red;
            }
        }

        public void StartConstructing()
        {
            MainRenderer.material.color = Color.white;
            unitStats.currentHealth = unitStats.maxHealth / 10;
            unitStatDisplay.SetActive(true);
            minimapMarker.SetActive(true);
            navMeshObstacle.enabled = true;
            Destroy(GetComponent<Rigidbody>());
            mainBuilding.SetActive(false);
            constructionModel.SetActive(true);
            gameObject.layer = 8;
            boxCol.isTrigger = false;
            isConstructing = true;
            StartCoroutine(Constructing());
        }

        private IEnumerator Constructing()
        {
            yield return new WaitForSecondsRealtime(spawnTime);
            FinishConstructing();
        }

        public void FinishConstructing()
        {
            constructionModel.SetActive(false);
            mainBuilding.SetActive(true);

            isConstructing = false;
            supplyModifier = GetComponent<SupplyModifier>();
            if(supplyModifier != null)
            {
                supplyModifier.enabled = true;
            }  

            iBuilding = GetComponent<Interactables.IBuilding>();
            if(iBuilding.isInteracting)
            {
                UI.InGame.ActionPanel.ActionFrame.instance.SetActionButtons(iBuilding.actions);
            }

            Destroy(this);
        }

        private void OnTriggerExit(Collider other)
        {
            _blockingUnits--;
        }

        private void OnTriggerEnter(Collider other) 
        {
            _blockingUnits++;
        }

        private void CreateBuildingTemplate()
        {
            boxCol = gameObject.GetComponent<BoxCollider>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            navMeshObstacle.enabled = false;
            unitStats = unitStatDisplay.GetComponent<UnitStatDisplay>();
            unitStatDisplay.SetActive(false);
            minimapMarker.SetActive(false);
            gameObject.layer = 12;
            boxCol.isTrigger = true;
        }
    }
}
