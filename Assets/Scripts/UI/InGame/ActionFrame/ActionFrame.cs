using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.ActionPanel
{
    public class ActionFrame : MonoBehaviour
    {
        public static ActionFrame instance = null;
        [SerializeField] private Button actionButton = null;
        [SerializeField] private Transform layoutGroup = null;
        private List<Button> buttons = new();
        public PlayerActions actionsList = null;
        
        void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        public void SetActionButtons(PlayerActions actions)
        {
            if(buttons.Count == 0)
            {
                actionsList = actions;

                if (actions.basicCommands.Count > 0)
                {
                    AddBasicCommandButtons(actions);
                }

                if (actions.basicUnits.Count > 0)
                {
                    AddUnitButtons(actions);
                }

                if (actions.basicBuildings.Count > 0)
                {
                    AddBuildingButtons(actions);
                }
            }
        }

        private void AddUnitButtons(PlayerActions actions)
        {
            foreach(Units.BasicUnit unit in actions.basicUnits)
            {      
                Button btn = Instantiate(actionButton, layoutGroup);
                btn.name = unit.name;
                UI.InGame.TooltipBox.Tooltip tooltip = btn.gameObject.AddComponent(typeof(UI.InGame.TooltipBox.Tooltip)) as UI.InGame.TooltipBox.Tooltip;
                tooltip.unitName = unit.unitName;
                tooltip.mPrice = unit.baseStats.costMinerals.ToString();
                tooltip.ePrice = unit.baseStats.costEnergium.ToString();
                tooltip.sPrice = unit.baseStats.costSupply.ToString();
                tooltip.bTime = unit.spawnTime.ToString();
                tooltip.message = unit.description;
                GameObject icon = Instantiate(unit.icon, btn.transform);
                buttons.Add(btn);
            }
        }

        private void AddBuildingButtons(PlayerActions actions)
        {
            foreach(Buildings.BasicBuilding building in actions.basicBuildings)
            {
                Button btn = Instantiate(actionButton, layoutGroup);
                btn.name = building.name;
                UI.InGame.TooltipBox.Tooltip tooltip = btn.gameObject.AddComponent(typeof(UI.InGame.TooltipBox.Tooltip)) as UI.InGame.TooltipBox.Tooltip;
                tooltip.unitName = building.name;
                tooltip.mPrice = building.baseStats.costMinerals.ToString();
                tooltip.ePrice = building.baseStats.costEnergium.ToString();
                tooltip.sPrice = 0.ToString();
                tooltip.bTime = building.baseStats.constructionTime.ToString();
                tooltip.message = building.description;
                GameObject icon = Instantiate(building.icon, btn.transform);
                buttons.Add(btn);
            }    
        }

        private void AddBasicCommandButtons(PlayerActions actions)
        {
            foreach(Units.Player.PlayerUnitCommands command in actions.basicCommands)
            {
                Button btn = Instantiate(actionButton, layoutGroup);
                btn.name = command.name;
                GameObject icon = Instantiate(command.icon, btn.transform);
                buttons.Add(btn);
            }
        }

        public void ClearActions()
        {
            foreach (Button btn in buttons)
            {
                Destroy(btn.gameObject);
            }
            buttons.Clear();
        }
    }
}

