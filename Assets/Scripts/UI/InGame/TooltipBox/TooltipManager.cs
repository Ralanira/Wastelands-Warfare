using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InGame.TooltipBox
{
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager instance;
        [SerializeField] private GameObject tooltipBox;
        [SerializeField] private TextMeshProUGUI objectName;
        [SerializeField] private TextMeshProUGUI objectMineralsPrice;
        [SerializeField] private TextMeshProUGUI objectEnergiumPrice;
        [SerializeField] private TextMeshProUGUI objectSupplyPrice;
        [SerializeField] private TextMeshProUGUI objectBuildingTime;
        [SerializeField] private TextMeshProUGUI objectDescription;

        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        void Start()
        {
            Cursor.visible = true;
            tooltipBox.SetActive(false);
        }

        public void SetAndShowTooltip(string unitName, string mPrice, string ePrice, string sPrice, string bTime, string message)
        {
            tooltipBox.SetActive(true);
            objectName.text = "Build a " + unitName;
            objectMineralsPrice.text = mPrice;
            objectEnergiumPrice.text = ePrice;
            objectSupplyPrice.text = sPrice;
            objectBuildingTime.text = bTime;
            objectDescription.text = message;
        }

        public void HideTooltip()
        {
            tooltipBox.SetActive(false);
            objectDescription.text = string.Empty;
        }
    }
}
