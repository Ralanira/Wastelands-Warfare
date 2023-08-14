using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.ActionPanel
{
    public class Action : MonoBehaviour
    {
        private UnitSpawner unitSpawner;
        
        public void OnClick()
        {
            if (InputManager.InputHandler.instance.selectedBuilding != null)
            {
                unitSpawner = InputManager.InputHandler.instance.selectedBuilding.GetComponent<UnitSpawner>();
                unitSpawner.StartSpawnTimer(name);
            }
                
            else if (InputManager.InputHandler.instance.selectedUnits != null)
            {
                BuildingsPlacementManager.instance.CreateFlyingBuilding(name);
            }  
        }
    }
}
