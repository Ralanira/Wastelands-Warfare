using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace UI.InGame.PlayerResources
{
    public class PlayerBank : MonoBehaviour
    {
        public static PlayerBank instance;
        [SerializeField] private float startingMineralsBalance = 0;
        [SerializeField] private float startingEnergiumBalance = 0;
        private float currentMineralsBalance;
        private float currentEnergiumBalance;
        private float currentSupplyUsed;
        private float currentSupplyLimit;
        private float startingUnitSupply;
        [SerializeField] private TextMeshProUGUI displayMineralsBalance;
        [SerializeField] private TextMeshProUGUI displayEnergiumBalance;
        [SerializeField] private TextMeshProUGUI displaySupplyBalance;
        [SerializeField] private GameObject playerUnits;
        private Units.Player.PlayerUnit pU;

        void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        void Start() 
        {
            currentMineralsBalance = startingMineralsBalance;  
            currentEnergiumBalance = startingEnergiumBalance;
            CheckForExistingUnits();
            UpdateDisplay();  
        }

        private void CheckForExistingUnits()
        {
            for (int i = 0; i < playerUnits.transform.childCount; i++)
            {
                for (int k = 0; k < playerUnits.transform.GetChild(i).gameObject.transform.childCount; k++)
                {
                    pU = playerUnits.transform.GetChild(i).gameObject.transform.GetChild(k).gameObject.GetComponent<Units.Player.PlayerUnit>();
                    if(pU != null)
                    {
                        startingUnitSupply = pU.baseStats.costSupply;
                        currentSupplyUsed += startingUnitSupply;
                        pU = null;
                    }
                }
            } 
        }

        private void UpdateDisplay()
        {
            displayMineralsBalance.text = currentMineralsBalance.ToString();
            displayEnergiumBalance.text = currentEnergiumBalance.ToString();
            displaySupplyBalance.text = currentSupplyUsed.ToString() + "/" + currentSupplyLimit.ToString();
        }

        public bool HasEnoughResourses(float requestedMinerals, float requestedEnergium)
        {
            if(requestedMinerals <= currentMineralsBalance && requestedEnergium <= currentEnergiumBalance)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public bool HasEnoughResourses(float requestedMinerals, float requestedEnergium, float requestedSupply)
        {
            if(requestedMinerals <= currentMineralsBalance && requestedEnergium <= currentEnergiumBalance && requestedSupply <= currentSupplyLimit - currentSupplyUsed)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public void DepositResources(float mineralsAmount, float energiumAmount)
        {
            currentMineralsBalance += mineralsAmount;
            currentEnergiumBalance += energiumAmount;
            UpdateDisplay();
        }

        public void WithdrawResources(float mineralsAmount, float energiumAmount)
        {
            currentMineralsBalance -= mineralsAmount;
            currentEnergiumBalance -= energiumAmount;
            UpdateDisplay();
        }

        public void ManageSupply(float supplyAmount, float supplyLimit)
        {
            currentSupplyUsed += supplyAmount;
            currentSupplyLimit += supplyLimit;
            UpdateDisplay();
        }
    }
}
