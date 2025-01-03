using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.InGame.SelectedObjectsData
{
    public class SelectedUnitFrame : MonoBehaviour
    {
        public static SelectedUnitFrame instance;
        [SerializeField] private GameObject selectedObjectInfo;
        [SerializeField] private GameObject selectedMultObjectsInfo;
        [SerializeField] private Image objectIcon;
        [SerializeField] private TextMeshProUGUI objectName;
        [SerializeField] private TextMeshProUGUI objectDamage;
        [SerializeField] private TextMeshProUGUI objectArmor;
        [SerializeField] private TextMeshProUGUI objectRange;
        [SerializeField] private TextMeshProUGUI objectSpeed;
        [SerializeField] private TextMeshProUGUI objectHealth;
        [SerializeField] private Image healthBarAmount;
        [SerializeField] private TextMeshProUGUI objectDescription;
        [SerializeField] private List<Transform> selectedUnits;
        [SerializeField] private Transform selectedBuilding;
        private Units.Player.PlayerUnit pU = null;
        private Buildings.Player.PlayerBuilding pB = null;

        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        void Start()
        {
            selectedObjectInfo.SetActive(false);
            selectedUnits = InputManager.InputHandler.instance.selectedUnits;
        }

        void Update() 
        {
            selectedBuilding = InputManager.InputHandler.instance.selectedBuilding;
            if(selectedUnits.Count == 1 || selectedBuilding != null)
            {
                selectedMultObjectsInfo.SetActive(false);
                selectedObjectInfo.SetActive(true);    

                if(selectedBuilding != null)
                {
                    SetBuildingInfo();
                }
                if(selectedUnits.Count == 1)
                {
                    SetUnitInfo();
                }
                HandleHealth();
            }
            else if (selectedUnits.Count > 1 && !selectedBuilding)
            {
                selectedObjectInfo.SetActive(false);
                selectedMultObjectsInfo.SetActive(true);
            }
            else if (selectedUnits.Count == 0 && !selectedBuilding)
            {
                selectedObjectInfo.SetActive(false);
                selectedMultObjectsInfo.SetActive(false);
            }
        }

        public void Hide()
        {
            instance.gameObject.SetActive(false);
        }

        public void Show()
        {
            instance.gameObject.SetActive(true);
        }

        public void SetUnitInfo()
        {
            if(selectedUnits.Count == 1)
            {
                pU = selectedUnits[0].gameObject.GetComponent<Units.Player.PlayerUnit>();
            }
            objectName.text = pU.unitType.unitName.ToString();
            objectDamage.text = pU.baseStats.Attack.ToString();
            objectArmor.text = pU.baseStats.Armor.ToString();
            objectRange.text = pU.baseStats.AttackRange.ToString();
            objectSpeed.text = pU.baseStats.Speed.ToString();
            objectIcon.sprite = pU.unitType.icon.gameObject.transform.GetComponentInChildren<Image>().sprite;  
            objectDescription.text = pU.unitType.description.ToString();
            HandleHealth();
        }

        public void SetBuildingInfo()
        {
            if(InputManager.InputHandler.instance.selectedBuilding != null)
            {
                pB = InputManager.InputHandler.instance.selectedBuilding.gameObject.GetComponent<Buildings.Player.PlayerBuilding>();
                        objectName.text = pB.buildingType.name.ToString();
                objectDamage.text = 0.ToString();
                objectArmor.text = pB.baseStats.armor.ToString();
                objectRange.text = 0.ToString();
                objectSpeed.text = 0.ToString();
                objectIcon.sprite = pB.buildingType.icon.gameObject.transform.GetComponentInChildren<Image>().sprite;  
                objectDescription.text = pB.buildingType.description.ToString();
                HandleHealth();
            }
        }

        private void HandleHealth()
        {
            if (selectedBuilding != null)
            {
                objectHealth.text = pB.statDisplay.currentHealth.ToString() + "/" + pB.statDisplay.maxHealth.ToString();
                healthBarAmount.fillAmount = pB.statDisplay.currentHealth / pB.statDisplay.maxHealth;
                return;
            }
            if (selectedUnits.Count == 1 )
            {
                objectHealth.text = pU.statDisplay.currentHealth.ToString() + "/" + pU.statDisplay.maxHealth.ToString();
                healthBarAmount.fillAmount = pU.statDisplay.currentHealth / pU.statDisplay.maxHealth;
                return;
            }
        }
    }
}
