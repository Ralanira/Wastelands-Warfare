using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InGame.TooltipBox
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public string unitName, mPrice, ePrice, sPrice, bTime, message;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.instance.SetAndShowTooltip(unitName, mPrice, ePrice, sPrice, bTime, message);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.instance.HideTooltip(); 
        }
    }
}

